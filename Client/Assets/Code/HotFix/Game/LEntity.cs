using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class LEntity
    {
        public LEntity() : this(++idValue)
        {

        }
        public LEntity(long id)
        {
            this.ID = id;
            SysEvent.RigisterListener(this);
        }

        static long idValue;

        List<TaskCompletionSource<IMessage>> _taskLst;

        /// <summary>
        /// ID  可自定义赋值
        /// </summary>
        public long ID { get; }

        /// <summary>
        /// 是否已被销毁
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        /// 释放
        /// </summary>
        public virtual void Dispose()
        {
            this.Disposed = true;
            SysEvent.RemoveListener(this);
            if (_taskLst != null)
            {
                int len = _taskLst.Count;
                for (int i = 0; i < len; i++)
                    _taskLst[i].TrySetCanceled();
            }
        }

        /// <summary>
        /// 每个类里面调用基类异步 方便做释放的管理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected Task<IMessage> SendAsync(IRequest request)
        {
            return SendAsync(0, request);
        }
        protected Task<IMessage> SendAsync(long actorId, IRequest request)
        {
            if (_taskLst == null) _taskLst = new List<TaskCompletionSource<IMessage>>();
            var task = SysNet.SendAsync(actorId, request);
            _taskLst.Add(task);
            return task.Task;
        }
    }
}
