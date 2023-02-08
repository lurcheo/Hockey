using Hockey.Client.Main.Model.Data;
using Hockey.Client.Main.Model.Data.Events;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.Model.Abstraction;

internal interface IGameStore : IReactiveObject
{
    ObservableCollection<EventInfo> Events { get; set; }
    ObservableCollection<IEventFactory> EventFactories { get; set; }
    ObservableCollection<CustomEventFactoryCreator> FactoryCreators { get; set; }

    TeamInfo HomeTeam { get; set; }
    TeamInfo GuestTeam { get; set; }
    long FrameNumber { get; set; }
    int MillisecondsPerFrame { get; set; }
}
