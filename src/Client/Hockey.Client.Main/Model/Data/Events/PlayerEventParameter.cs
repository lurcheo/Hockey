using Hockey.Client.Shared.Extensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Linq;

namespace Hockey.Client.Main.Model.Data.Events;

internal class PlayerEventParameter : EventParameter
{
    [Reactive] public TeamInfo Team { get; set; }
    [Reactive] public PlayerInfo Player { get; set; }

    public PlayerEventParameter(string name = "Игрок")
        : base(name)
    {
        this.WhenAnyValue(x => x.Team)
            .WhereNotNull()
            .Where(x => x.Players.Count > 0)
            .Select(x => x.Players[0])
            .Subscribe(x => Player = x)
            .Cache();
    }
}
