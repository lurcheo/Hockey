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

    private static IEnumerable<IEventFactory> GetDefaultEventFactories()
    {
        return new DefaultEventFactory[]
        {
            new DefaultEventFactory(new EventType("Гол"), type => new EventInfo(type,
                                                                         TimeSpan.FromSeconds(10),
                                                                         new TextEventParameter("Описание"),
                                                                         new PlayerEventParameter("Забивший"),
                                                                         new PlayerEventParameter("Ассистент"))),
            new DefaultEventFactory(new EventType("Толчок"), type => new EventInfo(type,
                                                                         TimeSpan.FromSeconds(10),
                                                                         new TextEventParameter("Описание"),
                                                                         new PlayerEventParameter("Провинившийся"),
                                                                         new PlayerEventParameter("Жертва"))),
            new DefaultEventFactory(new EventType("Игра в меньшистве"), type => new EventInfo(type,
                                                                         TimeSpan.FromSeconds(10),
                                                                         new TextEventParameter("Описание"),
                                                                         new TeamEventParameter())),
            new DefaultEventFactory(new EventType("Игра в большинстве"), type => new EventInfo(type,
                                                                         TimeSpan.FromSeconds(10),
                                                                         new TextEventParameter("Описание"),
                                                                         new TeamEventParameter())),
            new DefaultEventFactory(new EventType("Гол1"), type => new EventInfo(type,
                                                                         TimeSpan.FromSeconds(10),
                                                                         new TextEventParameter("Описание"),
                                                                         new PlayerEventParameter())),
            new DefaultEventFactory(new EventType("Гол2"), type => new EventInfo(type,
                                                                         TimeSpan.FromSeconds(10),
                                                                         new TextEventParameter("Описание"),
                                                                         new PlayerEventParameter())),
            new DefaultEventFactory(new EventType("Гол3"), type => new EventInfo(type,
                                                                         TimeSpan.FromSeconds(10),
                                                                         new TextEventParameter("Описание"),
                                                                         new PlayerEventParameter())),
            new DefaultEventFactory(new EventType("Гол4"), type => new EventInfo(type,
                                                                         TimeSpan.FromSeconds(10),
                                                                         new TextEventParameter("Описание"),
                                                                         new PlayerEventParameter())),
            new DefaultEventFactory(new EventType("Гол5"), type => new EventInfo(type,
                                                                         TimeSpan.FromSeconds(10),
                                                                         new TextEventParameter("Описание"),
                                                                         new PlayerEventParameter())),
            new DefaultEventFactory(new EventType("Гол6"), type => new EventInfo(type,
                                                                         TimeSpan.FromSeconds(10),
                                                                         new TextEventParameter("Описание"),
                                                                         new PlayerEventParameter())),
        };
    }

    private static TeamInfo GetDefaultHomeTeam()
    {
        return new("Команда хозяев", Enumerable.Range(1, 23)
                                               .Select(x => new PlayerInfo($"Игрок хозяев {x}", x, (PlayerPosition)(x % 4), x / 6 + 1))
                                               .ToArray());
    }

    private static TeamInfo GetDefaultGuestTeam()
    {
        return new("Команда гостей", Enumerable.Range(1, 23)
                                               .Select(x => new PlayerInfo($"Игрок гостей {x}", x, (PlayerPosition)(x % 4), x / 6 + 1))
                                               .ToArray());
    }
}
