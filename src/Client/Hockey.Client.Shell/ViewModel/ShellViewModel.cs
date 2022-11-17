using Hockey.Client.BusinessLayer.Abstraction;
using ReactiveUI;
using System.Windows.Input;

namespace Hockey.Client.Shell.ViewModel;

internal class ShellViewModel : ReactiveObject
{
	public IAppHelper AppHelper { get; }

	public ICommand DragCommand { get; }

	public ShellViewModel(IAppHelper appHelper)
	{
		AppHelper = appHelper;

		DragCommand = ReactiveCommand.Create(AppHelper.DragMove);
	}
}
