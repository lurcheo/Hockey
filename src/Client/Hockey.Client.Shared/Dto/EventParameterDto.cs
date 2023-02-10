namespace Hockey.Client.Shared.Dto;
public class EventParameterDto : BaseDto
{
    public int EventId { get; set; }
    public string Name { get; set; }
    public EventParameterType ParameterType { get; set; }
}
