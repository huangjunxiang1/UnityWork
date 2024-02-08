using System;
using System.Collections.Generic;
using Main;

namespace Game
{
    public class SObject : IDispose, IEvent, ITimer
    {
        public SObject(long rpc = 0)
        {
            this.gid = (((long)random.Next()) << 32) | (random.Next() & 0xffffffff);
            gidMap.Add(gid, this);

            this.rpc = rpc;
            ((IEvent)this).RigisterEvent();
            if (rpc != 0)
            {
                ((IEvent)this).RigisterRPCEvent(rpc);
                if (!rpcMap.TryGetValue(rpc, out var o))
                    rpcMap.Add(rpc, this);
                else
                    Loger.Error($"已经创建rpc对象 rpc={rpc} obj={o}");
            }
            _timerRigisterd = ((ITimer)this).RigisterTimer();

            newQueue.Enqueue(this);
        }

        static Random random = new((int)DateTime.Now.Ticks);
        static Queue<SObject> newQueue = ObjectPool<Queue<SObject>>.Get();
        static Dictionary<long, SObject> rpcMap = new();
        static Dictionary<long, SObject> gidMap = new();

        bool _eventEnable = true;
        long _eventRpcid;
        bool _timerRigisterd = false;
        Eventer _onDispose;
        Dictionary<Type, SComponent> components = ObjectPool<Dictionary<Type, SComponent>>.Get();

        public double value;
        public object data;

        /// <summary>
        /// 自增生成的ID
        /// </summary>
        public long gid { get; }

        /// <summary>
        /// 单位ID
        /// </summary>
        public long rpc { get; }

        /// <summary>
        /// 是否已被销毁
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        /// 事件监听
        /// </summary>
        public bool EventEnable
        {
            get => _eventEnable;
            set
            {
                if (_eventEnable == value) return;
                _eventEnable = value;
                if (value)
                {
                    foreach (var c in components.Values)
                        c.SetChange();
                }
            }
        }

        public bool TimerEnable { get; set; } = true;

        /// <summary>
        /// 父节点
        /// </summary>
        public STree Parent { get; internal set; }

        /// <summary>
        /// 根
        /// </summary>
        public SObject Root
        {
            get
            {
                SObject root = this;
                while (root.Parent != null)
                    root = root.Parent;
                return root;
            }
        }

        /// <summary>
        /// 节点层级
        /// </summary>
        public int Layer
        {
            get
            {
                int layer = 0;
                STree parent = this.Parent;
                while (parent != null)
                {
                    layer++;
                    parent = parent.Parent;
                }
                return layer;
            }
        }

        /// <summary>
        /// 在父节点的下标
        /// </summary>
        public int SiblingIndex
        {
            get
            {
                if (Parent == null)
                    return 0;
                return Parent._children.IndexOf(this);
            }
        }

        /// <summary>
        /// dispose 监听
        /// </summary>
        public Eventer onDispose => _onDispose ??= new Eventer(this);

        public void RigisterRPCEvent(long rpc)
        {
            if (rpc == 0)
            {
                Loger.Error($"rpc=0");
                return;
            }
            if (_eventRpcid != 0)
            {
                Loger.Error($"已经注册了rpc监听 rpc={rpc}");
                return;
            }
            _eventRpcid = rpc;
            GameM.Event.RigisteRPCEvent(rpc, this);
        }

