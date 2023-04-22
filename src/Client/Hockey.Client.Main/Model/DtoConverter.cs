using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data;
using Hockey.Client.Main.Model.Data.Events;
using Hockey.Client.Shared.Dto;
using Hockey.Client.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Hockey.Client.Main.Model;

internal class DtoConverter : IDtoConverter
{
    public void ConvertToExist(IGameStore store, GameProjectDto storeDto)
    {
        ReadTeams(storeDto, out var teamDictionary, out var guestTeam, out var homeTeam);
        ReadPlayers(storeDto, teamDictionary, out var playerDictionary);

        ReadTypes(storeDto, out var eventTypesDictionary);

        var eventFactories = ReadEventFactories(storeDto, eventTypesDictionary, out var eventFactoryDictionary);
        ReadEventFactoryParameters(storeDto, eventFactoryDictionary, playerDictionary, teamDictionary);

        var events = ReadEvents(storeDto, eventTypesDictionary, out var eventsDictionary);
        ReadEventsParameters(storeDto, eventsDictionary, playerDictionary, teamDictionary);

        store.GuestTeam = guestTeam;
        store.HomeTeam = homeTeam;
        store.EventFactories = new(eventFactories);
        store.Events = new(events);
        store.VideoPath = storeDto.VideoPath;
    }

    private static void ReadTeams(GameProjectDto store,
                                  out IReadOnlyDictionary<int, TeamInfo> teamDictionary,
                                  out TeamInfo guestTeam,
                                  out TeamInfo homeTeam)
    {
        store.Teams.GetIdDictionaryFromDto(x => new TeamInfo(x.Name), out var teamDic);
        teamDictionary = teamDic;
        guestTeam = teamDic[store.GuestTeamId];
        homeTeam = teamDic[store.HomeTeamId];
    }

    private static void ReadPlayers(GameProjectDto store,
                                   IReadOnlyDictionary<int, TeamInfo> teamDictionary,
                                   out IReadOnlyDictionary<int, PlayerInfo> playerDictionary)

    {
        store.Players.GetIdDictionaryFromDto(x =>
         {
             PlayerInfo player = new(x.Name, x.Number);
             teamDictionary[x.TeamId].Players.Add(player);
             return player;
         }, out playerDictionary);
    }

    private static void ReadTypes(GameProjectDto store, out IReadOnlyDictionary<int, EventType> eventTypeDictionary)
    {
        store.EventTypes.GetIdDictionaryFromDto(x => new EventType(x.Name), out eventTypeDictionary);
    }

    private static IEnumerable<EventFactory> ReadEventFactories(GameProjectDto store, IReadOnlyDictionary<int, EventType> eventTypeDictionary, out IReadOnlyDictionary<int, EventFactory> eventFactoryDictionary)
    {
        return store.EventFactories.GetIdDictionaryFromDto(x => new EventFactory
        {
            DefaultDuration = x.DefaultTimeSpan,
            EventType = eventTypeDictionary[x.EventTypeId],
            BindingKey = Enum.Parse<Key>(x.BindingKey)
        }, out eventFactoryDictionary);
    }

