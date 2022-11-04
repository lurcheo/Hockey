using Hockey.Client.BusinessLayer.Abstraction;
using ReactiveUI;

namespace Hockey.Client.Shell.ViewModel;

internal class ShellViewModel : ReactiveObject
{
	private readonly IAppHelper _appHelper;

	public ShellViewModel(IAppHelper appHelper)
	{
		_appHelper = appHelper;
	}
}
