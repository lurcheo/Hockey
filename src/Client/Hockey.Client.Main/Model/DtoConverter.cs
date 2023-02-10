using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data;
using Hockey.Client.Main.Model.Data.Events;
using Hockey.Client.Shared.Dto;
using Hockey.Client.Shared.Extensions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hockey.Client.Main.Model;

internal class DtoConverter : IDtoConverter
{
    public GameProjectDto Convert(IGameStore store)
    {
        var teamsDto = GetTeamsDto(store,
                                   out var teamDictionary,
                                   out int guestTeamId,
                                   out int homeTeamId);

        var playersDto = GetPlayersDto(store,
                                       teamDictionary,
                                       out var playersDictionary);

        var eventTypesDto = GetTypesDto(store,
                                        out var eventTypesDictionary);

        var eventFactoriesDto = GetEventFactoriesDto(store,
                                                     eventTypesDictionary,
                                                     out var eventFactoriesDictionary);

        var eventParameterFactoriesDto = GetEventParameterFactoriesDto(store,
                                                                       eventFactoriesDictionary,
                                                                       out var eventParameterFactoriesDictionary,
                                                                       out var allEventParameterFactories);

        var playerEventParameterFactoriesDto = GetPlayerEventParameterFactoriesDto(allEventParameterFactories,
                                                                                   eventParameterFactoriesDictionary,
                                                                                   teamDictionary,
                                                                                   playersDictionary);

        var teamEventParameterFactoriesDto = GetTeamEventParameterFactoriesDto(allEventParameterFactories,
                                                                               eventParameterFactoriesDictionary,
                                                                               teamDictionary);

        var textEventParameterFactoriesDto = GetTextEventParameterFactoriesDto(allEventParameterFactories,
                                                                               eventParameterFactoriesDictionary);


        GameProjectDto gameProject = new()
        {
            HomeTeamId = homeTeamId,
            GuestTeamId = guestTeamId,
            Teams = teamsDto,
            Players = playersDto,
            EventFactories = eventFactoriesDto,
            EventParameterFactories = eventParameterFactoriesDto,
            EventTypes = eventTypesDto,
            PlayerEventParameterFactories = playerEventParameterFactoriesDto,
            TeamEventParameterFactories = teamEventParameterFactoriesDto,
            TextEventParameterFactories = textEventParameterFactoriesDto
        };

        File.WriteAllText("test.hjson", JsonConvert.SerializeObject(gameProject, Formatting.Indented));

        return gameProject;
    }

    private static TeamDto[] GetTeamsDto(IGameStore store,
                                         out IReadOnlyDictionary<TeamInfo, int> teamDictionary,
                                         out int guestTeamId,
                                         out int homeTeamId)
    {
        var teams = new[] { store.GuestTeam, store.HomeTeam };
        var teamDic = teams.GetIdDictionary();

        guestTeamId = teamDic[store.GuestTeam];
        homeTeamId = teamDic[store.HomeTeam];
        teamDictionary = teamDic;

        return teams.Select(x => new TeamDto
        {
            Id = teamDic[x],
            Name = x.Name
        }).ToArray();
    }

    private static PlayerDto[] GetPlayersDto(IGameStore store,
                                             IReadOnlyDictionary<TeamInfo, int> teamDictionary,
                                             out IReadOnlyDictionary<PlayerInfo, int> playersDictionary)
    {
        var players = store.GuestTeam
                              .Players
                              .Select(x => (player: x, team: store.GuestTeam))
                              .Concat(store.HomeTeam
                                           .Players
                                           .Select(x => (player: x, team: store.HomeTeam)))
                              .ToArray();

        var playersDic = players.Select(x => x.player).GetIdDictionary();
        playersDictionary = playersDic;

        return players.Select(x => new PlayerDto
        {
            Id = playersDic[x.player],
            Link = x.player.Link,
            Name = x.player.Name,
            Number = x.player.Number,
            Position = x.player.Position,
            TeamId = teamDictionary[x.team]
        }).ToArray();
    }

    private static EventTypeDto[] GetTypesDto(IGameStore store, out IReadOnlyDictionary<EventType, int> eventTypesDictionary)
    {
        var types = store.EventFactories
                         .Select(x => x.EventType)
                         .ToArray();

        var typeDic = types.GetIdDictionary();
        eventTypesDictionary = typeDic;

        return types.Select(x => new EventTypeDto
        {
            Id = typeDic[x],
            Name = x.Name
        }).ToArray();
    }

    private static EventFactoryDto[] GetEventFactoriesDto(IGameStore store,
                                                          IReadOnlyDictionary<EventType, int> eventTypesDictionary,
                                                          out IReadOnlyDictionary<EventFactory, int> eventFactoriesDictionary)
    {
        var eventFactories = store.EventFactories.ToArray();
        var eventTypesDic = eventFactories.GetIdDictionary();
        eventFactoriesDictionary = eventTypesDic;

        return eventFactories.Select(x => new EventFactoryDto
        {
            Id = eventTypesDic[x],
            DefaultTimeSpan = x.DefaultDuration,
            EventTypeId = eventTypesDictionary[x.EventType],
        }).ToArray();
    }

