using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.Model.Data.Events;

internal class EventInfo : ReactiveObject
{
    [Reactive] public string Description { get; set; }
    [Reactive] public EventType EventType { get; set; }

    [Reactive] public long StartEventFrameNumber { get; set; }
    [Reactive] public long EndEventFrameNumber { get; set; }
    [Reactive] public int MillisecondsPerFrame { get; set; }

    public extern TimeSpan StartEventTime { [ObservableAsProperty] get; }
    public extern TimeSpan EndEventTime { [ObservableAsProperty] get; }

    public TimeSpan DefaultDuration { get; }

    public ObservableCollection<EventParameter> Parameters { get; }

    public EventInfo(EventType eventType, TimeSpan defaultDuration, params EventParameter[] parameters)
    {
        Description = eventType.Name;
        EventType = eventType;
        DefaultDuration = defaultDuration;
        Parameters = new(parameters);

        this.WhenAnyValue(x => x.StartEventFrameNumber,
                          x => x.MillisecondsPerFrame,
                          (number, mill) => TimeSpan.FromMilliseconds(number * mill))
            .ToPropertyEx(this, x => x.StartEventTime);

        this.WhenAnyValue(x => x.EndEventFrameNumber,
                          x => x.MillisecondsPerFrame,
                          (number, mill) => TimeSpan.FromMilliseconds(number * mill))
            .ToPropertyEx(this, x => x.EndEventTime);
    }
}
