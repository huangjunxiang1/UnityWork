﻿using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core
{
    public class SObject : SComponent, ITimer
    {
        public SObject()
        {
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
        bool _view = true;
        internal Dictionary<Type, SComponent> _components = ObjectPool.Get<Dictionary<Type, SComponent>>();

        public double value;
        public object data;

#if UNITY_EDITOR
        //做检视面板使用的
        internal List<ComponentFilter> _In = new();
        internal List<ComponentFilter> _Out = new();
        internal List<ComponentFilter> _Other = new();
        internal HashSet<__SystemHandle> _EventWatcher = new();
        internal static Action objChange;
#endif

        public sealed override World World
        {
            get => _world;
            internal set
            {
                if (this.Disposed) return;
                if (_world != null)
                {
                    Loger.Error("canot change the world");
                    return;
                }
                if (value == null)
                {
                    Loger.Error("canot set null world");
                    return;
                }
                _world = value;
                value.ObjectManager.Add(this);
                value.Event.RigisteEvent(this);
                _timerRigisterd = value.Timer.RigisterTimer(this);

                if (!this.Disposed)
                {
                    var cs = _components;
                    _components = ObjectPool.Get<Dictionary<Type, SComponent>>();
                    foreach (var c in cs)
                    {
                        if (c.Value.Disposed) continue;
                        _components[c.Key] = c.Value;
                        RigisterComponent(c.Value, c.Key);
                    }
                    cs.Clear();
                    ObjectPool.Return(cs);
                }
#if UNITY_EDITOR
                if (objChange != null)
                    _world.Timer.Add(0, 1, objChange);
#endif
            }
        }

        public sealed override SObject Entity { get => base.Entity; internal set => base.Entity = value; }

        /// <summary>
        /// 服务器生成的ID
        /// </summary>
        [ShowInInspector]
        [PropertyOrder(-100)]
        public sealed override long ActorId { get; init; }

        /// <summary>
        /// 随机生成的ID
        /// </summary>
        [ShowInInspector]
        [PropertyOrder(-100)]
        public sealed override long gid { get; } = Util.RandomLong();

        /// <summary>
        /// 自用 表id
        /// </summary>
        [ShowInInspector]
        [PropertyOrder(-100)]
        public long tid { get; init; }

        /// <summary>
        /// 对象组
        /// </summary>
        [ShowInInspector]
        [PropertyOrder(-100)]
        public int Group { get; init; }

        /// <summary>
        /// 事件监听
        /// </summary>
        [ShowInInspector]
        [PropertyOrder(-100)]
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
                        c.SetChangeFlag();
                }
            }
        }

        [ShowInInspector]
        [PropertyOrder(-100)]
        public virtual bool TimerEnable { get; set; } = true;

        /// <summary>
        /// 父节点
        /// </summary>
        public STree Parent { get; internal set; }

        /// <summary>
        /// 关键节点
        /// </summary>
        [ShowInInspector]
        [PropertyOrder(-99)]
        public sealed override STree CrucialRoot
        {
            get
            {
                if (this is not STree root) root = this.Parent;
                while (root != null)
                {
                    if (root.isCrucialRoot) return root;
                    root = root.Parent;
                }
                return null;
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
                SObject s = this;
                while (s.Parent != null)
                {
                    layer++;
                    s = s.Parent;
                }
                return layer;
            }
        }

        /// <summary>
        /// 在关键节点的层级
        /// </summary>
        public int CrucialLayer
        {
            get
            {
                int layer = 0;
                STree root = this as STree;
                while (root != null && !root.isCrucialRoot)
                {
                    layer++;
                    root = root.Parent;
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

        /// <summary>
        /// 显示组件 是否显示
        /// </summary>
        [ShowInInspector]
        [PropertyOrder(-100)]
        public bool View
        {
            get => _view;
            set
            {
                if (_view == value) return;
                _view = value;

                List<SComponent> lst = ObjectPool.Get<List<SComponent>>();
                foreach (var c in _components.Values)
                {
                    if (c is ViewComponent)
                        lst.Add(c);
                }
                for (int i = 0; i < lst.Count; i++)
                {
                    if (!lst[i].Disposed)
                        lst[i].SetChangeFlag();
                }
                lst.Clear();
                ObjectPool.Return(lst);

                if (this is STree tree)
                {
                    List<SObject> lstOs = ObjectPool.Get<List<SObject>>();
                    lstOs.AddRange(tree.GetChildren());
                    for (int i = 0; i < lstOs.Count; i++)
                    {
                        if (lstOs[i].Parent != this)
                            continue;
                        lstOs[i].View = _view;
                    }
                    lstOs.Clear();
                    ObjectPool.Return(lstOs);
                }
            }
        }

        public override void AcceptedEvent() { }
        public sealed override void SetChange() => base.SetChange();
        public sealed override void SetChangeFlag() => base.SetChangeFlag();

        public virtual void AddChild(SObject child) { throw new NotSupportedException(); }
        public virtual void Remove(SObject child) { }
        public virtual int GetChildIndex(SObject child) => -1;
        public T As<T>() where T : SObject => this as T;
        public T GetRoot<T>() where T : SObject
        {
            var root = this;
            do
            {
                if (root is T) return (T)root;
                root = root.Parent;
            } while (root != null);
            return default;
        }

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
            return default;
        }
        public IEnumerable<T> GetComponentIfIs<T>() where T : SComponent
        {
            if (this.Disposed)
            {
                Loger.Error($"实体已经销毁 entity={this}");
                yield return null;
            }
            foreach (var c in _components.Values)
            {
                if (c is T)
                    yield return c as T;
            }
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
            return RemoveComponentInternal(typeof(T));
        }
        public bool RemoveComponent(Type type)
        {
            if (this.Disposed)
            {
                Loger.Error($"实体已经销毁 entity={this}");
                return false;
            }
            return RemoveComponentInternal(type);
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
            if (c is not SObject)
                World.Event.RigisteEvent(c);

            World.System.RigisterHandler(type, c);
            World.System.Awake(type, c);
            if (c.Disposed) return;
            if (c.Enable)
                World.System.In(type, c);
            c.SetChangeFlag();
        }
        internal bool RemoveComponentInternal(Type type)
        {
            if (typeof(SObject).IsAssignableFrom(type))
            {
                Loger.Error($"不能卸载实体组件 ={type}");
                return false;
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
                World.Timer.RemoveTimer(this);

            if (this.Parent != null)
                this.Parent.Remove(this);

            foreach (var item in _components)
                World.System.Out(item.Key, this);

            this.dispose(true);

            var tmp = _components;
            _components = null;

            foreach (var item in tmp)
            {
                World.System.UnRigisterHandler(item.Key, item.Value);
                if (item.Value is not SObject)
                {
                    World.Event.RemoveEvent(item.Value);
                    item.Value.dispose(true);
                }
                World.System.Dispose(item.Key, item.Value);
            }
            tmp.Clear();
            ObjectPool.Return(tmp);

            _onDispose?.Call();
#if UNITY_EDITOR
            if (objChange != null)
                _world.Timer.Add(0, 1, objChange);
#endif
        }

        public override string ToString()
        {
            if (this.ActorId != 0) return $"{{actorId={this.ActorId} gid={this.gid} {this.GetType().FullName}}}";
            else return $"{{gid={this.gid} {this.GetType().FullName}}}";
        }
    }
}
