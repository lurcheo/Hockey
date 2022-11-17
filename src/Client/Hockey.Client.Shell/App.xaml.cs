using Hockey.Client.BusinessLayer;
using Hockey.Client.Main;
using Hockey.Client.Settings;
using Hockey.Client.Shell.ViewModel;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Unity;
using System.Windows;

namespace Hockey.Client.Shell;

public partial class App : PrismApplication
{
    protected override Window CreateShell()
    {
        return Container.Resolve<View.Shell>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.AddBusinessLayer();

        ViewModelLocationProvider.Register<View.Shell, ShellViewModel>();
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        base.ConfigureModuleCatalog(moduleCatalog);

        moduleCatalog.AddModule<MainModule>();
        moduleCatalog.AddModule<SettingsModule>();
    }
}
