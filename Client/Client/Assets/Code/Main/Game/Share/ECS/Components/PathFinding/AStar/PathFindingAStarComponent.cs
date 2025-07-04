using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

namespace Game
{
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
    public class PathFindingAStarComponent : SComponent
    {
        struct FindData
        {
            public int2 xy;
            public int last;
            public int next;
            public int cost;
            public int step;
            public int totalDistance;
        }

        [Sirenix.OdinInspector.ShowInInspector]
        public AStarData AStar { get; set; } = Client.Data?.Get<AStarData>();
        [Sirenix.OdinInspector.ShowInInspector]
        public int2 Current { get; private set; }= int.MinValue;

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
                if (AStar.isInScope(Current))
                {
                    _volume.Remove(AStar, Current);
                    value.Add(AStar, Current);
                }
                _volume = value;
            }
        }

        public float3[] points = new float3[10];

        AStarVolume _volume = AStarVolume.Empty;
        FindData[] paths = new FindData[100];
        int arrayIndex = -1;
        int currentIndex = 0;
        int2 to;
        int power;
        [Sirenix.OdinInspector.ShowInInspector]
        SValueTask<bool> waitTask;
        bool move = false;
        int callVersion;

        byte finalPointMask = 0;//mask    1 is point     2 is quaternion
        float3 finalPoint;
        quaternion finalQuaternion;

        public bool Finding(int2 to, int power = int.MaxValue, int near = 0, AStarVolume targetVolume = null, PathFindingMethod type = PathFindingMethod.AStar, PathFindingRound r = PathFindingRound.R4)
        {
            return _Finding(this.Current, to, power, near, targetVolume, type, r);
        }
        public bool Finding(int2 from, int2 to, int power = int.MaxValue, int near = 0, AStarVolume targetVolume = null, PathFindingMethod type = PathFindingMethod.AStar, PathFindingRound r = PathFindingRound.R4)
        {
            return _Finding(from, to, power, near, targetVolume, type, r);
        }
        public SValueTask<bool> Goto(int2 to,
            int power = int.MaxValue,
            int near = 0,
            AStarVolume targetVolume = null,
            PathFindingMethod type = PathFindingMethod.AStar,
            PathFindingRound r = PathFindingRound.R4)
        {
            this.finalPointMask = 0;
            ++callVersion;
            this._Goto(to, power, near, targetVolume, type, r);
            return waitTask;
        }
        public SValueTask<bool> Goto(int2 to,
            quaternion quaternion,
            int power = int.MaxValue,
            int near = 0,
            AStarVolume targetVolume = null,
            PathFindingMethod type = PathFindingMethod.AStar,
            PathFindingRound r = PathFindingRound.R4)
        {
            this.finalPointMask = 2;
            this.finalQuaternion = quaternion;
            ++callVersion;
            this._Goto(to, power, near, targetVolume, type, r);
            return waitTask;
        }

        public async SValueTask GotoMust(int2 to,
            int delay = 100,
            int power = int.MaxValue,
            int near = 0,
            AStarVolume targetVolume = null,
            PathFindingMethod type = PathFindingMethod.AStar,
            PathFindingRound r = PathFindingRound.R4)
        {
            this.finalPointMask = 0;
            var vs = ++callVersion;
            while (vs == callVersion && !await this._Goto(to, power, near, targetVolume, type, r))
                await SValueTask.Delay(delay);
        }
        public async SValueTask GotoMust(int2 to,
            quaternion quaternion,
            int delay = 100,
            int power = int.MaxValue,
            int near = 0,
            AStarVolume targetVolume = null,
            PathFindingMethod type = PathFindingMethod.AStar,
            PathFindingRound r = PathFindingRound.R4)
        {
            this.finalPointMask = 2;
            this.finalQuaternion = quaternion;
            var vs = ++callVersion;
            while (vs == callVersion && !await this._Goto(to, power, near, targetVolume, type, r))
                await SValueTask.Delay(delay);
        }
        public async SValueTask GotoTimely(int2 to,
            int delay = 100,
            int power = int.MaxValue,
            int near = 0,
            AStarVolume targetVolume = null,
            PathFindingMethod type = PathFindingMethod.AStar,
            PathFindingRound r = PathFindingRound.R4)
        {
            this.finalPointMask = 0;
            var vs = ++callVersion;
            var move = this.Entity.GetComponent<MoveToComponent>();
            if (move == null) return;
            while (vs == callVersion)
            {
                while (!this._Finding(this.Current, to, power, near, targetVolume, type, r))
                {
                    await SValueTask.Delay(delay);
                    if (vs != callVersion || move.Disposed) return;
                }
                int len = this.GetFindingPoints(ref this.points);
                for (int i = 0; i < len; i++)
                {
                    var xy = this.AStar.GetXY(this.points[i]);
                    this.SetPoint(xy, false);
                    await move.MoveToAsync(this.points[i]);
                    if (vs != callVersion || move.Disposed) return;
                    for (int j = i + 1; j < len; j++)
                    {
                        var xy2 = this.AStar.GetXY(this.points[j]);
                        if (!AStar.isEnable(xy2, Volume, Current))
                            goto next;
                    }
                }
            next: continue;
            }
        }
        public async SValueTask GotoTimely(int2 to,
            quaternion quaternion,
            int delay = 100,
            int power = int.MaxValue,
            int near = 0,
            AStarVolume targetVolume = null,
            PathFindingMethod type = PathFindingMethod.AStar,
            PathFindingRound r = PathFindingRound.R4)
        {
            this.finalPointMask = 2;
            this.finalQuaternion = quaternion;
            var vs = ++callVersion;
            var move = this.Entity.GetComponent<MoveToComponent>();
            if (move == null) return;
            while (vs == callVersion)
            {
                while (!this._Finding(this.Current, to, power, near, targetVolume, type, r))
                {
                    await SValueTask.Delay(delay);
                    if (vs != callVersion || move.Disposed) return;
                }
                int len = this.GetFindingPoints(ref this.points);
                for (int i = 0; i < len; i++)
                {
                    var xy = this.AStar.GetXY(this.points[i]);
                    this.SetPoint(xy, false);
                    if (i < len - 1)
                    {
                        await move.MoveToAsync(this.points[i]);
                        if (vs != callVersion || move.Disposed) return;
                        for (int j = i + 1; j < len; j++)
                        {
                            var xy2 = this.AStar.GetXY(this.points[j]);
                            if (!AStar.isEnable(xy2, Volume, Current))
                                goto next;
                        }
                    }
                    else
                    {
                        await move.MoveToAsync(this.points[i], this.finalQuaternion);
                        if (vs != callVersion || move.Disposed) return;
                    }
                }
            next: continue;
            }
        }

        public SValueTask<bool> GotoPoint(float3 point,
            int power = int.MaxValue,
            int near = 0,
            AStarVolume targetVolume = null,
            PathFindingMethod type = PathFindingMethod.AStar,
            PathFindingRound r = PathFindingRound.R4)
        {
            this.finalPointMask = 1;
            this.finalPoint = point;
            ++callVersion;
            this._Goto(this.AStar.GetXY(point), power, near, targetVolume, type, r);
            return waitTask;
        }
        public SValueTask<bool> GotoPoint(float3 point,
            quaternion quaternion,
            int power = int.MaxValue,
            int near = 0,
            AStarVolume targetVolume = null,
            PathFindingMethod type = PathFindingMethod.AStar,
            PathFindingRound r = PathFindingRound.R4)
        {
            this.finalPointMask = 1 | 2;
            this.finalPoint = point;
            this.finalQuaternion = quaternion;
            ++callVersion;
            this._Goto(this.AStar.GetXY(point), power, near, targetVolume, type, r);
            return waitTask;
        }

        public async SValueTask GotoPointMust(float3 point,
           int delay = 100,
           int power = int.MaxValue,
           int near = 0,
           AStarVolume targetVolume = null,
           PathFindingMethod type = PathFindingMethod.AStar,
           PathFindingRound r = PathFindingRound.R4)
        {
            this.finalPointMask = 1;
            this.finalPoint = point;
            var vs = ++callVersion;
            while (vs == callVersion && !await this._Goto(this.AStar.GetXY(point), power, near, targetVolume, type, r))
                await SValueTask.Delay(delay);
        }
        public async SValueTask GotoPointMust(float3 point,
           quaternion quaternion,
           int delay = 100,
           int power = int.MaxValue,
           int near = 0,
           AStarVolume targetVolume = null,
           PathFindingMethod type = PathFindingMethod.AStar,
           PathFindingRound r = PathFindingRound.R4)
        {
            this.finalPointMask = 1 | 2;
            this.finalPoint = point;
            this.finalQuaternion = quaternion;
            var vs = ++callVersion;
            while (vs == callVersion && !await this._Goto(this.AStar.GetXY(point), power, near, targetVolume, type, r))
                await SValueTask.Delay(delay);
        }
        public async SValueTask GotoTimely(float3 to,
           int delay = 100,
           int power = int.MaxValue,
           int near = 0,
           AStarVolume targetVolume = null,
           PathFindingMethod type = PathFindingMethod.AStar,
           PathFindingRound r = PathFindingRound.R4)
        {
            this.finalPointMask = 1;
            this.finalPoint = to;
            var vs = ++callVersion;
            var move = this.Entity.GetComponent<MoveToComponent>();
            if (move == null) return;
            int2 target = this.AStar.GetXY(to);
            while (vs == callVersion)
            {
                while (!this._Finding(this.Current, target, power, near, targetVolume, type, r))
                {
                    await SValueTask.Delay(delay);
                    if (vs != callVersion || move.Disposed) return;
                }
                int len = this.GetFindingPoints(ref this.points);
                for (int i = 0; i < len; i++)
                {
                    var xy = this.AStar.GetXY(this.points[i]);
                    this.SetPoint(xy, false);
                    await move.MoveToAsync(this.points[i]);
                    if (vs != callVersion || move.Disposed) return;
                    for (int j = i + 1; j < len; j++)
                    {
                        var xy2 = this.AStar.GetXY(this.points[j]);
                        if (!AStar.isEnable(xy2, Volume, Current))
                            goto next;
                    }
                }
            next: continue;
            }
        }
        public async SValueTask GotoTimely(float3 to,
            quaternion quaternion,
            int delay = 100,
            int power = int.MaxValue,
            int near = 0,
            AStarVolume targetVolume = null,
            PathFindingMethod type = PathFindingMethod.AStar,
            PathFindingRound r = PathFindingRound.R4)
        {
            this.finalPointMask = 1 | 2;
            this.finalPoint = to;
            this.finalQuaternion = quaternion;
            var vs = ++callVersion;
            var move = this.Entity.GetComponent<MoveToComponent>();
            if (move == null) return;
            int2 target = this.AStar.GetXY(to);
            while (vs == callVersion)
            {
                while (!this._Finding(this.Current, target, power, near, targetVolume, type, r))
                {
                    await SValueTask.Delay(delay);
                    if (vs != callVersion || move.Disposed) return;
                }
                int len = this.GetFindingPoints(ref this.points);
                for (int i = 0; i < len; i++)
                {
                    var xy = this.AStar.GetXY(this.points[i]);
                    this.SetPoint(xy, false);
                    if (i < len - 1)
                    {
                        await move.MoveToAsync(this.points[i]);
                        if (vs != callVersion || move.Disposed) return;
                        for (int j = i + 1; j < len; j++)
                        {
                            var xy2 = this.AStar.GetXY(this.points[j]);
                            if (!AStar.isEnable(xy2, Volume, Current))
                                goto next;
                        }
                    }
                    else
                    {
                        await move.MoveToAsync(this.points[i], this.finalQuaternion);
                        if (vs != callVersion || move.Disposed) return;
                    }
                }
            next: continue;
            }
        }

        public void Stop()
        {
            ++callVersion;
            move = false;
            this.Entity.GetComponent<MoveToComponent>()?.Stop();
            waitTask.TryCancel();
        }

        bool _Finding(int2 from, 
            int2 to, 
            int power = int.MaxValue,
            int near = 0,
            AStarVolume targetVolume = null,
            PathFindingMethod type = PathFindingMethod.AStar,
            PathFindingRound r = PathFindingRound.R4)
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
            if ((AStar.data[from.y * AStar.width + from.x] & 1) == 0 || (AStar.data[to.y * AStar.width + to.x] & 1) == 0)
                return false;

            if (AStar.isFinding)
            {
                Loger.Error("cannot finding in mul thread");
                return false;
            }
            targetVolume ??= AStarVolume.Empty;
            AStar.isFinding = true;
            arrayIndex = 0;
            int pre = math.abs(from.x - to.x) + math.abs(from.y - to.y);
            paths[arrayIndex++] = new FindData { xy = from, last = -1, next = -1, cost = 0, step = 1, totalDistance = pre };
            if (targetVolume.isNear(from, to, near))
            {
                AStar.isFinding = false;
                this.to = to;
                return true;
            }

            if (AStar.vs == byte.MaxValue)
            {
                AStar.vs = 0;
                Array.Clear(AStar.vsArray, 0, AStar.vsArray.Length);
            }
            AStar.vsArray[from.y * AStar.width + from.x] = ++AStar.vs;
            this.to = to;
            this.power = power;
            currentIndex = 0;

            if (type == PathFindingMethod.Breadth)
            {
                if (r == PathFindingRound.R4)
                {
                    do
                    {
                        FindData now = paths[currentIndex];

                        if (now.xy.x > 0)
                            if (breadth(new int2(now.xy.x - 1, now.xy.y), targetVolume, near))
                                break;
                        if (now.xy.y > 0)
                            if (breadth(new int2(now.xy.x, now.xy.y - 1), targetVolume, near))
                                break;
                        if (now.xy.x < AStar.width - 1)
                            if (breadth(new int2(now.xy.x + 1, now.xy.y), targetVolume, near))
                                break;
                        if (now.xy.y < AStar.height - 1)
                            if (breadth(new int2(now.xy.x, now.xy.y + 1), targetVolume, near))
                                break;

                        currentIndex++;
                    } while (currentIndex != arrayIndex);
                }
                else
                {
                    do
                    {
                        FindData now = paths[currentIndex];

                        if (now.xy.x > 0)
                            if (breadth(new int2(now.xy.x - 1, now.xy.y), targetVolume, near))
                                break;
                        if (now.xy.y > 0)
                            if (breadth(new int2(now.xy.x, now.xy.y - 1), targetVolume, near))
                                break;
                        if (now.xy.x < AStar.width - 1)
                            if (breadth(new int2(now.xy.x + 1, now.xy.y), targetVolume, near))
                                break;
                        if (now.xy.y < AStar.height - 1)
                            if (breadth(new int2(now.xy.x, now.xy.y + 1), targetVolume, near))
                                break;

                        if (now.xy.x > 0 && now.xy.y > 0)
                            if (breadth(new int2(now.xy.x - 1, now.xy.y - 1), targetVolume, near))
                                break;
                        if (now.xy.x > 0 && now.xy.y < AStar.height - 1)
                            if (breadth(new int2(now.xy.x - 1, now.xy.y + 1), targetVolume, near))
                                break;
                        if (now.xy.x < AStar.width - 1 && now.xy.y > 0)
                            if (breadth(new int2(now.xy.x + 1, now.xy.y - 1), targetVolume, near))
                                break;
                        if (now.xy.x < AStar.width - 1 && now.xy.y < AStar.height - 1)
                            if (breadth(new int2(now.xy.x + 1, now.xy.y + 1), targetVolume, near))
                                break;

                        currentIndex++;
                    } while (currentIndex != arrayIndex);
                }
            }
            else
            {
                if (r == PathFindingRound.R4)
                {
                    do
                    {
                        FindData now = paths[currentIndex];

                        if (now.xy.x > 0)
                        {
                            if (round4(new int2(now.xy.x - 1, now.xy.y), targetVolume, near))
                                break;
                        }
                        if (now.xy.y > 0)
                        {
                            if (round4(new int2(now.xy.x, now.xy.y - 1), targetVolume, near))
                                break;
                        }
                        if (now.xy.x < AStar.width - 1)
                        {
                            if (round4(new int2(now.xy.x + 1, now.xy.y), targetVolume, near))
                                break;
                        }
                        if (now.xy.y < AStar.height - 1)
                        {
                            if (round4(new int2(now.xy.x, now.xy.y + 1), targetVolume, near))
                                break;
                        }

                        currentIndex = paths[currentIndex].next;
                    } while (currentIndex != -1);
                }
                else
                {
                    do
                    {
                        FindData now = paths[currentIndex];
                        if (now.xy.x > 0)
                            if (round8(new int2(now.xy.x - 1, now.xy.y), targetVolume, near))
                                break;
                        if (now.xy.y > 0)
                            if (round8(new int2(now.xy.x, now.xy.y - 1), targetVolume, near))
                                break;
                        if (now.xy.x < AStar.width - 1)
                            if (round8(new int2(now.xy.x + 1, now.xy.y), targetVolume, near))
                                break;
                        if (now.xy.y < AStar.height - 1)
                            if (round8(new int2(now.xy.x, now.xy.y + 1), targetVolume, near))
                                break;

                        if (now.xy.x > 0 && now.xy.y > 0)
                            if (round8(new int2(now.xy.x - 1, now.xy.y - 1), targetVolume, near))
                                break;
                        if (now.xy.x > 0 && now.xy.y < AStar.height - 1)
                            if (round8(new int2(now.xy.x - 1, now.xy.y + 1), targetVolume, near))
                                break;
                        if (now.xy.x < AStar.width - 1 && now.xy.y > 0)
                            if (round8(new int2(now.xy.x + 1, now.xy.y - 1), targetVolume, near))
                                break;
                        if (now.xy.x < AStar.width - 1 && now.xy.y < AStar.height - 1)
                            if (round8(new int2(now.xy.x + 1, now.xy.y + 1), targetVolume, near))
                                break;
                        currentIndex = paths[currentIndex].next;
                    } while (currentIndex != -1);
                }
            }

            AStar.isFinding = false;

            if (targetVolume.isNear(paths[arrayIndex - 1].xy, to, near))
                return true;
            arrayIndex = -1;
            return false;
        }
        SValueTask<bool> _Goto(int2 to,
            int power = int.MaxValue,
            int near = 0,
            AStarVolume targetVolume = null,
            PathFindingMethod type = PathFindingMethod.AStar,
            PathFindingRound r = PathFindingRound.R4)
        {
            waitTask.TrySetResult(false);
            waitTask = default;
            if (this.Finding(to, power, near, targetVolume, type, r))
            {
                waitTask = SValueTask<bool>.Create();
                move = true;
                this.SetChangeFlag();
            }
            return waitTask;
        }
        bool breadth(int2 xy, AStarVolume targetVolume , int near)
        {
            int i = xy.y * AStar.width + xy.x;
            int cost = paths[currentIndex].cost + (AStar.data[i] >> 1);
            var move = AStar.vsArray[i] != AStar.vs && cost <= power && AStar.isEnable(xy, Volume, Current);
            AStar.vsArray[i] = AStar.vs;
            if (move)
            {
                if (arrayIndex >= paths.Length)
                    Array.Resize(ref paths, arrayIndex * 2);
                paths[arrayIndex++] = new FindData
                {
                    xy = xy,
                    last = currentIndex,
                    cost = cost,
                    step = (paths[currentIndex].step + 1),
                };
                return targetVolume.isNear(xy, to, near);
            }
            return false;
        }
        bool round4(int2 xy, AStarVolume targetVolume , int near)
        {
            int i = xy.y * AStar.width + xy.x;
            int cost = paths[currentIndex].cost + (AStar.data[i] >> 1);
            var move = AStar.vsArray[i] != AStar.vs && cost <= power && AStar.isEnable(xy, Volume, Current);
            AStar.vsArray[i] = AStar.vs;
            if (move)
            {
                if (arrayIndex >= paths.Length)
                    Array.Resize(ref paths, arrayIndex * 2);
                int pre = math.abs(xy.x - to.x) + math.abs(xy.y - to.y);
                paths[arrayIndex++] = new FindData
                {
                    xy = xy,
                    last = currentIndex,
                    next = -1,
                    cost = cost,
                    step = paths[currentIndex].step + 1,
                    totalDistance = paths[currentIndex].step + pre
                };

                if (targetVolume.isNear(xy, to, near))
                    return true;

                int lastIdx = currentIndex;
                int nextIdx = paths[currentIndex].next;
                while (nextIdx != -1 && paths[nextIdx].totalDistance < paths[arrayIndex - 1].totalDistance)
                {
                    lastIdx = nextIdx;
                    nextIdx = paths[nextIdx].next;
                }
                paths[lastIdx].next = arrayIndex - 1;
                paths[arrayIndex - 1].next = nextIdx;
            }
            return false;
        }
        bool round8(int2 xy, AStarVolume targetVolume, int near)
        {
            int i = xy.y * AStar.width + xy.x;
            int cost = paths[currentIndex].cost + (AStar.data[i] >> 1);
            var move = AStar.vsArray[i] != AStar.vs && cost <= power && AStar.isEnable(xy, Volume, Current);
            if (move)
            {
                AStar.vsArray[i] = AStar.vs;
                int pre = math.abs(xy.x - to.x) + math.abs(xy.y - to.y);
                if (arrayIndex >= paths.Length)
                    Array.Resize(ref paths, arrayIndex * 2);
                paths[arrayIndex++] = new FindData
                {
                    xy = xy,
                    last = currentIndex,
                    next = -1,
                    cost = cost,
                    step = paths[currentIndex].step + 1,
                    totalDistance = paths[currentIndex].step + pre
                };

                if (targetVolume.isNear(xy, to, near))
                    return true;

                int lastIdx = currentIndex;
                int nextIdx = paths[currentIndex].next;
                while (nextIdx != -1 && paths[nextIdx].totalDistance < paths[arrayIndex - 1].totalDistance)
                {
                    lastIdx = nextIdx;
                    nextIdx = paths[nextIdx].next;
                }
                paths[lastIdx].next = arrayIndex - 1;
                paths[arrayIndex - 1].next = nextIdx;
            }
            return false;
        }

        public float3[] GetFindingPoints()
        {
            if (arrayIndex == -1) return Array.Empty<float3>();
            var n = this.paths[arrayIndex - 1];
            int len = n.step;
            var ret = new float3[len];
            while (true)
            {
                ret[n.step - 1] = AStar.GetPosition(n.xy);
                if (n.last == -1)
                    break;
                n = this.paths[n.last];
            }
            var t = this.Entity.GetComponent<TransformComponent>();
            if (t != null)
                ret[0] = t.position;
            if ((this.finalPointMask & 1) != 0)
                ret[len - 1] = this.finalPoint;
            return ret;
        }
        public void GetFindingPoints(List<float3> ret)
        {
            if (arrayIndex == -1) return;
            var n = this.paths[arrayIndex - 1];
            int len = n.step;
            int index = ret.Count;
            while (true)
            {
                ret.Add(AStar.GetPosition(n.xy));
                if (n.last == -1)
                    break;
                n = this.paths[n.last];
            }
            ret.Reverse(index, ret.Count - index);
            var t = this.Entity.GetComponent<TransformComponent>();
            if (t != null)
                ret[0] = t.position;
            if ((this.finalPointMask & 1) != 0)
                ret[len - 1] = this.finalPoint;
        }
        public int GetFindingPoints(ref float3[] ret)
        {
            if (arrayIndex == -1) return 0;
            var n = this.paths[arrayIndex - 1];
            int len = n.step;
            if (ret.Length < len)
                Array.Resize(ref ret, len);
            while (true)
            {
                ret[n.step - 1] = AStar.GetPosition(n.xy);
                if (n.last == -1)
                    break;
                n = this.paths[n.last];
            }
            var t = this.Entity.GetComponent<TransformComponent>();
            if (t != null)
                ret[0] = t.position;
            if ((this.finalPointMask & 1) != 0)
                ret[len - 1] = this.finalPoint;
            return len;
        }
        public int2[] GetFindingIDs()
        {
            if (arrayIndex == -1) return Array.Empty<int2>();
            var n = this.paths[arrayIndex - 1];
            var array = new int2[n.step];
            while (true)
            {
                array[n.step - 1] = n.xy;
                if (n.last == -1)
                    break;
                n = this.paths[n.last];
            }
            return array;
        }
        public void GetFindingIDs(List<int2> ret)
        {
            if (arrayIndex == -1) return;
            var n = this.paths[arrayIndex - 1];
            int index = ret.Count;
            while (true)
            {
                ret.Add(n.xy);
                if (n.last == -1)
                    break;
                n = this.paths[n.last];
            }
            ret.Reverse(index, ret.Count - index);
        }
        public int GetFindingIDs(ref int2[] ret)
        {
            if (arrayIndex == -1) return 0;
            var n = this.paths[arrayIndex - 1];
            if (ret.Length < n.step)
                ret = new int2[n.step];
            int len = n.step;
            while (true)
            {
                ret[n.step - 1] = n.xy;
                if (n.last == -1)
                    break;
                n = this.paths[n.last];
            }
            return len;
        }

        public void SetPoint(float3 point, bool setPosition = true)
        {
            this.SetPoint(this.AStar.GetXY(point), false);
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
            var last = this.Current;
            if (last.Equals(xy)) return;
            if (!last.Equals(int.MinValue))
                Volume.Remove(AStar, last);
            if (AStar.isInScope(xy))
            {
                if (AStar.Occupation[xy.y * AStar.width + xy.x] < 255)
                {
                    Volume.Add(AStar, xy);
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
                    t.position = AStar.GetPosition(xy);
                    t.SetChange();
                }
            }
        }

        [ChangeSystem]
        static async void Change(PathFindingAStarComponent finding, MoveToComponent move)
        {
            if (!finding.move) return;
            finding.move = false;
            var task = finding.waitTask;
            finding.waitTask = default;
            if (finding.arrayIndex != -1)
            {
                int len = finding.GetFindingPoints(ref finding.points);
                int v = finding.finalPointMask;
                finding.finalPointMask = 0;
                quaternion r = finding.finalQuaternion;
                for (int i = 0; i < len; i++)
                {
                    var to = finding.AStar.GetXY(finding.points[i]);
                    if (finding.AStar.isEnable(to, finding.Volume, finding.Current))
                    {
                        finding.SetPoint(to, false);
                        if (i < len - 1 || (v & 2) == 0)
                        {
                            if (!await move.MoveToAsync(finding.points[i]))
                            {
                                task.TrySetResult(false);
                                return;
                            }
                        }
                        else
                        {
                            if (!await move.MoveToAsync(finding.points[i], r))
                            {
                                task.TrySetResult(false);
                                return;
                            }
                        }
                    }
                    else
                    {
                        task.TrySetResult(false);
                        return;
                    }
                }
                task.TrySetResult(true);
            }
            else
                task.TrySetResult(false);
        }
        [InSystem]
        static void In(TransformComponent transform, PathFindingAStarComponent finding)
        {
            if (finding.AStar == null) return;
            finding.SetPoint(finding.AStar.GetXY(transform.position), false);
        }
        [OutSystem]
        static void Out(TransformComponent transform, PathFindingAStarComponent finding)
        {
            ++finding.callVersion;
            finding.Stop();
            if (finding.AStar != null)
                finding.SetPoint((int2)int.MinValue, false);
        }
    }
}
