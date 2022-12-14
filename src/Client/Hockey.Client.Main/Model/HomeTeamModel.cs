using Hockey.Client.Main.Model.Abstraction;

namespace Hockey.Client.Main.Model;

internal class HomeTeamModel : TeamModel, IHomeTeamModel
{
    public HomeTeamModel(IGameStore store)
        : base(store, x => x.HomeTeam)
    { }
}
