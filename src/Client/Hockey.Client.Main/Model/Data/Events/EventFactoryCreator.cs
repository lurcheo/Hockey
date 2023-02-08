using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.Model.Data.Events;

internal class EventFactoryCreator : ReactiveObject
{
    [Reactive] public string Name { get; set; }
    [Reactive] public TimeSpan DefaultTimeSpan { get; set; }
    [Reactive] public bool IsCreated { get; private set; }

    public ObservableCollection<EventParameterFactory> ParameterFactories { get; }

    public EventFactoryCreator()
    {
        ParameterFactories = new();
    }

    public EventFactoryCreator(IEnumerable<EventParameterFactory> factories)
    {
        ParameterFactories = new(factories);
    }

    public EventFactory CreateFactory()
    {
        //if (IsCreated)
        //{
        //    throw new Exception("Невозможно создать фабрику создания, т.к. она уже была создана");
        //}

        //IsCreated = true;

        return new EventFactory(this);
    }
}
