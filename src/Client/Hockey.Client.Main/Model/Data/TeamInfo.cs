using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.Model.Data;

internal class TeamInfo : ReactiveObject
{
    [Reactive] public string Name { get; set; }
    public ObservableCollection<PlayerInfo> Players { get; }

    public TeamInfo(string name, params PlayerInfo[] players)
    {
        Name = name;
        Players = new(players);
    }
}
