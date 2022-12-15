using System;
using System.Globalization;

namespace Hockey.Client.Shared.Converter;

public class ReverseBoolConverter : ConverterBase<ReverseBoolConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !(bool)value;
    }
}
