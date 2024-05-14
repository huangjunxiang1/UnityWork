using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

internal static class ArrayCache
{
    public const int Max = 5;

    class Cache<T>
    {
        static ConcurrentDictionary<int, T[][]> safeDic = new();
        internal static T[] Get(int index)
        {
            int id = Environment.CurrentManagedThreadId;
            if (!safeDic.TryGetValue(id, out var arr))
            {
                safeDic[id] = arr = new T[Max][]
                {
                    new T[1],
                    new T[2],
                    new T[3],
                    new T[4],
                    new T[5],
                };
            }
            return arr[index];
        }
    }

    public static T[] Get<T>(int num) => Cache<T>.Get(num - 1);
}
