using Hockey.Client.Main.Model.Data;
using Hockey.Client.Main.Model.Data.Events;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.Model.Abstraction;

internal interface IEventConstructorModel : IReactiveObject
{
    ObservableCollection<EventFactory> Factories { get; set; }
    IEnumerable<TeamInfo> Teams { get; set; }
}
