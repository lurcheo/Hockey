using Hockey.Client.BusinessLayer.Abstraction;
using Hockey.Client.BusinessLayer.Data;
using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Shared.Extensions;
using OpenCvSharp;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Hockey.Client.Main.Model;

internal class MainModel : ReactiveObject, IMainModel
{
    public IVideoService VideoService { get; }
    public IGameStore GameStore { get; }

    [Reactive] public Mat CurrentFrame { get; set; }
    [Reactive] public bool IsPaused { get; set; }
    [Reactive] public bool IsUserClick { get; set; }
    [Reactive] public long FrameNumber { get; set; } = 0;
    [Reactive] public long FramesCount { get; set; } = 100;

    private IVideoReader videoReader;


    public MainModel(IVideoService videoService, IGameStore gameStore)
    {
        VideoService = videoService;
        GameStore = gameStore;

        this.WhenAnyValue(x => x.FrameNumber)
            .Where(_ => videoReader is not null && IsUserClick)
            .Throttle(TimeSpan.FromMilliseconds(200))
            .ObserveOnDispatcher()
            .Do(x => GameStore.FrameNumber = x)
            .Subscribe(SetPosition)
            .Cache();
    }

    public void SetPosition(long position)
    {
        lock (this)
        {
            videoReader.SetPosition(position);

            FrameInfo info = null;
            lock (this)
            {
                info = videoReader.GetFrame();
            }

            if (info == default)
            {
                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                FrameNumber = info.FrameNumber;
                CurrentFrame = info.Frame;
            });
        }
    }

    public Task ReadVideoFromFile(string fileName, CancellationToken cancellationToken)
    {
        return Task.Run
        (
            async () =>
            {
                videoReader?.Dispose();
                videoReader = VideoService.ReadVideoFromFile(fileName);
                GameStore.MillisecondsPerFrame = videoReader.MillisecondsPerFrame;
                FramesCount = videoReader.FramesCount;

                var st = Stopwatch.StartNew();

                while (!cancellationToken.IsCancellationRequested)
                {
                    while (IsPaused || IsUserClick)
                    {
                        await Task.Delay(100);
                        continue;
                    }

                    st.Restart();

                    FrameInfo info = null;
                    lock (this)
                    {
                        info = videoReader.GetFrame();
                    }

                    if (info == default)
                    {
                        continue;
                    }

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        FrameNumber = info.FrameNumber;
                        CurrentFrame = info.Frame;
                    });

                    int delay = (int)(videoReader.MillisecondsPerFrame - st.ElapsedMilliseconds);

                    if (delay > 0)
                    {
                        Thread.Sleep(delay);
                    }
                }
            }
        );
    }
}
