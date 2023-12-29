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
        public SObject(long cid)
        {
            this.gid = IDGenerate.GenerateID();
            this.cid = cid;
            if (!this.GetType().IsDefined(typeof(DisableAutoRegisteredEventAttribute), true))
                this.EventEnable = true;
            if (cid != 0)
            {
                if (!this.GetType().IsDefined(typeof(DisableAutoRegisteredRPCEventAttribute), true))
                    this.RigisteRPCEvent(cid);
            }
            if (!this.GetType().IsDefined(typeof(DisableAutoRegisteredTimerAttribute), true))
                _timerRigisterd = STimer.AutoRigisterTimer(this);
            ThreadSynchronizationContext.Instance.PostNext(() => SGameM.Event.RunEvent(new EC_NewSObject { obj = this }));
        }

        bool _eventEnable = false;
        bool _rpcEventEnable = false;
        long _rpcid;
        bool _timerRigisterd = false;
        Dictionary<Type, SComponent> components = new();

        public long value;
        public object data;

        /// <summary>
        /// 自增生成的ID
        /// </summary>
        public long gid { get; }

        /// <summary>
        /// 自定义ID
        /// </summary>
        public long cid { get; }

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
                        SGameM.Event.RigisteEvent(this);
                    }
                }
                else
                {
                    if (_eventEnable)
                    {
                        _eventEnable = false;
                        SGameM.Event.RemoveEvent(this);
                    }
                }
            }
        }


        public T AddComponent<T>() where T : SComponent, new()
        {
            if (components.TryGetValue(typeof(T), out var c))
            {
                Loger.Error($"已经包含 component={typeof(T)}");
                return (T)c;
            }
            c = components[typeof(T)] = new T() { Entity = this };
            SSystem.RigisteComponent(c);
            SGameM.Event.RigisteEvent(c);
            if (this.cid > 0)
                SGameM.Event.RigisteRPCEvent(this.cid, c); 
            SSystem.Run<AwakeAttribute>(c);
            return (T)c;
        }
        public T AddComponent<T>(T c) where T : SComponent
        {
            if (c.Entity != null)
            {
                Loger.Error($"已经添加到实体 component={c}");
                return c;
            }
            if (components.ContainsKey(typeof(T)))
            {
                Loger.Error($"已经包含 component={c}");
                return c;
            }
            components[c.GetType()] = c;
            c.Entity = this;
            SSystem.RigisteComponent(c);
            SGameM.Event.RigisteEvent(c);
            if (this.cid > 0)
                SGameM.Event.RigisteRPCEvent(this.cid, c);
            SSystem.Run<AwakeAttribute>(c);
            return c;
        }
        public T GetComponent<T>() where T : SComponent, new()
        {
            if (!components.TryGetValue(typeof(T), out var c))
                Loger.Error($"未包含 component={typeof(T)}");
            return (T)c;
        }
        public bool RemoveComponent<T>() where T : SComponent
        {
            if (!components.TryGetValue(typeof(T), out var c))
            {
                Loger.Error($"未包含 component={typeof(T)}");
                return false;
            }
            c.Dispose();
            return true;
        }
        public bool RemoveComponent(SComponent c)
        {
            if (!components.ContainsKey(c.GetType()))
            {
                Loger.Error($"未包含 component={c.GetType()}");
                return false;
            }
            c.Dispose();
            return true;
        }
        internal void UnPack(SComponent c)
        {
            components.Remove(c.GetType());
            c.Entity = null;
        }
        public bool HasComponent<T>() where T : SComponent
        {
            return components.ContainsKey(typeof(T));
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

            foreach (var item in components.Values)
                item.Dispose();
            this.Disposed = true;
            if (_eventEnable)
                SGameM.Event.RemoveEvent(this);
            if (_rpcEventEnable)
                SGameM.Event.RemoveRPCEvent(_rpcid, this);
            if (_timerRigisterd)
                STimer.AutoRemoveTimer(this);
            ThreadSynchronizationContext.Instance.PostNext(() => SGameM.Event.RunEvent(new EC_DisposeSObject { obj = this }));
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
            SGameM.Event.RigisteRPCEvent(rpc, this);
        }
    }
}
