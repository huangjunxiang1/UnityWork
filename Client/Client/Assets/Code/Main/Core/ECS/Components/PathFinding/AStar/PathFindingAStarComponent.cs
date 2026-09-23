using Core;
using System;
using System.Collections.Generic;
using Unity.Mathematics;

public enum PathFindingColliderHandle
{
    None,//碰撞之后 不处理
    Cancel,//碰撞之后 取消移动
    FindNewPath,//碰撞之后 修正(重新寻路)
}
public class PathFindingAStarComponent : SComponent
{
    public PathFindingAStarComponent()
    {
        _astar = Game.Data?.Get<AStarData>(false);
        Finder.Init(_astar);
    }

    public int2 Current { get; private set; } = int.MinValue;

    /// <summary>
    /// 单位占用体积
    /// </summary>
    public AStarVolume Volume
    {
        get => _volume;
        set
        {
            value ??= AStarVolume.Empty;
            if (value == _volume) return;
            if (_astar.isInScope(Current))
            {
                _volume.Remove(_astar, Current);
                value.Add(_astar, Current);
            }
            _volume = value;
        }
    }
    public AStarData AStar
    {
        get => _astar;
        set
        {
            value ??= AStarData.Empty;
            if (value == _astar) return;
            if (_astar.isInScope(Current))
                _volume.Remove(_astar, Current);
            if (value.isInScope(Current))
                _volume.Add(value, Current);
            if (this.ShowGrid)
                this._viewGrid(false);
            _astar = value;
            Finder.Init(value);
        }
    }

    public AStarFinder Finder { get; } = new();

    /// <summary>
    /// 设置此参数的  最多不能超过16个
    /// </summary>
    public bool ShowGrid
    {
        get => _showGrid;
        set
        {
            if (_showGrid == value)
                return;
            _showGrid = value;
            _viewGrid(value);
        }
    }

    public FastList<float3> point_float = new();
    public FastList<int2> point_int = new();

    AStarData _astar;
    AStarVolume _volume = AStarVolume.Empty;
    int _callVersion;
    bool _showGrid = false;

    byte finalPointMask = 0;//mask    1 is point     2 is quaternion
    float3 finalPoint;
    quaternion finalQuaternion;


    public bool Finding(int2 to,
        int power = int.MaxValue,
        int near = 0,
        AStarVolume targetVolume = null,
        PathFindingMethod algorithm = PathFindingMethod.AStar,
        PathFindingRound round = PathFindingRound.R4,
        PathFindingSolve solve = PathFindingSolve.Best)
    {
        //暂时移除占用标记  以便于搜索
        Volume.Remove(_astar, Current);
        //cancel view grid
        if (_showGrid)
            this._viewGrid(false);

        try
        {
            return Finder.Finding(this.Current, to, power, near, targetVolume, algorithm, round, solve);
        }
        catch (Exception ex)
        {
            Loger.Error(ex);
        }
        finally
        {
            Volume.Add(_astar, Current);
            //show view grid
            if (_showGrid)
                this._viewGrid(true);
        }

        return false;
    }
    public SValueTask<bool> Goto(int2 to,
        int power = int.MaxValue,
        int near = 0,
        AStarVolume targetVolume = null,
        PathFindingMethod algorithm = PathFindingMethod.AStar,
        PathFindingRound round = PathFindingRound.R4,
        PathFindingSolve solve = PathFindingSolve.Best,
        PathFindingColliderHandle colliderHandle = PathFindingColliderHandle.None,
        MoveStyle style = MoveStyle.CatmullRom)
    {
        this.finalPointMask = 0;
        return _Goto(to, power, near, targetVolume, algorithm, round, solve, colliderHandle, style);
    }

