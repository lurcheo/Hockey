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
                                           .Select(x => x.ParameterType switch
                                           {
                                               EventParameterType.Player => new PlayerEventParameter(name: x.Name) as EventParameter,
                                               EventParameterType.Team => new TeamEventParameter(x.Name),
                                               EventParameterType.Text => new TextEventParameter(x.Name),
                                           }).ToArray());
    }
}
