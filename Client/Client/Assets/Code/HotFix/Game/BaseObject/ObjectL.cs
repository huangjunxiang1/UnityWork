using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    abstract class ObjectL : IAsyncDisposed
    {
        public ObjectL() : this(0) { }
        public ObjectL(long cid)
        {
            this.gid = IDGenerate.GenerateID();
            this.cid = cid;
            if (!Types.HasDefineAttribute(this.GetType(), typeof(DisableAutoRegisteredEventAttribute)))
                this.ListenerEnable = true;
            if (cid != 0)
            {
                if (!Types.HasDefineAttribute(this.GetType(), typeof(DisableAutoRegisteredRPCEventAttribute)))
                    this.RigisteRPCListener(cid);
            }
            if (!Types.HasDefineAttribute(this.GetType(), typeof(DisableAutoRegisteredTimerAttribute)))
                Timer.AutoRigisterTimer(this);
        }

        bool _listenerEnable = false;
        bool _keyListenerEnable = false;
        long _eventKey;

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
        public bool ListenerEnable
        {
            get => _listenerEnable;
            set
            {
                if (value)
                {
                    if (!_listenerEnable)
                    {
                        _listenerEnable = true;
                        GameM.Event.RigisteListener(this);
                    }
                }
                else
                {
                    if (_listenerEnable)
                    {
                        _listenerEnable = false;
                        GameM.Event.RemoveListener(this);
                    }
                }
            }
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
            if (_listenerEnable)
                GameM.Event.RemoveListener(this);
            if (_keyListenerEnable)
                GameM.Event.RemoveRPCListener(_eventKey, this);
            Timer.AutoRemoveTimer(this);
        }

        protected void RigisteRPCListener(long key)
        {
            if (key == 0)
            {
                Loger.Error($"key=0");
                return;
            }
            if (_keyListenerEnable)
            {
                Loger.Error($"已经注册了key监听 key={_eventKey}");
                return;
            }
            _eventKey = key;
            _keyListenerEnable = true;
            GameM.Event.RigisteRPCListener(key, this);
        }
    }
}
