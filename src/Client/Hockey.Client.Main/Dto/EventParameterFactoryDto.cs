namespace Hockey.Client.Main.Dto;

internal class EventParameterFactoryDto : BaseDto
{
    public int EventFactoryId { get; set; }
    public string Name { get; set; }
    public EventParameterFactoryType ParameterFactoryType { get; set; }
}
