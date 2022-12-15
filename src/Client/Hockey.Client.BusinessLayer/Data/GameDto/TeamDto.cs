using System;

namespace Hockey.Client.BusinessLayer.Data.GameDto;

internal class TeamDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public PlayerDto[] Players { get; set; }
}
