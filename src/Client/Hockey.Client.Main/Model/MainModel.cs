using Hockey.Client.BusinessLayer.Abstraction;
using Hockey.Client.BusinessLayer.Data;
using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Events;
using Hockey.Client.Shared.Extensions;
using OpenCvSharp;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Hockey.Client.Main.Model;

internal class MainModel : ReactiveObject, IMainModel
{
    private readonly IVideoService _videoService;

    public TeamModel GuestTeam { get; } = new() { Name = "Команда гостей" };
    public TeamModel HomeTeam { get; } = new() { Name = "Команда хозяев" };
    public ObservableCollection<EventModel> Events { get; } = new();
    public IEnumerable<EventTypeModel> EventTypes { get; }

    [Reactive] public Mat CurrentFrame { get; set; }
    [Reactive] public bool IsPaused { get; set; }
    [Reactive] public bool IsUserClick { get; set; }
    [Reactive] public long FrameNumber { get; set; } = 0;
    [Reactive] public long FramesCount { get; set; } = 100;
    private IVideoReader _videoReader { get; set; }

    public MainModel(IVideoService videoService)
    {
        _videoService = videoService;
        EventTypes = Enum.GetValues<EventType>().Select(x =>
        {
            var memberInfos = typeof(EventType).GetMember(x.ToString())[0];
            var description = memberInfos.GetCustomAttribute<DescriptionAttribute>().Description;

            return new EventTypeModel(x, description);
        }).ToArray();


        this.WhenAnyValue(x => x.FrameNumber)
            .Where(_ => _videoReader is not null && IsUserClick)
            .Throttle(TimeSpan.FromMilliseconds(200))
            .ObserveOnDispatcher()
            .Subscribe(SetPosition)
            .Cache();
    }

    public void SetPosition(long position)
    {
        lock (this)
        {
            _videoReader.SetPosition(position);

            FrameInfo info = null;
            lock (this)
            {
                info = _videoReader.GetFrame();
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
                _videoReader?.Dispose();
                _videoReader = _videoService.ReadVideoFromFile(fileName);
                FramesCount = _videoReader.FramesCount;

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
                        info = _videoReader.GetFrame();
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

                    int delay = (int)(_videoReader.MillisecondsPerFrame - st.ElapsedMilliseconds);

                    if (delay > 0)
                    {
                        Thread.Sleep(delay);
                    }
                }
            }
        );
    }
}
