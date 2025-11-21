using System;
using System.Collections.Generic;
using Unity.Mathematics;

public enum PathFindingMethod
{
    AStar,//A星算法
    Breadth,//广度搜索
}
public enum PathFindingRound
{
    R4,//四方位
    R8,//八方位
}
public enum PathFindingSolve
{
    Best,//最优解
    Fast,//最快解
}
public class AStarFinder
{
    public AStarFinder(AStarData astar)
    {
        this.AStar = astar ?? AStarData.Empty;
        for (int i = 0; i < groups.Length; i++)
            groups[i] = new();
    }

    internal class AGroup
    {
        public int preStep;
        public int last;
        public int next;

        public int index;
    }
    internal struct AData
    {
        public int2 xy;
        public int dataIndex;

        public int cost;
        public int next;
        public int step;
    }

    public AStarData AStar { get; internal set; }
    public int2[] point = new int2[10];
    public bool findResult => dataIndex != -1;
    public int dataIndex = -1;

    internal AGroup[] groups = new AGroup[10];
    internal AData[] datas = new AData[100];
    int groudCount;
    int groupIndex;

    int2 to;
    int power;
    PathFindingRound round;

    public bool Finding(int2 from,
            int2 to,
            int power = int.MaxValue,
            int near = 0,
            AStarVolume targetVolume = null,
            PathFindingMethod algorithm = PathFindingMethod.AStar,
            PathFindingRound round = PathFindingRound.R4,
            PathFindingSolve solve = PathFindingSolve.Best)
    {
        if (from.x < 0 || from.y < 0 || from.x >= AStar.width || from.y >= AStar.height)
        {
            Loger.Error(new ArgumentOutOfRangeException());
            return false;
        }
        if (to.x < 0 || to.y < 0 || to.x >= AStar.width || to.y >= AStar.height)
        {
            Loger.Error(new ArgumentOutOfRangeException());
            return false;
        }
        if ((AStar.data[from.y * AStar.width + from.x].data & 1) == 0 || (AStar.data[to.y * AStar.width + to.x].data & 1) == 0)
            return false;

        if (AStar.isFinding)
        {
            Loger.Error("cannot finding in mul thread");
            return false;
        }
        targetVolume ??= AStarVolume.Empty;
        AStar.isFinding = true;

        //乘1.5倍   是为了让启发值略微偏大一点 避免过多的搜索
        int step = round == PathFindingRound.R4 ? maths.ManhattanDistance(from, to) : maths.ManhattanShortDistance(from, to);
        groups[0].preStep = step + (step >> 1);
        groups[0].last = -1;
        groups[0].next = -1;
        groups[0].index = 0;
        datas[0] = new AData
        {
            xy = from,
            dataIndex = -1,

            cost = 0,
            next = -1,
            step = 0,
        };
        groudCount = 1;
        dataIndex = 1;

        if (targetVolume.isNear(from, to, near))
        {
            AStar.isFinding = false;
            this.to = to;
            return true;
        }

        AStar.CheckVersionValue();
        AStar.data[from.y * AStar.width + from.x].vs = ++AStar.vs;
        this.to = to;
        this.power = power;
        this.round = round;
        groupIndex = 0;


        if (algorithm == PathFindingMethod.Breadth)
        {
            AStar.FindTarget(xy => targetVolume.isNear(xy, to, near), from, out var target, round);
        }
        else
        {

            do
            {
            next8: AGroup n = groups[groupIndex];

                do
                {
                    AData now = datas[n.index];

                    if (round == PathFindingRound.R8)
                    {
                        //先循环斜向的
                        if (now.xy.x > 0 && now.xy.y > 0)
                        {
                            if (_round(new int2(now.xy.x - 1, now.xy.y - 1), targetVolume, near, solve, out var moveNext))
                                goto end;
                            if (moveNext)
                                goto next8;
                        }
                        if (now.xy.x > 0 && now.xy.y < AStar.height - 1)
                        {
                            if (_round(new int2(now.xy.x - 1, now.xy.y + 1), targetVolume, near, solve, out var moveNext))
                                goto end;
                            if (moveNext)
                                goto next8;
                        }
                        if (now.xy.x < AStar.width - 1 && now.xy.y > 0)
                        {
                            if (_round(new int2(now.xy.x + 1, now.xy.y - 1), targetVolume, near, solve, out var moveNext))
                                goto end;
                            if (moveNext)
                                goto next8;
                        }
                        if (now.xy.x < AStar.width - 1 && now.xy.y < AStar.height - 1)
                        {
                            if (_round(new int2(now.xy.x + 1, now.xy.y + 1), targetVolume, near, solve, out var moveNext))
                                goto end;
                            if (moveNext)
                                goto next8;
                        }
                    }

                    //再循环直线的
                    if (now.xy.x > 0)
                    {
                        if (_round(new int2(now.xy.x - 1, now.xy.y), targetVolume, near, solve, out var moveNext))
                            goto end;
                        if (moveNext)
                            goto next8;
                    }
                    if (now.xy.y > 0)
                    {
                        if (_round(new int2(now.xy.x, now.xy.y - 1), targetVolume, near, solve, out var moveNext))
                            goto end;
                        if (moveNext)
                            goto next8;
                    }
                    if (now.xy.x < AStar.width - 1)
                    {
                        if (_round(new int2(now.xy.x + 1, now.xy.y), targetVolume, near, solve, out var moveNext))
                            goto end;
                        if (moveNext)
                            goto next8;
                    }
                    if (now.xy.y < AStar.height - 1)
                    {
                        if (_round(new int2(now.xy.x, now.xy.y + 1), targetVolume, near, solve, out var moveNext))
                            goto end;
                        if (moveNext)
                            goto next8;
                    }

                    n.index = datas[n.index].next;
                } while (n.index != -1);

                groupIndex = n.next;
                while (groupIndex != -1 && groups[groupIndex].index == -1)
                    groupIndex = groups[groupIndex].next;
            } while (groupIndex != -1);
        }

    end: AStar.isFinding = false;

        if (targetVolume.isNear(datas[dataIndex - 1].xy, to, near))
            return true;
        dataIndex = -1;
        return false;
    }

