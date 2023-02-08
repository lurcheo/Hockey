using Hockey.Client.Main.Model.Abstraction;
using System;

namespace Hockey.Client.Main.Model.Data.Events;

internal class DefaultEventFactory : IEventFactory
{
	public EventType EventType { get; }
	public bool IsCustom => false;


	private readonly Func<EventType, EventInfo> factory;

	public DefaultEventFactory(EventType eventType, Func<EventType, EventInfo> factory)
	{
		EventType = eventType;

		this.factory = factory;
	}

	public EventInfo Create()
	{
		return factory(EventType);
	}
}
