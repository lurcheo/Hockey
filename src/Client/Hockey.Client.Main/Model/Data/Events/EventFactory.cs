using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Hockey.Client.Main.Model.Data.Events;

internal class EventFactory : ReactiveObject
{
    [Reactive] public EventType EventType { get; set; }
    [Reactive] public Key BindingKey { get; set; } = Key.None;
    [Reactive] public TimeSpan DefaultDuration { get; set; }

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
