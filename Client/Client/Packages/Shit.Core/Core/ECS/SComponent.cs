using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public abstract class SComponent : IDispose, IEvent
    {
        bool _enable = true;
        bool _eventEnable = true;
        internal List<__ChangeHandle> _changeHandles;
        internal bool _setChanged = false;

        public SObject Entity { get; internal set; }
        public bool Disposed { get; private set; }

        public bool Enable
        {
            get => _enable;
            set
            {
                if (_enable == value) return;
                _enable = value;
                if (value)
                    this.SetChange();
                Entity.world.System.Enable(this.GetType(), this);
            }
        }
        public bool EventEnable
        {
            get => _eventEnable && Entity != null && Entity.EventEnable;
            set
            {
                if (_eventEnable == value) return;
                _eventEnable = value;
                if (value)
                    this.SetChange();
            }
        }
        void IEvent.AcceptEventHandler(bool isInvokeMethod)
        {
            if (!isInvokeMethod)
                this.SetChange();
        }

        public void SetChange()
        {
            if (_setChanged || !_enable) return;
            _setChanged = true;
            Entity.world.System.SetChange(this);
        }
        public void Dispose()
        {
            if (this.Disposed)
            {
                Loger.Error("重复Dispose->" + this);
                return;
            }
            this.dispose(false);
        }
        public override string ToString() => $"this={base.ToString()} from={(Entity == null ? "Null" : Entity.ToString())}";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">0=主动dispose 1=entity dispose</param>
        internal void dispose(bool isDisposeObject)
        {
            this.Disposed = true;

            if (_changeHandles != null)
            {
                for (int i = 0; i < _changeHandles.Count; i++)
                    _changeHandles[i].Dispose();
                _changeHandles.Clear();
                ObjectPool.Return(_changeHandles);
            }

            this.Entity.world.Event.RemoveEvent(this);
            if (this.Entity.rpc != 0)
                this.Entity.world.Event.RemoveRPCEvent(this.Entity.rpc, this);

            if (!isDisposeObject)
            {
                Entity.RemoveFromComponents(this);
            }
            Entity.world.System.Dispose(this.GetType(), this);
        }
    }
}
