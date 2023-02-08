using ReactiveUI.Fody.Helpers;

namespace Hockey.Client.Main.Model.Data.Events;

internal class CustomPlayerEventParameterFactory : CustomEventParameterFactory
{
    [Reactive] public string TeamName { get; set; }
    [Reactive] public TeamInfo DefaultTeam { get; set; }
    [Reactive] public PlayerInfo DefaultPlayer { get; set; }
}
