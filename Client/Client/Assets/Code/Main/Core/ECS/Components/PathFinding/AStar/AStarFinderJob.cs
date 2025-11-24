using Core;
using System;
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
    internal struct AGroup
    {
        public int preStep;
        public int last;
        public int next;

        public int index;
    }
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
    internal NativeList<AGroup> groups;
    internal NativeList<AData> datas;
    internal NativeHashSet<int2> targets;
#else
    internal FastList<AGroup> groups;
    internal FastList<AData> datas;
    internal HashSet<int2> targets;
#endif
    public int groupIndex;

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
        do
        {
        next: ref var g = ref groups.ElementAt(groupIndex);
            do
            {
                ref var n = ref datas.ElementAt(g.index);

                if (round == PathFindingRound.R8)
                {
                    //先循环斜向的
                    if (n.xy.x > 0 && n.xy.y > 0)
                    {
                        if (_round(new int2(n.xy.x - 1, n.xy.y - 1), solve, out var moveNext))
                            return;
                        if (moveNext)
                            goto next;
                    }
                    if (n.xy.x > 0 && n.xy.y < size.y - 1)
                    {
                        if (_round(new int2(n.xy.x - 1, n.xy.y + 1), solve, out var moveNext))
                            return;
                        if (moveNext)
                            goto next;
                    }
                    if (n.xy.x < size.x - 1 && n.xy.y > 0)
                    {
                        if (_round(new int2(n.xy.x + 1, n.xy.y - 1), solve, out var moveNext))
                            return;
                        if (moveNext)
                            goto next;
                    }
                    if (n.xy.x < size.x - 1 && n.xy.y < size.y - 1)
                    {
                        if (_round(new int2(n.xy.x + 1, n.xy.y + 1), solve, out var moveNext))
                            return;
                        if (moveNext)
                            goto next;
                    }
                }

                //再循环直线的
                if (n.xy.x > 0)
                {
                    if (_round(new int2(n.xy.x - 1, n.xy.y), solve, out var moveNext))
                        return;
                    if (moveNext)
                        goto next;
                }
                if (n.xy.y > 0)
                {
                    if (_round(new int2(n.xy.x, n.xy.y - 1), solve, out var moveNext))
                        return;
                    if (moveNext)
                        goto next;
                }
                if (n.xy.x < size.x - 1)
                {
                    if (_round(new int2(n.xy.x + 1, n.xy.y), solve, out var moveNext))
                        return;
                    if (moveNext)
                        goto next;
                }
                if (n.xy.y < size.y - 1)
                {
                    if (_round(new int2(n.xy.x, n.xy.y + 1), solve, out var moveNext))
                        return;
                    if (moveNext)
                        goto next;
                }

                g.index = n.next;
            } while (g.index != -1);

            groupIndex = g.next;
            while (groupIndex != -1 && groups[groupIndex].index == -1)
                groupIndex = groups[groupIndex].next;
        } while (groupIndex != -1);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool isEnable(int2 xy)
    {
        int index = xy.y * size.x + xy.x;
        return (data[index].data & 1) == 1 && data[index].Occupation == 0;
    }

#if Native && Burst
    [Unity.Burst.BurstCompile]
#endif
    bool _round(int2 xy, PathFindingSolve solve, out bool moveNext)
    {
        int i = xy.y * size.x + xy.x;
        ref var g = ref groups.ElementAt(groupIndex);
        ref var n = ref datas.ElementAt(g.index);
        ref var grid = ref data.ElementAt(i);
        int cost = n.cost + (grid.data >> 1);
        bool move;
        if (solve == PathFindingSolve.Best)
            move = (grid.vs != vs || n.step + 1 < grid.step) && cost <= power && isEnable(xy);
        else
            move = grid.vs != vs && cost <= power && isEnable(xy);
        moveNext = false;
        if (move)
        {
            grid.vs = vs;
            grid.step = n.step + 1;

            int pre = round == PathFindingRound.R4 ? maths.ManhattanDistance(xy, to) : maths.ManhattanShortDistance(xy, to);
            //乘1.5倍   是为了让启发值略微偏大一点 避免过多的搜索
            pre = n.step + (pre + (pre >> 1)) + 1;

            int index = groupIndex;
            if (groups[index].preStep < pre)
            {
                while (groups[index].next != -1 && groups[index].preStep < pre)
                    index = groups[index].next;
            }
            else if (groups[index].preStep > pre)
            {
                while (groups[index].last != -1 && groups[index].preStep > pre)
                    index = groups[index].last;
            }
            ref var t = ref groups.ElementAt(index);
            if (t.preStep == pre)
            {
                datas.Add(new()
                {
                    xy = xy,
                    link = g.index,
                    cost = cost,
                    next = t.index != -1 ? datas[t.index].next : -1,
                    step = n.step + 1
                });
                if (t.index == -1)
                    t.index = datas.Length - 1;
                else
                    datas.ElementAt(t.index).next = datas.Length - 1;

                if (moveNext = (pre < g.preStep))
                    groupIndex = index;
            }
            else
            {
                groups.Length++;
                ref var pp = ref groups.ElementAt(groups.Length - 1);
                pp.preStep = pre;
                datas.Add(new()
                {
                    xy = xy,
                    link = g.index,
                    cost = cost,
                    next = -1,
                    step = n.step + 1
                });
                pp.index = datas.Length - 1;
                if (t.preStep > pre)
                {
                    pp.last = t.last;
                    pp.next = index;
                    if (pp.last != -1)
                        groups.ElementAt(pp.last).next = groups.Length - 1;
                    t.last = groups.Length - 1;
                }
                else
                {
                    pp.last = index;
                    pp.next = t.next;
                    if (pp.next != -1)
                        groups.ElementAt(pp.next).last = groups.Length - 1;
                    t.next = groups.Length - 1;
                }
                if (moveNext = (pre < g.preStep))
                    groupIndex = groups.Length - 1;
            }

            if (targets.Contains(xy))
                return true;
        }
        return false;
    }
}
