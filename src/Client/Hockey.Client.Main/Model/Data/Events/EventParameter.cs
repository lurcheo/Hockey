using ReactiveUI;

namespace Hockey.Client.Main.Model.Data.Events;

internal abstract class EventParameter : ReactiveObject
{
    public string Name { get; }

    protected EventParameter(string name)
    {
        Name = name;
    }
}
