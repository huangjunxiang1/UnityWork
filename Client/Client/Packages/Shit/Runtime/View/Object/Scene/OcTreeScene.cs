using Core;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;

namespace Game
{

    /// <summary>
    /// 八叉树场景
    /// </summary>
    public class OcTreeScene : Scene
    {
        OcTreeNode _root;

        public MinMaxAABB aabb => _root == null ? default : _root.aabb;

        public override void AddChild(SObject child)
        {
            base.AddChild(child);
            if (_root == null)
            {
                Loger.Error("not set AABB");
                return;
            }
            child.AddComponent(new OcTreeComponent { scene = this });
        }
        public override void Remove(SObject child)
        {
            base.Remove(child);
            child.RemoveComponent<OcTreeComponent>();
        }

        /// <summary>
        /// AABB 范围查询。结果会 append 到 result，调用方负责 Clear。
        /// </summary>
        public void Query(MinMaxAABB range, List<SObject> result)
        {
            if (_root == null || result == null) return;
            _root.Query(range, result);
        }

        /// <summary>
        /// 球范围查询。
        /// </summary>
        public void QuerySphere(float3 center, float radius, List<SObject> result)
        {
            if (_root == null || result == null) return;
            _root.QuerySphere(center, radius, result);
        }

        /// <summary>
        /// 最近邻查询，返回 null 表示没有对象。
        /// </summary>
        public SObject QueryNearest(float3 center)
        {
            if (_root == null) return null;
            SObject best = null;
            float bestDist2 = float.MaxValue;
            _root.QueryNearest(center, ref best, ref bestDist2);
            return best;
        }

        protected void SetAABB(MinMaxAABB aabb)
        {
            if (_root != null)
            {
                Loger.Error("already set it");
                return;
            }
            _root = new(aabb);
        }


        class OcTreeComponent : SComponent
        {
            public OcTreeScene scene { get; init; }
            public OcTreeNode node;
            public TransformComponent transform;

            [InSystem]
            static void In(TransformComponent t, OcTreeComponent o)
            {
                o.transform = t;
            }
            [OutSystem]
            static void Out(TransformComponent t, OcTreeComponent o)
            {
                o.node.Remove(o);
            }
            [ChangeSystem]
            static void change(TransformComponent t, OcTreeComponent o)
            {
                (o.node ?? o.scene._root).Set(t.position, o);
            }
        }

        class OcTreeNode
        {
            public MinMaxAABB aabb;

            OcTreeNode[] nodes = new OcTreeNode[8];
            HashSet<OcTreeComponent> trees = new();
            bool final = true;
            int depth = 0;

            public OcTreeNode(MinMaxAABB aabb)
            {
                this.aabb = aabb;
            }
            public void Set(float3 v3, OcTreeComponent o)
            {
                if (o.node == null)
                {
                    (o.node = Get(v3)).Add(o);
                    return;
                }
                if (o.node.aabb.Contains(v3))
                    return;
                o.node.Remove(o);
                (o.node = o.scene._root.Get(v3)).Add(o);
            }
            public void Add(OcTreeComponent o)
            {
                if (depth < SSetting.ViewSetting.Tree_MaxTreeDepth && trees.Count + 1 > SSetting.ViewSetting.Tree_MaxTreePerNode)
                {
                    final = false;
                    foreach (var item in trees)
                        (item.node = Get(item.transform.position)).Add(item);
                    trees.Clear();
                    (o.node = Get(o.transform.position)).Add(o);
                }
                else
                    trees.Add(o);
            }
            public void Remove(OcTreeComponent o)
            {
                trees.Remove(o);
            }

