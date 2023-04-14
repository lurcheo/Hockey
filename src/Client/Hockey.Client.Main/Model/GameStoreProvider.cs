using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data;
using Hockey.Client.Main.Model.Data.Events;
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
                EventType = new("Автогол"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Игрок",
                        TeamName= "Команда",
                    }
                }
            )
            {
                EventType = new("Атака"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Игрок",
                        TeamName= "Команда",
                    }
                }
            )
            {
                EventType = new("Атака сзади"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Игрок",
                        TeamName= "Команда",
                    }
                }
            )
            {
                EventType = new("Блокировка"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Игрок",
                        TeamName= "Команда",
                    }
                }
            )
            {
                EventType = new("Бросок"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Игрок",
                        TeamName= "Команда",
                    }
                }
            )
            {
                EventType = new("Бросок с неудобной руки"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Заработавший",
                        TeamName= "Команда",
                    },
                    new PlayerEventParameterFactory
                    {
                        Name = "Реализуюищй",
                        TeamName= "Команда",
                    },
                }
            )
            {
                EventType = new("Буллит"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Игрок",
                        TeamName= "Команда",
                    }
                }
            )
            {
                EventType = new("Ван-таймер"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Игрок",
                        TeamName= "Команда",
                    }
                }
            )
            {
                EventType = new("Вбрасывание"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Игрок",
                        TeamName= "Команда",
                    }
                }
            )
            {
                EventType = new("Гол"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Подающий",
                        TeamName= "Команда",
                    },
                    new PlayerEventParameterFactory
                    {
                        Name = "Принимающий",
                        TeamName= "Команда",
                    },
                }
            )
            {
                EventType = new("Голевой пас"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                EventType = new("Задержка соперника"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                EventType = new("Задержка клюшки соперника (руками)"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                EventType = new("Задержка клюшки соперника (зацеп)"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Обороняющиеся",
                        TeamName= "Команда обороняющихся",
                    },
                    new PlayerEventParameterFactory
                    {
                        Name = "Нападающие",
                        TeamName= "Команда нападающих",
                    },
                }
            )
            {
                EventType = new("Защита"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Игрок",
                        TeamName= "Команда",
                    }
                }
            )
            {
                EventType = new("Кистевой бросок"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Игрок",
                        TeamName= "Команда",
                    }
                }
            )
            {
                EventType = new("Мельница"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Игрок",
                        TeamName= "Команда",
                    }
                }
            )
            {
                EventType = new("Обводка"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        TeamName= "Команда",
                    },
                    new PlayerEventParameterFactory
                    {
                        Name = "Жертва",
                        TeamName= "Команда",
                    },
                }
            )
            {
                EventType = new("Игра высоко поднятой клюшкой"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Игрок отнявший шайбу",
                        TeamName= "Команда",
                    },
                    new PlayerEventParameterFactory
                    {
                        Name = "Игрок потерявший шайбу",
                        TeamName= "Команда",
                    },
                }
            )
            {
                EventType = new("Отбор шайбы"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Пасующий игрок",
                        TeamName= "Команда",
                    },
                    new PlayerEventParameterFactory
                    {
                        Name = "Принимающий игрок",
                        TeamName= "Команда",
                    },
                }
            )
            {
                EventType = new("Пас"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Пасующий игрок",
                        TeamName= "Команда",
                    },
                    new PlayerEventParameterFactory
                    {
                        Name = "Принимающий игрок",
                        TeamName= "Команда",
                    },
                }
            )
            {
                EventType = new("Пас через две линии"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Игрок",
                        TeamName= "Команда",
                    }
                }
            )
            {
                EventType = new("Положение «Вне игры»"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Игрок",
                        TeamName= "Команда",
                    }
                }
            )
            {
                EventType = new("Прессинг"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Игрок",
                        TeamName= "Команда",
                    }
                }
            )
            {
                EventType = new("Проброс шайбы"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                EventType = new("Силовой прием"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Игрок",
                        TeamName= "Команда",
                    }
                }
            )
            {
                EventType = new("Спин-о-рама"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Игрок",
                        TeamName= "Команда",
                    }
                }
            )
            {
                EventType = new("Сэйв"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Игрок",
                        TeamName= "Команда",
                    }
                }
            )
            {
                EventType = new("Финт"),
                DefaultDuration = TimeSpan.FromSeconds(10),
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
                        Name = "Игрок",
                        TeamName= "Команда",
                    }
                }
            )
            {
                EventType = new("Щелчок"),
                DefaultDuration = TimeSpan.FromSeconds(10),
            },
        };
    }

    private static TeamInfo GetDefaultHomeTeam()
    {
        TeamInfo team = new("Команда хозяев",
                        Enumerable.Range(1, 40)
                                  .Select(x => new PlayerInfo($"Игрок хозяев {x}", x))
                                  .ToArray());

        return team;
    }

    private static TeamInfo GetDefaultGuestTeam()
    {
        TeamInfo team = new("Команда гостей",
                        Enumerable.Range(1, 40)
                                  .Select(x => new PlayerInfo($"Игрок гостей {x}", x))
                                  .ToArray());

        return team;
    }
}
