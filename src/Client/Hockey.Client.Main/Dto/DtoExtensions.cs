using Hockey.Client.Main.Model.Abstraction;
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

        var playersDic = players.GetIdDictionary();
        var teamDic = teams.GetIdDictionary();

        var playersDto = players.Select(x => new PlayerDto
        {
            Id = playersDic[x],
            Link = x.player.Link,
            Name = x.player.Name,
            Number = x.player.Number,
            Position = x.player.Position,
            TeamId = teamDic[x.team]
        }).ToArray();

        TeamDto guestTeamDto = new()
        {
            Id = teamDic[store.GuestTeam],
            Name = store.GuestTeam.Name
        };

        TeamDto homeTeamDto = new()
        {
            Id = teamDic[store.HomeTeam],
            Name = store.HomeTeam.Name
        };
    }
}
