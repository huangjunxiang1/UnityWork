using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class ArrayCache<T>
{
    public const int Max = 5;
    static T[][] arrays = new T[Max][]
    {
        new T[1],
        new T[2],
        new T[3],
        new T[4],
        new T[5],
    };
    public static T[] Get(int num)
    {
        return arrays[num - 1];
    }
}
