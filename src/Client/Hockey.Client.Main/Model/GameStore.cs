using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data;
using Hockey.Client.Main.Model.Data.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.Model;

internal class GameStore : ReactiveObject, IGameStore
{
    [Reactive] public ObservableCollection<EventInfo> Events { get; set; }
    [Reactive] public ObservableCollection<EventFactory> EventFactories { get; set; }

    [Reactive] public TeamInfo HomeTeam { get; set; }
    [Reactive] public TeamInfo GuestTeam { get; set; }
    [Reactive] public TimeSpan CurrentTime { get; set; }

    public GameStore(IEnumerable<EventInfo> events, IEnumerable<EventFactory> factories, TeamInfo homeTeam, TeamInfo guestTeam)
    {
        Events = new(events);
        EventFactories = new(factories);

        HomeTeam = homeTeam;
        GuestTeam = guestTeam;

        //EventFactories.ToRemoveObservable()
        //              .Select(x => x.EventType)
        //              .Select(x => Events.Where(ev => ev.EventType == x))
        //              .Select(x => x.ToArray())
        //              .SelectMany(x => x)
        //              .Subscribe(x => Events.Remove(x))
        //              .Cache();
    }
}