using Hockey.Client.BusinessLayer.Abstraction;
using Hockey.Client.BusinessLayer.Data;
using Hockey.Client.Main.Events;
using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data;
using Hockey.Client.Main.Model.Data.Events;
using Hockey.Client.Shared.Extensions;
using Prism.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Hockey.Client.Main.Model;

internal class EventModel : ReactiveObject, IEventModel
{
    [Reactive] public ObservableCollection<EventInfo> Events { get; set; }
    [Reactive] public ObservableCollection<EventFactory> EventFactories { get; set; }
    [Reactive] public IEnumerable<TeamInfo> Teams { get; set; }

    [Reactive] public int VideoSavingProgress { get; set; } = 0;

    public IGameStore Store { get; }
    public IVideoService VideoService { get; }
    public IEventAggregator EventAggregator { get; }

    private IDisposable eventAddedDisposable;

    public EventModel(IGameStore store, IVideoService videoService, IEventAggregator eventAggregator)
    {
        Store = store;
        VideoService = videoService;
        EventAggregator = eventAggregator;

        Store.WhenAnyValue(x => x.Events)
             .Subscribe(x => Events = x)
             .Cache();

        this.WhenAnyValue(x => x.Events)
            .Do(_ => eventAddedDisposable?.Dispose())
            .WhereNotNull()
            .Select(x => x.ToAddObservable())
            .Select(x => x.Subscribe(EventAggregator.GetEvent<EventAdded>().Publish))
            .Subscribe(x => eventAddedDisposable = x)
            .Cache();

        Store.WhenAnyValue(x => x.EventFactories)
             .Subscribe(x => EventFactories = x)
             .Cache();

        Store.WhenAnyValue(x => x.HomeTeam,
                           x => x.GuestTeam,
                           (home, guest) => new[] { home, guest })
            .Subscribe(x => Teams = x)
            .Cache();
    }

    public EventInfo CreateEvent(EventFactory factory)
    {
        var eventInfo = factory.Create();

        eventInfo.StartEventTime = Store.CurrentTime;
        eventInfo.EndEventTime = eventInfo.StartEventTime + factory.DefaultDuration;

        return eventInfo;
    }

    public void PlayEvent(EventInfo eventInfo)
    {
        EventAggregator.GetEvent<PlayEvent>()
                       .Publish(eventInfo);
    }

    public async Task WriteVideoFromEvents(string filePath)
    {
        VideoSavingProgress = 0;

        var moments = Events.Select(x => new GameMoment(x.StartEventTime,
                                                        x.EndEventTime))
                            .ToList();

        await VideoService.WriteMomentToVideoFile(Store.VideoPath, filePath, moments, x => VideoSavingProgress = (int)(x * 100));

        VideoSavingProgress = 0;
    }
}
