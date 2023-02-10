using Hockey.Client.BusinessLayer.Abstraction;
using MaterialDesignThemes.Wpf;

namespace Hockey.Client.BusinessLayer.Implementation;

internal class AppHelper : IAppHelper
{
    private readonly PaletteHelper _paletteHelper = new();

    public void ChangeTheme(bool isDark)
    {
        var theme = _paletteHelper.GetTheme();
        theme.SetBaseTheme(isDark ? Theme.Dark : Theme.Light);
        _paletteHelper.SetTheme(theme);
    }
}
