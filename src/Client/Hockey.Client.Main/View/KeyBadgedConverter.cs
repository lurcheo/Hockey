using Hockey.Client.Shared.Converter;
using System;
using System.Globalization;
using System.Windows.Input;

namespace Hockey.Client.Main.View;

internal class KeyBadgedConverter : TwoValuesConverterBase<KeyBadgedConverter, ModifierKeys, Key>
{
    private KeyConverter keyConverter = new();

    public override object Convert(ModifierKeys first, Key second, Type targetType, object parameter, CultureInfo culture)
    {
        return (first, second) switch
        {
            (_, Key.None) => null,
            (ModifierKeys.None, _) => second.ToString(),
            (_, _) => $"{first} + {second}"
        };
    }
}
