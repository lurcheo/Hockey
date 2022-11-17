using Hockey.Client.BusinessLayer.Abstraction;
using MaterialDesignThemes.Wpf;
using System.Windows;

namespace Hockey.Client.BusinessLayer.Implementation;

internal class AppHelper : IAppHelper
{
    private readonly PaletteHelper _paletteHelper = new();
    private readonly Window _window = Application.Current.MainWindow;

    public void ChangeTheme(bool isDark)
    {
        var theme = _paletteHelper.GetTheme();
        theme.SetBaseTheme(isDark ? Theme.Dark : Theme.Light);
        _paletteHelper.SetTheme(theme);
    }

    public void DragMove()
    {
        _window.DragMove();
    }

    public void ChangeScreen(bool full)
    {
        _window.WindowState = full ? WindowState.Maximized : WindowState.Normal;
    }

    public void Close()
    {
        Application.Current.Shutdown();
    }
}
