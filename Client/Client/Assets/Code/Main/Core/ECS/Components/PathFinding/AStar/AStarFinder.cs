using Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

public class AStarFinder
{
    public AStarData astar { get; private set; }
    public bool findResult => job.nodes.Length != 0;

    public AStarFinderJob job;

    public void Init(AStarData astar)
    {
        this.astar = astar;
        job.data = astar.data;
#if Native
        if (!job.groups.IsCreated)
            job.groups = new(10, Unity.Collections.Allocator.Persistent);
        if (!job.nodes.IsCreated)
            job.nodes = new(100, Unity.Collections.Allocator.Persistent);
        if (!job.targets.IsCreated)
            job.targets = new(10, Unity.Collections.Allocator.Persistent);
#else
        job.groups ??= new(10);
        job.datas ??= new(100);
        job.targets ??= new();
#endif
    }
    public bool Finding(int2 from,
                        int2 to,
                        int power = int.MaxValue,
                        int near = 0,
                        AStarVolume targetVolume = null,
                        PathFindingMethod algorithm = PathFindingMethod.AStar,
                        PathFindingRound round = PathFindingRound.R4,
                        PathFindingSolve solve = PathFindingSolve.Best)
    {

        if (from.x < 0 || from.y < 0 || from.x >= this.astar.size.x || from.y >= this.astar.size.y)
        {
            Loger.Error(new ArgumentOutOfRangeException());
            return false;
        }
        if (to.x < 0 || to.y < 0 || to.x >= this.astar.size.x || to.y >= this.astar.size.y)
        {
            Loger.Error(new ArgumentOutOfRangeException());
            return false;
        }
        if ((this.astar.data.ElementAt(from.y * this.astar.size.x + from.x).data & 1) == 0 || (this.astar.data.ElementAt(to.y * this.astar.size.x + to.x).data & 1) == 0)
            return false;

        if (astar.isFinding)
        {
            Loger.Error("cannot finding in mul thread");
            return false;
        }
        targetVolume ??= AStarVolume.Empty;
        job.groups.Clear();
        job.nodes.Clear();

        job.targets.Clear();
        foreach (var item in targetVolume.GetAllNearPoints(this.astar, to, near))
            job.targets.Add(item);
        if (job.targets.Contains(from))
        {
            astar.isFinding = false;
            job.to = to;
            return true;
        }

        astar.CheckVersionValue();
        astar.data.ElementAt(from.y * this.astar.size.x + from.x).vs = ++astar.vs;
        astar.data.ElementAt(from.y * this.astar.size.x + from.x).step = 0;

        astar.isFinding = true;
        bool ret;
        if (algorithm == PathFindingMethod.Breadth)
            ret = astar.FindTarget(xy => job.targets.Contains(xy), from, out var target, round);
        else
        {
            if (astar.gridType == PathGridType.Rect)
                job.minStep = round == PathFindingRound.R4 ? maths.ManhattanDistance(from, to) : maths.ManhattanLongDistance(from, to);
            else
                job.minStep = maths.ManhattanShortDistance(from, to);
            job.nodes.Add(new()
            {
                xy = from,
                link = -1,
                cost = 0,
                next = -1,
                step = 0,
            });

            int distance = job.CalWeight(0, job.minStep);
            for (int i = 0; i < distance - job.minStep + 1; i++)
                job.groups.Add(-1);
            job.groups[distance - job.minStep] = 0;

            job.size = this.astar.size;

            job.to = to;
            job.power = power;
            job.round = round;
            job.vs = astar.vs;
            job.solve = solve;
            job.gridType = astar.gridType;
            job.hexParityType = astar.hexParityType;
            job.hexFacingType = astar.hexFacingType;

#if Native
            Unity.Jobs.IJobExtensions.Schedule(job).Complete();
#else
            job.Execute();
#endif

            ret = job.targets.Contains(job.nodes[job.nodes.Length - 1].xy);
            if (!ret)
                job.nodes.Clear();
        }
        astar.isFinding = false;

        return ret;
    }

    public void GetGrids(FastList<int2> ret)
    {
        if (!findResult) return;
        var t = job.nodes[job.nodes.Length - 1];
        int index = ret.Count;
        while (true)
        {
            ret.Add(t.xy);
            if (t.link == -1)
                break;
            t = job.nodes[t.link];
        }
        ret.Reverse(index, ret.Count - index);
    }

    public void Dispose()
    {
        job.groups.Dispose();
        job.nodes.Dispose();
#if Native
        job.targets.Dispose();
#endif
    }
}
