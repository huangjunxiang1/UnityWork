using System.Collections.Generic;

public static class ObjectPool<T> where T : class, new()
{
    static readonly Queue<T> pool = new();
    public static T Get()
    {
        if (pool.Count > 0) return pool.Dequeue();
        return new T();
    }
    public static void Return(T t) => pool.Enqueue(t);
}
