using Core;
using System.Collections.Generic;
using System.Reflection;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;

namespace Game
{
    public class Scene : STree
    {
        internal object[] _paramObjects;

        public virtual void OnEnter() { }

        public T GetParam<T>(int index)
        {
            if (_paramObjects != null && index < _paramObjects.Length)
                return (T)_paramObjects[index];
            return default;
        }
    }

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
                if (depth < SSetting.ViewSetting.OcTree_MaxOcTreeDepth && trees.Count + 1 > SSetting.ViewSetting.OcTree_MaxTreesPerNode)
                {
                    foreach (var item in trees)
                        (item.node = Get(item.transform.position)).Add(item);
                    trees.Clear();
                    (o.node = Get(o.transform.position)).Add(o);
                    final = false;
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
                if (final || depth >= SSetting.ViewSetting.OcTree_MaxOcTreeDepth)
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
        }
    }
}
