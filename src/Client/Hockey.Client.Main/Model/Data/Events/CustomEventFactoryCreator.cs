using Hockey.Client.Main.Model.Abstraction;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.Model.Data.Events;

internal class CustomEventFactoryCreator : ReactiveObject
{
    [Reactive] public string Name { get; set; }
    [Reactive] public TimeSpan DefaultTimeSpan { get; set; }
    [Reactive] public bool IsCreated { get; private set; } = false;

    public ObservableCollection<CustomEventParameterFactory> ParameterFactories { get; } = new();

    public IEventFactory CreateFactory()
    {
        if (IsCreated)
        {
            throw new Exception("Невозможно создать фабрику создания, т.к. она уже была создана");
        }

        IsCreated = true;

        return new CustomEventFactory(this);
    }
}
