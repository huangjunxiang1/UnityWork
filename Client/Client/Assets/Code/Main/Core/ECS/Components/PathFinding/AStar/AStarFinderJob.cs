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
    internal struct AData
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
    internal NativeList<AData> nodes;
    internal NativeHashSet<int2> targets;
#else
    internal FastList<int> groups;
    internal FastList<AData> nodes;
    internal HashSet<int2> targets;
#endif
    public int minStep;

    public int2 to;
    public int power;
    public PathFindingRound round;
    public byte vs;
    public PathFindingSolve solve;
    public PathGridType gridType;
    public PathGridHexParityType hexParityType;
    public PathGridHexFacingType hexFacingType;

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

            var n = nodes.ElementAt(index);
            groups[i] = n.next;

            if (gridType == PathGridType.Rect)
            {
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
            else
            {
                if (hexFacingType == PathGridHexFacingType.Up)
                {
                    if (n.xy.x > 0)
                    {
                        if (_round(new int2(n.xy.x - 1, n.xy.y), solve, index, ref i))
                            return;
                    }
                    if (n.xy.x < size.x - 1)
                    {
                        if (_round(new int2(n.xy.x + 1, n.xy.y), solve, index, ref i))
                            return;
                    }
                    int mask = (n.xy.y & 1);
                    int offset = (mask & (1 - (int)hexParityType)) + ((1 - mask) & (int)hexParityType);
                    for (int x = 0; x < 2; x++)
                    {
                        for (int y = 0; y < 3; y += 2)
                        {
                            int2 xy = new int2(n.xy.x + x - offset, n.xy.y - 1 + y);
                            if (xy.x >= 0 && xy.x < size.x && xy.y >= 0 && xy.y < size.y)
                                if (_round(xy, solve, index, ref i))
                                    return;
                        }
                    }
                }
                else
                {
                    if (n.xy.y > 0)
                    {
                        if (_round(new int2(n.xy.x, n.xy.y - 1), solve, index, ref i))
                            return;
                    }
                    if (n.xy.y < size.y - 1)
                    {
                        if (_round(new int2(n.xy.x, n.xy.y + 1), solve, index, ref i))
                            return;
                    }
                    int mask = (n.xy.x & 1);
                    int offset = (mask & (1 - (int)hexParityType)) + ((1 - mask) & (int)hexParityType);
                    for (int x = 0; x < 3; x += 2)
                    {
                        for (int y = 0; y < 2; y++)
                        {
                            int2 xy = new int2(n.xy.x - 1 + x, n.xy.y + y - offset);
                            if (xy.x >= 0 && xy.x < size.x && xy.y >= 0 && xy.y < size.y)
                                if (_round(xy, solve, index, ref i))
                                    return;
                        }
                    }
                }
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CalWeight(int step, int distance) => step + (distance << 1);

#if Native && Burst
    [Unity.Burst.BurstCompile]
#endif
    bool _round(int2 xy, PathFindingSolve solve,int nowIdx,ref int i)
    {
        ref var node = ref nodes.ElementAt(nowIdx);
        ref var grid = ref data.ElementAt(xy.y * size.x + xy.x);
        int cost = node.cost + (grid.data >> 1);
        bool move;
        if (solve == PathFindingSolve.Best)
            move = (grid.vs != vs || node.step + 1 < grid.step) && cost <= power && (grid.data & 1) == 1 && grid.Occupation == 0;
        else
            move = grid.vs != vs && cost <= power && (grid.data & 1) == 1 && grid.Occupation == 0;
        if (move)
        {
            grid.vs = vs;
            grid.step = node.step + 1;

            int pre;
            if (gridType == PathGridType.Rect)
                pre = round == PathFindingRound.R4 ? maths.ManhattanDistance(xy, to) : maths.ManhattanLongDistance(xy, to);
            else
                pre = maths.ManhattanShortDistance(xy, to);
            pre = CalWeight(node.step + 1, pre);

            int len = groups.Length;
            int group = pre - minStep;
            i = math.min(i, group);
            groups.Length = math.max(len, group + 1);
            for (int j = len; j < groups.Length; j++)
                groups[j] = -1;

            nodes.Add(new()
            {
                xy = xy,
                link = nowIdx,
                cost = cost,
                next = groups[group],
                step = node.step + 1
            });
            groups[group] = nodes.Length - 1;

            if (targets.Contains(xy))
                return true;
        }
        return false;
    }
}
