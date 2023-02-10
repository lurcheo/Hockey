using Hockey.Client.Main.Model.Data;
using Hockey.Client.Shared.Dto;

namespace Hockey.Client.Main.Model.Abstraction;
internal interface IDtoConverter
{
    GameProjectDto Convert(IGameStore store);
    TeamProjectDto Convert(TeamInfo teamInfo);
}
