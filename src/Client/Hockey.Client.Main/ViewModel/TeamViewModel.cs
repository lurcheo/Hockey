using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data;
using ReactiveUI;
using System.Windows.Input;

namespace Hockey.Client.Main.ViewModel;

internal class TeamViewModel : ReactiveObject
{
    public ITeamModel Model { get; }

    public ICommand RemovePlayerCommand { get; }
    public ICommand AddPlayerCommand { get; }

    public TeamViewModel(ITeamModel model)
    {
        Model = model;

        RemovePlayerCommand = ReactiveCommand.Create<PlayerInfo>(x => Model.Team.Players.Remove(x));
        AddPlayerCommand = ReactiveCommand.Create(() => Model.Team.Players.Add(new("Новый игрок команды", 99)));
    }
}
