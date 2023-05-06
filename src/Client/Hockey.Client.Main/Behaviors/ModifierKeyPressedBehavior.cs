using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hockey.Client.Main.Behaviors;

internal class ModifierKeyPressedBehavior : Behavior<ComboBox>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.KeyUp += AssociatedObjectKeyUp;
    }

    private void AssociatedObjectKeyUp(object sender, KeyEventArgs e)
    {
        AssociatedObject.SelectedItem = (e.Key == Key.System ? e.SystemKey : e.Key) switch
        {
            Key.LeftCtrl or Key.RightCtrl => ModifierKeys.Control,
            Key.LeftShift or Key.RightShift => ModifierKeys.Shift,
            _ => ModifierKeys.None
        };

        e.Handled = true;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.KeyUp -= AssociatedObjectKeyUp;
    }
}
