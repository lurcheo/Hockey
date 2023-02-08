using ReactiveUI.Fody.Helpers;

namespace Hockey.Client.Main.Model.Data.Events;

internal class TeamEventParameterFactory : EventParameterFactory
{
    [Reactive] public TeamInfo DefaultTeam { get; set; }
}
