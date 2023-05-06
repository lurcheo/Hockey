using Hockey.Client.Main.Model.Data;
using Hockey.Client.Main.Model.Data.Events;
using Hockey.Client.Shared;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Hockey.Client.Main.Model.Abstraction;

internal interface IEventModel : IReactiveObject
{
    ObservableCollection<EventInfo> Events { get; set; }
    ObservableCollection<EventFactory> EventFactories { get; set; }

    IEnumerable<TeamInfo> Teams { get; set; }
    int VideoSavingProgress { get; set; }
    IEnumerable<EventInfo> FiltredEvents { get; set; }
    IEnumerable<Selected<EventType>> SelectedEventTypes { get; set; }

    EventInfo CreateEvent(EventFactory factory);
    Task WriteVideoFromEvents(string filePath);
    void PlayEvent(EventInfo eventInfo);
    IReadOnlyList<EventInfo> Filter(IEnumerable<EventInfo> eventInfos);
}
