using Hockey.Client.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hockey.Client.Shared.Extensions;
public static class ManualExtensionsMethods
{
    public static IReadOnlyDictionary<T, int> GetIdDictionary<T>(this IEnumerable<T> items)
    {
        return items.Select((item, i) => (item, id: i + 1))
                    .ToDictionary(x => x.item, x => x.id);
    }

    public static IEnumerable<TConvert> GetIdDictionaryFromDto<TDto, TConvert>(this IEnumerable<TDto> items,
                                                                               Func<TDto, TConvert> func,
                                                                               out IReadOnlyDictionary<int, TConvert> dictionary)
        where TDto : BaseDto
    {
        var fullItems = items.Select(x => (id: x.Id, item: func(x))).ToArray();

        dictionary = fullItems.ToDictionary(x => x.id, x => x.item);
        return fullItems.Select(x => x.item).ToArray();
    }
}
