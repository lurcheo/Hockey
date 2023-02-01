using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Hockey.Client.Main.ViewModel;

internal class LinkViewModel : ReactiveObject
{
    [Reactive] public int Number { get; set; }
}
