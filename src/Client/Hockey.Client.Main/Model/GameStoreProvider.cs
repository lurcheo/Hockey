using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data;
using Hockey.Client.Main.Model.Data.Events;
using Hockey.Client.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hockey.Client.Main.Model;

internal class GameStoreProvider : IGameStoreProvider
{
    public IGameStore CreateDefault()
    {
        return new GameStore(Enumerable.Empty<EventInfo>(),
                             GetDefaultEventFactories(),
                             GetDefaultHomeTeam(),
                             GetDefaultGuestTeam());
    }

    private static IEnumerable<EventFactory> GetDefaultEventFactories()
    {
        return new EventFactory[]
        {
            new EventFactory(new EventType("Гол"), type => new EventInfo(type,
                                                                         TimeSpan.FromSeconds(10),
                                                                         new PlayerEventParameter("Забивший"),
                                                                         new PlayerEventParameter("Ассистент"))),
            new EventFactory(new EventType("Толчок"), type => new EventInfo(type,
                                                                         TimeSpan.FromSeconds(10),
                                                                         new PlayerEventParameter("Провинившийся"),
                                                                         new PlayerEventParameter("Жертва"))),
            new EventFactory(new EventType("Игра в меньшистве"), type => new EventInfo(type,
                                                                         TimeSpan.FromSeconds(10),
                                                                         new TeamEventParameter())),
            new EventFactory(new EventType("Игра в большинстве"), type => new EventInfo(type,
                                                                         TimeSpan.FromSeconds(10),
                                                                         new TeamEventParameter())),
            new EventFactory(new EventType("Гол1"), type => new EventInfo(type,
                                                                         TimeSpan.FromSeconds(10),
                                                                         new PlayerEventParameter())),
            new EventFactory(new EventType("Гол2"), type => new EventInfo(type,
                                                                         TimeSpan.FromSeconds(10),
                                                                         new PlayerEventParameter())),
            new EventFactory(new EventType("Гол3"), type => new EventInfo(type,
                                                                         TimeSpan.FromSeconds(10),
                                                                         new PlayerEventParameter())),
            new EventFactory(new EventType("Гол4"), type => new EventInfo(type,
                                                                         TimeSpan.FromSeconds(10),
                                                                         new PlayerEventParameter())),
            new EventFactory(new EventType("Гол5"), type => new EventInfo(type,
                                                                         TimeSpan.FromSeconds(10),
                                                                         new PlayerEventParameter())),
            new EventFactory(new EventType("Гол6"), type => new EventInfo(type,
                                                                         TimeSpan.FromSeconds(10),
                                                                         new PlayerEventParameter())),
        };
    }

    private static TeamInfo GetDefaultHomeTeam()
    {
        return new("Команда хозяев", Enumerable.Range(1, 10)
                                               .Select(x => new PlayerInfo($"Игрок {x}", x, PlayerPosition.AttackPlayer))
                                               .ToArray());
    }

    private static TeamInfo GetDefaultGuestTeam()
    {
        return new("Команда гостей", Enumerable.Range(100, 10)
                                               .Select(x => new PlayerInfo($"Игрок {x}", x, PlayerPosition.AttackPlayer))
                                               .ToArray());
    }
}
