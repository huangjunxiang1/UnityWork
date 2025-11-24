using Core;
using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace Game
{
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
            _astar = Client.Data?.Get<AStarData>(false);
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

        public float3[] point_float = new float3[10];
        public int2[] point_int = new int2[10];

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
                return Finder.Finding(this.Current, to, _astar, power, near, targetVolume, algorithm, round, solve);
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
                    int len = this.GetFindingPoints(ref point_float);
                    if (len > 0)
                    {
                        if ((finalPointMask & 2) == 0)
                            return await move.MoveToAsync(point_float, 0, len - 1, style);
                        else
                            return await move.MoveToAsync(point_float, finalQuaternion, 0, len - 1, style);
                    }
                }
            }
            else
            {
                do
                {
                    if (this.Finding(to, power, near, targetVolume, algorithm, round, solve))
                    {
                        int len = this.GetFindingPoints(ref point_float);
                        for (int i = 0; i < len; i++)
                        {
                            var p = point_float[i];
                            int2 xy = _astar.GetXY(p);
                            if (_astar.isEnableExceptSelfVolume(xy, Volume, Current))
                            {
                                this.SetPoint(xy, false);
                                if (i < len - 1 || (finalPointMask & 2) == 0)
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

        public float3[] GetFindingPoints()
        {
            if (!Finder.findResult) return Array.Empty<float3>();
            var t = Finder.job.datas[Finder.job.datas.Length - 1];
            int len = t.step + 1;
            var ret = new float3[len];
            while (true)
            {
                ret[t.step] = _astar.GetPosition(t.xy);
                if (t.link == -1)
                    break;
                t = Finder.job.datas[t.link];
            }
            var c = this.Entity.GetComponent<TransformComponent>();
            if (c != null)
                ret[0] = c.position;
            if ((this.finalPointMask & 1) != 0)
                ret[len - 1] = this.finalPoint;
            return ret;
        }
        public void GetFindingPoints(List<float3> ret)
        {
            if (!Finder.findResult) return;
            var t = Finder.job.datas[Finder.job.datas.Length - 1];
            int len = t.step + 1;
            int index = ret.Count;
            while (true)
            {
                ret.Add(_astar.GetPosition(t.xy));
                if (t.link == -1)
                    break;
                t = Finder.job.datas[t.link];
            }
            ret.Reverse(index, ret.Count - index);
            var c = this.Entity.GetComponent<TransformComponent>();
            if (c != null)
                ret[0] = c.position;
            if ((this.finalPointMask & 1) != 0)
                ret[len - 1] = this.finalPoint;
        }
        public int GetFindingPoints(ref float3[] ret)
        {
            if (!Finder.findResult) return 0;
            var t = Finder.job.datas[Finder.job.datas.Length - 1];
            int len = t.step + 1;
            if (ret == null || ret.Length < len)
                Array.Resize(ref ret, len);
            while (true)
            {
                ret[t.step] = _astar.GetPosition(t.xy);
                if (t.link == -1)
                    break;
                t = Finder.job.datas[t.link];
            }
            var c = this.Entity.GetComponent<TransformComponent>();
            if (c != null)
                ret[0] = c.position;
            if ((this.finalPointMask & 1) != 0)
                ret[len - 1] = this.finalPoint;
            return len;
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
                if (_astar.data[xy.y * _astar.width + xy.x].Occupation < 255)
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
            int len = Finder.GetGrids(ref this.point_int);
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                    _astar.SetPathOccupation(this.point_int[i], show, true);
            }
            for (int i = 0; i < Finder.job.datas.Length; i++)
                _astar.SetPathOccupation(Finder.job.datas[i].xy, show, false);
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
}