        public T AddComponent<T>() where T : SComponent, new()
        {
            if (components.TryGetValue(typeof(T), out var c))
                return (T)c;
            return AddComponent(new T());
        }
        public T AddComponent<T>(T c) where T : SComponent
        {
            if (c.Disposed)
            {
                Loger.Error($"组件已经销毁 ={this}");
                return default;
            }
            if (c.Entity != null)
            {
                Loger.Error($"组件已经在实体 ={c.Entity}");
                return default;
            }
            if (this.Disposed)
            {
                Loger.Error($"实体已经销毁 ={this}");
                return default;
            }
            if (SSystem.isAutoAddComponent(c.GetType()))
            {
                Loger.Error($"有AddComponentIf标记的组件 不能手动修改 c={c}");
                return null;
            }
            if (components.TryGetValue(typeof(T), out var cc))
            {
                Loger.Error($"已经包含 component={cc}");
                return (T)cc;
            }
            AddComponentInternal(c);
            return c;
        }
        public SComponent AddComponent(Type type)
        {
            if (SSystem.isAutoAddComponent(type))
            {
                Loger.Error($"有AddComponentIf标记的组件 不能手动修改 c={type}");
                return null;
            }
            return AddComponentInternal(type);
        }
        public T GetComponent<T>() where T : SComponent
        {
            if (this.Disposed)
            {
                Loger.Error($"实体已经销毁 entity={this}");
                return default;
            }
            if (components.TryGetValue(typeof(T), out var c))
                return (T)c;
            Loger.Error($"未包含 component={typeof(T)}");
            return default;
        }
        public SComponent GetComponent(Type type)
        {
            if (this.Disposed)
            {
                Loger.Error($"实体已经销毁 entity={this}");
                return default;
            }
            if (components.TryGetValue(type, out var c))
                return c;
            Loger.Error($"未包含 component={type}");
            return default;
        }
        public bool TryGetComponent<T>(out T c) where T : SComponent
        {
            if (this.Disposed)
            {
                Loger.Error($"实体已经销毁 entity={this}");
                c = default;
                return false;
            }
            if (components.TryGetValue(typeof(T), out var cc))
            {
                c = (T)cc;
                return true;
            }
            c = default;
            return false;
        }
        public bool TryGetComponent(Type type, out SComponent c)
        {
            if (this.Disposed)
            {
                Loger.Error($"实体已经销毁 entity={this}");
                c = default;
                return false;
            }
            if (components.TryGetValue(type, out c))
                return true;
            c = default;
            return false;
        }
        public bool RemoveComponent<T>() where T : SComponent
        {
            if (this.Disposed)
            {
                Loger.Error($"实体已经销毁 entity={this}");
                return false;
            }
            if (SSystem.isAutoAddComponent(typeof(T)))
            {
                Loger.Error($"有AddComponentIf标记的组件 不能手动修改 c={typeof(T)}");
                return false;
            }
            if (!RemoveComponentInternal(typeof(T)))
            {
                Loger.Error($"未包含 component={typeof(T)}");
                return false;
            }
            return true;
        }
        public bool RemoveComponent(Type type)
        {
            if (this.Disposed)
            {
                Loger.Error($"实体已经销毁 entity={this}");
                return false;
            }
            if (SSystem.isAutoAddComponent(type))
            {
                Loger.Error($"有AddComponentIf标记的组件 不能手动修改 c={type}");
                return false;
            }
            if (!RemoveComponentInternal(type))
            {
                Loger.Error($"未包含 component={type}");
                return false;
            }
            return true;
        }
        public bool HasComponent<T>() where T : SComponent
        {
            if (this.Disposed)
            {
                Loger.Error($"实体已经销毁 entity={this}");
                return false;
            }
            return components.ContainsKey(typeof(T));
        }
        public bool HasComponent(Type type)
        {
            if (this.Disposed)
            {
                Loger.Error($"实体已经销毁 entity={this}");
                return false;
            }
            return components.ContainsKey(type);
        }

