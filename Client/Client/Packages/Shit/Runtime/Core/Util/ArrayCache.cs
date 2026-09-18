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
        [ThreadStatic]
        static T[][] array;
        internal static T[] Get(int index)
        {
            if (array == null)
            {
                array = new T[Max][];
                for (int i = 0; i < Max; i++)
                    array[i] = new T[i + 1];
            }
            return array[index];
        }
    }

    public static T[] Get<T>(int num) => Cache<T>.Get(num - 1);
}
