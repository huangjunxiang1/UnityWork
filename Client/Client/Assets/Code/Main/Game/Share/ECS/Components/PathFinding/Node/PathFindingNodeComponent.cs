using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    public class PathFindingNodeComponent : SComponent
    {
        [Sirenix.OdinInspector.ShowInInspector]
#if !Server
        public MulNode Root { get; set; } = PathFindingNode.GetCurrent();
#else
        public MulNode Root { get; set; }
#endif
        public long CurrentId;//

        FindData[] paths = new FindData[20];
        int finalIndex = -1;
        STask<bool> move;
        long toId;

        [Sirenix.OdinInspector.ShowInInspector]
        float3[] points = new float3[10];

        public bool Finding(long toId)
        {
            return this.Finding(this.CurrentId, toId);
        }
        public bool Finding(long fromId, long toId)
        {
            if (Root == null)
            {
                Loger.Error(new NullReferenceException());
                return false;
            }
            var from = Root.GetNode(fromId);
            var to = Root.GetNode(toId);
            if (from == null || to == null)
            {
                Loger.Error("cannot find node");
                return false;
            }
            if (Root.isFinding)
            {
                Loger.Error("cannot finding in mul thread");
                return false;
            }
            Root.isFinding = true;

            int arrayIndex = 0;
            paths[arrayIndex++] = new FindData { last = -1, next = -1, id = from.id, step = 1 };
            if (from == to)
            {
                finalIndex = 0;
                Root.isFinding = false;
                this.toId = toId;
                return true;
            }

            int index = 0;
            finalIndex = -1;
            if (Root.vs == int.MaxValue)
            {
                Root.vs = 0;
                foreach (var n in Root.Nodes.Values)
                    n.vs = 0;
            }
            int vs = ++Root.vs;

            do
            {
                var now = Root.GetNode(paths[index].id);
                for (var i = 0; i < now.Next.Count; i++)
                {
                    if (toId != now.Next[i].Node.id && now.Next[i].Node.vs == vs)
                        continue;
                    float d = paths[index].totalDistance + now.Next[i].Distance;
                    if (finalIndex != -1 && d >= paths[finalIndex].totalDistance)
                        continue;
                    now.Next[i].Node.vs = vs;
                    if (arrayIndex >= paths.Length)
                        Array.Resize(ref paths, arrayIndex * 2);
                    paths[arrayIndex++] = new FindData
                    {
                        last = index,
                        next = -1,
                        id = now.Next[i].Node.id,
                        step = paths[index].step + 1,
                        totalDistance = d
                    };
                    int lastIdx = index;
                    int nextIdx = paths[index].next;
                    while (nextIdx != -1 && paths[nextIdx].totalDistance < paths[arrayIndex - 1].totalDistance)
                    {
                        lastIdx = nextIdx;
                        nextIdx = paths[nextIdx].next;
                    }
                    paths[lastIdx].next = arrayIndex - 1;
                    if (nextIdx != -1)
                        paths[arrayIndex - 1].next = nextIdx;
                    if (to.id == now.Next[i].Node.id)
                    {
                        if (finalIndex == -1)
                            finalIndex = arrayIndex - 1;
                        else
                        {
                            if (paths[arrayIndex - 1].totalDistance < paths[finalIndex].totalDistance)
                                finalIndex = arrayIndex - 1;
                        }
                    }
                }
                index = paths[index].next;
            } while (index != finalIndex);
            Root.isFinding = false;
            this.toId = toId;

            return finalIndex != -1;
        }
        public void Move()
        {
            if (finalIndex == -1) return;
            if (move == null)
            {
                move = new();
                this.SetChangeFlag();
            }
        }
        public async Task<bool> MoveAsync()
        {
            if (finalIndex == -1) return false;
            if (move == null)
            {
                move = new();
                this.SetChangeFlag();
            }
            return await move;
        }
        public float3[] GetFindingPoints()
        {
            if (finalIndex == -1) return Array.Empty<float3>();
            var n = this.paths[finalIndex];
            var array = new float3[n.step];
            while (true)
            {
                array[n.step - 1] = this.Root.GetNode(n.id).position;
                if (n.last == -1)
                    break;
                n = this.paths[n.last];
            }
            return array;
        }
        public void GetFindingPoints(List<float3> ret)
        {
            if (finalIndex == -1) return;
            var n = this.paths[finalIndex];
            int index = ret.Count;
            while (true)
            {
                ret.Add(this.Root.GetNode(n.id).position);
                if (n.last == -1)
                    break;
                n = this.paths[n.last];
            }
            ret.Reverse(index, ret.Count - index);
        }
        public int GetFindingPoints(ref float3[] ret)
        {
            if (finalIndex == -1) return 0;
            var n = this.paths[finalIndex];
            if (ret.Length < n.step)
                ret = new float3[n.step];
            int len = n.step;
            while (true)
            {
                ret[n.step - 1] = this.Root.GetNode(n.id).position;
                if (n.last == -1)
                    break;
                n = this.paths[n.last];
            }
            return len;
        }
        public long[] GetFindingIDs()
        {
            if (finalIndex == -1) return Array.Empty<long>();
            var n = this.paths[finalIndex];
            var array = new long[n.step];
            while (true)
            {
                array[n.step - 1] = this.Root.GetNode(n.id).id;
                if (n.last == -1)
                    break;
                n = this.paths[n.last];
            }
            return array;
        }
        public void GetFindingIDs(List<long> ret)
        {
            if (finalIndex == -1) return;
            var n = this.paths[finalIndex];
            int index = ret.Count;
            while (true)
            {
                ret.Add(this.Root.GetNode(n.id).id);
                if (n.last == -1)
                    break;
                n = this.paths[n.last];
            }
            ret.Reverse(index, ret.Count - index);
        }
        public int GetFindingIDs(ref long[] ret)
        {
            if (finalIndex == -1) return 0;
            var n = this.paths[finalIndex];
            if (ret.Length < n.step)
                ret = new long[n.step];
            int len = n.step;
            while (true)
            {
                ret[n.step - 1] = this.Root.GetNode(n.id).id;
                if (n.last == -1)
                    break;
                n = this.paths[n.last];
            }
            return len;
        }

        [ChangeSystem]
        static async void Change(PathFindingNodeComponent finding, MoveToComponent move)
        {
            if (finding.finalIndex != -1)
            {
                int len = finding.GetFindingPoints(ref finding.points);
                var to = finding.toId;
                var v = await move.MoveToAsync(finding.points, 0, len - 1);
                finding.CurrentId = to;
                if (finding.move != null)
                {
                    var m = finding.move;
                    finding.move = null;
                    m.TrySetResult(v);
                }
            }
        }

        struct FindData
        {
            public int last;
            public int next;
            public long id;
            public int step;
            public float totalDistance;
        }
    }
}
