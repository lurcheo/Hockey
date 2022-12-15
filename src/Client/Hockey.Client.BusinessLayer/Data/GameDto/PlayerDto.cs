using Hockey.Client.Shared.Data;
using System;

namespace Hockey.Client.BusinessLayer.Data.GameDto;

public class PlayerDto
{
    public Guid Id { get; set; }
    public PlayerPosition Position { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }
}
