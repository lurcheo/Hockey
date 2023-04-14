using Hockey.Client.Main.Events;
using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data.Events;
using Hockey.Client.Shared.Extensions;
using Prism.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Windows.Input;
using static Hockey.Client.Shared.Extensions.DialogExtensionsMethods;

namespace Hockey.Client.Main.ViewModel;
internal class EventsViewModel : ReactiveObject
{
    public IEventModel Model { get; }
    public IEventAggregator EventAggregator { get; }

    public ICommand AddEventCommand { get; }
    public ICommand RemoveEventCommand { get; }
    public ICommand PlayEventCommand { get; }
    public ICommand WriteVideoFromEventsCommand { get; }

    [Reactive] public EventInfo SelectedEvent { get; set; }

    public EventsViewModel(IEventModel model, IEventAggregator eventAggregator)
    {
        Model = model;
        EventAggregator = eventAggregator;

        AddEventCommand = ReactiveCommand.Create<EventFactory>(x => Model.Events.Add(Model.CreateEvent(x)));
        RemoveEventCommand = ReactiveCommand.Create<EventInfo>(x => Model.Events.Remove(x));

        PlayEventCommand = ReactiveCommand.Create<EventInfo>(Model.PlayEvent);

        EventAggregator.GetEvent<EventAdded>()
                       .ToObservable()
                       .Subscribe(x => SelectedEvent = x)
                       .Cache();


        WriteVideoFromEventsCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (!TrySaveFileDialog($"События.MP4",
                                   "Видео".ConcatExtensions("MP4"),
                                   out var fileName))
            {
                return;
            }

            await Model.WriteVideoFromEvents(fileName);
        });
    }
}
