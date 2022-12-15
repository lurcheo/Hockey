using DynamicData;
using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data;
using Hockey.Client.Shared.Data;
using Hockey.Client.Shared.Extensions;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

	public ICommand CreatePlayerCommand { get; }
	public ICommand RemovePlayerCommand { get; }

	public TeamViewModel(ITeamModel model)
	{
		Model = model;

		CreatePlayerCommand = ReactiveCommand.Create<PlayerPosition>(CreatePlayer);
		RemovePlayerCommand = ReactiveCommand.Create<PlayerInfo>(RemovePlayer);

		Players = new[]
		{
			AttackPlayers,
			DefendPlayers,
			Goalkepers,
			ReservePlayers
		};

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
		   .ToObservable(NotifyCollectionChangedAction.Add)
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
