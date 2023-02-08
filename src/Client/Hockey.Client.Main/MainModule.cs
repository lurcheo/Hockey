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

        regionManager.RegisterViewWithRegion<GuestTeamControl>(Regions.GuestTeam)
                     .RegisterViewWithRegion<HomeTeamControl>(Regions.HomeTeam)
                     .RegisterViewWithRegion<EventsCreatingControl>(Regions.EventsCreating)
                     .RegisterViewWithRegion<EventsControl>(Regions.Events)
                     .RegisterViewWithRegion<EventConstructorControl>(Regions.EventConstructor);
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IMainModel, MainModel>();
        containerRegistry.RegisterSingleton<IGameStoreProvider, GameStoreProvider>();

        containerRegistry.RegisterSingleton<IGameStore>(s => s.Resolve<IGameStoreProvider>()
                                                              .CreateDefault());

        containerRegistry.RegisterSingleton<IHomeTeamModel, HomeTeamModel>()
                         .RegisterSingleton<IGuestTeamModel, GuestTeamModel>()
                         .RegisterSingleton<IEventModel, EventModel>()
                         .RegisterSingleton<IEventConstructorModel, EventConstructorModel>();

        ViewModelLocationProvider.Register<MainControl, MainViewModel>();
        ViewModelLocationProvider.Register<GuestTeamControl, GuestTeamViewModel>();
        ViewModelLocationProvider.Register<HomeTeamControl, HomeTeamViewModel>();
        ViewModelLocationProvider.Register<EventsCreatingControl, EventsViewModel>();
        ViewModelLocationProvider.Register<EventsControl, EventsViewModel>();
        ViewModelLocationProvider.Register<EventConstructorControl, EventConstructorViewModel>();
    }
}
