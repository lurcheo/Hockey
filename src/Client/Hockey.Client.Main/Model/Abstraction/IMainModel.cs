using Hockey.Client.Main.Model.Data;
using Hockey.Client.Main.Model.Data.Events;
using OpenCvSharp;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Hockey.Client.Main.Model.Abstraction;

internal interface IMainModel : IReactiveObject
{
    Mat CurrentFrame { get; set; }

    bool IsPaused { get; set; }
    bool IsUserClick { get; set; }
    int MillisecondsPerFrame { get; set; }
    long FrameNumber { get; set; }
    long EndEventFrameNumber { get; set; }

    TimeSpan CurrentTime { get; set; }
    TimeSpan EndTime { get; set; }

    PlayingState PlayingState { get; set; }
    long FramesCount { get; set; }
    IReadOnlyList<PlaybackSpeed> PlaybackSpeeds { get; }
    PlaybackSpeed SelectedPlaybackSpeed { get; set; }
    bool IsVideoOpen { get; set; }
    ObservableCollection<EventFactory> EventFactories { get; set; }
    ObservableCollection<EventInfo> Events { get; set; }

    Task ReadVideoFromFile(string fileName, CancellationToken cancellationToken);
    void SetPosition(long position);
    void PlayEvent(EventInfo eventInfo);
    Task SaveProjectToFile(string fileName);
    Task SaveHomeTeamToFile(string fileName);
    Task ReadHomeTeamToFile(string fileName);
    Task SaveGuestTeamToFile(string fileName);
    Task ReadGuestTeamToFile(string fileName);
    Task ReadProjectFromFile(string fileName, CancellationToken cancellationToken);
    void SetUserPosition(long position);
    EventInfo CreateEvent(EventFactory factory);
}
