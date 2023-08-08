using Hockey.Client.Main.Model.Data;
using Hockey.Client.Main.Model.Data.Events;
using Hockey.Client.Shared.Dto;
using System.Collections.Generic;

namespace Hockey.Client.Main.Model.Abstraction;
internal interface IDtoConverter
{
    void ConvertToExist(IGameStore gameStore, GameProjectDto gameProjectDto);
    GameProjectDto Convert(IGameStore store);

    TeamProjectDto ConvertToTeamsProject(TeamInfo teamInfo);
    TeamInfo ConvertFromTeamsProject(TeamProjectDto team);


    EventsProjectDto ConvertToEventsProject(IGameStore gameStore);
    List<EventFactory> ConvertFromEventsProject(EventsProjectDto eventsProject);
}
