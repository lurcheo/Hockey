using System;
using System.Globalization;

namespace Hockey.Client.Shared.Converter;
public class BooleanTextConverter : ConverterBase<BooleanTextConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var words = parameter.ToString().Split("___");

        return ((bool)value) ? words[0] : words[1];
    }
}
