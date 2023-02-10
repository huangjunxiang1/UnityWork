using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main;
using UnityEngine;

namespace Game
{
    public abstract class ObjectM : IDisposable
    {
        public ObjectM() : this(0) { }
        public ObjectM(long cid)
        {
            this.gid = IDGenerate.GenerateID();
            this.cid = cid;
            if (!this.GetType().IsDefined(typeof(DisableAutoRegisteredEvent), true))
                this.ListenerEnable = true;
            if (cid != 0)
            {
                if (!this.GetType().IsDefined(typeof(DisableAutoRegisteredKeyEvent), true))
                    this.RigisteKeyListener(cid);
            }
        }

        TaskManager _taskManager;
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
        /// 异步拯救者 - =|
        /// </summary>
        public TaskManager TaskManager
        {
            get
            {
                if (this.Disposed)
                    return null;
                if (_taskManager == null)
                    _taskManager = new TaskManager();
                return _taskManager;
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
                GameM.Event.RemoveKeyListener(_eventKey, this);
            _taskManager?.Dispose();
        }

        protected void RigisteKeyListener(long key)
        {
            if (key == 0)
            {
                Loger.Error($"key=0");
                return;
            }
            if (_keyListenerEnable)
            {
                Loger.Error($"已经注册了key监听 key={key}");
                return;
            }
            _eventKey = key;
            _keyListenerEnable = true;
            GameM.Event.RigisteKeyListener(key, this);
        }
    }
}
