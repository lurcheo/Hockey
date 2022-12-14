using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Hockey.Client.Shared.Converter;

public abstract class ConverterBase<T> : MarkupExtension, IValueConverter
where T : ConverterBase<T>, new()
{
    private static readonly T _converter = new();

    public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

    public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return _converter;
    }
}
