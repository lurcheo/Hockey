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
    [Reactive] public int LinksCount { get; set; } = 4;

    [Reactive] public IEnumerable<LinkViewModel> Links { get; set; }

    public ICommand AddLinkCommand { get; }
    public ICommand RemoveLinkCommand { get; }

    public ICommand SelectLinkCommand { get; }

    public ICommand CreatePlayerCommand { get; }
    public ICommand RemovePlayerCommand { get; }

    public TeamViewModel(ITeamModel model)
    {
        Model = model;

        CreatePlayerCommand = ReactiveCommand.Create<PlayerPosition>(CreatePlayer);
        RemovePlayerCommand = ReactiveCommand.Create<PlayerInfo>(RemovePlayer);
        SelectLinkCommand = ReactiveCommand.Create<LinkViewModel>(x =>
        {
            //TODO поменять игроков на игроков другово звена

            int link = x.Number;
        });

        AddLinkCommand = ReactiveCommand.Create(() => LinksCount++);
        RemoveLinkCommand = ReactiveCommand.Create(() => LinksCount--);

        Players = new[]
        {
            AttackPlayers,
            DefendPlayers,
            Goalkepers,
            ReservePlayers
        };

        this.WhenAnyValue(x => x.LinksCount)
            .Select(x => Enumerable.Range(1, LinksCount))
            .Select(x => x.Select(i => new LinkViewModel { Number = i }))
            .Select(x => x.ToArray())
            .Subscribe(x => Links = x)
            .Cache();

        var playersChanged = Model.WhenAnyValue(x => x.Team)
                                  .Select(x => x.Players)
                                  .Select(x => x.AsEnumerable());

        Init(playersChanged, AttackPlayers);
        Init(playersChanged, DefendPlayers);
        Init(playersChanged, Goalkepers);
        Init(playersChanged, ReservePlayers);
    }

    private static void Init(IObservable<IEnumerable<PlayerInfo>> src, PositionPlayers dst)
    {
        src.Subscribe(players =>
        {
            dst.Players.Clear();
            dst.Players.AddRange(players.Where(x => x.Position == dst.Position));
        }).Cache();

        dst.Players
           .ToAddObservable()
           .Where(x => x.Position != dst.Position)
           .Subscribe(x =>
           {
               x.Position = dst.Position;
           }).Cache();
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

    private void CreatePlayer(PlayerPosition playerPosition)
    {
        var player = new PlayerInfo("Новый игрок", 0, playerPosition);

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
