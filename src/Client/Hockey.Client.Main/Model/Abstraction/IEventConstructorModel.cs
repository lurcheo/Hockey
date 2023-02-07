using ReactiveUI;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.Model.Abstraction;

internal interface IEventConstructorModel : IReactiveObject
{
    ObservableCollection<IEventFactory> EventFactories { get; set; }
}
