using System;
using System.Globalization;

namespace Hockey.Client.Shared.Converter;

public class TimeSpanConverter : TwoValuesConverterBase<TimeSpanConverter, long, int>
{
    public override object Convert(long first, int second, Type targetType, object parameter, CultureInfo culture)
    {
        return TimeSpan.FromMilliseconds(first * second);
    }
}
