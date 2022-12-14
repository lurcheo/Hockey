using System;
using System.Globalization;

namespace Hockey.Client.Shared.Converter;

public class NullableConverter : ConverterBase<NullableConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (parameter?.ToString().Equals("reverse") ?? false) ^ value is null;
    }
}
