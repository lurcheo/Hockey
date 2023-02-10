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
}
