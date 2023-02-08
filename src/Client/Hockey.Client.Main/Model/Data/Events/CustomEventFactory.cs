using Hockey.Client.Main.Model.Abstraction;
using System.Linq;

namespace Hockey.Client.Main.Model.Data.Events;

internal class CustomEventFactory : IEventFactory
{
    public EventType EventType { get; }
    public bool IsCustom => false;

    public CustomEventFactoryCreator FactoryCreator { get; }

    public CustomEventFactory(CustomEventFactoryCreator factoryCreator)
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
                                               CustomPlayerEventParameterFactory f => new PlayerEventParameter(f.TeamName, f.Name)
                                               {
                                                   Player = f.DefaultPlayer,
                                                   Team = f.DefaultTeam,
                                               } as EventParameter,
                                               CustomTeamEventParameterFactory f => new TeamEventParameter(f.Name)
                                               {
                                                   Team = f.DefaultTeam,
                                               },
                                               CustomTextEventParameterFactory f => new TextEventParameter(f.Name)
                                               {
                                                   Text = f.DefaultText
                                               },
                                           }).ToArray());
    }
}
