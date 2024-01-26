using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class ParametersArrayCache
{
    public const int Max = 5;
    static object[][] oss = new object[Max][]
    {
        new object[1],
        new object[2],
        new object[3],
        new object[4],
        new object[5],
    };
    public static object[] Get(int num)
    {
        return oss[num - 1];
    }
}
