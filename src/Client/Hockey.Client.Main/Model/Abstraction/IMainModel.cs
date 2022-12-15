using Hockey.Client.Main.Model.Data;
using Hockey.Client.Main.Model.Data.Events;
using OpenCvSharp;
using ReactiveUI;
using System.Threading;
using System.Threading.Tasks;

namespace Hockey.Client.Main.Model.Abstraction;

internal interface IMainModel : IReactiveObject
{
    bool IsPaused { get; set; }
    Mat CurrentFrame { get; set; }
    long FrameNumber { get; set; }
    long FramesCount { get; set; }

    bool IsUserClick { get; set; }
    long EndFrameNumber { get; set; }
    PlayingState PlayingState { get; set; }
    int MillisecondsPerFrame { get; set; }
    bool IsPlayEvent { get; set; }

    Task ReadVideoFromFile(string fileName, CancellationToken cancellationToken);
    void SetPosition(long position);
    void PlayEvent(EventInfo eventInfo);
}
