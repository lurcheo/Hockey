using Hockey.Client.Main.Model.Events;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hockey.Client.Main.View;

internal class EventsControl : Control
{
    public IEnumerable<EventTypeModel> EventTypes
    {
        get => (IEnumerable<EventTypeModel>)GetValue(EventTypesProperty);
        set => SetValue(EventTypesProperty, value);
    }

    public static readonly DependencyProperty EventTypesProperty =
        DependencyProperty.Register(nameof(EventTypes), typeof(IEnumerable<EventTypeModel>), typeof(EventsControl));

    public ICommand AddEventCommand
    {
        get => (ICommand)GetValue(AddEventCommandProperty);
        set => SetValue(AddEventCommandProperty, value);
    }

    public static readonly DependencyProperty AddEventCommandProperty =
        DependencyProperty.Register(nameof(AddEventCommand), typeof(ICommand), typeof(EventsControl));

    static EventsControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(EventsControl), new FrameworkPropertyMetadata(typeof(EventsControl)));
    }
}
