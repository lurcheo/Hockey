using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hockey.Client.Main.Behaviors;

internal class KeyPressedBehavior : Behavior<ComboBox>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.KeyUp += AssociatedObjectKeyUp;
    }

    private void AssociatedObjectKeyUp(object sender, KeyEventArgs e)
    {
        AssociatedObject.SelectedItem = e.Key;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.KeyUp -= AssociatedObjectKeyUp;
    }
}
