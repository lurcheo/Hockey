using ReactiveUI.Fody.Helpers;

namespace Hockey.Client.Main.Model.Data.Events;

internal class CustomTextEventParameterFactory : CustomEventParameterFactory
{
    [Reactive] public string DefaultText { get; set; }
}
