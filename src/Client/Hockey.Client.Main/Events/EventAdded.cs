using Hockey.Client.Main.Model.Data.Events;
using Prism.Events;

namespace Hockey.Client.Main.Events;

internal class EventAdded : PubSubEvent<EventInfo>
{
}
