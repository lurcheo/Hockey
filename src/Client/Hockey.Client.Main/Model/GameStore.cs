﻿using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data;
using Hockey.Client.Main.Model.Data.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.Model;

internal class GameStore : ReactiveObject, IGameStore
{
    [Reactive] public ObservableCollection<EventInfo> Events { get; set; }
    [Reactive] public ObservableCollection<EventFactory> EventFactories { get; set; }

    [Reactive] public TeamInfo HomeTeam { get; set; }
    [Reactive] public TeamInfo GuestTeam { get; set; }
    [Reactive] public long FrameNumber { get; set; }
    [Reactive] public int MillisecondsPerFrame { get; set; } = 1;

    public GameStore(IEnumerable<EventInfo> events, IEnumerable<EventFactory> factories, TeamInfo homeTeam, TeamInfo guestTeam)
    {
        Events = new(events);
        EventFactories = new(factories);

        HomeTeam = homeTeam;
        GuestTeam = guestTeam;
    }
}