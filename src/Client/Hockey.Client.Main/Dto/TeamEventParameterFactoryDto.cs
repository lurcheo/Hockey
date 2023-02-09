namespace Hockey.Client.Main.Dto;

internal class TeamEventParameterFactoryDto : BaseDto
{
    public int ParameterId { get; set; }
    public int? DefaultTeamId { get; set; }
}
