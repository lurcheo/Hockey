namespace Hockey.Client.Shared.Dto;

public class PlayerEventParameterDto : BaseDto
{
    public int ParameterId { get; set; }
    public string TeamName { get; set; }
    public int? TeamId { get; set; }
    public int? PlayerId { get; set; }
}
