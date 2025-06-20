using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

public static class maths
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ManhattanDistance(int2 a, int2 b)
    {
        int2 v = math.abs(a - b);
        return v.x + v.y;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ManhattanDistance(int3 a, int3 b)
    {
        int3 v = math.abs(a - b);
        return v.x + v.y + v.z;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ManhattanDistance(float2 a, float2 b)
    {
        float2 v = math.abs(a - b);
        return v.x + v.y;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ManhattanDistance(float3 a, float3 b)
    {
        float3 v = math.abs(a - b);
        return v.x + v.y + v.z;
    }
}
