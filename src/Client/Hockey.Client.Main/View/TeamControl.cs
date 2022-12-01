using Hockey.Client.Main.Model;
using System.Windows;
using System.Windows.Controls;

namespace Hockey.Client.Main.View;

internal class TeamControl : Control
{
    public TeamModel Team
    {
        get => (TeamModel)GetValue(TeamProperty);
        set => SetValue(TeamProperty, value);
    }

    public static readonly DependencyProperty TeamProperty =
        DependencyProperty.Register(nameof(Team), typeof(TeamModel), typeof(TeamControl));

    static TeamControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TeamControl), new FrameworkPropertyMetadata(typeof(TeamControl)));
    }
}
