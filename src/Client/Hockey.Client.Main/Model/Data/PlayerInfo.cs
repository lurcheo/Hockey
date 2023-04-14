using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Hockey.Client.Main.Model.Data;

internal class PlayerInfo : ReactiveObject
{
    [Reactive] public string Name { get; set; }
    [Reactive] public int Number { get; set; }

    public PlayerInfo(string name, int number)
    {
        Name = name;
        Number = number;
    }
}
