using Hockey.Client.Main.Model.Abstraction;
using Prism.Services.Dialogs;

namespace Hockey.Client.Main.ViewModel;

internal class GuestTeamViewModel : TeamViewModel
{
    public GuestTeamViewModel(IGuestTeamModel model, IDialogService dialogService)
        : base(model, dialogService)
    {
    }
}
