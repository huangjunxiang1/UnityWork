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
    public static int max(int a, int b, int c) => math.max(math.max(a, b), c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int max(int a, int b, int c, int d) => math.max(math.max(a, b), math.max(c, d));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 max(int2 a, int2 b, int2 c) => math.max(math.max(a, b), c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 max(int2 a, int2 b, int2 c, int2 d) => math.max(math.max(a, b), math.max(c, d));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int3 max(int3 a, int3 b, int3 c) => math.max(math.max(a, b), c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int3 max(int3 a, int3 b, int3 c, int3 d) => math.max(math.max(a, b), math.max(c, d));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int4 max(int4 a, int4 b, int4 c) => math.max(math.max(a, b), c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int4 max(int4 a, int4 b, int4 c, int4 d) => math.max(math.max(a, b), math.max(c, d));


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float max(float a, float b, float c) => math.max(math.max(a, b), c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float max(float a, float b, float c, float d) => math.max(math.max(a, b), math.max(c, d));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float2 max(float2 a, float2 b, float2 c) => math.max(math.max(a, b), c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float2 max(float2 a, float2 b, float2 c, float2 d) => math.max(math.max(a, b), math.max(c, d));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3 max(float3 a, float3 b, float3 c) => math.max(math.max(a, b), c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3 max(float3 a, float3 b, float3 c, float3 d) => math.max(math.max(a, b), math.max(c, d));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 max(float4 a, float4 b, float4 c) => math.max(math.max(a, b), c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 max(float4 a, float4 b, float4 c, float4 d) => math.max(math.max(a, b), math.max(c, d));


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int min(int a, int b, int c) => math.min(math.min(a, b), c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int min(int a, int b, int c, int d) => math.min(math.min(a, b), math.min(c, d));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 min(int2 a, int2 b, int2 c) => math.min(math.min(a, b), c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 min(int2 a, int2 b, int2 c, int2 d) => math.min(math.min(a, b), math.min(c, d));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int3 min(int3 a, int3 b, int3 c) => math.min(math.min(a, b), c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int3 min(int3 a, int3 b, int3 c, int3 d) => math.min(math.min(a, b), math.min(c, d));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int4 min(int4 a, int4 b, int4 c) => math.min(math.min(a, b), c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int4 min(int4 a, int4 b, int4 c, int4 d) => math.min(math.min(a, b), math.min(c, d));


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float min(float a, float b, float c) => math.min(math.min(a, b), c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float min(float a, float b, float c, float d) => math.min(math.min(a, b), math.min(c, d));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float2 min(float2 a, float2 b, float2 c) => math.min(math.min(a, b), c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float2 min(float2 a, float2 b, float2 c, float2 d) => math.min(math.min(a, b), math.min(c, d));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3 min(float3 a, float3 b, float3 c) => math.min(math.min(a, b), c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3 min(float3 a, float3 b, float3 c, float3 d) => math.min(math.min(a, b), math.min(c, d));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 min(float4 a, float4 b, float4 c) => math.min(math.min(a, b), c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 min(float4 a, float4 b, float4 c, float4 d) => math.min(math.min(a, b), math.min(c, d));


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
    public static int ManhattanShortDistance(int2 a, int2 b)
    {
        int2 v = math.abs(a - b);
        return math.max(v.x, v.y);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ManhattanShortDistance(int3 a, int3 b)
    {
        int3 v = math.abs(a - b);
        return maths.max(v.x, v.y, v.z);
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ManhattanShortDistance(float2 a, float2 b)
    {
        float2 v = math.abs(a - b);
        return math.max(v.x, v.y);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ManhattanShortDistance(float3 a, float3 b)
    {
        float3 v = math.abs(a - b);
        return maths.max(v.x, v.y, v.z);
    }
}
