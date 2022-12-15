using System;
using System.Globalization;
using System.Windows.Data;

namespace Hockey.Client.Shared.Converter;

public abstract class ConverterBase<T> : MarkupBase<T>, IValueConverter
where T : ConverterBase<T>, new()
{

    public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

    public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
