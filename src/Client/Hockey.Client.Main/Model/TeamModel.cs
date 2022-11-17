using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;

namespace Hockey.Client.Main.Model;


internal class TeamModel : ReactiveObject
{
    [Reactive] public string Name { get; set; }
    public ObservableCollection<PlayerModel> Players { get; } = new();
}
