using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main;
using UnityEngine;

namespace Game
{
    public class EntityM : IDisposable
    {
        public EntityM() : this(++idValue)
        {

        }
        public EntityM(long id)
        {
            this.ID = id;
            SysEvent.RigisterListener(this);
        }

        static long idValue;

        TaskAwaiterCreater taskCreater;

        /// <summary>
        /// ID  可自定义赋值
        /// </summary>
        public long ID { get; }

        /// <summary>
        /// 是否已被销毁
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        /// 异步拯救者 - =|
        /// </summary>
        public TaskAwaiterCreater TaskCreater
        {
            get { return taskCreater ??= new TaskAwaiterCreater(); }
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
            SysEvent.RemoveListener(this);
            taskCreater?.Dispose();
        }
    }
}
