using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.Model.Data;

internal class SquadInfo : ReactiveObject
{
    [Reactive] public string Name { get; set; }
    public ObservableCollection<PartSquadInfo> Parts { get; }

    public SquadInfo(string name, params PartSquadInfo[] parts)
    {
        Name = name;
        Parts = new(parts);
    }
}
