using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.Model.Data.Events;

internal class EventInfo : ReactiveObject
{
    [Reactive] public EventType EventType { get; set; }

    [Reactive] public long StartEventFrameNumber { get; set; }
    [Reactive] public long EndEventFrameNumber { get; set; }
    [Reactive] public int MillisecondsPerFrame { get; set; }

    [Reactive] public TimeSpan DefaultDuration { get; set; }

    public ObservableCollection<EventParameter> Parameters { get; }

    public EventInfo(EventType eventType, TimeSpan defaultDuration, params EventParameter[] parameters)
    {
        EventType = eventType;
        DefaultDuration = defaultDuration;
        Parameters = new(parameters);
    }
}
