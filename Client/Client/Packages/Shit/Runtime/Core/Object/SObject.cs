﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class SObject : SComponent, ITimer
    {
        public SObject(long rpc = 0)
        {
            this.gid = Util.RandomLong();
            this.rpc = rpc;

            this.Entity = this;

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
                _components[types[i]] = this;

            types.Clear();
            ObjectPool.Return(types);
        }

        World _world;
        bool _eventEnable = true;
        bool _timerRigisterd = false;
        Eventer _onDispose;
        internal Dictionary<Type, SComponent> _components = ObjectPool.Get<Dictionary<Type, SComponent>>();

        public double value;
        public object data;

#if UNITY_EDITOR
        //做检视面板使用的
        internal List<EventSystem.EvtData> _Awake = new();
        internal List<EventSystem.EvtData> _In = new();
        internal List<__UpdateHandle> _updates = new();
        internal List<__Timer> _timers = new();
        internal static Action objChange;
#endif

        public sealed override World World
        {
            get => _world;
            internal set
            {
                if (_world == value || this.Disposed) return;
                if (_world != null)
                {
                    Loger.Error("所属世界不能切换");
                    return;
                }
                _world = value;
                if (value != null)
                {
                    value.ObjectManager.Add(this);
                    value.Event.RigisteEvent(this);
                    _timerRigisterd = value.Timer.RigisterTimer(this);

                    if (!this.Disposed)
                    {
                        var cs = _components;
                        _components = ObjectPool.Get<Dictionary<Type, SComponent>>();
                        foreach (var c in cs)
                        {
                            _components[c.Key] = c.Value;
                            RigisterComponent(c.Value, c.Key);
                        }
                        cs.Clear();
                        ObjectPool.Return(cs);
                    }
                }
#if UNITY_EDITOR
                objChange?.Invoke();
#endif
            }
        }

        /// <summary>
        /// 服务器生成的ID
        /// </summary>
        public sealed override long rpc { get; }

        /// <summary>
        /// 随机生成的ID
        /// </summary>
        public sealed override long gid { get; }

        /// <summary>
        /// 自用 表id
        /// </summary>
        public long tid { get; set; }

        /// <summary>
        /// 事件监听
        /// </summary>
        public override bool EventEnable
        {
            get => _eventEnable;
            set
            {
                if (_eventEnable == value) return;
                _eventEnable = value;
                if (value)
                {
                    foreach (var c in _components.Values)
                        c.SetChange();
                }
            }
        }

        public bool TimerEnable { get; set; } = true;

        /// <summary>
        /// 父节点
        /// </summary>
        public SObject Parent { get; internal set; }

        /// <summary>
        /// 根
        /// </summary>
        public sealed override SObject Root
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
                SObject parent = this.Parent;
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
                    return -1;
                return Parent.GetChildIndex(this);
            }
        }

        /// <summary>
        /// dispose 监听
        /// </summary>
        public Eventer onDispose => _onDispose ??= new Eventer(this);

        public override void AcceptedEvent() { }

        public virtual void AddChild(SObject child) { throw new NotSupportedException(); }
        public virtual void Remove(SObject child) { }
        public virtual int GetChildIndex(SObject child) => -1;
        public T As<T>() where T : SObject => this as T;

        public T AddComponent<T>() where T : SComponent, new()
        {
            if (_components.TryGetValue(typeof(T), out var c))
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
            if (_components.TryGetValue(typeof(T), out var cc))
            {
                Loger.Error($"已经包含 component={cc}");
                return (T)cc;
            }
            AddComponentInternal(c);
            return c;
        }
        public SComponent AddComponent(Type type)
        {
            if (typeof(SComponent).IsAssignableFrom(type))
            {
                Loger.Error($"{type}不是{nameof(SComponent)}组件");
                return null;
            }
            if (_components.TryGetValue(type, out var c))
                return c;
            c = (SComponent)Activator.CreateInstance(type);
            return AddComponentInternal(c);
        }
        public T GetComponent<T>() where T : SComponent
        {
            if (this.Disposed)
            {
                Loger.Error($"实体已经销毁 entity={this}");
                return default;
            }
            if (_components.TryGetValue(typeof(T), out var c))
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
            if (_components.TryGetValue(type, out var c))
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
            if (_components.TryGetValue(typeof(T), out var cc))
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
            if (_components.TryGetValue(type, out c))
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
            return _components.ContainsKey(typeof(T));
        }
        public bool HasComponent(Type type)
        {
            if (this.Disposed)
            {
                Loger.Error($"实体已经销毁 entity={this}");
                return false;
            }
            return _components.ContainsKey(type);
        }

        internal SComponent AddComponentInternal(SComponent c)
        {
            if (typeof(SObject).IsAssignableFrom(c.GetType()))
            {
                Loger.Error($"不能挂载实体组件 ={c.GetType()}");
                return default;
            }
            c.Entity = this;
            _components[c.GetType()] = c;

            if (_world != null)
                RigisterComponent(c, c.GetType());
            
            return c;
        }
        void RigisterComponent(SComponent c, Type type)
        {
            World.ObjectManager.AddComponent(c, type);
            if (c is not SObject)
                World.Event.RigisteEvent(c);

            World.System.RigisterHandler(type, c);
#if UNITY_EDITOR
            if (World.Event.GetEventQueue(typeof(Awake<>).MakeGenericType(type), out var queue))
                _Awake.AddRange(queue);
#endif
            World.Event.RunGenericEvent(typeof(Awake<>), c, type);
            if (c.Disposed) return;
            World.System.In(type, this);
            c.SetChange();
        }
        internal bool RemoveComponentInternal(Type type)
        {
            if (typeof(SObject).IsAssignableFrom(type))
            {
                Loger.Error($"不能卸载实体组件 ={type}");
                return default;
            }
            if (!_components.TryGetValue(type, out var c))
                return false;
            c.Dispose();
            return true;
        }
        internal void RemoveFromComponents(SComponent c)
        {
            _components.Remove(c.GetType());
        }

        /// <summary>
        /// 释放
        /// </summary>
        public override void Dispose()
        {
            if (this.Disposed)
            {
                Loger.Error("重复Dispose->" + this);
                return;
            }
            World.ObjectManager.Remove(this);
            World.Event.RemoveEvent(this);
            if (this._timerRigisterd)
                World.Timer.RemoveTimer();

            if (this.Parent != null)
                this.Parent.Remove(this);

            var outHandles = ObjectPool.Get<Dictionary<Type, __OutHandle>>();
            foreach (var item in _components)
                World.System.Out(item.Key, this, outHandles);

            this.dispose(false);

            var tmp = _components;
            _components = null;

            foreach (var item in tmp)
            {
                if (item.Value is not SObject)
                {
                    item.Value.dispose(false);
                    World.Event.RemoveEvent(item.Value);
                }
                World.Event.RunGenericEvent(typeof(Dispose<>), item.Value, item.Key);
            }
            tmp.Clear();
            ObjectPool.Return(tmp);

            foreach (var item in outHandles)
                item.Value.Invoke(this);
            outHandles.Clear();
            ObjectPool.Return(outHandles);

            _onDispose?.Call();
#if UNITY_EDITOR
            objChange?.Invoke();
#endif
        }

        public override string ToString()
        {
            if (this.rpc != 0) return $"{{rpc={this.rpc} gid={this.gid} {this.GetType().FullName}}}";
            else return $"{{gid={this.gid} {this.GetType().FullName}}}";
        }
    }
}