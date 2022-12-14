using Hockey.Client.Main.Model.Data;
using ReactiveUI;

namespace Hockey.Client.Main.Model.Abstraction;

internal interface ITeamModel : IReactiveObject
{
    TeamInfo Team { get; set; }
}
