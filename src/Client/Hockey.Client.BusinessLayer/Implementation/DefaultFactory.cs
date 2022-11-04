using Hockey.Client.BusinessLayer.Abstraction;
using System;

namespace Hockey.Client.BusinessLayer.Implementation;

public class DefaultFactory<T> : IFactory<T>
{
    private readonly Func<T> _factory;

    public DefaultFactory(Func<T> factory)
    {
        _factory = factory;
    }

    public T Create()
    {
        return _factory();
    }
}
