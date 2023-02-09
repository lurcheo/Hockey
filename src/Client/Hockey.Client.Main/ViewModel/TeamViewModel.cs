using DynamicData;
using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data;
using Hockey.Client.Shared.Data;
using Hockey.Client.Shared.Extensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;

namespace Hockey.Client.Main.ViewModel;

internal class TeamViewModel : ReactiveObject
{
    public ITeamModel Model { get; }

    public PositionPlayers AttackPlayers { get; } = new(PlayerPosition.AttackPlayer);
    public PositionPlayers DefendPlayers { get; } = new(PlayerPosition.DefensePlayer);
    public PositionPlayers Goalkepers { get; } = new(PlayerPosition.Goalkeeper);
    public PositionPlayers ReservePlayers { get; } = new(PlayerPosition.Reserve);

    public IEnumerable<PositionPlayers> Players { get; }

    [Reactive] public int LinksCount { get; set; }
    [Reactive] public IEnumerable<LinkViewModel> Links { get; set; }
    [Reactive] public LinkViewModel SelectedLink { get; set; }

    public ICommand AddLinkCommand { get; }
    public ICommand RemoveLinkCommand { get; }

    public ICommand CreatePlayerCommand { get; }
    public ICommand MovePlayerCommand { get; }
    public ICommand RemovePlayerCommand { get; }

    public TeamViewModel(ITeamModel model)
    {
        Model = model;

        MovePlayerCommand = ReactiveCommand.Create<PlayerMoveCommandParameter>(x =>
        {
            x.PlayerInfo.Link = x.NewLink;

            (x.PlayerInfo.Position switch
            {
                PlayerPosition.AttackPlayer => AttackPlayers,
                PlayerPosition.DefensePlayer => DefendPlayers,
                PlayerPosition.Goalkeeper => Goalkepers,
                PlayerPosition.Reserve => ReservePlayers
            }).Players.Remove(x.PlayerInfo);
        });

        CreatePlayerCommand = ReactiveCommand.Create<PlayerPosition>
        (
            x => CreatePlayer(SelectedLink.Number, x),
            this.WhenAnyValue(x => x.SelectedLink).Select(x => x is not null)
        );

        RemovePlayerCommand = ReactiveCommand.Create<PlayerInfo>(RemovePlayer);

        AddLinkCommand = ReactiveCommand.Create
        (
            () => LinksCount++,
            this.WhenAnyValue(x => x.LinksCount)
                .Select(x => x < 10)
        );

        RemoveLinkCommand = ReactiveCommand.Create
        (
            () =>
            {
                foreach (var player in Model.Team.Players.Where(x => x.Link == LinksCount))
                {
                    player.Link--;
                }

                LinksCount--;
            },
            this.WhenAnyValue(x => x.LinksCount)
                .Select(x => x > 1)
        );

        Players = new[]
        {
            AttackPlayers,
            DefendPlayers,
            Goalkepers,
            ReservePlayers
        };

        Model.WhenAnyValue(x => x.Team)
            .Select(x => x.Players)
            .Select(x => x.Max(x => x.Link))
            .Subscribe(x => LinksCount = x)
            .Cache();

        this.WhenAnyValue(x => x.LinksCount)
            .Select(x => Enumerable.Range(1, LinksCount))
            .Select(x => x.Select(i => new LinkViewModel { Number = i }))
            .Select(x => x.ToArray())
            .Subscribe(x => Links = x)
            .Cache();

        var linkChanged = this.WhenAnyValue(x => x.SelectedLink)
                              .WhereNotNull();

        this.WhenAnyValue(x => x.Links)
            .WithLatestFrom
            (
                this.WhenAnyValue(x => x.SelectedLink)
                    .WhereNotNull()
                    .Merge(Observable.Return(Links.First())),
                (links, link) => (links, link)
            ).Subscribe(x =>
            {
                SelectedLink = x.links.FirstOrDefault(l => l.Number == x.link.Number)
                                                           ?? x.links.MaxBy(x => x.Number);
            })
            .Cache();

        var playersChanged = Model.WhenAnyValue(x => x.Team)
                                  .Select(x => x.Players)
                                  .Select(x => x.AsEnumerable());

        Init(playersChanged, linkChanged, AttackPlayers);
        Init(playersChanged, linkChanged, DefendPlayers);
        Init(playersChanged, linkChanged, Goalkepers);
        Init(playersChanged, linkChanged, ReservePlayers);
    }

    private static void Init(IObservable<IEnumerable<PlayerInfo>> src, IObservable<LinkViewModel> link, PositionPlayers dst)
    {
        src.CombineLatest(link, (players, link) => players.Where(x => x.Link == link.Number))
           .Select(x => x.Where(x => x.Position == dst.Position))
           .Do(_ => dst.Players.Clear())
           .Subscribe(dst.Players.AddRange)
           .Cache();

        dst.Players
           .ToAddObservable()
           .Where(x => x.Position != dst.Position)
           .Subscribe(x => x.Position = dst.Position)
           .Cache();
    }

    private void RemovePlayer(PlayerInfo player)
    {
        Model.Team.Players.Remove(player);

        (player.Position switch
        {
            PlayerPosition.AttackPlayer => AttackPlayers,
            PlayerPosition.DefensePlayer => DefendPlayers,
            PlayerPosition.Goalkeeper => Goalkepers,
            PlayerPosition.Reserve => ReservePlayers,
        }).Players.Remove(player);
    }

    private void CreatePlayer(int link, PlayerPosition playerPosition)
    {
        var player = new PlayerInfo("Новый игрок", 0, playerPosition, link);

        Model.Team.Players.Add(player);
        (playerPosition switch
        {
            PlayerPosition.AttackPlayer => AttackPlayers,
            PlayerPosition.DefensePlayer => DefendPlayers,
            PlayerPosition.Goalkeeper => Goalkepers,
            PlayerPosition.Reserve => ReservePlayers,
        }).Players.Add(player);
    }
}
