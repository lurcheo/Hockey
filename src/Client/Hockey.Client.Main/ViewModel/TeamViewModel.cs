using Hockey.Client.Main.Model.Abstraction;
using ReactiveUI;
using System.Reactive.Linq;
using System.Windows.Input;

namespace Hockey.Client.Main.ViewModel;

internal class TeamViewModel : ReactiveObject
{
    public ITeamModel Model { get; }

    public ICommand CreateSquadCommand { get; }

    public TeamViewModel(ITeamModel model)
    {
        Model = model;

        CreateSquadCommand = ReactiveCommand.Create
        (
            () => Model.Team.Squads.Add(new("Новый состав")),
            Model.WhenAnyValue(x => x.Team).Select(x => x is not null)
        );
    }
}
