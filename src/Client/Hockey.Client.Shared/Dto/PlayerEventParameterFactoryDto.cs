namespace Hockey.Client.Shared.Dto;

public class PlayerEventParameterFactoryDto : BaseDto
{
    public int ParameterId { get; set; }
    public string TeamName { get; set; }
    public int? DefaultTeamId { get; set; }
    public int? DefaultPlayerId { get; set; }
}
