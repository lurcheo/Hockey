using Hockey.Client.BusinessLayer.Abstraction;
using Hockey.Client.Main.Model.Data.Events;

namespace Hockey.Client.Main.Model.Abstraction;

internal interface IEventFactory : IFactory<EventInfo>
{
    EventType EventType { get; }
    bool IsCustom { get; }
}
