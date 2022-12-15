using System;
using System.Windows.Markup;

namespace Hockey.Client.Shared.Converter;
public class MarkupBase<T> : MarkupExtension
where T : MarkupBase<T>, new()
{
    private static readonly T _markup = new();

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return _markup;

    }
}
