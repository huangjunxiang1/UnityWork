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
        internal bool _setChanged = false;

        public SObject Entity { get; internal set; }
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
        public virtual long rpc => this.Entity.rpc;

        /// <summary>
        /// 自增生成的ID
        /// </summary>
        public virtual long gid => this.Entity.gid;
        public virtual SObject Root => this.Entity.Root;

        public bool Enable
        {
            get => _enable;
            set
            {
                if (_enable == value) return;
                _enable = value;
                if (World != null)
                {
                    if (value)
                        this.SetChange();
                    if (value)
                        World.System.In(this.GetType(), this.Entity);
                    else
                        World.System.Out(this.GetType(), this);
                }
            }
        }

        public virtual bool EventEnable { get => Entity.EventEnable; set => throw new NotSupportedException(); }

        public void SetChange()
        {
            if (_changeHandles == null || _setChanged || !_enable || World == null) return;
            _setChanged = true;
            for (int i = 0; i < _changeHandles.Count; i++)
                World.System.AddToChangeWaitInvoke(_changeHandles[i]);
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
                    _changeHandles[i].AddToRemoveWait();
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
            this.SetChange();
        }
    }
}
