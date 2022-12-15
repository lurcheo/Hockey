using Prism.Events;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Hockey.Client.Shared.Extensions;

public static class ObservableExtensionsMethods
{
    private static readonly List<IDisposable> _displosables = new();

    public static IDisposable Cache(this IDisposable disposable)
    {
        _displosables.Add(disposable);
        return disposable;
    }

    public static IObservable<T> ToObservable<T>(this PubSubEvent<T> pubSubEvent)
    {
        return Observable.Create<T>(obs => pubSubEvent.Subscribe(obs.OnNext));
    }

    public static IObservable<Unit> ToObservable(this PubSubEvent pubSubEvent)
    {
        return Observable.FromEvent
        (
            act => pubSubEvent.Subscribe(act),
            pubSubEvent.Unsubscribe
        );
    }

    public static IObservable<(NotifyCollectionChangedAction action, T el)> ToObservable<T>(this ObservableCollection<T> collection)
    {
        return Observable.FromEvent<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>
        (
            act => (sender, e) => act(e),
            act => collection.CollectionChanged += act,
            act => collection.CollectionChanged -= act
        )
        .Select(x => x.Action switch
        {
            NotifyCollectionChangedAction.Add => (x.Action, items: x.NewItems),
            NotifyCollectionChangedAction.Remove => (x.Action, items: x.OldItems),
            _ => default
        }).WhereNotNull()
        .SelectMany(x => x.items.Cast<T>().Select(y => (x.Action, y)));
    }

    public static IObservable<T> ToObservable<T>(this ObservableCollection<T> collection, NotifyCollectionChangedAction action)
    {
        return collection.ToObservable()
                         .Where(x => x.action == action)
                         .Select(x => x.el);
    }
}
