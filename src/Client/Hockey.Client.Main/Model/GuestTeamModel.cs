using Hockey.Client.Main.Model.Abstraction;

namespace Hockey.Client.Main.Model;

internal class GuestTeamModel : TeamModel, IGuestTeamModel
{
    public GuestTeamModel(IGameStore store)
        : base(store, x => x.GuestTeam)
    {
    }
}
