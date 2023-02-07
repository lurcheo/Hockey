using Hockey.Client.Shared.Data;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Hockey.Client.Main.Model.Data;

internal class PlayerInfo : ReactiveObject
{
    [Reactive] public PlayerPosition Position { get; set; }
    [Reactive] public string Name { get; set; }
    [Reactive] public int Number { get; set; }
    [Reactive] public int Link { get; set; }

    public PlayerInfo(string name, int number, PlayerPosition position, int link)
    {
        Name = name;
        Number = number;
        Position = position;
        Link = link;
    }
}
