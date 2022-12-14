using Hockey.Client.Main.Model.Abstraction;
using ReactiveUI;

namespace Hockey.Client.Main.ViewModel;

internal class TeamViewModel : ReactiveObject
{
	public ITeamModel Model { get; }

	public TeamViewModel(ITeamModel model)
	{
		Model = model;
	}
}