    public SValueTask<bool> Goto(int2 to,
        quaternion quaternion,
        int power = int.MaxValue,
        int near = 0,
        AStarVolume targetVolume = null,
        PathFindingMethod algorithm = PathFindingMethod.AStar,
        PathFindingRound round = PathFindingRound.R4,
        PathFindingSolve solve = PathFindingSolve.Best,
        PathFindingColliderHandle colliderHandle = PathFindingColliderHandle.None,
        MoveStyle style = MoveStyle.CatmullRom)
    {
        this.finalPointMask = 2;
        this.finalQuaternion = quaternion;
        return _Goto(to, power, near, targetVolume, algorithm, round, solve, colliderHandle, style);
    }

    public SValueTask<bool> Goto(float3 point,
        int power = int.MaxValue,
        int near = 0,
        AStarVolume targetVolume = null,
        PathFindingMethod algorithm = PathFindingMethod.AStar,
        PathFindingRound round = PathFindingRound.R4,
        PathFindingSolve solve = PathFindingSolve.Best,
        PathFindingColliderHandle colliderHandle = PathFindingColliderHandle.None,
        MoveStyle style = MoveStyle.CatmullRom)
    {
        this.finalPointMask = 1;
        this.finalPoint = point;
        return _Goto(_astar.GetXY(point), power, near, targetVolume, algorithm, round, solve, colliderHandle, style);
    }

    public SValueTask<bool> Goto(float3 point,
        quaternion quaternion,
        int power = int.MaxValue,
        int near = 0,
        AStarVolume targetVolume = null,
        PathFindingMethod algorithm = PathFindingMethod.AStar,
        PathFindingRound round = PathFindingRound.R4,
        PathFindingSolve solve = PathFindingSolve.Best,
        PathFindingColliderHandle colliderHandle = PathFindingColliderHandle.None,
        MoveStyle style = MoveStyle.CatmullRom)
    {
        this.finalPointMask = 1 | 2;
        this.finalPoint = point;
        this.finalQuaternion = quaternion;
        return _Goto(_astar.GetXY(point), power, near, targetVolume, algorithm, round, solve, colliderHandle, style);
    }

    async SValueTask<bool> _Goto(int2 to,
        int power = int.MaxValue,
        int near = 0,
        AStarVolume targetVolume = null,
        PathFindingMethod algorithm = PathFindingMethod.AStar,
        PathFindingRound round = PathFindingRound.R4,
        PathFindingSolve solve = PathFindingSolve.Best,
        PathFindingColliderHandle colliderHandle = PathFindingColliderHandle.None,
        MoveStyle style = MoveStyle.CatmullRom)
    {
        var move = this.Entity.GetComponent<MoveToComponent>();
        if (move == null)
            return false;
        var vs = ++_callVersion;
        if (colliderHandle == PathFindingColliderHandle.None)
        {
            if (this.Finding(to, power, near, targetVolume, algorithm, round, solve))
            {
                point_float.Clear();
                this.GetFindingPoints(point_float);
                if (point_float.Count > 0)
                {
                    if ((finalPointMask & 2) == 0)
                        return await move.MoveToAsync(point_float, 0, point_float.Count - 1, style);
                    else
                        return await move.MoveToAsync(point_float, finalQuaternion, 0, point_float.Count - 1, style);
                }
            }
        }
        else
        {
            do
            {
                if (this.Finding(to, power, near, targetVolume, algorithm, round, solve))
                {
                    point_float.Clear();
                    this.GetFindingPoints(point_float);
                    for (int i = 0; i < point_float.Count; i++)
                    {
                        var p = point_float[i];
                        int2 xy = _astar.GetXY(p);
                        if (_astar.isEnableExceptSelfVolume(xy, Volume, Current))
                        {
                            this.SetPoint(xy, false);
                            if (i < point_float.Count - 1 || (finalPointMask & 2) == 0)
                                await move.MoveToAsync(p);
                            else
                                await move.MoveToAsync(p, finalQuaternion);
                            if (vs != _callVersion) return false;
                            /*for (int j = i + 1; j < len; j++)
                            {
                                var xy2 = this.finder.AStar.GetXY(this.point_float[j]);
                                if (finder.AStar.isEnableExceptSelfVolume(xy2, Volume, Current))
                                    goto next;
                            }*/
                        }
                        else
                        {
                            switch (colliderHandle)
                            {
                                case PathFindingColliderHandle.Cancel:
                                    return false;
                                default:
                                    goto next;//等待下一次循环重新寻路
                            }
                        }
                    }
                    return true;
                }
            next: await SValueTask.Delay(100);
            } while (vs == _callVersion);
        }

        return false;
    }

