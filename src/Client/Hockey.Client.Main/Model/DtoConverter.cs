﻿using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data;
using Hockey.Client.Main.Model.Data.Events;
using Hockey.Client.Shared.Dto;
using Hockey.Client.Shared.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Hockey.Client.Main.Model;

internal class DtoConverter : IDtoConverter
{
    public TeamProjectDto Convert(TeamInfo teamInfo)
    {
        return new()
        {
            Team = new TeamDto
            {
                Id = -1,
                Name = teamInfo.Name,
            },
            Players = teamInfo.Players.Select(x => new PlayerDto
            {
                Id = -1,
                Link = x.Link,
                Name = x.Name,
                Number = x.Number,
                Position = x.Position,
                TeamId = -1
            }).ToArray()
        };
    }

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
        var eventsDto = GetEventsDto(store,
                                     eventTypesDictionary,
                                     out var eventsDictionary);

        var eventParametersDto = GetEventParametersDto(store,
                                                       eventsDictionary,
                                                       out var eventParametersDictionary,
                                                       out var allEventParameters);

        var playerEventParametersDto = GetPlayerEventParametersDto(allEventParameters,
                                                                   eventParametersDictionary,
                                                                   teamDictionary,
                                                                   playersDictionary);

        var teamEventParametersDto = GetTeamEventParametersDto(allEventParameters,
                                                               eventParametersDictionary,
                                                               teamDictionary);

        var textEventParametersDto = GetTextEventParametersDto(allEventParameters,
                                                               eventParametersDictionary);


        GameProjectDto gameProject = new()
        {
            HomeTeamId = homeTeamId,
            GuestTeamId = guestTeamId,
            VideoPath = store.VideoPath,
            Teams = teamsDto,
            Players = playersDto,
            EventFactories = eventFactoriesDto,
            EventParameterFactories = eventParameterFactoriesDto,
            EventTypes = eventTypesDto,
            PlayerEventParameterFactories = playerEventParameterFactoriesDto,
            TeamEventParameterFactories = teamEventParameterFactoriesDto,
            TextEventParameterFactories = textEventParameterFactoriesDto,
            Events = eventsDto,
            EventParameters = eventParametersDto,
            PlayerEventParameters = playerEventParametersDto,
            TeamEventParameters = teamEventParametersDto,
            TextEventParameters = textEventParametersDto
        };

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

    private static EventTypeDto[] GetTypesDto(IGameStore store,
                                              out IReadOnlyDictionary<EventType, int> eventTypesDictionary)
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
                PlayerEventParameterFactory => EventParameterType.Player,
                TeamEventParameterFactory => EventParameterType.Team,
                TextEventParameterFactory => EventParameterType.Text,
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

    private static EventDto[] GetEventsDto(IGameStore store,
                                           IReadOnlyDictionary<EventType, int> eventTypesDictionary,
                                           out IReadOnlyDictionary<EventInfo, int> eventsDictionary)
    {
        var events = store.Events.ToArray();

        var eventsDic = events.GetIdDictionary();
        eventsDictionary = eventsDic;

        return events.Select(x => new EventDto
        {
            Id = eventsDic[x],
            EndEventTime = x.EndEventTime,
            StartEventTime = x.StartEventTime,
            EventTypeId = eventTypesDictionary[x.EventType]
        }).ToArray();
    }

    private static EventParameterDto[] GetEventParametersDto(IGameStore store,
                                                             IReadOnlyDictionary<EventInfo, int> eventsDictionary,
                                                             out IReadOnlyDictionary<EventParameter, int> eventParametersDictionary,
                                                             out IEnumerable<EventParameter> allEventParameters)
    {
        var events = store.Events.ToArray();
        var parameters = events.SelectMany
        (
            x => x.Parameters
                  .Select(p => (@event: x, parameter: p))
        ).ToArray();

        var eventParametersDic = parameters.Select(x => x.parameter)
                                                              .GetIdDictionary();
        eventParametersDictionary = eventParametersDic;
        allEventParameters = parameters.Select(x => x.parameter).ToArray();

        return parameters.Select(x => new EventParameterDto
        {
            Id = eventParametersDic[x.parameter],
            Name = x.parameter.Name,
            EventId = eventsDictionary[x.@event],
            ParameterType = x.parameter switch
            {
                PlayerEventParameter => EventParameterType.Player,
                TeamEventParameter => EventParameterType.Team,
                TextEventParameter => EventParameterType.Text,
            }
        }).ToArray();
    }

    private static PlayerEventParameterDto[] GetPlayerEventParametersDto(IEnumerable<EventParameter> allEventParameters,
                                                                                  IReadOnlyDictionary<EventParameter, int> eventParametersDictionary,
                                                                                  IReadOnlyDictionary<TeamInfo, int> teamDictionary,
                                                                                  IReadOnlyDictionary<PlayerInfo, int> playersDictionary)
    {
        var playerEventParameters = allEventParameters.Select(x => x as PlayerEventParameter)
                                                      .Where(x => x != null)
                                                      .ToArray();

        var playerEventParametersDic = playerEventParameters.GetIdDictionary();

        return playerEventParameters.Select(x => new PlayerEventParameterDto
        {
            Id = playerEventParametersDic[x],
            TeamName = x.TeamName,
            PlayerId = x.Player is not null ? playersDictionary[x.Player] : null,
            TeamId = x.Team is not null ? teamDictionary[x.Team] : null,
            ParameterId = eventParametersDictionary[x],
        }).ToArray();
    }

    private static TeamEventParameterDto[] GetTeamEventParametersDto(IEnumerable<EventParameter> allEventParameters,
                                                                     IReadOnlyDictionary<EventParameter, int> eventParametersDictionary,
                                                                     IReadOnlyDictionary<TeamInfo, int> teamDictionary)
    {
        var teamEventParameters = allEventParameters.Select(x => x as TeamEventParameter)
                                                    .Where(x => x != null)
                                                    .ToArray();

        var teamEventParametersDic = teamEventParameters.GetIdDictionary();

        return teamEventParameters.Select(x => new TeamEventParameterDto
        {
            Id = teamEventParametersDic[x],
            TeamId = x.Team is not null ? teamDictionary[x.Team] : null,
            ParameterId = eventParametersDictionary[x],
        }).ToArray();
    }

    private static TextEventParameterDto[] GetTextEventParametersDto(IEnumerable<EventParameter> allEventParameters,
                                                                     IReadOnlyDictionary<EventParameter, int> eventParametersDictionary)
    {
        var textEventParameters = allEventParameters.Select(x => x as TextEventParameter)
                                                    .Where(x => x != null)
                                                    .ToArray();


        var textEventParametersDic = textEventParameters.GetIdDictionary();

        return textEventParameters.Select(x => new TextEventParameterDto
        {
            Id = textEventParametersDic[x],
            ParameterId = eventParametersDictionary[x],
            Text = x.Text
        }).ToArray();
    }
}
