using Hockey.Client.BusinessLayer.Abstraction;
using MaterialDesignThemes.Wpf;

namespace Hockey.Client.BusinessLayer.Implementation;

internal class AppHelper : IAppHelper
{
    private readonly PaletteHelper _paletteHelper = new();

    public void ChangeTheme(bool isDark)
    {
        ITheme theme = _paletteHelper.GetTheme();
        IBaseTheme baseTheme = isDark ? new MaterialDesignDarkTheme() : new MaterialDesignLightTheme();
        theme.SetBaseTheme(baseTheme);
        _paletteHelper.SetTheme(theme);
    }
}
