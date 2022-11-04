using Hockey.Client.Main.Model;
using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.View;
using Hockey.Client.Main.ViewModel;
using Hockey.Client.Shared.View;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;

namespace Hockey.Client.Main;

public class MainModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
        var regionManager = containerProvider.Resolve<IRegionManager>();

        regionManager.RegisterViewWithRegion<MainControl>(GlobalRegions.Main);
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IMainModel, MainModel>();

        ViewModelLocationProvider.Register<MainControl, MainViewModel>();
    }
}
