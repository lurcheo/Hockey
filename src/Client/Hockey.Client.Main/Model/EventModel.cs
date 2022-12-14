using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data;
using Hockey.Client.Main.Model.Data.Events;
using Hockey.Client.Shared.Extensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.Model;

internal class EventModel : ReactiveObject, IEventModel
{
    [Reactive] public ObservableCollection<EventInfo> Events { get; set; }
    [Reactive] public ObservableCollection<EventFactory> EventFactories { get; set; }
    [Reactive] public IEnumerable<TeamInfo> Teams { get; set; }

    public EventModel(IGameStore store)
    {
        store.WhenAnyValue(x => x.Events)
             .Subscribe(x => Events = x)
             .Cache();

        store.WhenAnyValue(x => x.EventFactories)
             .Subscribe(x => EventFactories = x)
             .Cache();

        store.WhenAnyValue(x => x.HomeTeam,
                           x => x.GuestTeam,
                           (home, guest) => new[] { home, guest })
            .Subscribe(x => Teams = x)
            .Cache();
    }
}
