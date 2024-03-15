using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Core
{
    public class SObject : IDispose, IEvent, ITimer
    {
        public SObject(World world, long rpc = 0)
        {
            this.world = world;
            this.gid = (((long)random.Next()) << 32) | (random.Next() & 0xffffffff);
            this.rpc = rpc;

            if (world != null)
            {
                world.ObjectManager.Add(this);
                world.Event.RigisteEvent(this);
                if (rpc != 0)
                    world.Event.RigisteRPCEvent(rpc, this);
                _timerRigisterd = world.Timer.RigisterTimer(this);
            }
        }

        static Random random = new((int)DateTime.Now.Ticks);
        bool _eventEnable = true;
        bool _timerRigisterd = false;
        Eventer _onDispose;
        Dictionary<Type, SComponent> components = ObjectPool.Get<Dictionary<Type, SComponent>>();


        public double value;
        public object data;

        public World world { get; }
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

        protected SComponent AddComponentInternal(SComponent c)
        {
            c.Entity = this;
            components[c.GetType()] = c;

            c.Entity.world.Event.RigisteEvent(c);
            if (this.rpc != 0)
                c.Entity.world.Event.RigisteRPCEvent(rpc, c);

            SSystem.TryAddChangeHandler(c);
            SSystem.TryAddUpdateHandler(c);
            this.world.System.Awake(c.GetType(), this);
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
        internal void RemoveFromComponents(SComponent c)
        {
            components.Remove(c.GetType());
        }

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

            world.ObjectManager.Remove(this);
            world.Event.RemoveEvent(this);
            if (this.rpc != 0)
                world.Event.RemoveRPCEvent(this.rpc, this);
            if (this._timerRigisterd)
                world.Timer.RemoveTimer();

            this.Disposed = true;
            if (this.Parent != null)
                this.Parent.Remove(this);

            var tmp = components;
            components = null;
            foreach (var item in tmp.Values)
                item.dispose(true);
            tmp.Clear();
            ObjectPool.Return(tmp);

            var types = ObjectPool.Get<List<Type>>();
            var type = this.GetType();
            types.Add(type);
            do
            {
                if (type == typeof(SObject)) break;
                type = type.BaseType;
                types.Add(type);
            } while (true);

            for (int i = types.Count - 1; i >= 0; i--)
                this.world.System.Dispose(types[i], this);
            types.Clear();
            ObjectPool.Return(types);

            _onDispose?.Call();
        }

        public override string ToString()
        {
            if (this.rpc != 0) return $"{{rpc={this.rpc} gid={this.gid} {this.GetType().FullName}}}";
            else return $"{{gid={this.gid} {this.GetType().FullName}}}";
        }
    }
}
