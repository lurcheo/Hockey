using Hockey.Client.Shared.Data;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Hockey.Client.Main.Model.Data;

internal class PlayerInfo : ReactiveObject
{
    [Reactive] public PlayerPosition Position { get; set; }
    [Reactive] public string Name { get; set; }
    [Reactive] public int Number { get; set; }

    public PlayerInfo(string name, int number, PlayerPosition position)
    {
        Name = name;
        Number = number;
        Position = position;
    }
}
