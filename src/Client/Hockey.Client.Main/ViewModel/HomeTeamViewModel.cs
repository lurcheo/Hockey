using Hockey.Client.Main.Model.Abstraction;
using Prism.Services.Dialogs;

namespace Hockey.Client.Main.ViewModel;

internal class HomeTeamViewModel : TeamViewModel
{
    public HomeTeamViewModel(IHomeTeamModel model, IDialogService dialogService)
        : base(model, dialogService)
    {
    }
}