using Hockey.Client.Main.Model.Data;
using Hockey.Client.Main.Model.Data.Events;
using OpenCvSharp;
using ReactiveUI;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hockey.Client.Main.Model.Abstraction;

internal interface IMainModel : IReactiveObject
{
    TeamInfo HomeTeam { get; set; }
    TeamInfo GuestTeam { get; set; }

    Mat CurrentFrame { get; set; }

    bool IsPaused { get; set; }
    bool IsUserClick { get; set; }
    bool IsPlayEvent { get; set; }
    int MillisecondsPerFrame { get; set; }
    long FrameNumber { get; set; }
    long EndEventFrameNumber { get; set; }

    TimeSpan CurrentTime { get; set; }
    TimeSpan EndTime { get; set; }

    PlayingState PlayingState { get; set; }
    long FramesCount { get; set; }

    Task ReadVideoFromFile(string fileName, CancellationToken cancellationToken);
    void SetPosition(long position);
    void PlayEvent(EventInfo eventInfo);
    Task SaveProjectToFile(string fileName);
    Task SaveTeamToFile(string fileName, TeamInfo teamInfo);
}
