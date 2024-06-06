using System;
using UnityEngine;
using System.Collections.Concurrent;


public class ServiceLocator 
{
    private static readonly ConcurrentDictionary<Type, Lazy<object>> _services = new();
    
    public static void RegisterService<T>(Func<T> serviceFactory) where T : new()
    {
        var type = typeof(T);
        _services[type] = new Lazy<object>(() => serviceFactory(), true);
        Debug.Log("Service registered: " + type);
    }
    
    public static T GetService<T>() where T : new()
    {
        var type = typeof(T);
        if (_services.TryGetValue(type, out var service))
        {
            return (T)service.Value;
        }
        throw new Exception($"Service of type {type} not found");
    }
    
    public static void UnregisterService<T>()
    {
        var type = typeof(T);
        _services.TryRemove(type, out _);
        Debug.Log("Service unregistered: " + type);
    }
}