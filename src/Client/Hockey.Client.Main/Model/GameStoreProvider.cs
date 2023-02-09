using Hockey.Client.Main.Dto;
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
        var factories = GetDefaultEventFactoryCreators();

        var store = new GameStore(Enumerable.Empty<EventInfo>(),
                                  factories,
                                  GetDefaultHomeTeam(),
                                  GetDefaultGuestTeam());

        store.Save();

        return store;
    }

    private static IEnumerable<EventFactory> GetDefaultEventFactoryCreators()
    {
        return new EventFactory[]
        {
            new EventFactory
            (
                new EventParameterFactory[]
                {
                    new TextEventParameterFactory
                    {
                        Name = "Описание"
                    },
                    new PlayerEventParameterFactory
                    {
                        Name = "Игрок",
                        TeamName= "Команда",
                    }
                }
            )
            {
                EventType = new("Гол"),
                DefaultTimeSpan = TimeSpan.FromSeconds(10),
            },
            new EventFactory
            (
                new EventParameterFactory[]
                {
                    new TextEventParameterFactory
                    {
                        Name = "Описание"
                    },
                    new PlayerEventParameterFactory
                    {
                        Name = "Провинившийся",
                        TeamName= "Команда провинившегося",
                    },
                    new PlayerEventParameterFactory
                    {
                        Name = "Жертва",
                        TeamName= "Команда жертвы",
                    },
                }
            )
            {
                EventType = new("Толчок"),
                DefaultTimeSpan = TimeSpan.FromSeconds(10),
            },
            new EventFactory
            (
                new EventParameterFactory[]
                {
                    new TextEventParameterFactory
                    {
                        Name = "Описание"
                    },
                    new TeamEventParameterFactory
                    {
                        Name = "Команда",
                    },
                }
            )
            {
                EventType = new("Игра в меньшинстве"),
                DefaultTimeSpan = TimeSpan.FromSeconds(10),
            },
            new EventFactory
            (
                new EventParameterFactory[]
                {
                    new TextEventParameterFactory
                    {
                        Name = "Описание"
                    },
                    new TeamEventParameterFactory
                    {
                        Name = "Команда",
                    },
                }
            )
            {
                EventType = new("Игра в большинстве"),
                DefaultTimeSpan = TimeSpan.FromSeconds(10),
            }
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
