namespace Hockey.Client.Shared.Dto;

public class GameProjectDto
{
    public int GuestTeamId { get; set; }
    public int HomeTeamId { get; set; }

    public TeamDto[] Teams { get; set; }
    public PlayerDto[] Players { get; set; }
    public EventTypeDto[] EventTypes { get; set; }
    public EventFactoryDto[] EventFactories { get; set; }
    public EventParameterFactoryDto[] EventParameterFactories { get; set; }
    public PlayerEventParameterFactoryDto[] PlayerEventParameterFactories { get; set; }
    public TeamEventParameterFactoryDto[] TeamEventParameterFactories { get; set; }
    public TextEventParameterFactoryDto[] TextEventParameterFactories { get; set; }
    public EventDto[] Events { get; set; }
    public EventParameterDto[] EventParameters { get; set; }
    public PlayerEventParameterDto[] PlayerEventParameters { get; set; }
    public TeamEventParameterDto[] TeamEventParameters { get; set; }
    public TextEventParameterDto[] TextEventParameters { get; set; }
}
