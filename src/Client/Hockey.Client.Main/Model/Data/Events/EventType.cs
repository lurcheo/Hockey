using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Hockey.Client.Main.Model.Data.Events;

internal class EventType : ReactiveObject
{
	[Reactive] public string Name { get; set; }

	public EventType(string name)
	{
		Name = name;
	}
}