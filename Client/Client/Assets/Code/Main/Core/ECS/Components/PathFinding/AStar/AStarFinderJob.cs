using Core;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

#if Native
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
#endif

#if Native && Burst
[Unity.Burst.BurstCompile]
#endif
public struct AStarFinderJob
#if Native
    : Unity.Jobs.IJob
#endif
{
    internal struct AData2
    {
        public int2 xy;
        public int link;

        public int cost;
        public int next;
        public int step;
    }

#if Native
    public UnsafeList<AStarGrid> data;
#else
    public AStarGrid[] data;
#endif

    public int2 size;

#if Native
    internal NativeList<int> groups;
    internal NativeList<AData2> datas;
    internal NativeHashSet<int2> targets;
#else
    internal FastList<int> groups;
    internal FastList<AData2> datas;
    internal HashSet<int2> targets;
#endif
    public int minStep;

    public int2 to;
    public int power;
    public PathFindingRound round;
    public byte vs;
    public PathFindingSolve solve;

#if Native && Burst
    [Unity.Burst.BurstCompile]
#endif
    public void Execute()
    {
        for (int i = CalWeight(0, minStep) - minStep; i < groups.Length;)
        {
            int index = groups[i];
            if (index == -1)
            {
                i++;
                continue;
            }

            var n = datas.ElementAt(index);
            groups[i] = n.next;
            if (round == PathFindingRound.R8)
            {
                //先循环斜向的
                if (n.xy.x > 0 && n.xy.y > 0)
                {
                    if (_round(new int2(n.xy.x - 1, n.xy.y - 1), solve, index, ref i))
                        return;
                }
                if (n.xy.x > 0 && n.xy.y < size.y - 1)
                {
                    if (_round(new int2(n.xy.x - 1, n.xy.y + 1), solve, index, ref i))
                        return;
                }
                if (n.xy.x < size.x - 1 && n.xy.y > 0)
                {
                    if (_round(new int2(n.xy.x + 1, n.xy.y - 1), solve, index, ref i))
                        return;
                }
                if (n.xy.x < size.x - 1 && n.xy.y < size.y - 1)
                {
                    if (_round(new int2(n.xy.x + 1, n.xy.y + 1), solve, index, ref i))
                        return;
                }
            }

            //再循环直线的
            if (n.xy.x > 0)
            {
                if (_round(new int2(n.xy.x - 1, n.xy.y), solve, index, ref i))
                    return;
            }
            if (n.xy.y > 0)
            {
                if (_round(new int2(n.xy.x, n.xy.y - 1), solve, index, ref i))
                    return;
            }
            if (n.xy.x < size.x - 1)
            {
                if (_round(new int2(n.xy.x + 1, n.xy.y), solve, index, ref i))
                    return;
            }
            if (n.xy.y < size.y - 1)
            {
                if (_round(new int2(n.xy.x, n.xy.y + 1), solve, index, ref i))
                    return;
            }
        }
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool isEnable(int2 xy)
    {
        int index = xy.y * size.x + xy.x;
        return (data[index].data & 1) == 1 && data[index].Occupation == 0;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CalWeight(int step, int distance) => step + (distance << 1);

#if Native && Burst
    [Unity.Burst.BurstCompile]
#endif
    bool _round(int2 xy, PathFindingSolve solve,int nowIdx,ref int i)
    {
        var n = datas.ElementAt(nowIdx);
        ref var grid = ref data.ElementAt(xy.y * size.x + xy.x);
        int cost = n.cost + (grid.data >> 1);
        bool move;
        if (solve == PathFindingSolve.Best)
            move = (grid.vs != vs || n.step + 1 < grid.step) && cost <= power && isEnable(xy);
        else
            move = grid.vs != vs && cost <= power && isEnable(xy);
        if (move)
        {
            grid.vs = vs;
            grid.step = n.step + 1;

            int pre = round == PathFindingRound.R4 ? maths.ManhattanDistance(xy, to) : maths.ManhattanShortDistance(xy, to);
            pre = CalWeight(n.step + 1, pre);

            int len = groups.Length;
            int group = pre - minStep;
            i = math.min(i, group);
            groups.Length = math.max(len, group + 1);
            for (int j = len; j < groups.Length; j++)
                groups[j] = -1;

            datas.Add(new()
            {
                xy = xy,
                link = nowIdx,
                cost = cost,
                next = groups[group],
                step = n.step + 1
            });
            groups[group] = datas.Length - 1;

            if (targets.Contains(xy))
                return true;
        }
        return false;
    }
}
