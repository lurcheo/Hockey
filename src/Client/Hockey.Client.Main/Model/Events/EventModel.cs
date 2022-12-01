using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Hockey.Client.Main.Model.Events;

internal class EventModel : ReactiveObject
{
    [Reactive] public EventTypeModel EventType { get; set; }
    [Reactive] public long FramePosition { get; set; }

    public EventModel(EventTypeModel eventType, long framePosition)
    {
        EventType = eventType;
        FramePosition = framePosition;
    }

}