    public void Stop()
    {
        ++_callVersion;
        this.Entity.GetComponent<MoveToComponent>()?.Stop();
    }

    public void GetFindingPoints(FastList<float3> ret)
    {
        if (!Finder.findResult) return;
        var t = Finder.job.nodes[Finder.job.nodes.Length - 1];
        int len = t.step + 1;
        int index = ret.Count;
        while (true)
        {
            ret.Add(_astar.GetPosition(t.xy));
            if (t.link == -1)
                break;
            t = Finder.job.nodes[t.link];
        }
        ret.Reverse(index, ret.Count - index);
        var c = this.Entity.GetComponent<TransformComponent>();
        if (c != null)
            ret[0] = c.position;
        if ((this.finalPointMask & 1) != 0)
            ret[len - 1] = this.finalPoint;
    }

    public void SetPoint(float3 point, bool setPosition = true)
    {
        this.SetPoint(_astar.GetXY(point), false);
        if (setPosition)
        {
            var t = Entity.GetComponent<TransformComponent>();
            if (t != null)
            {
                t.position = point;
                t.SetChange();
            }
        }
    }
    public void SetPoint(int2 xy, bool setPosition = true)
    {
        var old = this.Current;
        if (old.Equals(xy)) return;
        if (_astar.isInScope(old))
            Volume.Remove(_astar, old);
        if (_astar.isInScope(xy))
        {
            if (_astar.data[xy.y * _astar.size.x + xy.x].Occupation < 255)
            {
                Volume.Add(_astar, xy);
                this.Current = xy;
            }
            else
            {
                Loger.Error("Occupation > 255");
                this.Current = int.MinValue;
            }
        }
        else
            this.Current = int.MinValue;
        if (setPosition)
        {
            var t = Entity.GetComponent<TransformComponent>();
            if (t != null)
            {
                t.position = _astar.GetPosition(xy);
                t.SetChange();
            }
        }
    }

    void _viewGrid(bool show)
    {
        if (!Finder.findResult) return;
        this.point_int.Clear();
        Finder.GetGrids(this.point_int);
        if (this.point_int.Count > 0)
        {
            for (int i = 0; i < this.point_int.Count; i++)
                _astar.SetPathOccupation(this.point_int[i], show, true);
        }
        for (int i = 0; i < Finder.job.nodes.Length; i++)
            _astar.SetPathOccupation(Finder.job.nodes[i].xy, show, false);
        _astar.ChangeHandle();
    }

    [ChangeSystem]
    static void Change(TransformComponent transform, PathFindingAStarComponent finding)
    {
        finding.SetPoint(finding.AStar.GetXY(transform.position), false);
    }
    [InSystem]
    static void In(TransformComponent transform, PathFindingAStarComponent finding)
    {
        finding.SetPoint(finding._astar.GetXY(transform.position), false);
    }
    [OutSystem]
    static void Out(TransformComponent transform, PathFindingAStarComponent finding)
    {
        ++finding._callVersion;
        finding.Stop();
        if (!finding.Disposed)
            finding.SetPoint((int2)int.MinValue, false);
    }

    [InSystem]
    static void In(PathFindingAStarComponent finding)
    {
        if (finding.ShowGrid)
            finding._viewGrid(true);
    }
    [OutSystem]
    static void Out(PathFindingAStarComponent finding)
    {
        if (finding.ShowGrid)
            finding._viewGrid(false);
        if (finding.Disposed)
            finding.Finder.Dispose();
    }
}