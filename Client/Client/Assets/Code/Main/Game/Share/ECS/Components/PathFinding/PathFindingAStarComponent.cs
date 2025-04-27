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
    public class AStarData
    {
        public AStarData(int width,int height, ushort[] data)
        {
            if (data.Length < width * height)
            {
                Loger.Error(new IndexOutOfRangeException());
                return;
            }
            this.width = width;
            this.height = height;
            this.data = data;
            this.vsArray = new int[width * height];
        }
        public int width { get; private set; }
        public int height { get; private set; }
        public ushort[] data { get; private set; }//每个值 低位第一个bit是 是否激活 后续bit是消耗

        internal int vs;
        internal int[] vsArray;
        internal bool isFinding;
    }
    public class PathFindingAStarComponent : SComponent
    {
        public AStarData AStar;
        public float3 start;//起始坐标
        public float3 size = new float3(1, 0, 1);//块间隔

        FindData[] paths = new FindData[100];
        int arrayIndex = -1;
        int currentIndex = 0;
        int2 to;
        int power;
        [Sirenix.OdinInspector.ShowInInspector]
        float3[] points = new float3[10];

        public bool Finding(int2 from, int2 to, int power = int.MaxValue, PathFindingMethod type = PathFindingMethod.AStar, PathFindingRound r = PathFindingRound.R4, bool moveto = true)
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
            AStar.isFinding = true;
            arrayIndex = 0;
            int pre = math.abs(from.x - to.x) + math.abs(from.y - to.y);
            paths[arrayIndex++] = new FindData { xy = from, last = -1, next = -1, cost = 0, step = 1, totalDistance = pre };
            if (from.Equals(to))
            {
                AStar.isFinding = false;
                return true;
            }

            if (AStar.vs == int.MaxValue)
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
                            if (breadth(new int2(now.xy.x - 1, now.xy.y)))
                                break;
                        if (now.xy.y > 0)
                            if (breadth(new int2(now.xy.x, now.xy.y - 1)))
                                break;
                        if (now.xy.x < AStar.width - 1)
                            if (breadth(new int2(now.xy.x + 1, now.xy.y)))
                                break;
                        if (now.xy.y < AStar.height - 1)
                            if (breadth(new int2(now.xy.x, now.xy.y + 1)))
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
                            if (breadth(new int2(now.xy.x - 1, now.xy.y)))
                                break;
                        if (now.xy.y > 0)
                            if (breadth(new int2(now.xy.x, now.xy.y - 1)))
                                break;
                        if (now.xy.x < AStar.width - 1)
                            if (breadth(new int2(now.xy.x + 1, now.xy.y)))
                                break;
                        if (now.xy.y < AStar.height - 1)
                            if (breadth(new int2(now.xy.x, now.xy.y + 1)))
                                break;

                        if (now.xy.x > 0 && now.xy.y > 0)
                            if (breadth(new int2(now.xy.x - 1, now.xy.y - 1)))
                                break;
                        if (now.xy.x > 0 && now.xy.y < AStar.height - 1)
                            if (breadth(new int2(now.xy.x - 1, now.xy.y + 1)))
                                break;
                        if (now.xy.x < AStar.width - 1 && now.xy.y > 0)
                            if (breadth(new int2(now.xy.x + 1, now.xy.y - 1)))
                                break;
                        if (now.xy.x < AStar.width - 1 && now.xy.y < AStar.height - 1)
                            if (breadth(new int2(now.xy.x + 1, now.xy.y + 1)))
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
                            if (round4(new int2(now.xy.x - 1, now.xy.y), out bool move)) break;
                            if (move) continue;
                        }
                        if (now.xy.y > 0)
                        {
                            if (round4(new int2(now.xy.x, now.xy.y - 1), out bool move)) break;
                            if (move) continue;
                        }
                        if (now.xy.x < AStar.width - 1)
                        {
                            if (round4(new int2(now.xy.x + 1, now.xy.y), out bool move)) break;
                            if (move) continue;
                        }
                        if (now.xy.y < AStar.height - 1)
                        {
                            if (round4(new int2(now.xy.x, now.xy.y + 1), out bool move)) break;
                            if (move) continue;
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
                            if (round8(new int2(now.xy.x - 1, now.xy.y)))
                                break;
                        if (now.xy.y > 0)
                            if (round8(new int2(now.xy.x, now.xy.y - 1)))
                                break;
                        if (now.xy.x < AStar.width - 1)
                            if (round8(new int2(now.xy.x + 1, now.xy.y)))
                                break;
                        if (now.xy.y < AStar.height - 1)
                            if (round8(new int2(now.xy.x, now.xy.y + 1)))
                                break;

                        if (now.xy.x > 0 && now.xy.y > 0)
                            if (round8(new int2(now.xy.x - 1, now.xy.y - 1)))
                                break;
                        if (now.xy.x > 0 && now.xy.y < AStar.height - 1)
                            if (round8(new int2(now.xy.x - 1, now.xy.y + 1)))
                                break;
                        if (now.xy.x < AStar.width - 1 && now.xy.y > 0)
                            if (round8(new int2(now.xy.x + 1, now.xy.y - 1)))
                                break;
                        if (now.xy.x < AStar.width - 1 && now.xy.y < AStar.height - 1)
                            if (round8(new int2(now.xy.x + 1, now.xy.y + 1)))
                                break;
                        currentIndex = paths[currentIndex].next;
                    } while (currentIndex != -1);
                }
            }

            AStar.isFinding = false;

            if (paths[arrayIndex - 1].xy.Equals(to))
            {
                if (moveto)
                    this.SetChangeFlag();
                return true;
            }
            arrayIndex = -1;
            return false;
        }
        bool breadth(int2 xy)
        {
            int i = xy.y * AStar.width + xy.x;
            int cost = paths[currentIndex].cost + (AStar.data[i] >> 1);
            var move = (AStar.data[i] & 1) == 1 && AStar.vsArray[i] != AStar.vs && cost <= power;
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
                    step = paths[currentIndex].step + 1,
                };
                return xy.Equals(to);
            }
            return false;
        }
        bool round4(int2 xy, out bool move)
        {
            int i = xy.y * AStar.width + xy.x;
            int cost = paths[currentIndex].cost + (AStar.data[i] >> 1);
            move = (AStar.data[i] & 1) == 1 && AStar.vsArray[i] != AStar.vs && cost <= power;
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

                if (xy.Equals(to))
                    return true;

                int lastIdx = -1;
                int nextIdx = currentIndex;
                while (nextIdx != -1 && paths[nextIdx].totalDistance < paths[arrayIndex - 1].totalDistance)
                {
                    lastIdx = nextIdx;
                    nextIdx = paths[nextIdx].next;
                }
                if (lastIdx != -1)
                    paths[lastIdx].next = arrayIndex - 1;
                else
                    currentIndex = arrayIndex - 1;
                if (nextIdx != -1)
                    paths[arrayIndex - 1].next = nextIdx;
            }
            return false;
        }
        bool round8(int2 xy)
        {
            int i = xy.y * AStar.width + xy.x;
            int cost = paths[currentIndex].cost + (AStar.data[i] >> 1);
            var move = (AStar.data[i] & 1) == 1 && AStar.vsArray[i] != AStar.vs && cost <= power;
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

                if (xy.Equals(to))
                    return true;

                int lastIdx = currentIndex;
                int nextIdx = paths[currentIndex].next;
                while (nextIdx != -1 && paths[nextIdx].totalDistance < paths[arrayIndex - 1].totalDistance)
                {
                    lastIdx = nextIdx;
                    nextIdx = paths[nextIdx].next;
                }
                paths[lastIdx].next = arrayIndex - 1;
                if (nextIdx != -1)
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
                array[n.step - 1] = start + size * new float3(n.xy.x, 0, n.xy.y);
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
                ret.Add(start + size * new float3(n.xy.x, 0, n.xy.y));
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
                ret[n.step - 1] = start + size * new float3(n.xy.x, 0, n.xy.y);
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

        [ChangeSystem]
        static void Change(PathFindingAStarComponent finding, MoveToComponent move)
        {
            if (finding.arrayIndex != -1)
            {
                int len = finding.GetFindingPoints(ref finding.points);
                move.MoveTo(finding.points, 0, len - 1);
            }
        }

        struct FindData
        {
            public int2 xy;
            public int last;
            public int next;
            public int cost;
            public int step;
            public int totalDistance;
        }
    }
}
