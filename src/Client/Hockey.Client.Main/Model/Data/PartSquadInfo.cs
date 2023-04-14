using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.Model.Data;

internal class PartSquadInfo : ReactiveObject
{
    [Reactive] public string Name { get; set; }
    public ObservableCollection<PlayerInfo> Players { get; }

    public PartSquadInfo(string name, params PlayerInfo[] players)
    {
        Name = name;
        Players = new(players);
    }
}
