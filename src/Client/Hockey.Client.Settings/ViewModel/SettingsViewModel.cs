using Hockey.Client.Settings.Model.Abstraction;
using ReactiveUI;
using System.Windows.Input;

namespace Hockey.Client.Settings.ViewModel;

internal class SettingsViewModel : ReactiveObject
{
	public ISettingsModel Model { get; }

	public ICommand ReverseThemeCommand { get; }
	public ICommand ReverseWindowStateCommand { get; }
	public ICommand CloseCommand { get; }

	public SettingsViewModel(ISettingsModel model)
	{
		Model = model;

		ReverseThemeCommand = ReactiveCommand.Create(() => Model.IsDark = !Model.IsDark);
	}
}
