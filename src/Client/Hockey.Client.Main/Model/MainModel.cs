using Hockey.Client.BusinessLayer.Abstraction;
using Hockey.Client.Main.Model.Abstraction;
using OpenCvSharp;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Hockey.Client.Main.Model;

internal class MainModel : ReactiveObject, IMainModel
{
    private readonly IVideoService _videoService;

    [Reactive] public Mat CurrentFrame { get; set; }
    [Reactive] public bool Paused { get; set; }

    public MainModel(IVideoService videoService)
    {
        _videoService = videoService;
    }

    public Task ReadVideoFromFile(string fileName, CancellationToken cancellationToken)
    {
        return Task.Run
        (
            async () =>
            {
                using var frames = _videoService.ReadVideoFromFile(fileName);
                foreach (var frame in frames)
                {
                    var st = Stopwatch.StartNew();

                    await Application.Current.Dispatcher.Invoke
                    (
                        async () =>
                        {
                            CurrentFrame = frame;

                            while (Paused)
                            {
                                await Task.Delay(100);
                            }
                        }
                    );

                    long delay = frames.MillisecondsPerFrame - st.ElapsedMilliseconds;

                    if (delay > 0)
                    {
                        Thread.Sleep((int)delay);
                    }

                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }
        );
    }
}
