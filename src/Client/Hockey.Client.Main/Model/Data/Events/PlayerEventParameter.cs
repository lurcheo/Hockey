using Hockey.Client.Shared.Extensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Linq;

namespace Hockey.Client.Main.Model.Data.Events;

internal class PlayerEventParameter : EventParameter
{
    public string TeamName { get; }
    [Reactive] public TeamInfo Team { get; set; }
    [Reactive] public PlayerInfo Player { get; set; }

    public PlayerEventParameter(string teamName = "Команда", string name = "Игрок")
        : base(name)
    {
        TeamName = teamName;

        this.WhenAnyValue(x => x.Team)
            .WhereNotNull()
            .Where(x => x.Players.Count > 0)
            .Select(x => x.Players[0])
            .Subscribe(x => Player = x)
            .Cache();
    }
}
