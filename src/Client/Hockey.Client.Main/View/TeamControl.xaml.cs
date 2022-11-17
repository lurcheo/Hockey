using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hockey.Client.Main.View;
/// <summary>
/// Логика взаимодействия для TeamControl.xaml
/// </summary>
public partial class TeamControl : UserControl
{

    public ICommand AddCommand
    {
        get => (ICommand)GetValue(AddCommandProperty);
        set => SetValue(AddCommandProperty, value);
    }

    public static readonly DependencyProperty AddCommandProperty =
        DependencyProperty.Register(nameof(AddCommand), typeof(ICommand), typeof(TeamControl));

    public ICommand RemoveCommand
    {
        get => (ICommand)GetValue(RemoveCommandProperty);
        set => SetValue(RemoveCommandProperty, value);
    }

    public static readonly DependencyProperty RemoveCommandProperty =
        DependencyProperty.Register(nameof(RemoveCommand), typeof(ICommand), typeof(TeamControl));

    public TeamControl()
    {
        InitializeComponent();
    }
}
