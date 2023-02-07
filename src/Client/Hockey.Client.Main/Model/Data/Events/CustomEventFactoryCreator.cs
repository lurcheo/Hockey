using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.Model.Data.Events;

internal class CustomEventFactoryCreator : ReactiveObject
{
    [Reactive] public string Name { get; set; }
    [Reactive] public TimeSpan DefaultTimeSpan { get; set; }

    public ObservableCollection<CustomEventParameterFactory> ParameterFactories { get; } = new();
}
