using Hockey.Client.Main.Model.Abstraction;

namespace Hockey.Client.Main.ViewModel;

internal class GuestTeamViewModel : TeamViewModel
{
    public GuestTeamViewModel(IGuestTeamModel model)
        : base(model)
    {
    }
}
