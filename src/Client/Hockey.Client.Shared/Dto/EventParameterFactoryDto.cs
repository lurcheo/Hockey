namespace Hockey.Client.Shared.Dto;

public class EventParameterFactoryDto : BaseDto
{
    public int EventFactoryId { get; set; }
    public string Name { get; set; }
    public EventParameterType ParameterFactoryType { get; set; }
}
