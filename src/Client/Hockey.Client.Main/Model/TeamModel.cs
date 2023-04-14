using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data;
using Hockey.Client.Shared.Extensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Linq.Expressions;

namespace Hockey.Client.Main.Model;

internal class TeamModel : ReactiveObject, ITeamModel
{
	[Reactive] public TeamInfo Team { get; set; }
	public IGameStore Store { get; }

	public TeamModel(IGameStore store, Expression<Func<IGameStore, TeamInfo>> teamExpresion)
	{
		Store = store;
		Store.WhenAnyValue(teamExpresion)
			 .Subscribe(x => Team = x)
			 .Cache();
	}
}
