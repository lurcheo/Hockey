using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Hockey.Client.Main.Model.Data.Events;

internal abstract class EventParameterFactory : ReactiveObject
{
    [Reactive] public string Name { get; set; }
}
