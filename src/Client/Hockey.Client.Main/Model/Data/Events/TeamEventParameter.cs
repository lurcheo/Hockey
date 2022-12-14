using ReactiveUI.Fody.Helpers;

namespace Hockey.Client.Main.Model.Data.Events;

internal class TeamEventParameter : EventParameter
{
    [Reactive] public TeamInfo Team { get; set; }

    public TeamEventParameter(string name = "Команда")
        : base(name)
    { }
}