    bool _round(int2 xy, AStarVolume targetVolume, int near, PathFindingSolve solve, out bool moveNext)
    {
        int i = xy.y * AStar.width + xy.x;
        var p = groups[groupIndex];
        var t = datas[p.index];
        int cost = t.cost + (AStar.data[i].data >> 1);
        bool move;
        if (solve == PathFindingSolve.Best)
            move = (AStar.data[i].vs != AStar.vs || t.step + 1 < AStar.data[i].step) && cost <= power && AStar.isEnable(xy);
        else
            move = AStar.data[i].vs != AStar.vs && cost <= power && AStar.isEnable(xy);
        moveNext = false;
        if (move)
        {
            AStar.data[i].vs = AStar.vs;

            int pre = round == PathFindingRound.R4 ? maths.ManhattanDistance(xy, to) : maths.ManhattanShortDistance(xy, to);
            //乘1.5倍   是为了让启发值略微偏大一点 避免过多的搜索
            pre = t.step + (pre + (pre >> 1)) + 1;
            if (solve == PathFindingSolve.Best)
                AStar.data[i].step = t.step + 1;

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
            var tmp = groups[index];
            if (dataIndex >= datas.Length)
                Array.Resize(ref datas, dataIndex * 2);
            if (tmp.preStep == pre)
            {
                datas[dataIndex++] = new AData
                {
                    xy = xy,
                    dataIndex = p.index,
                    cost = cost,
                    next = tmp.index != -1 ? datas[tmp.index].next : -1,
                    step = t.step + 1
                };
                if (tmp.index == -1)
                    tmp.index = dataIndex - 1;
                else
                    datas[tmp.index].next = dataIndex - 1;

                if (moveNext = (pre < p.preStep))
                    groupIndex = index;
            }
            else
            {
                if (groudCount >= groups.Length)
                {
                    Array.Resize(ref groups, groudCount * 2);
                    for (int j = groudCount; j < groups.Length; j++)
                        groups[j] = new();
                }
                var pp = groups[groudCount++];
                pp.preStep = pre;
                datas[dataIndex++] = new AData
                {
                    xy = xy,
                    dataIndex = p.index,
                    cost = cost,
                    next = -1,
                    step = t.step + 1
                };
                pp.index = dataIndex - 1; 
                if (tmp.preStep > pre)
                {
                    pp.last = tmp.last;
                    pp.next = index;
                    if (pp.last != -1)
                        groups[pp.last].next = groudCount - 1;
                    tmp.last = groudCount - 1;
                }
                else
                {
                    pp.last = index;
                    pp.next = tmp.next;
                    if (pp.next != -1)
                        groups[pp.next].last = groudCount - 1;
                    tmp.next = groudCount - 1;
                }
                if (moveNext = (pre < p.preStep))
                    groupIndex = groudCount - 1;
            }

            if (targetVolume.isNear(xy, to, near))
                return true;
        }
        return false;
    }

    public int2[] GetGrids()
    {
        if (!findResult) return Array.Empty<int2>();
        var t = datas[dataIndex - 1];
        var array = new int2[t.step + 1];
        while (true)
        {
            array[t.step] = t.xy;
            if (t.dataIndex == -1)
                break;
            t = datas[t.dataIndex];
        }
        return array;
    }
    public void GetGrids(List<int2> ret)
    {
        if (!findResult) return;
        var t = datas[dataIndex - 1];
        int index = ret.Count;
        while (true)
        {
            ret.Add(t.xy);
            if (t.dataIndex == -1)
                break;
            t = datas[t.dataIndex];
        }
        ret.Reverse(index, ret.Count - index);
    }
    public int GetGrids(ref int2[] ret)
    {
        if (!findResult) return 0;
        var t = datas[dataIndex - 1];
        if (ret == null || ret.Length < t.step + 1)
            ret = new int2[t.step + 1];
        int len = t.step + 1;
        while (true)
        {
            ret[t.step] = t.xy;
            if (t.dataIndex == -1)
                break;
            t = datas[t.dataIndex];
        }
        return len;
    }
}
