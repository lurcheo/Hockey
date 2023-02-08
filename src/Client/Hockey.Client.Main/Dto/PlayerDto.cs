using Hockey.Client.Shared.Data;

namespace Hockey.Client.Main.Dto;

internal class PlayerDto : BaseDto
{
    public PlayerPosition Position { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }
    public int Link { get; set; }
    public int TeamId { get; set; }
}
