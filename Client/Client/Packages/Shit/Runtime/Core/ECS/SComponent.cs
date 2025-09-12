using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public abstract class SComponent : IEvent
    {
        bool _enable = true;
        internal List<ComponentFilter> _Handles = ObjectPool.Get<List<ComponentFilter>>();

        public virtual SObject Entity { get; internal set; }
        public bool Disposed { get; internal set; }

        public virtual World World
        {
            get => this.Entity.World;
            internal set
            {
                Loger.Error($"{nameof(SComponent)} can not set {nameof(World)}");
            }
        }

        /// <summary>
        /// 服务器生成的ID
        /// </summary>
        public virtual long ActorId
        {
            get => this.Entity.ActorId;
            init => throw new NotSupportedException();
        }

        /// <summary>
        /// 自增生成的ID
        /// </summary>
        public virtual long gid => this.Entity.gid;
        public virtual STree CrucialRoot => this.Entity.CrucialRoot;

        [ShowInInspector]
        [PropertyOrder(-100)]
        public virtual bool Enable
        {
            get => _enable;
            set
            {
                if (_enable == value || this.Disposed) return;
                bool enable = this.Enable;
                _enable = value;
                for (int i = 0; i < this._Handles.Count; i++)
                    this._Handles[i].EnableCounter += value ? -1 : 1;
                if (World != null)
                {
                    if (Thread.CurrentThread.ManagedThreadId != this.World.Thread.threadId)
                    {
                        Loger.Error($"canot {nameof(Enable)} in other thread");
                        return;
                    }
                    if (this.Enable)
                        this.SetChangeFlag();
                    if (!enable && this.Enable)
                        World.System.In(this.GetType(), this);
                    if (enable && !this.Enable)
                        World.System.Out(this.GetType(), this);
                }
            }
        }

        public virtual bool EventEnable { get => Entity.EventEnable; set => throw new NotSupportedException(); }

        public virtual void SetChange()
        {
            if (this.Disposed || !_enable || World == null) return;
            if (Thread.CurrentThread.ManagedThreadId != this.World.Thread.threadId)
            {
                Loger.Error($"canot {nameof(SetChange)} in other thread");
                return;
            }
            this.World.System.Change(this, true);
        }
        public virtual void SetChangeFlag()
        {
            if (this.Disposed || !_enable || World == null) return;
            this.World.System.Change(this, false);
        }
        public virtual void Dispose()
        {
            if (this.Disposed)
            {
                Loger.Error("重复Dispose->" + this);
                return;
            }
            if (Thread.CurrentThread.ManagedThreadId != this.World.Thread.threadId)
            {
                Loger.Error($"canot {nameof(Dispose)} in other thread");
                return;
            }

            World.System.UnRigisterHandler(this.GetType(), this);
            World.Event.RemoveEvent(this);
            this.Disposed = true;//
            World.System.Out(this.GetType(), this);
            this.dispose(false);
        }
        public override string ToString() => $"this={base.ToString()} from={(Entity == null ? "Null" : Entity.ToString())}";

        internal void dispose(bool isDestroyEntity)
        {
            this.Disposed = true;

            for (int i = 0; i < _Handles.Count; i++)
            {
                _Handles[i].Disposed = true;
                if (!isDestroyEntity) _Handles[i]._handle_waitRemove(this.World.System.waitRemove);
            }

            _Handles.Clear();
            ObjectPool.Return(_Handles);
            _Handles = null;

            if (!isDestroyEntity)
                Entity.RemoveFromComponents(this);
        }

        public virtual void AcceptedEvent()
        {
            this.SetChangeFlag();
        }
    }
}
