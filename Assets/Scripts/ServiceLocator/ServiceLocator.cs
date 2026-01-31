using System;
using System.Collections.Generic;

public class ServiceLocator
{
    public static ServiceLocator Instance => _instance ??= new ServiceLocator();
    private static ServiceLocator _instance;

    private readonly Dictionary<Type, object> _services = new();

    public void Add<TService>(TService service)
    {
        _services.Add(typeof(TService), service);
    }

    public TService Get<TService>()
    {
        return (TService)_services[typeof(TService)];
    }

    public bool HasKey(Type key)
    {
        return _services.ContainsKey(key);
    }
}