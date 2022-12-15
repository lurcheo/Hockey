using Hockey.Client.Main.Model.Data;
using Hockey.Client.Shared.Data;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.ViewModel;

internal class PositionPlayers
{
    public PlayerPosition Position { get; }
    public ObservableCollection<PlayerInfo> Players { get; } = new();

    public PositionPlayers(PlayerPosition position)
    {
        Position = position;
    }
}