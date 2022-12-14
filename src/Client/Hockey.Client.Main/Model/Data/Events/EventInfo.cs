using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.Model.Data.Events;

internal class EventInfo : ReactiveObject
{
    [Reactive] public string Description { get; set; }
    [Reactive] public EventType EventType { get; set; }

    [Reactive] public int StartEventFrameNumber { get; set; }
    [Reactive] public int EndEventFrameNumber { get; set; }

    public ObservableCollection<EventParameter> Parameters { get; }

    public EventInfo(EventType eventType, params EventParameter[] parameters)
    {
        Description = eventType.Name;
        EventType = eventType;
        Parameters = new(parameters);
    }
}
