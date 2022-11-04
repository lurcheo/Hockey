using System;
using System.Collections.Generic;

namespace Hockey.Client.Shared.Extensions;

public static class ObservableExtensionsMethods
{
    private static readonly List<IDisposable> _displosables = new();

    public static IDisposable Cache(this IDisposable disposable)
    {
        _displosables.Add(disposable);
        return disposable;
    }
}
