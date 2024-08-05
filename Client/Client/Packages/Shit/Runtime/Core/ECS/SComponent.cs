using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public abstract class SComponent : IEvent
    {
        bool _enable = true;
        internal List<__ChangeHandle> _changeHandles;
        internal List<__KVWatcher> _kvWatcherHandles;

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
        public virtual long rpc
        {
            get => this.Entity.rpc;
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
                if (_enable == value) return;
                bool enable = this.Enable;
                _enable = value;
                if (World != null)
                {
                    if (this.Enable)
                        this.SetChangeFlag();
                    if (!enable && this.Enable)
                        World.System.In(this.GetType(), this.Entity);
                    if (enable && !this.Enable)
                        World.System.Out(this.GetType(), this);
                }
            }
        }

        public virtual bool EventEnable { get => Entity.EventEnable; set => throw new NotSupportedException(); }

        public virtual void SetChange()
        {
            if (_changeHandles == null || !_enable || World == null) return;
            for (int i = 0; i < _changeHandles.Count; i++)
            {
                if (_changeHandles[i].Disposed) continue;
                _changeHandles[i].setInvokeWaiting = false;
                _changeHandles[i].Invoke();
            }
        }
        public virtual void SetChangeFlag()
        {
            if (_changeHandles == null || !_enable || World == null) return;
            int len = _changeHandles.Count;
            for (int i = 0; i < len; i++)
            {
                if (!_changeHandles[i].setInvokeWaiting)
                {
                    _changeHandles[i].setInvokeWaiting = true;
                    World.System.AddToChangeWaitInvoke(_changeHandles[i]);
                }
            }
        }
        public virtual void Dispose()
        {
            if (this.Disposed)
            {
                Loger.Error("重复Dispose->" + this);
                return;
            }
            World.Event.RemoveEvent(this);
            this.dispose(true);
            World.Event.RunGenericEvent(typeof(Dispose<>), this, this.GetType());
            World.System.Out(this.GetType(), this);
        }
        public override string ToString() => $"this={base.ToString()} from={(Entity == null ? "Null" : Entity.ToString())}";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">0=主动dispose 1=entity dispose</param>
        internal void dispose(bool RemoveFromComponents)
        {
            this.Disposed = true;

            if (_changeHandles != null)
            {
                for (int i = 0; i < _changeHandles.Count; i++)
                    _changeHandles[i].Dispose();
            }
            if (_kvWatcherHandles != null)
            {
                for (int i = 0; i < _kvWatcherHandles.Count; i++)
                    _kvWatcherHandles[i].Dispose();
            }

            if (RemoveFromComponents)
                Entity.RemoveFromComponents(this);
        }

        public virtual void AcceptedEvent()
        {
            this.SetChangeFlag();
        }

        internal void AddChangeHandler(__ChangeHandle c)
        {
            this._changeHandles ??= ObjectPool.Get<List<__ChangeHandle>>();
            this._changeHandles.Add(c);
            if (!c.setInvokeWaiting)
            {
                c.setInvokeWaiting = true;
                this.World.System.AddToChangeWaitInvoke(c);
            }
        }
    }
}
