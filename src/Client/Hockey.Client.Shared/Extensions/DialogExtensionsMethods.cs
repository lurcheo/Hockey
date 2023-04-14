using Microsoft.Win32;
using System.Linq;

namespace Hockey.Client.Shared.Extensions;

public static class DialogExtensionsMethods
{
    public static bool TryOpenFileDialog(string filter, out string fileName)
    {
        OpenFileDialog saveFileDialog = new();

        saveFileDialog.Filter = filter;

        if (saveFileDialog.ShowDialog() == false)
        {
            fileName = default;
            return false;
        }

        fileName = saveFileDialog.FileName;
        return true;
    }

    public static bool TrySaveFileDialog(string defaultFileName, string filter, out string fileName)
    {
        SaveFileDialog saveFileDialog = new();

        saveFileDialog.Filter = filter;
        saveFileDialog.FileName = defaultFileName;

        if (saveFileDialog.ShowDialog() == false)
        {
            fileName = default;
            return false;
        }

        fileName = saveFileDialog.FileName;
        return true;
    }

    public static string ConcatExtensions(this string name, params string[] extensions)
    {
        string ext = string.Join(';', extensions.Select(x => $"*.{x}"));
        return $"{name} ({ext})|{ext}";
    }
}
