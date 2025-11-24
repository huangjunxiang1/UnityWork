using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
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
    public AStarData data { get; private set; }
    public bool findResult => job.datas.Length != 0;

    public AStarFinderJob job;

    public void Init(AStarData astar)
    {
        data = astar;
        job.data = astar.data;
#if Native
        if (!job.groups.IsCreated)
            job.groups = new(10, Unity.Collections.AllocatorManager.Persistent);
        if (!job.datas.IsCreated)
            job.datas = new(100, Unity.Collections.AllocatorManager.Persistent);
        if (!job.targets.IsCreated)
            job.targets = new(10, Unity.Collections.AllocatorManager.Persistent);
#else
        job.groups ??= new(10);
        job.datas ??= new(100);
        job.targets ??= new();
#endif
    }
    public bool Finding(int2 from,
                        int2 to,
                        AStarData astar,
                        int power = int.MaxValue,
                        int near = 0,
                        AStarVolume targetVolume = null,
                        PathFindingMethod algorithm = PathFindingMethod.AStar,
                        PathFindingRound round = PathFindingRound.R4,
                        PathFindingSolve solve = PathFindingSolve.Best)
    {

        if (from.x < 0 || from.y < 0 || from.x >= data.width || from.y >= data.height)
        {
            Loger.Error(new ArgumentOutOfRangeException());
            return false;
        }
        if (to.x < 0 || to.y < 0 || to.x >= data.width || to.y >= data.height)
        {
            Loger.Error(new ArgumentOutOfRangeException());
            return false;
        }
        if ((data.data.ElementAt(from.y * data.width + from.x).data & 1) == 0 || (data.data.ElementAt(to.y * data.width + to.x).data & 1) == 0)
            return false;

        if (astar.isFinding)
        {
            Loger.Error("cannot finding in mul thread");
            return false;
        }
        targetVolume ??= AStarVolume.Empty;
        job.groups.Clear();
        job.datas.Clear();

        //乘1.5倍   是为了让启发值略微偏大一点 避免过多的搜索
        int step = round == PathFindingRound.R4 ? maths.ManhattanDistance(from, to) : maths.ManhattanShortDistance(from, to);
        job.groups.Add(new()
        {
            preStep = step + (step >> 1),
            last = -1,
            next = -1,
            index = 0,
        });
        job.datas.Add(new()
        {
            xy = from,
            link = -1,
            cost = 0,
            next = -1,
            step = 0,
        });

        job.targets.Clear();
        foreach (var item in targetVolume.GetAllNearPoints(data, to, near))
            job.targets.Add(item);
        if (job.targets.Contains(from))
        {
            astar.isFinding = false;
            job.to = to;
            return true;
        }

        astar.CheckVersionValue();
        astar.data.ElementAt(from.y * data.width + from.x).vs = ++astar.vs;
        astar.data.ElementAt(from.y * data.width + from.x).step = 0;

        astar.isFinding = true;
        bool ret;
        if (algorithm == PathFindingMethod.Breadth)
            ret = astar.FindTarget(xy => job.targets.Contains(xy), from, out var target, round);
        else
        {
            job.size = new int2(data.width, data.height);

            job.groupIndex = 0;

            job.to = to;
            job.power = power;
            job.round = round;
            job.vs = astar.vs;
            job.solve = solve;
#if Native
            Unity.Jobs.IJobExtensions.Schedule(job).Complete();
#else
            job.Execute();
#endif

            ret = job.targets.Contains(job.datas[job.datas.Length - 1].xy);
            if (!ret)
                job.datas.Clear();
        }
        astar.isFinding = false;

        return ret;
    }

    public int2[] GetGrids()
    {
        if (!findResult) return Array.Empty<int2>();
        var t = job.datas[job.datas.Length - 1];
        var array = new int2[t.step + 1];
        while (true)
        {
            array[t.step] = t.xy;
            if (t.link == -1)
                break;
            t = job.datas[t.link];
        }
        return array;
    }
    public void GetGrids(List<int2> ret)
    {
        if (!findResult) return;
        var t = job.datas[job.datas.Length - 1];
        int index = ret.Count;
        while (true)
        {
            ret.Add(t.xy);
            if (t.link == -1)
                break;
            t = job.datas[t.link];
        }
        ret.Reverse(index, ret.Count - index);
    }
    public int GetGrids(ref int2[] ret)
    {
        if (!findResult) return 0;
        var t = job.datas[job.datas.Length - 1];
        if (ret == null || ret.Length < t.step + 1)
            ret = new int2[t.step + 1];
        int len = t.step + 1;
        while (true)
        {
            ret[t.step] = t.xy;
            if (t.link == -1)
                break;
            t = job.datas[t.link];
        }
        return len;
    }

    public void Dispose()
    {
        job.groups.Dispose();
        job.datas.Dispose();
#if Native
        job.targets.Dispose();
#endif
    }
}
