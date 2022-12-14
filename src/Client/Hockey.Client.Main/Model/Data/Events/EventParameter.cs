using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Hockey.Client.Main.Model.Data.Events;

internal abstract class EventParameter : ReactiveObject
{
    [Reactive] public string Name { get; set; }

    protected EventParameter(string name)
    {
        Name = name;
    }
}
