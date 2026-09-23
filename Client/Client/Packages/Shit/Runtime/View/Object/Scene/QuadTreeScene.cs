using Core;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;

/// <summary>
/// 四叉树场景（x-z 平面分割，y 轴不参与）
/// </summary>
public class QuadTreeScene : Scene
{
    QuadTreeNode _root;

    public MinMaxAABB aabb => _root == null ? default : _root.aabb;

    public override void AddChild(SObject child)
    {
        base.AddChild(child);
        if (_root == null)
        {
            Loger.Error("not set AABB");
            return;
        }
        child.AddComponent(new QuadTreeComponent { scene = this });
    }

    public override void Remove(SObject child)
    {
        base.Remove(child);
        child.RemoveComponent<QuadTreeComponent>();
    }

    /// <summary>
    /// AABB 范围查询（y 轴不参与过滤，只按 x-z 判断）。
    /// </summary>
    public void Query(MinMaxAABB range, List<SObject> result)
    {
        if (_root == null || result == null) return;
        _root.Query(range, result);
    }

    /// <summary>
    /// 圆形范围查询（x-z 平面上的圆）。
    /// </summary>
    public void QueryCircle(float3 center, float radius, List<SObject> result)
    {
        if (_root == null || result == null) return;
        _root.QueryCircle(center, radius, result);
    }

    /// <summary>
    /// 最近邻查询（x-z 平面距离）。
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

    class QuadTreeComponent : SComponent
    {
        public QuadTreeScene scene { get; init; }
        public QuadTreeNode node;
        public TransformComponent transform;

        [InSystem]
        static void In(TransformComponent t, QuadTreeComponent o)
        {
            o.transform = t;
        }
        [OutSystem]
        static void Out(TransformComponent t, QuadTreeComponent o)
        {
            o.node?.Remove(o);
            o.node = null;
        }
        [ChangeSystem]
        static void change(TransformComponent t, QuadTreeComponent o)
        {
            (o.node ?? o.scene._root).Set(t.position, o);
        }
    }

    class QuadTreeNode
    {
        public MinMaxAABB aabb;

        QuadTreeNode[] nodes = new QuadTreeNode[4];
        HashSet<QuadTreeComponent> trees = new();
        bool final = true;
        int depth = 0;

        public QuadTreeNode(MinMaxAABB aabb)
        {
            this.aabb = aabb;
        }

        public void Set(float3 v3, QuadTreeComponent o)
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

        public void Add(QuadTreeComponent o)
        {
            if (final
                && depth < SSetting.ViewSetting.Tree_MaxTreeDepth
                && trees.Count + 1 > SSetting.ViewSetting.Tree_MaxTreePerNode)
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

        public void Remove(QuadTreeComponent o)
        {
            trees.Remove(o);
        }

        public QuadTreeNode Get(float3 v3)
        {
#if DebugEnable
            if (!aabb.Contains(v3))
                Loger.Error($"out of bound {v3} {aabb}");
#endif
            if (final || depth >= SSetting.ViewSetting.Tree_MaxTreeDepth)
                return this;

            int idx = GetQuadrantIndex(v3, aabb.Center);
            return (nodes[idx] ??= new QuadTreeNode(MakeChildAABB(idx))).Get(v3);
        }

        /// <summary>
        /// 象限索引：bit0 = x（0 负 1 正），bit1 = z（0 负 1 正）。
        /// 0: (-x, -z)  1: (+x, -z)  2: (-x, +z)  3: (+x, +z)
        /// </summary>
        static int GetQuadrantIndex(float3 v, float3 c)
        {
            int x = v.x < c.x ? 0 : 1;
            int z = v.z < c.z ? 0 : 1;
            return x | (z << 1);
        }

        /// <summary>
        /// 按象限生成子节点 AABB。y 轴范围保持完整（不分割）。
        /// </summary>
        MinMaxAABB MakeChildAABB(int idx)
        {
            float3 c = aabb.Center;
            int bitX = idx & 1;
            int bitZ = (idx >> 1) & 1;

            float3 min = new float3(
                bitX == 0 ? aabb.Min.x : c.x,
                aabb.Min.y,
                bitZ == 0 ? aabb.Min.z : c.z);
            float3 max = new float3(
                bitX == 0 ? c.x : aabb.Max.x,
                aabb.Max.y,
                bitZ == 0 ? c.z : aabb.Max.z);

            return new MinMaxAABB(min, max);
        }

        /// <summary>
        /// AABB 范围查询。因为四叉树不分割 y，所以用 x-z 平面的重叠判断。
        /// </summary>
        public void Query(MinMaxAABB range, List<SObject> result)
        {
            if (!OverlapsXZ(aabb, range))
                return;

            if (!final)
            {
                for (int i = 0; i < nodes.Length; i++)
                    nodes[i]?.Query(range, result);
                return;
            }

            foreach (var t in trees)
            {
                if (t.Disposed) continue;
                float3 p = t.transform.position;
                if (p.x >= range.Min.x && p.x <= range.Max.x
                    && p.z >= range.Min.z && p.z <= range.Max.z)
                    result.Add(t.Entity);
            }
        }

        /// <summary>
        /// 圆形查询（x-z 平面圆）。
        /// </summary>
        public void QueryCircle(float3 center, float radius, List<SObject> result)
        {
            if (!CircleOverlapsXZ(center, radius, aabb))
                return;

            if (!final)
            {
                for (int i = 0; i < nodes.Length; i++)
                    nodes[i]?.QueryCircle(center, radius, result);
                return;
            }

            float r2 = radius * radius;
            foreach (var t in trees)
            {
                if (t.Disposed) continue;
                float3 p = t.transform.position;
                float dx = p.x - center.x;
                float dz = p.z - center.z;
                if (dx * dx + dz * dz <= r2)
                    result.Add(t.Entity);
            }
        }

        /// <summary>
        /// 最近邻查询（x-z 平面距离）。
        /// </summary>
        public void QueryNearest(float3 center, ref SObject best, ref float bestDist2)
        {
            if (!CircleOverlapsXZ(center, math.sqrt(bestDist2), aabb))
                return;

            if (!final)
            {
                for (int i = 0; i < nodes.Length; i++)
                    nodes[i]?.QueryNearest(center, ref best, ref bestDist2);
                return;
            }

            foreach (var t in trees)
            {
                if (t.Disposed) continue;
                float3 p = t.transform.position;
                float dx = p.x - center.x;
                float dz = p.z - center.z;
                float d2 = dx * dx + dz * dz;
                if (d2 < bestDist2)
                {
                    bestDist2 = d2;
                    best = t.Entity;
                }
            }
        }

        /// <summary>
        /// x-z 平面上的 AABB 重叠判断。
        /// </summary>
        static bool OverlapsXZ(MinMaxAABB a, MinMaxAABB b)
        {
            return a.Min.x <= b.Max.x && a.Max.x >= b.Min.x
                && a.Min.z <= b.Max.z && a.Max.z >= b.Min.z;
        }

        /// <summary>
        /// 圆与 AABB 在 x-z 平面是否相交。
        /// </summary>
        static bool CircleOverlapsXZ(float3 center, float radius, MinMaxAABB aabb)
        {
            float cx = math.clamp(center.x, aabb.Min.x, aabb.Max.x);
            float cz = math.clamp(center.z, aabb.Min.z, aabb.Max.z);
            float dx = center.x - cx;
            float dz = center.z - cz;
            return dx * dx + dz * dz <= radius * radius;
        }
    }
}