using Hockey.Client.BusinessLayer.Abstraction;
using OpenCvSharp;
using ReactiveUI;
using System.Threading;
using System.Threading.Tasks;

namespace Hockey.Client.Main.Model.Abstraction;

internal interface IMainModel : IReactiveObject
{
    bool IsPaused { get; set; }
    IVideoReader VideoReader { get; set; }
    Mat CurrentFrame { get; set; }
    long FrameNumber { get; set; }

    TeamModel HomeTeam { get; }
    TeamModel GuestTeam { get; }

    Task ReadVideoFromFile(string fileName, CancellationToken cancellationToken);
}
