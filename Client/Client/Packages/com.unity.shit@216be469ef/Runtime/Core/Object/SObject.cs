using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main;
using UnityEngine;

namespace Game
{
    public abstract class SObject : IDisposable, IAsyncCancel
    {
        public SObject() : this(0) { }
        public SObject(long rpc)
        {
            this.gid = ++generateId;
            this.rpc = rpc;
            gidMap.Add(gid, this);
            if (!this.GetType().IsDefined(typeof(DisableAutoRegisteredEventAttribute), true))
                this.EventEnable = true;
            if (rpc != 0)
            {
                if (!this.GetType().IsDefined(typeof(DisableAutoRegisteredRPCEventAttribute), true))
                    this.RigisteRPCEvent(rpc);
                if (!rpcMap.TryGetValue(rpc, out var o))
                    rpcMap.Add(rpc, this);
                else
                    Loger.Error($"已经创建rpc对象 rpc={rpc} obj={o}");
            }
            if (!this.GetType().IsDefined(typeof(DisableAutoRegisteredTimerAttribute), true))
                _timerRigisterd = STimer.AutoRigisterTimer(this);
            GameM.Event.RunEvent(new EC_NewSObject { obj = this });
        }

        static long generateId = 0;
        static Dictionary<long, SObject> rpcMap = new();
        static Dictionary<long, SObject> gidMap = new();

        bool _eventEnable = false;
        bool _rpcEventEnable = false;
        long _rpcid;
        bool _timerRigisterd = false;
        internal Dictionary<Type, SComponent> components = new();

        public long value;
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
                if (value)
                {
                    if (!_eventEnable)
                    {
                        _eventEnable = true;
                        GameM.Event.RigisteEvent(this);
                    }
                }
                else
                {
                    if (_eventEnable)
                    {
                        _eventEnable = false;
                        GameM.Event.RemoveEvent(this);
                    }
                }
            }
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
                Loger.Error($"有AddComponentIf标记的组件 不能手动添加 c={c}");
                return null;
            }
            if (components.TryGetValue(typeof(T), out var cc))
            {
                Loger.Error($"已经包含 component={cc}");
                return (T)cc;
            }
            addComponent(c);
            return c;
        }
        public SComponent AddComponent(Type type)
        {
            if (SSystem.isAutoAddComponent(type))
            {
                Loger.Error($"有AddComponentIf标记的组件 不能手动添加 c={type}");
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
            if (components.TryGetValue(type, out var cc))
            {
                c = cc;
                return true;
            }
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
                Loger.Error($"有AddComponentIf标记的组件 不能手动添加 c={typeof(T)}");
                return false;
            }
            if (!components.TryGetValue(typeof(T), out var c))
            {
                Loger.Error($"未包含 component={typeof(T)}");
                return false;
            }
            c.Dispose();
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
                Loger.Error($"有AddComponentIf标记的组件 不能手动添加 c={type}");
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

        internal SComponent AddComponentInternal(Type type)
        {
            if (components.TryGetValue(type, out var c))
                return c;
            c = (SComponent)Activator.CreateInstance(type);
            return addComponent(c);
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
                Loger.Error("重复Dispose->" + this.GetType().FullName);
                return;
            }

            this.Disposed = true;
            if (rpc > 0)
                rpcMap.Remove(rpc);
            gidMap.Remove(gid);
            foreach (var item in components.Values)
                item.dispose(false);
            components.Clear();
            if (_eventEnable)
                GameM.Event.RemoveEvent(this);
            if (_rpcEventEnable)
                GameM.Event.RemoveRPCEvent(_rpcid, this);
            if (_timerRigisterd)
                STimer.AutoRemoveTimer(this);
            GameM.Event.RunEvent(new EC_DisposeSObject { obj = this });
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

        SComponent addComponent(SComponent c)
        {
            c.Entity = this;
            components[c.GetType()] = c;
            SSystem.RigisteComponent(c);
            SSystem.Run<AwakeAttribute>(c);
            c.Change();
            return c;
        }
        protected void RigisteRPCEvent(long rpc)
        {
            if (rpc == 0)
            {
                Loger.Error($"key=0");
                return;
            }
            if (_rpcEventEnable)
            {
                Loger.Error($"已经注册了key监听 key={rpc}");
                return;
            }
            _rpcid = rpc;
            _rpcEventEnable = true;
            GameM.Event.RigisteRPCEvent(rpc, this);
        }
    }
}
