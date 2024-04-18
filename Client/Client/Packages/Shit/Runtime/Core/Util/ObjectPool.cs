using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

public static class ObjectPool
{
    static class Pool<T> where T : class, new()
    {
        static readonly ConcurrentQueue<T> pool = new();
        public static T Get()
        {
            if (pool.TryDequeue(out var t))
                return t;
            return new T();
        }
        public static void Return(T t) => pool.Enqueue(t);
    }

    public static T Get<T>() where T : class, new() => Pool<T>.Get();
    public static void Return<T>(T t) where T : class, new()
    {
        if (t == null) return;
        Pool<T>.Return(t);
    }
}
