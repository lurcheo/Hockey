using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Hockey.Client.Main.Model;

internal class PlayerModel : ReactiveObject
{
    public Guid Id { get; } = Guid.NewGuid();

    [Reactive] public int PlayerNumber { get; set; }
    [Reactive] public string Name { get; set; }
}
