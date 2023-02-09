using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data;
using Hockey.Client.Main.Model.Data.Events;
using Hockey.Client.Shared.Extensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.Model;

internal class EventConstructorModel : ReactiveObject, IEventConstructorModel
{
	public ObservableCollection<EventFactory> Factories { get; set; }
	[Reactive] public IEnumerable<TeamInfo> Teams { get; set; }
	public IGameStore Store { get; }

	public EventConstructorModel(IGameStore store)
	{
		Store = store;

		Store.WhenAnyValue(x => x.EventFactories)
			 .Subscribe(x => Factories = x)
			 .Cache();


		Store.WhenAnyValue(x => x.HomeTeam,
						   x => x.GuestTeam,
						   (home, guest) => new[] { home, guest })
			.Subscribe(x => Teams = x)
			.Cache();
	}
}
