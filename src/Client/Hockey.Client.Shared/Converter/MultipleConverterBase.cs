using System;
using System.Globalization;
using System.Windows.Data;

namespace Hockey.Client.Shared.Converter;
public abstract class MultipleConverterBase<T> : MarkupBase<T>, IMultiValueConverter
    where T : MultipleConverterBase<T>, new()
{
    public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

    public virtual object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
