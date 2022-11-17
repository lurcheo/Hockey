using Hockey.Client.Settings.Model;
using Hockey.Client.Settings.Model.Abstraction;
using Hockey.Client.Settings.View;
using Hockey.Client.Settings.ViewModel;
using Hockey.Client.Shared.View;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;

namespace Hockey.Client.Settings;

public class SettingsModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
        var regionManager = containerProvider.Resolve<IRegionManager>();
        regionManager.RegisterViewWithRegion<SettingsControl>(GlobalRegions.Settings);
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<ISettingsModel, SettingsModel>();
        ViewModelLocationProvider.Register<SettingsControl, SettingsViewModel>();
    }
}
