using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Hockey.Client.Main.Model.Data.Events;

internal abstract class CustomEventParameterFactory : ReactiveObject
{
    [Reactive] public string Name { get; set; }
}
