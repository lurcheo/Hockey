using Hockey.Client.Shared.Dto;

namespace Hockey.Client.Main.Model.Abstraction;
internal interface IDtoConverter
{
    GameProjectDto Convert(IGameStore store);
}
