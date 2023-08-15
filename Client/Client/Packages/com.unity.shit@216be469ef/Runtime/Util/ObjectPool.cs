using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ObjectPool
{
    static Dictionary<Type, List<object>> pool = new();
    public static T Get<T>() where T : class, new()
    {
        if (pool.TryGetValue(typeof(T), out var value))
        {
            if (value.Count > 0)
            {
                var o = value[^1];
                value.RemoveAt(value.Count - 1);
                return (T)o;
            }
        }
        return new T();
    }
    public static void Return<T>(T t) where T : class, new()
    {
        if (!pool.TryGetValue(typeof(T), out var value))
            pool[typeof(T)] = value = new List<object>();
        value.Add(t);
    }
}
