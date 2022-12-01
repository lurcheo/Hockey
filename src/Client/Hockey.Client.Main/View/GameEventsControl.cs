using Hockey.Client.Main.Model.Events;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hockey.Client.Main.View;

internal class GameEventsControl : Control
{
    public ObservableCollection<EventModel> Events
    {
        get => (ObservableCollection<EventModel>)GetValue(EventsProperty);
        set => SetValue(EventsProperty, value);
    }

    public static readonly DependencyProperty EventsProperty =
        DependencyProperty.Register(nameof(Events), typeof(ObservableCollection<EventModel>), typeof(GameEventsControl));


    public ICommand EditEventCommand
    {
        get => (ICommand)GetValue(EditEventCommandProperty);
        set => SetValue(EditEventCommandProperty, value);
    }

    public static readonly DependencyProperty EditEventCommandProperty =
        DependencyProperty.Register(nameof(EditEventCommand), typeof(ICommand), typeof(GameEventsControl));

    static GameEventsControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(GameEventsControl), new FrameworkPropertyMetadata(typeof(GameEventsControl)));
    }
}
