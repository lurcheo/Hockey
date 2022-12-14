using Hockey.Client.Main.Model.Abstraction;

namespace Hockey.Client.Main.ViewModel;

internal class HomeTeamViewModel : TeamViewModel
{
    public HomeTeamViewModel(IHomeTeamModel model)
        : base(model)
    {
    }
}