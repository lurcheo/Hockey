using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Windows.Input;

namespace Hockey.Client.Main.ViewModel;
internal class EventsViewModel : ReactiveObject
{
	public IEventModel Model { get; }

	public ICommand AddEventCommand { get; }
	public ICommand RemoveEventCommand { get; }

	[Reactive] public EventInfo SelectedEvent { get; set; }

	public EventsViewModel(IEventModel model)
	{
		Model = model;

		AddEventCommand = ReactiveCommand.Create<EventFactory>(x => Model.Events.Add(Model.CreateEvent(x)));

		RemoveEventCommand = ReactiveCommand.Create<EventInfo>(x => Model.Events.Remove(x));
	}
}
