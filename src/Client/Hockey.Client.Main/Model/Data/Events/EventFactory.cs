using System;

namespace Hockey.Client.Main.Model.Data.Events;
internal class EventFactory
{
	public EventType EventType { get; }
	private readonly Func<EventType, EventInfo> factory;

	public EventFactory(EventType eventType, Func<EventType, EventInfo> factory)
	{
		EventType = eventType;
		this.factory = factory;
	}

	public EventInfo Create()
	{
		return factory(EventType);
	}
}
