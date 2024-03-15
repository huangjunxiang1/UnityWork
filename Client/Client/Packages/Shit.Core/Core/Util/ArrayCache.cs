using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class ArrayCache
{
    public const int Max = 5;

    class Cache<T>
    {
        internal static T[][] arrays = new T[Max][]
        {
            new T[1],
            new T[2],
            new T[3],
            new T[4],
            new T[5],
        };
    }

    public static T[] Get<T>(int num) => Cache<T>.arrays[num - 1];
}
