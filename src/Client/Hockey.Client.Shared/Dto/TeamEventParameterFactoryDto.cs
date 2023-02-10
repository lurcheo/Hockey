namespace Hockey.Client.Shared.Dto;

public class TeamEventParameterFactoryDto : BaseDto
{
    public int ParameterId { get; set; }
    public int? DefaultTeamId { get; set; }
}
