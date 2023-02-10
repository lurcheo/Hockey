using System.Linq;

namespace Hockey.Client.Shared.Extensions;

public static class DialogExtensionsMethods
{
    public static string ConcatExtensions(this string name, params string[] extensions)
    {
        string ext = string.Join(';', extensions.Select(x => $"*.{x}"));
        return $"{name} ({ext})|{ext}";
    }
}
