using Hockey.Client.Main.Model.Data;
using Hockey.Client.Main.Model.Data.Events;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.Model.Abstraction;

internal interface IGameStore : IReactiveObject
{
    ObservableCollection<EventInfo> Events { get; set; }
    ObservableCollection<EventFactory> EventFactories { get; set; }
    TeamInfo HomeTeam { get; set; }
    TeamInfo GuestTeam { get; set; }
    TimeSpan CurrentTime { get; set; }
}
