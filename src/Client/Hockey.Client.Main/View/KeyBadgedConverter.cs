using Hockey.Client.Shared.Converter;
using System;
using System.Globalization;
using System.Windows.Input;

namespace Hockey.Client.Main.View;

internal class KeyBadgedConverter : ConverterBase<KeyBadgedConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var key = (Key)value;

        return value switch
        {
            Key.None => null,
            _ => key
        };
    }
}
