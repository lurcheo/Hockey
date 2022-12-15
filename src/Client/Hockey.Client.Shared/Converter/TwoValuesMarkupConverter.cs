using System;
using System.Globalization;

namespace Hockey.Client.Shared.Converter;

public abstract class TwoValuesConverterBase<T, TFirst, TSecond> : MultipleConverterBase<T>
    where T : TwoValuesConverterBase<T, TFirst, TSecond>, new()
{
    public abstract object Convert(TFirst first, TSecond second, Type targetType, object parameter, CultureInfo culture);

    public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        TFirst first = values[0] is TFirst ? (TFirst)values[0] : default;
        TSecond second = values[1] is TSecond ? (TSecond)values[1] : default;

        return Convert(first, second, targetType, parameter, culture);
    }
}
