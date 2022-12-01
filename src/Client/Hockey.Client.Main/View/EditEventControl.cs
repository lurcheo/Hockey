using Hockey.Client.Main.Model.Events;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hockey.Client.Main.View;

internal class EditEventControl : Control
{
    public EventModel Event
    {
        get => (EventModel)GetValue(EventProperty);
        set => SetValue(EventProperty, value);
    }

    public static readonly DependencyProperty EventProperty =
        DependencyProperty.Register(nameof(Event), typeof(EventModel), typeof(EditEventControl));

    public int MillisecondsPerFrame
    {
        get => (int)GetValue(MillisecondsPerFrameProperty);
        set => SetValue(MillisecondsPerFrameProperty, value);
    }

    public static readonly DependencyProperty MillisecondsPerFrameProperty =
        DependencyProperty.Register(nameof(MillisecondsPerFrame), typeof(int), typeof(EditEventControl));

    public ICommand ShowEventCommand
    {
        get => (ICommand)GetValue(ShowEventCommandProperty);
        set => SetValue(ShowEventCommandProperty, value);
    }

    public static readonly DependencyProperty ShowEventCommandProperty =
        DependencyProperty.Register(nameof(ShowEventCommand), typeof(ICommand), typeof(EditEventControl));

    static EditEventControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(EditEventControl), new FrameworkPropertyMetadata(typeof(EditEventControl)));
    }
}
