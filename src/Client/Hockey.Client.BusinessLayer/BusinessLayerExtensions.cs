using Hockey.Client.BusinessLayer.Abstraction;
using Hockey.Client.BusinessLayer.Implementation;
using Prism.Ioc;

namespace Hockey.Client.BusinessLayer;

public static class BusinessLayerExtensions
{
    public static IContainerRegistry AddBusinessLayer(this IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IVideoService, VideoService>();
        containerRegistry.RegisterSingleton<IAppHelper, AppHelper>();

        return containerRegistry;
    }
}