            public OcTreeNode Get(float3 v3)
            {
#if DebugEnable
                if (!aabb.Contains(v3))
                    Loger.Error($"out of bound {v3} {aabb}");
#endif
                if (final || depth >= SSetting.ViewSetting.Tree_MaxTreeDepth)
                    return this;
                OcTreeNode node;
                if (v3.y < aabb.Center.y)
                {
                    if (v3.z < aabb.Center.z)
                    {
                        if (v3.x < aabb.Center.x)
                            node = nodes[0] ??= new(new(aabb.Min, aabb.Center));
                        else
                            node = nodes[1] ??= new(new(new float3(aabb.Center.x, aabb.Min.y, aabb.Min.z), new float3(aabb.Max.x, aabb.Center.y, aabb.Center.z)));
                    }
                    else
                    {
                        if (v3.x < aabb.Center.x)
                            node = nodes[2] ??= new(new(new float3(aabb.Min.x, aabb.Min.y, aabb.Center.z), new float3(aabb.Center.x, aabb.Center.y, aabb.Max.z)));
                        else
                            node = nodes[3] ??= new(new(new float3(aabb.Center.x, aabb.Min.y, aabb.Center.z), new float3(aabb.Max.x, aabb.Center.y, aabb.Max.z)));
                    }
                }
                else
                {
                    if (v3.z < aabb.Center.z)
                    {
                        if (v3.x < aabb.Center.x)
                            node = nodes[4] ??= new(new(new float3(aabb.Min.x, aabb.Center.y, aabb.Min.z), new float3(aabb.Center.x, aabb.Max.y, aabb.Center.z)));
                        else
                            node = nodes[5] ??= new(new(new float3(aabb.Center.x, aabb.Center.y, aabb.Min.z), new float3(aabb.Max.x, aabb.Max.y, aabb.Center.z)));
                    }
                    else
                    {
                        if (v3.x < aabb.Center.x)
                            node = nodes[6] ??= new(new(new float3(aabb.Min.x, aabb.Center.y, aabb.Center.z), new float3(aabb.Center.x, aabb.Max.y, aabb.Max.z)));
                        else
                            node = nodes[7] ??= new(new(aabb.Center, aabb.Max));
                    }
                }
                return node.Get(v3);
            }

            /// <summary>
            /// AABB 范围查询：收集与 range 相交的节点里的所有对象。
            /// </summary>
            public void Query(MinMaxAABB range, List<SObject> result)
            {
                // 节点 AABB 与查询范围不相交，整棵子树跳过
                if (!aabb.Overlaps(range))
                    return;

                // 已分裂：递归子节点
                if (!final)
                {
                    for (int i = 0; i < nodes.Length; i++)
                        nodes[i]?.Query(range, result);
                    return;
                }

                // 叶子节点：逐个精确判断
                foreach (var t in trees)
                {
                    if (t.Disposed) continue;
                    // 如果对象有半径/尺寸，用它的 AABB 判断；否则用点判断
                    if (range.Contains(t.transform.position))
                        result.Add(t.Entity);
                }
            }

            /// <summary>
            /// 球范围查询。
            /// </summary>
            public void QuerySphere(float3 center, float radius, List<SObject> result)
            {
                float3 closest = math.clamp(center, aabb.Min, aabb.Max);
                float3 d = center - closest;
                if (math.dot(d, d) > radius * radius)
                    return;

                if (!final)
                {
                    for (int i = 0; i < nodes.Length; i++)
                        nodes[i]?.QuerySphere(center, radius, result);
                    return;
                }

                float r2 = radius * radius;
                foreach (var t in trees)
                {
                    if (t.Disposed) continue;
                    float3 p = t.transform.position;
                    float dx = p.x - center.x;
                    float dy = p.y - center.y;
                    float dz = p.z - center.z;
                    if (dx * dx + dy * dy + dz * dz <= r2)
                        result.Add(t.Entity);
                }
            }

            /// <summary>
            /// 最近邻查询。best 是当前最优，bestDist2 是最优距离平方。
            /// </summary>
            public void QueryNearest(float3 center, ref SObject best, ref float bestDist2)
            {
                float3 closest = math.clamp(center, aabb.Min, aabb.Max);
                float3 d = center - closest;
                if (math.dot(d, d) >= bestDist2)
                    return;

                if (!final)
                {
                    // 按距离排序子节点，优先搜索近的，能更早剪枝
                    // 简化版：直接遍历，靠剪枝也能接受
                    for (int i = 0; i < nodes.Length; i++)
                        nodes[i]?.QueryNearest(center, ref best, ref bestDist2);
                    return;
                }

                foreach (var t in trees)
                {
                    if (t.Disposed) continue;
                    float3 p = t.transform.position;
                    float ddx = p.x - center.x;
                    float ddy = p.y - center.y;
                    float ddz = p.z - center.z;
                    float d2 = ddx * ddx + ddy * ddy + ddz * ddz;
                    if (d2 < bestDist2)
                    {
                        bestDist2 = d2;
                        best = t.Entity;
                    }
                }
            }
        }
    }
}
