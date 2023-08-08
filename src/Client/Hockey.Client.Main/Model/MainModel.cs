using Hockey.Client.BusinessLayer.Abstraction;
using Hockey.Client.BusinessLayer.Data;
using Hockey.Client.Main.Events;
using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data;
using Hockey.Client.Main.Model.Data.Events;
using Hockey.Client.Shared.Dto;
using Hockey.Client.Shared.Extensions;
using OpenCvSharp;
using Prism.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public IEventAggregator EventAggregator { get; }
    public IDtoConverter DtoConverter { get; }
    public IFileService ProjectService { get; }

    [Reactive] public ObservableCollection<EventFactory> EventFactories { get; set; }
    [Reactive] public ObservableCollection<EventInfo> Events { get; set; }

    [Reactive] public Mat CurrentFrame { get; set; }
    [Reactive] public PlaybackSpeed SelectedPlaybackSpeed { get; set; }
    public IReadOnlyList<PlaybackSpeed> PlaybackSpeeds { get; } = new PlaybackSpeed[]
    {
        new (0.25, "0.25x"),
        new (0.5, "0.5x"),
        new (0.75, "0.75x"),
        new (1, "Обычная"),
        new (1.5, "1.5x"),
        new (2, "2x"),
    };

    [Reactive] public bool IsPaused { get; set; }
    [Reactive] public bool IsUserClick { get; set; }
    [Reactive] public bool IsVideoOpen { get; set; } = false;

    [Reactive] public int MillisecondsPerFrame { get; set; } = 0;
    [Reactive] public long FrameNumber { get; set; } = 0;
    [Reactive] public long FramesCount { get; set; } = 0;
    [Reactive] public long EndEventFrameNumber { get; set; } = 0;

    [Reactive] public TimeSpan CurrentTime { get; set; }
    [Reactive] public TimeSpan EndTime { get; set; }

    [Reactive] public PlayingState PlayingState { get; set; }
    [Reactive] public TeamInfo HomeTeam { get; set; }
    [Reactive] public TeamInfo GuestTeam { get; set; }

    private IVideoReader videoReader;


    public MainModel(IVideoService videoService,
                     IGameStore gameStore,
                     IEventAggregator eventAggregator,
                     IDtoConverter dtoConverter,
                     IFileService projectService)
    {
        VideoService = videoService;
        GameStore = gameStore;
        EventAggregator = eventAggregator;
        DtoConverter = dtoConverter;
        ProjectService = projectService;

        SelectedPlaybackSpeed = PlaybackSpeeds[3];

        GameStore.WhenAnyValue(x => x.EventFactories)
                 .Subscribe(x => EventFactories = x)
                 .Cache();

        GameStore.WhenAnyValue(x => x.Events)
                 .Subscribe(x => Events = x)
                 .Cache();

        GameStore.WhenAnyValue(x => x.GuestTeam)
                 .Subscribe(x => GuestTeam = x)
                 .Cache();

        GameStore.WhenAnyValue(x => x.HomeTeam)
                 .Subscribe(x => HomeTeam = x)
                 .Cache();

        this.WhenAnyValue(x => x.FrameNumber)
            .Do(x => CurrentTime = TimeSpan.FromMilliseconds(x * MillisecondsPerFrame))
            .Where(_ => IsVideoOpen && IsUserClick)
            .Do(_ =>
            {
                lock (this)
                {
                    PlayingState = PlayingState.PlayVideo;
                }
            })
            .Throttle(TimeSpan.FromMilliseconds(200))
            .ObserveOnDispatcher()
            .Subscribe(SetPosition)
            .Cache();

        this.WhenAnyValue(x => x.CurrentTime)
            .Subscribe(x => GameStore.CurrentTime = x)
            .Cache();

        EventAggregator.GetEvent<PlayEvent>()
                       .ToObservable()
                       .Subscribe(PlayEvent)
                       .Cache();
    }

    public EventInfo CreateEvent(EventFactory factory)
    {
        var eventInfo = factory.Create();

        eventInfo.StartEventTime = GameStore.CurrentTime;
        eventInfo.EndEventTime = eventInfo.StartEventTime + factory.DefaultDuration;

        return eventInfo;
    }

    public void SetUserPosition(long position)
    {
        if (!IsVideoOpen)
        {
            return;
        }

        lock (this)
        {
            PlayingState = PlayingState.PlayVideo;
        }

        SetPosition(position);
    }

    public void SetPosition(long position)
    {
        if (!IsVideoOpen)
        {
            return;
        }

        if (position < 0)
        {
            position = 0;
        }

        if (position > FramesCount - 1)
        {
            position = FramesCount - 1;
        }

        lock (this)
        {
            videoReader.SetPosition(position);
            FrameInfo info = GetFrameSafety(videoReader);
            if (info == default)
            {
                return;
            }
            SetFrameSafety(info.FrameNumber, info.Frame);
        }
    }

    public async Task ReadProjectFromFile(string fileName, CancellationToken cancellationToken)
    {
        var project = await ProjectService.GetFromFile<GameProjectDto>(fileName);
        DtoConverter.ConvertToExist(GameStore, project);

        if (string.IsNullOrEmpty(GameStore.VideoPath))
        {
            return;
        }

        await ReadVideoFromFile(GameStore.VideoPath, cancellationToken);
    }


    public Task ReadVideoFromFile(string fileName, CancellationToken cancellationToken)
    {
        return Task.Run
        (
            async () =>
            {
                videoReader?.Dispose();

                videoReader = VideoService.ReadVideoFromFile(fileName);

                IsPaused = false;
                GameStore.VideoPath = fileName;
                PlayingState = PlayingState.PlayVideo;
                MillisecondsPerFrame = videoReader.MillisecondsPerFrame;
                FramesCount = videoReader.FramesCount;
                EndTime = TimeSpan.FromMilliseconds(FramesCount * MillisecondsPerFrame);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    IsVideoOpen = true;
                });

                var st = Stopwatch.StartNew();

                while (!cancellationToken.IsCancellationRequested)
                {
                    lock (this)
                    {
                        if (PlayingState == PlayingState.PlayEvent && FrameNumber >= EndEventFrameNumber)
                        {
                            IsPaused = true;
                            PlayingState = PlayingState.PlayVideo;
                        }
                    }

                    if (IsPaused || IsUserClick)
                    {
                        await Task.Delay(100);
                        continue;
                    }

                    st.Restart();

                    FrameInfo info = GetFrameSafety(videoReader);

                    if (info == default)
                    {
                        continue;
                    }

                    SetFrameSafety(info.FrameNumber, info.Frame);

                    int delay = (int)(videoReader.MillisecondsPerFrame / (SelectedPlaybackSpeed?.Multiplier ?? 1) - st.ElapsedMilliseconds);

                    if (delay > 0)
                    {
                        Thread.Sleep(delay);
                    }
                }
            }
        );
    }

    private FrameInfo GetFrameSafety(IVideoReader videoReader)
    {
        FrameInfo info = null;

        lock (this)
        {
            info = videoReader.GetFrame();
        }

        return info;
    }

    private void SetFrameSafety(long frameNumber, Mat frame)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            FrameNumber = frameNumber;
            CurrentFrame = frame;
        });
    }

    public Task SaveProjectToFile(string fileName)
    {
        return ProjectService.SaveToFile(fileName, DtoConverter.Convert(GameStore));
    }

    public void PlayEvent(EventInfo eventInfo)
    {
        if (!IsVideoOpen)
        {
            return;
        }

        SetPosition((long)(eventInfo.StartEventTime.TotalMilliseconds / MillisecondsPerFrame));

        lock (this)
        {
            EndEventFrameNumber = (long)(eventInfo.EndEventTime.TotalMilliseconds / MillisecondsPerFrame);
            PlayingState = PlayingState.PlayEvent;
            IsPaused = false;
        }
    }

    public Task SaveHomeTeamToFile(string fileName)
    {
        return ProjectService.SaveToFile(fileName, DtoConverter.ConvertToTeamsProject(GameStore.HomeTeam));
    }

    public Task SaveGuestTeamToFile(string fileName)
    {
        return ProjectService.SaveToFile(fileName, DtoConverter.ConvertToTeamsProject(GameStore.GuestTeam));
    }

    public async Task ReadHomeTeamToFile(string fileName)
    {
        GameStore.HomeTeam = DtoConverter.ConvertFromTeamsProject(await ProjectService.GetFromFile<TeamProjectDto>(fileName));
    }

    public async Task ReadGuestTeamToFile(string fileName)
    {
        GameStore.GuestTeam = DtoConverter.ConvertFromTeamsProject(await ProjectService.GetFromFile<TeamProjectDto>(fileName));
    }

    public async Task ReadEventsFromFile(string fileName)
    {
        GameStore.EventFactories = new(DtoConverter.ConvertFromEventsProject(await ProjectService.GetFromFile<EventsProjectDto>(fileName)));
        GameStore.Events = new();
    }

    public Task SaveEventsToFile(string fileName)
    {
        return ProjectService.SaveToFile(fileName, DtoConverter.ConvertToEventsProject(GameStore));
    }
}
