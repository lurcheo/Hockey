using Hockey.Client.BusinessLayer.Abstraction;
using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Shared.Extensions;
using OpenCvSharp;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Hockey.Client.Main.Model;

internal class MainModel : ReactiveObject, IMainModel
{
    private readonly IVideoService _videoService;

    public TeamModel GuestTeam { get; }
    public TeamModel HomeTeam { get; }

    [Reactive] public Mat CurrentFrame { get; set; }
    [Reactive] public bool IsPaused { get; set; }
    [Reactive] public long FrameNumber { get; set; }
    [Reactive] public IVideoReader VideoReader { get; set; }

    public MainModel(IVideoService videoService)
    {
        _videoService = videoService;

        //Чтение из файла
        GuestTeam = new() { Name = "Команда гостей" };
        HomeTeam = new() { Name = "Команда хозяев" };

        this.WhenAnyValue(x => x.FrameNumber)
            .Where(_ => VideoReader is not null && IsPaused)
            .Throttle(TimeSpan.FromMilliseconds(500))
            .Subscribe(x => VideoReader.SetPosition(x))
            .Cache();
    }

    public Task ReadVideoFromFile(string fileName, CancellationToken cancellationToken)
    {
        return Task.Run
        (
            async () =>
            {
                VideoReader?.Dispose();
                VideoReader = _videoService.ReadVideoFromFile(fileName);

                foreach (var frameInfo in VideoReader)
                {
                    await Application.Current.Dispatcher.Invoke
                    (
                        async () =>
                        {
                            (CurrentFrame, FrameNumber) = frameInfo;

                            while (IsPaused)
                            {
                                await Task.Delay(100);
                            }
                        }
                    );

                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }
        );
    }
}
