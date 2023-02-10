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
    [Reactive] public Mat CurrentFrame { get; set; }

    [Reactive] public bool IsPaused { get; set; }
    [Reactive] public bool IsUserClick { get; set; }
    [Reactive] public bool IsPlayEvent { get; set; } = false;

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

        GameStore.WhenAnyValue(x => x.GuestTeam)
                 .Subscribe(x => GuestTeam = x)
                 .Cache();

        GameStore.WhenAnyValue(x => x.HomeTeam)
                 .Subscribe(x => HomeTeam = x)
                 .Cache();

        this.WhenAnyValue(x => x.FrameNumber)
            .Do(x => CurrentTime = TimeSpan.FromMilliseconds(x * MillisecondsPerFrame))
            .Where(_ => videoReader is not null && IsUserClick)
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

                GameStore.VideoPath = fileName;
                PlayingState = PlayingState.PlayVideo;
                MillisecondsPerFrame = videoReader.MillisecondsPerFrame;
                FramesCount = videoReader.FramesCount;
                EndTime = TimeSpan.FromMilliseconds(FramesCount * MillisecondsPerFrame);

                var st = Stopwatch.StartNew();

                while (!cancellationToken.IsCancellationRequested)
                {
                    lock (this)
                    {
                        if (PlayingState == PlayingState.PlayEvent && FrameNumber >= EndEventFrameNumber)
                        {
                            IsPaused = true;
                            PlayingState = PlayingState.PlayVideo;
                            IsPlayEvent = false;
                        }
                    }

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

    public Task SaveProjectToFile(string fileName)
    {
        return ProjectService.SaveToFile(fileName, DtoConverter.Convert(GameStore));
    }

    public void PlayEvent(EventInfo eventInfo)
    {
        SetPosition((long)(eventInfo.StartEventTime.TotalMilliseconds / MillisecondsPerFrame));

        lock (this)
        {
            EndEventFrameNumber = (long)(eventInfo.EndEventTime.TotalMilliseconds / MillisecondsPerFrame);
            PlayingState = PlayingState.PlayEvent;
            IsPaused = false;
            IsPlayEvent = true;
        }
    }

    public Task SaveHomeTeamToFile(string fileName)
    {
        return ProjectService.SaveToFile(fileName, DtoConverter.Convert(GameStore.HomeTeam));
    }

    public Task SaveGuestTeamToFile(string fileName)
    {
        return ProjectService.SaveToFile(fileName, DtoConverter.Convert(GameStore.GuestTeam));
    }

    public async Task ReadHomeTeamToFile(string fileName)
    {
        GameStore.HomeTeam = DtoConverter.Convert(await ProjectService.GetFromFile<TeamProjectDto>(fileName));
    }

    public async Task ReadGuestTeamToFile(string fileName)
    {
        GameStore.GuestTeam = DtoConverter.Convert(await ProjectService.GetFromFile<TeamProjectDto>(fileName));
    }
}
