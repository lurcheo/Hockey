using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Hockey.Client.Main.Model.Data.Events;

internal class CustomEventParameterFactory : ReactiveObject
{
    [Reactive] public string Name { get; set; }
    [Reactive] public EventParameterType ParameterType { get; set; }
}
