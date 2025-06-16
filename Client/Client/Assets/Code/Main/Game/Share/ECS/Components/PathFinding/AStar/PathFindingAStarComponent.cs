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

        public PathFindingAStarComponent(AStarData aStar, AStarVolume volume = null)
        {
            this.AStar = aStar;
            this.Volume = volume ?? AStarVolume.One;
        }

        [Sirenix.OdinInspector.ShowInInspector]
        public AStarData AStar { get; private set; }
        [Sirenix.OdinInspector.ShowInInspector]
        public int2 Current { get; private set; }= int.MinValue;

        /// <summary>
        /// 单位占用体积
        /// </summary>
        public AStarVolume Volume { get; private set; }

        FindData[] paths = new FindData[100];
        int arrayIndex = -1;
        int currentIndex = 0;
        int2 to;
        int power;
        [Sirenix.OdinInspector.ShowInInspector]
        float3[] points = new float3[10];
        SValueTask<bool> waitTask;
        bool move = false;

        public bool Finding(int2 to, int power = int.MaxValue, int near = 0, AStarVolume targetVolume = null, PathFindingMethod type = PathFindingMethod.AStar, PathFindingRound r = PathFindingRound.R4)
        {
            return this.Finding(this.Current, to, power, near, targetVolume, type, r);
        }
        public bool Finding(int2 from, int2 to, int power = int.MaxValue, int near = 0, AStarVolume targetVolume = null, PathFindingMethod type = PathFindingMethod.AStar, PathFindingRound r = PathFindingRound.R4)
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
        public SValueTask<bool> Goto(int2 to, int power = int.MaxValue, int near = 0, AStarVolume targetVolume = null, PathFindingMethod type = PathFindingMethod.AStar, PathFindingRound r = PathFindingRound.R4)
        {
            if (this.Finding(to, power, near, targetVolume, type, r))
            {
                waitTask.TrySetResult(false);
                waitTask = SValueTask<bool>.Create();
                move = true;
                this.SetChangeFlag();
                return waitTask;
            }
            else
                return default;
        }
        public void Stop()
        {
            move = false;
            this.Entity.GetComponent<MoveToComponent>()?.Stop();
        }

        bool breadth(int2 xy, AStarVolume targetVolume , int near)
        {
            int i = xy.y * AStar.width + xy.x;
            int cost = paths[currentIndex].cost + (AStar.data[i] >> 1);
            var move = AStar.vsArray[i] != AStar.vs && cost <= power && AStar.isEnable(xy, Volume, Current);
            if (move)
            {
                AStar.vsArray[i] = AStar.vs;
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
            if (move)
            {
                AStar.vsArray[i] = AStar.vs;
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
            var array = new float3[n.step];
            while (true)
            {
                array[n.step - 1] = AStar.GetPosition(n.xy);
                if (n.last == -1)
                    break;
                n = this.paths[n.last];
            }
            return array;
        }
        public void GetFindingPoints(List<float3> ret)
        {
            if (arrayIndex == -1) return;
            var n = this.paths[arrayIndex - 1];
            int index = ret.Count;
            while (true)
            {
                ret.Add(AStar.GetPosition(n.xy));
                if (n.last == -1)
                    break;
                n = this.paths[n.last];
            }
            ret.Reverse(index, ret.Count - index);
        }
        public int GetFindingPoints(ref float3[] ret)
        {
            if (arrayIndex == -1) return 0;
            var n = this.paths[arrayIndex - 1];
            if (ret.Length < n.step)
                ret = new float3[n.step];
            int len = n.step;
            while (true)
            {
                ret[n.step - 1] = AStar.GetPosition(n.xy);
                if (n.last == -1)
                    break;
                n = this.paths[n.last];
            }
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

        void SetPoint(int2 xy)
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
                for (int i = 0; i < len; i++)
                {
                    var to = finding.AStar.GetXY(finding.points[i]);
                    if (finding.AStar.isEnable(to, finding.Volume, finding.Current))
                    {
                        finding.SetPoint(to);
                        var v = await move.MoveToAsync(finding.points[i]);
                        if (!v)
                        {
                            task.TrySetResult(false);
                            return;
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
            finding.SetPoint(finding.AStar.GetXY(transform.position));
        }
        [OutSystem]
        static void Out(PathFindingAStarComponent finding)
        {
            finding.SetPoint(int.MinValue);
        }
    }
}
