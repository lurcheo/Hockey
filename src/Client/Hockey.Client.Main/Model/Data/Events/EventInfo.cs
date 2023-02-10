using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.Model.Data.Events;

internal class EventInfo : ReactiveObject
{
    [Reactive] public EventType EventType { get; set; }

    [Reactive] public TimeSpan StartEventTime { get; set; }
    [Reactive] public TimeSpan EndEventTime { get; set; }

    public ObservableCollection<EventParameter> Parameters { get; }

    public EventInfo(EventType eventType, params EventParameter[] parameters)
    {
        EventType = eventType;
        Parameters = new(parameters);
    }
}