        internal SComponent AddComponentInternal(SComponent c, bool isMove = false)
        {
            c.Entity = this;
            components[c.GetType()] = c;
            SSystem.RigisteComponent(c);
            if (this.rpc != 0)
                ((IEvent)c).RigisterRPCEvent(this.rpc);
            if (!isMove)
            {
                ((IEvent)c).RigisterEvent();
                c._timerRigisterd = ((ITimer)c).RigisterTimer();
                var ps = ArrayCache<object>.Get(1);
                ps[0] = c;
                GameM.Event.RunEvent(Types.InstanceGenericObject(typeof(Awake<>), c.GetType(), ps));
            }
            c.SetChange();
            return c;
        }
        internal SComponent AddComponentInternal(Type type)
        {
            if (components.TryGetValue(type, out var c))
                return c;
            c = (SComponent)Activator.CreateInstance(type);
            return AddComponentInternal(c);
        }
        internal bool RemoveComponentInternal(Type type)
        {
            if (!components.TryGetValue(type, out var c))
                return false;
            c.Dispose();
            return true;
        }
        internal void RemoveFromComponents(SComponent c) => components.Remove(c.GetType());

        /// <summary>
        /// 释放
        /// </summary>
        public virtual void Dispose()
        {
            if (this.Disposed)
            {
                Loger.Error("重复Dispose->" + this);
                return;
            }

            this.Disposed = true;
            if (this.Parent != null)
                this.Parent.Remove(this);
            if (rpc != 0)
                rpcMap.Remove(rpc);
            gidMap.Remove(gid);

            ((IEvent)this).RemoveAllEvent(_eventRpcid);
            if (_timerRigisterd)
                ((ITimer)this).RemoveTimer();

            var tmp = components;
            components = null;
            foreach (var item in tmp.Values)
                item.dispose(2);
            tmp.Clear();
            ObjectPool<Dictionary<Type, SComponent>>.Return(tmp);

            var types = ObjectPool<List<Type>>.Get();
            var type = this.GetType();
            types.Add(type);
            do
            {
                if (type == typeof(SObject)) break;
                type = type.BaseType;
                types.Add(type);
            } while (true);

            var ps = ArrayCache<object>.Get(1);
            for (int i = types.Count - 1; i >= 0; i--)
            {
                ps[0] = this;
                GameM.Event.RunEvent(Types.InstanceGenericObject(typeof(Dispose<>), types[i], ps));
            }
            types.Clear();
            ObjectPool<List<Type>>.Return(types);

            _onDispose?.Call();
        }

        public override string ToString()
        {
            if (this.rpc != 0) return $"{{rpc={this.rpc} gid={this.gid} {this.GetType().FullName}}}";
            else return $"{{gid={this.gid} {this.GetType().FullName}}}";
        }

        public static SObject GetWithRpc(long rpc)
        {
            rpcMap.TryGetValue(rpc, out var obj);
            return obj;
        }
        public static SObject GetWithGid(long gid)
        {
            gidMap.TryGetValue(gid, out var obj);
            return obj;
        }
        public static bool TryGetWithRpc(long rpc, out SObject obj)
        {
            return rpcMap.TryGetValue(rpc, out obj);
        }
        public static bool TryGetWithGid(long gid, out SObject obj)
        {
            return gidMap.TryGetValue(gid, out obj);
        }

        internal static void Update()
        {
            if (newQueue.Count > 0)
            {
                var queue = newQueue;
                newQueue = ObjectPool<Queue<SObject>>.Get();
                var types = ObjectPool<List<Type>>.Get();
                var ps = ArrayCache<object>.Get(1);
                Type lastType = null;
                while (queue.TryDequeue(out var item))
                {
                    if (item.Disposed) continue;

                    var type = item.GetType();
                    if (lastType != type)
                    {
                        lastType = type;
                        types.Clear();
                        types.Add(type);
                        do
                        {
                            if (type == typeof(SObject)) break;
                            type = type.BaseType;
                            types.Add(type);
                        } while (true);
                    }

                    for (int i = types.Count - 1; i >= 0; i--)
                    {
                        ps[0] = item;
                        GameM.Event.RunEvent(Types.InstanceGenericObject(typeof(Awake<>), types[i], ps));
                        if (item.Disposed) break;
                    }
                }
                ObjectPool<Queue<SObject>>.Return(queue);
                types.Clear();
                ObjectPool<List<Type>>.Return(types);
            }
        }
    }
}
