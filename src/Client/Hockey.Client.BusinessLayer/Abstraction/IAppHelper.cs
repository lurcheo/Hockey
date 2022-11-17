namespace Hockey.Client.BusinessLayer.Abstraction;

public interface IAppHelper
{
    void ChangeScreen(bool full);
    void ChangeTheme(bool isDark);
    void Close();
    void DragMove();
}
