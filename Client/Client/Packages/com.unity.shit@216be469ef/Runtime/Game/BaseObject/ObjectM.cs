﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main;
using UnityEngine;

namespace Game
{
    public abstract class ObjectM : IDisposable, IAsyncCancel
    {
        public ObjectM() : this(0) { }
        public ObjectM(long cid)
        {
            this.gid = IDGenerate.GenerateID();
            this.cid = cid;
            if (!this.GetType().IsDefined(typeof(DisableAutoRegisteredEventAttribute), true))
                this.ListenerEnable = true;
            if (cid != 0)
            {
                if (!this.GetType().IsDefined(typeof(DisableAutoRegisteredRPCEventAttribute), true))
                    this.RigisteRPCListener(cid);
            }
            if (!this.GetType().IsDefined(typeof(DisableAutoRegisteredTimerAttribute), true))
                _timerRigisterd = Timer.AutoRigisterTimer(this);
        }

        bool _eventListenerEnable = false;
        bool _rpcListenerEnable = false;
        long _rpcid;
        bool _timerRigisterd = false;

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
            get => _eventListenerEnable;
            set
            {
                if (value)
                {
                    if (!_eventListenerEnable)
                    {
                        _eventListenerEnable = true;
                        GameM.Event.RigisteListener(this);
                    }
                }
                else
                {
                    if (_eventListenerEnable)
                    {
                        _eventListenerEnable = false;
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
            if (_eventListenerEnable)
                GameM.Event.RemoveListener(this);
            if (_rpcListenerEnable)
                GameM.Event.RemoveRPCListener(_rpcid, this);
            if (_timerRigisterd)
                Timer.AutoRemoveTimer(this);
        }

        protected void RigisteRPCListener(long rpc)
        {
            if (rpc == 0)
            {
                Loger.Error($"key=0");
                return;
            }
            if (_rpcListenerEnable)
            {
                Loger.Error($"已经注册了key监听 key={rpc}");
                return;
            }
            _rpcid = rpc;
            _rpcListenerEnable = true;
            GameM.Event.RigisteRPCListener(rpc, this);
        }
    }
}