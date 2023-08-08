namespace Hockey.Client.Shared.Dto;

public class EventsProjectDto
{
    public EventTypeDto[] EventTypes { get; set; }
    public EventFactoryDto[] EventFactories { get; set; }
    public EventParameterFactoryDto[] EventParameterFactories { get; set; }
    public TextEventParameterFactoryDto[] TextEventParameterFactories { get; set; }
}