    private static void ReadEventFactoryParameters(GameProjectDto store,
                                                   IReadOnlyDictionary<int, EventFactory> eventFactoryDictionary,
                                                   IReadOnlyDictionary<int, PlayerInfo> playersDictionary,
                                                   IReadOnlyDictionary<int, TeamInfo> teamsDictionary)
    {
        var playerDic = store.PlayerEventParameterFactories.ToDictionary(x => x.ParameterId, x => x);
        var teamDic = store.TeamEventParameterFactories.ToDictionary(x => x.ParameterId, x => x);
        var textDic = store.TextEventParameterFactories.ToDictionary(x => x.ParameterId, x => x);

        foreach (var x in store.EventParameterFactories)
        {

            switch (x.ParameterFactoryType)
            {
                case EventParameterType.Player:
                {
                    var player = playerDic[x.Id];
                    PlayerEventParameterFactory parameter = new()
                    {
                        DefaultPlayer = player.DefaultPlayerId is null ? null : playersDictionary[player.DefaultPlayerId.Value],
                        DefaultTeam = player.DefaultTeamId is null ? null : teamsDictionary[player.DefaultTeamId.Value],
                        TeamName = player.TeamName,
                        Name = x.Name,
                    };

                    eventFactoryDictionary[x.EventFactoryId].ParameterFactories.Add(parameter);
                    break;
                }
                case EventParameterType.Team:
                {
                    var team = teamDic[x.Id];
                    TeamEventParameterFactory parameter = new()
                    {
                        DefaultTeam = team.DefaultTeamId is null ? null : teamsDictionary[team.DefaultTeamId.Value],
                        Name = x.Name,
                    };

                    eventFactoryDictionary[x.EventFactoryId].ParameterFactories.Add(parameter);
                    break;
                }
                case EventParameterType.Text:
                {
                    var text = textDic[x.Id];
                    TextEventParameterFactory parameter = new()
                    {
                        DefaultText = text.DefaultText,
                        Name = x.Name,
                    };

                    eventFactoryDictionary[x.EventFactoryId].ParameterFactories.Add(parameter);
                    break;
                }
            }
        }
    }

    private static IEnumerable<EventInfo> ReadEvents(GameProjectDto store, IReadOnlyDictionary<int, EventType> eventTypeDictionary, out IReadOnlyDictionary<int, EventInfo> eventsDictionary)
    {
        return store.Events.GetIdDictionaryFromDto(x => new EventInfo(eventTypeDictionary[x.EventTypeId])
        {
            StartEventTime = x.StartEventTime,
            EndEventTime = x.EndEventTime,
        }, out eventsDictionary);
    }

    private static void ReadEventsParameters(GameProjectDto store,
                                             IReadOnlyDictionary<int, EventInfo> eventsDictionary,
                                             IReadOnlyDictionary<int, PlayerInfo> playersDictionary,
                                             IReadOnlyDictionary<int, TeamInfo> teamsDictionary)
    {
        var playerDic = store.PlayerEventParameters.ToDictionary(x => x.ParameterId, x => x);
        var teamDic = store.TeamEventParameters.ToDictionary(x => x.ParameterId, x => x);
        var textDic = store.TextEventParameters.ToDictionary(x => x.ParameterId, x => x);

        foreach (var x in store.EventParameters)
        {
            switch (x.ParameterType)
            {
                case EventParameterType.Player:
                {
                    var player = playerDic[x.Id];
                    PlayerEventParameter parameter = new(player.TeamName, x.Name)
                    {
                        Player = player.PlayerId is null ? null : playersDictionary[player.PlayerId.Value],
                        Team = player.TeamId is null ? null : teamsDictionary[player.TeamId.Value],
                    };

                    eventsDictionary[x.EventId].Parameters.Add(parameter);
                    break;
                }
                case EventParameterType.Team:
                {
                    var team = teamDic[x.Id];
                    TeamEventParameter parameter = new(x.Name)
                    {
                        Team = team.TeamId is null ? null : teamsDictionary[team.TeamId.Value],
                    };

                    eventsDictionary[x.EventId].Parameters.Add(parameter);
                    break;
                }
                case EventParameterType.Text:
                {
                    var text = textDic[x.Id];
                    TextEventParameter parameter = new(x.Name)
                    {
                        Text = text.Text
                    };

                    eventsDictionary[x.EventId].Parameters.Add(parameter);
                    break;
                }
            }

        }
    }

    public TeamInfo Convert(TeamProjectDto team)
    {
        return new(team.Team.Name,
                   team.Players
                       .Select(x => new PlayerInfo(x.Name, x.Number))
                       .ToArray());
    }

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
                Name = x.Name,
                Number = x.Number,
                TeamId = -1
            }).ToArray()
        };
    }

    #region Convert to Dto
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
            Name = x.player.Name,
            Number = x.player.Number,
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
            BindingKey = x.BindingKey.ToString()
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
    #endregion
}
