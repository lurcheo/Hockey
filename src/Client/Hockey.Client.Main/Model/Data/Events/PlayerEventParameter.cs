using ReactiveUI.Fody.Helpers;

namespace Hockey.Client.Main.Model.Data.Events;

internal class PlayerEventParameter : EventParameter
{
    public string TeamName { get; }
    [Reactive] public TeamInfo Team { get; set; }
    [Reactive] public PlayerInfo Player { get; set; }

    public PlayerEventParameter(string teamName = "Команда", string name = "Игрок")
        : base(name)
    {
        TeamName = teamName;
    }
}
