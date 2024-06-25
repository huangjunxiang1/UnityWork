using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

public static class ObjectPool
{
#if UNITY_EDITOR
    internal static Dictionary<Type, List<object>> p_malloc = new();
    internal static Dictionary<Type, List<object>> p_pool = new();
#endif
    static class Pool<T> where T : class, new()
    {
        static readonly ConcurrentQueue<T> pool = new();
        public static T Get()
        {
            if (!pool.TryDequeue(out T ret))
                ret = new T();
#if UNITY_EDITOR
            lock (p_malloc)
            {
                if (!p_malloc.TryGetValue(typeof(T), out var lst))
                    p_malloc[typeof(T)] = lst = new();
                lst.Add(ret);
            }
            lock (p_pool)
            {
                if (p_pool.TryGetValue(typeof(T), out var lst))
                    lst.Remove(ret);
            }
#endif
            return ret;
        }
        public static void Return(T ret)
        {
#if UNITY_EDITOR
            lock (p_pool)
            {
                if (!p_pool.TryGetValue(typeof(T), out var lst))
                    p_pool[typeof(T)] = lst = new();
                lst.Add(ret);
            }
            lock (p_malloc)
            {
                if (p_malloc.TryGetValue(typeof(T), out var lst))
                    lst.Remove(ret);
            }
#endif
            pool.Enqueue(ret);
        }
    }

    public static T Get<T>() where T : class, new() => Pool<T>.Get();
    public static void Return<T>(T t) where T : class, new()
    {
        if (t == null) return;
        Pool<T>.Return(t);
    }
}
