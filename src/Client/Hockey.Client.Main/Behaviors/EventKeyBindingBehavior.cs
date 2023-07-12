using Hockey.Client.Main.Model.Data.Events;
using Microsoft.Xaml.Behaviors;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Hockey.Client.Main.Behaviors;

internal class EventKeyBindingBehavior : Behavior<UIElement>
{
    public IEnumerable<EventFactory> EventFactories
    {
        get => (IEnumerable<EventFactory>)GetValue(EventFactoriesProperty);
        set => SetValue(EventFactoriesProperty, value);
    }

    public static readonly DependencyProperty EventFactoriesProperty =
        DependencyProperty.Register(nameof(EventFactories), typeof(IEnumerable<EventFactory>), typeof(EventKeyBindingBehavior));

    public ICommand KeyUpCallback
    {
        get => (ICommand)GetValue(KeyUpCallbackProperty);
        set => SetValue(KeyUpCallbackProperty, value);
    }

    public static readonly DependencyProperty KeyUpCallbackProperty =
        DependencyProperty.Register(nameof(KeyUpCallback), typeof(ICommand), typeof(EventKeyBindingBehavior));

    public bool IsEnabled
    {
        get => (bool)GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    public static readonly DependencyProperty IsEnabledProperty =
        DependencyProperty.Register(nameof(IsEnabled), typeof(bool), typeof(EventKeyBindingBehavior), new PropertyMetadata(true));

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.KeyDown += AssociatedObjectKeyDown;
    }


    private void AssociatedObjectKeyDown(object sender, KeyEventArgs e)
    {
        if (!IsEnabled)
        {
            return;
        }

        foreach (var eventFactory in EventFactories)
        {
            if (Keyboard.Modifiers == eventFactory.AdditionalBindingKey
                && eventFactory.BindingKey == e.Key)
            {
                KeyUpCallback.Execute(eventFactory);
            }
        }
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.KeyDown -= AssociatedObjectKeyDown;
    }
}
