using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Hockey.Client.Main.Model.Data.Events;

internal class EventFactory : ReactiveObject
{
    [Reactive] public EventType EventType { get; set; }
    [Reactive] public TimeSpan DefaultTimeSpan { get; set; }
    [Reactive] public bool IsCreated { get; private set; }

    public ObservableCollection<EventParameterFactory> ParameterFactories { get; }

    public EventFactory()
    {
        ParameterFactories = new();
    }

    public EventFactory(IEnumerable<EventParameterFactory> factories)
    {
        ParameterFactories = new(factories);
    }

    public EventInfo Create()
    {
        return new EventInfo(EventType,
                             DefaultTimeSpan,
                             ParameterFactories.Select
                             (
                                 x => x switch
                                 {
                                     PlayerEventParameterFactory f => new PlayerEventParameter(f.TeamName, f.Name)
                                     {
                                         Player = f.DefaultPlayer,
                                         Team = f.DefaultTeam,
                                     } as EventParameter,
                                     TeamEventParameterFactory f => new TeamEventParameter(f.Name)
                                     {
                                         Team = f.DefaultTeam,
                                     },
                                     TextEventParameterFactory f => new TextEventParameter(f.Name)
                                     {
                                         Text = f.DefaultText
                                     },
                                 }
                             ).ToArray());
    }
}
