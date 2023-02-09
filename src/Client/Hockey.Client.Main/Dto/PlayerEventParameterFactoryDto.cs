namespace Hockey.Client.Main.Dto;

internal class PlayerEventParameterFactoryDto : BaseDto
{
    public int ParameterId { get; set; }
    public string TeamName { get; set; }
    public int? DefaultTeamId { get; set; }
    public int? DefaultPlayerId { get; set; }
}
