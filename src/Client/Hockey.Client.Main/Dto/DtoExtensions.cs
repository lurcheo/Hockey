using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data.Events;
using System.Collections.Generic;
using System.Linq;

namespace Hockey.Client.Main.Dto;

public static class DtoExtensions
{
    public static IReadOnlyDictionary<T, int> GetIdDictionary<T>(this IEnumerable<T> items)
    {
        return items.Select((item, i) => (item, id: i + 1)).ToDictionary(x => x.item, x => x.id);
    }

    internal static void Save(this IGameStore store)
    {
        var teams = new[] { store.GuestTeam, store.HomeTeam };

        var players = store.GuestTeam
                              .Players
                              .Select(x => (player: x, team: store.GuestTeam))
                              .Concat(store.HomeTeam
                                           .Players
                                           .Select(x => (player: x, team: store.HomeTeam)))
                              .ToArray();

        var playersDic = players.Select(x => x.player).GetIdDictionary();
        var teamDic = teams.GetIdDictionary();

        var playersDto = players.Select(x => new PlayerDto
        {
            Id = playersDic[x.player],
            Link = x.player.Link,
            Name = x.player.Name,
            Number = x.player.Number,
            Position = x.player.Position,
            TeamId = teamDic[x.team]
        }).ToArray();

        var teamsDto = teams.Select(x => new TeamDto
        {
            Id = teamDic[x],
            Name = x.Name
        });

        var guestTeamId = teamDic[store.GuestTeam];
        var homeTeamId = teamDic[store.HomeTeam];

        var types = store.EventFactories
                         .Select(x => x.EventType)
                         .ToArray();

        var typeDic = types.GetIdDictionary();
        var typesDto = types.Select(x => new EventTypeDto
        {
            Id = typeDic[x],
            Name = x.Name
        }).ToList();

        var eventFactories = store.EventFactories.ToArray();
        var eventFactoriesDic = eventFactories.GetIdDictionary();

        var eventFactoriesDto = eventFactories.Select(x => new EventFactoryDto
        {
            Id = eventFactoriesDic[x],
            DefaultTimeSpan = x.DefaultTimeSpan,
            EventTypeId = typeDic[x.EventType],
        }).ToArray();

        var eventFactoryParameters = eventFactories.SelectMany
        (
            x => x.ParameterFactories
                  .Select(p => (eventFactory: x, eventParameterFactory: p))
        ).ToArray();

        var eventFactoryParametersDic = eventFactoryParameters.Select(x => x.eventParameterFactory)
                                                              .GetIdDictionary();

        var eventFactoryParametersDto = eventFactoryParameters.Select(x => new EventParameterFactoryDto
        {
            Id = eventFactoryParametersDic[x.eventParameterFactory],
            EventFactoryId = eventFactoriesDic[x.eventFactory],
            Name = x.eventParameterFactory.Name,
            ParameterFactoryType = x.eventParameterFactory switch
            {
                PlayerEventParameterFactory => EventParameterFactoryType.Player,
                TeamEventParameterFactory => EventParameterFactoryType.Team,
                TextEventParameterFactory => EventParameterFactoryType.Text,
            }
        }).ToArray();

        var allEventFactoryParameters = eventFactoryParameters.Select(x => x.eventParameterFactory).ToArray();

        var playerEventFactoryParameters = allEventFactoryParameters.Select(x => x as PlayerEventParameterFactory)
                                                                    .Where(x => x != null)
                                                                    .ToArray();

        var teamEventFactoryParameters = allEventFactoryParameters.Select(x => x as TeamEventParameterFactory)
                                                                   .Where(x => x != null)
                                                                   .ToArray();

        var textEventFactoryParameters = allEventFactoryParameters.Select(x => x as TextEventParameterFactory)
                                                                   .Where(x => x != null)
                                                                   .ToArray();

        var playerEventFactoryParametersDic = playerEventFactoryParameters.GetIdDictionary();
        var teamEventFactoryParametersDic = teamEventFactoryParameters.GetIdDictionary();
        var textEventFactoryParametersDic = textEventFactoryParameters.GetIdDictionary();

        var playerEventFactoryParametersDto = playerEventFactoryParameters.Select(x => new PlayerEventParameterFactoryDto
        {
            Id = playerEventFactoryParametersDic[x],
            TeamName = x.TeamName,
            DefaultPlayerId = x.DefaultPlayer is not null ? playersDic[x.DefaultPlayer] : null,
            DefaultTeamId = x.DefaultTeam is not null ? teamDic[x.DefaultTeam] : null,
            ParameterId = eventFactoryParametersDic[x],
        }).ToArray();

        var teamEventFactoryParametersDto = teamEventFactoryParameters.Select(x => new TeamEventParameterFactoryDto
        {
            Id = teamEventFactoryParametersDic[x],
            DefaultTeamId = x.DefaultTeam is not null ? teamDic[x.DefaultTeam] : null,
            ParameterId = eventFactoryParametersDic[x],
        }).ToArray();

        var textEventFactoryParametersDto = textEventFactoryParameters.Select(x => new TextEventParameterFactoryDto
        {
            Id = textEventFactoryParametersDic[x],
            DefaultText = x.DefaultText,
            ParameterId = eventFactoryParametersDic[x],
        }).ToArray();
    }
}
