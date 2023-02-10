namespace Hockey.Client.Shared.Dto;

public class TeamEventParameterDto : BaseDto
{
    public int ParameterId { get; set; }
    public int? TeamId { get; set; }
}
