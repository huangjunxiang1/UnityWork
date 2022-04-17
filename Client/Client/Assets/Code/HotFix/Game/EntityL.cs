using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    class EntityL
    {
        public EntityL() : this(++idValue)
        {
           
        }
        public EntityL(long id)
        {
            this.ID = id;
            if (this.AutoRigisterEvent)
                this.ListenerEnable = true;
        }

        static long idValue;

        TaskAwaiterCreater taskCreater;
        bool listenerEnable = false;

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
        public virtual bool AutoRigisterEvent { get; } = true;

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
                        SysEvent.RigisterListener(this);
                        listenerEnable = true;
                    }
                }
                else
                {
                    if (listenerEnable)
                    {
                        SysEvent.RemoveListener(this);
                        listenerEnable = false;
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
                SysEvent.RemoveListener(this);
            taskCreater?.Dispose();
        }
    }
}
