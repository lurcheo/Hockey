using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data;
using Prism.Services.Dialogs;
using ReactiveUI;
using System.Windows.Input;

namespace Hockey.Client.Main.ViewModel;

internal class TeamViewModel : ReactiveObject
{
    public ITeamModel Model { get; }
    public IDialogService DialogService { get; }

    public ICommand RemovePlayerCommand { get; }
    public ICommand AddPlayerCommand { get; }

    public TeamViewModel(ITeamModel model, IDialogService dialogService)
    {
        Model = model;
        DialogService = dialogService;
        RemovePlayerCommand = ReactiveCommand.CreateFromTask<PlayerInfo>(async x =>
        {
            //if (!await DialogService.Confirm($"Удалить игрока {x.Name}"))
            //{
            //    return;
            //}

            Model.Team.Players.Remove(x);
        });
        AddPlayerCommand = ReactiveCommand.Create(() => Model.Team.Players.Add(new("Новый игрок команды", 99)));
    }
}