    private static EventParameterFactoryDto[] GetEventParameterFactoriesDto(IGameStore store,
                                                                            IReadOnlyDictionary<EventFactory, int> eventFactoriesDictionary,
                                                                            out IReadOnlyDictionary<EventParameterFactory, int> evenParameterFactoriesDictionary,
                                                                            out IEnumerable<EventParameterFactory> allEventParameterFactories)
    {
        var eventFactories = store.EventFactories.ToArray();
        var eventFactoryParameters = eventFactories.SelectMany
        (
            x => x.ParameterFactories
                  .Select(p => (eventFactory: x, eventParameterFactory: p))
        ).ToArray();

        var eventFactoryParametersDic = eventFactoryParameters.Select(x => x.eventParameterFactory)
                                                              .GetIdDictionary();
        evenParameterFactoriesDictionary = eventFactoryParametersDic;
        allEventParameterFactories = eventFactoryParameters.Select(x => x.eventParameterFactory).ToArray();

        return eventFactoryParameters.Select(x => new EventParameterFactoryDto
        {
            Id = eventFactoryParametersDic[x.eventParameterFactory],
            EventFactoryId = eventFactoriesDictionary[x.eventFactory],
            Name = x.eventParameterFactory.Name,
            ParameterFactoryType = x.eventParameterFactory switch
            {
                PlayerEventParameterFactory => EventParameterFactoryType.Player,
                TeamEventParameterFactory => EventParameterFactoryType.Team,
                TextEventParameterFactory => EventParameterFactoryType.Text,
            }
        }).ToArray();
    }

    private static PlayerEventParameterFactoryDto[] GetPlayerEventParameterFactoriesDto(IEnumerable<EventParameterFactory> allEventParameterFactories,
                                                                                  IReadOnlyDictionary<EventParameterFactory, int> eventParameterFactoriesDictionary,
                                                                                  IReadOnlyDictionary<TeamInfo, int> teamDictionary,
                                                                                  IReadOnlyDictionary<PlayerInfo, int> playersDictionary)
    {
        var playerEventFactoryParameters = allEventParameterFactories.Select(x => x as PlayerEventParameterFactory)
                                                                   .Where(x => x != null)
                                                                   .ToArray();
        var playerEventFactoryParametersDic = playerEventFactoryParameters.GetIdDictionary();

        return playerEventFactoryParameters.Select(x => new PlayerEventParameterFactoryDto
        {
            Id = playerEventFactoryParametersDic[x],
            TeamName = x.TeamName,
            DefaultPlayerId = x.DefaultPlayer is not null ? playersDictionary[x.DefaultPlayer] : null,
            DefaultTeamId = x.DefaultTeam is not null ? teamDictionary[x.DefaultTeam] : null,
            ParameterId = eventParameterFactoriesDictionary[x],
        }).ToArray();
    }

    private static TeamEventParameterFactoryDto[] GetTeamEventParameterFactoriesDto(IEnumerable<EventParameterFactory> allEventParameterFactories,
                                                                                 IReadOnlyDictionary<EventParameterFactory, int> eventParameterFactoriesDictionary,
                                                                                 IReadOnlyDictionary<TeamInfo, int> teamDictionary)
    {
        var teamEventFactoryParameters = allEventParameterFactories.Select(x => x as TeamEventParameterFactory)
                                                                  .Where(x => x != null)
                                                                  .ToArray();
        var teamEventFactoryParametersDic = teamEventFactoryParameters.GetIdDictionary();

        return teamEventFactoryParameters.Select(x => new TeamEventParameterFactoryDto
        {
            Id = teamEventFactoryParametersDic[x],
            DefaultTeamId = x.DefaultTeam is not null ? teamDictionary[x.DefaultTeam] : null,
            ParameterId = eventParameterFactoriesDictionary[x],
        }).ToArray();
    }

    private static TextEventParameterFactoryDto[] GetTextEventParameterFactoriesDto(IEnumerable<EventParameterFactory> allEventParameterFactories,
                                                                                 IReadOnlyDictionary<EventParameterFactory, int> eventParameterFactoriesDictionary)
    {
        var textEventFactoryParameters = allEventParameterFactories.Select(x => x as TextEventParameterFactory)
                                                                 .Where(x => x != null)
                                                                 .ToArray();
        var textEventFactoryParametersDic = textEventFactoryParameters.GetIdDictionary();
        return textEventFactoryParameters.Select(x => new TextEventParameterFactoryDto
        {
            Id = textEventFactoryParametersDic[x],
            DefaultText = x.DefaultText,
            ParameterId = eventParameterFactoriesDictionary[x],
        }).ToArray();
    }
}
