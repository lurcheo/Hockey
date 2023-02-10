using ReactiveUI;

namespace Hockey.Client.Settings.Model.Abstraction;
public interface ISettingsModel : IReactiveObject
{
    bool IsDark { get; set; }
}
