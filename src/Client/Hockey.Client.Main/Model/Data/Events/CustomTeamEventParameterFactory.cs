using ReactiveUI.Fody.Helpers;

namespace Hockey.Client.Main.Model.Data.Events;

internal class CustomTeamEventParameterFactory : CustomEventParameterFactory
{
    [Reactive] public TeamInfo DefaultTeam { get; set; }
}
