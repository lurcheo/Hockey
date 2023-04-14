namespace Hockey.Client.Shared.Dto;

public class PlayerDto : BaseDto
{
    public string Name { get; set; }
    public int Number { get; set; }
    public int TeamId { get; set; }
}
