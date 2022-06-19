using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main;
using UnityEngine;

namespace Game
{
    public abstract class EntityM : IDisposable
    {
        public EntityM() : this(++GenerateID) { }
        public EntityM(long id)
        {
            this.ID = id;
            if (this.AutoRigisteEvent)
                this.ListenerEnable = true;
        }

        static long GenerateID;

        TaskAwaiterCreater taskCreater;
        bool listenerEnable = false;
        bool keyListenerEnable = false;
        long eventKey;

        /// <summary>
        /// ID  可自定义赋值
        /// </summary>
        public long ID { get; }

        /// <summary>
        /// 是否已被销毁
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        /// 自动注册事件监听
        /// </summary>
        public virtual bool AutoRigisteEvent { get; } = true;

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

            this.Disposed = true;
            if (listenerEnable)
                GameM.Event.RemoveListener(this);
            if (keyListenerEnable)
                GameM.Event.RemoveKeyListener(eventKey, this);
            taskCreater?.Dispose();
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
