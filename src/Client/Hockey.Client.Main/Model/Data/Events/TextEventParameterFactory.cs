using ReactiveUI.Fody.Helpers;

namespace Hockey.Client.Main.Model.Data.Events;

internal class TextEventParameterFactory : EventParameterFactory
{
    [Reactive] public string DefaultText { get; set; }
}
