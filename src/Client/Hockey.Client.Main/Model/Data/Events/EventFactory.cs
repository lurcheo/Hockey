using System.Linq;

namespace Hockey.Client.Main.Model.Data.Events;

internal class EventFactory
{
    public EventType EventType { get; }
    public EventFactoryCreator FactoryCreator { get; }

    public EventFactory(EventFactoryCreator factoryCreator)
    {
        FactoryCreator = factoryCreator;
        EventType = new(factoryCreator.Name);
    }

    public EventInfo Create()
    {
        return new EventInfo(EventType,
                             FactoryCreator.DefaultTimeSpan,
                             FactoryCreator.ParameterFactories
                                           .Select(x => x switch
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
                                           }).ToArray());
    }
}
