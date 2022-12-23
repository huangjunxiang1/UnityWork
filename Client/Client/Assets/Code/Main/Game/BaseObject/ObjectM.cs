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
            this.GID = IDGenerate.GenerateID();
            this.CID = cid;
            Objects.Add(this.GID, this);
            if (!this.GetType().IsDefined(typeof(DisableAutoRegisteredEvent), true))
                this.ListenerEnable = true;
        }

        TaskAwaiterCreater taskCreater;
        bool listenerEnable = false;
        bool keyListenerEnable = false;
        long eventKey;

        /// <summary>
        /// 自增生成的ID
        /// </summary>
        public long GID { get; }

        /// <summary>
        /// 自定义ID
        /// </summary>
        public long CID { get; }

        /// <summary>
        /// 是否已被销毁
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        /// 事件监听
        /// </summary>
        public bool ListenerEnable
        {
            get => listenerEnable;
            set
            {
                if (value)
                {
                    if (!listenerEnable)
                    {
                        listenerEnable = true;
                        GameM.Event.RigisteListener(this);
                    }
                }
                else
                {
                    if (listenerEnable)
                    {
                        listenerEnable = false;
                        GameM.Event.RemoveListener(this);
                    }
                }
            }
        }

        /// <summary>
        /// 异步拯救者 - =|
        /// </summary>
        public TaskAwaiterCreater TaskCreater
        {
            get { return taskCreater ??= new(); }
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

            Objects.Remove(this.GID);
            this.Disposed = true;
            if (listenerEnable)
                GameM.Event.RemoveListener(this);
            if (keyListenerEnable)
                GameM.Event.RemoveKeyListener(eventKey, this);
            taskCreater?.Dispose();
        }

        protected void RigisteKeyListener()
        {
            if (CID == 0)
            {
                Loger.Error($"CID=0");
                return;
            }
            if (keyListenerEnable)
            {
                Loger.Error($"已经注册了key监听 key={CID}");
                return;
            }
            eventKey = CID;
            keyListenerEnable = true;
            GameM.Event.RigisteKeyListener(CID, this);
        }
        protected void RigisteKeyListener(long key)
        {
            if (key == 0)
            {
                Loger.Error($"key=0");
                return;
            }
            if (keyListenerEnable)
            {
                Loger.Error($"已经注册了key监听 key={key}");
                return;
            }
            eventKey = key;
            keyListenerEnable = true;
            GameM.Event.RigisteKeyListener(key, this);
        }
    }
}
