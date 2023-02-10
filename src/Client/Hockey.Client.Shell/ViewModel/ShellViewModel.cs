using Hockey.Client.BusinessLayer.Abstraction;
using ReactiveUI;

namespace Hockey.Client.Shell.ViewModel;

internal class ShellViewModel : ReactiveObject
{
	public IAppHelper AppHelper { get; }

	public ShellViewModel(IAppHelper appHelper)
	{
		AppHelper = appHelper;
	}
}
