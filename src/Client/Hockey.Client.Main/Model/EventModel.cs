using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.Model;

internal class EventModel : ReactiveObject
{
    [Reactive] public EventType EventType { get; set; }
    public ObservableCollection<PlayerModel> Players { get; set; }
}
