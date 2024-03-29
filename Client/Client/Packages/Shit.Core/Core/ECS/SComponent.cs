using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public abstract class SComponent : IDispose
    {
        bool _enable = true;
        internal List<__ChangeHandle> _changeHandles;
        internal List<__KVWatcher> _kvWatcherHandles;
        internal bool _setChanged = false;

        public SObject Entity { get; internal set; }
        public bool Disposed { get; internal set; }

        public World World => this.Entity.World;
        /// <summary>
        /// 服务器生成的ID
        /// </summary>
        public long rpc => this.Entity.rpc;

        /// <summary>
        /// 自增生成的ID
        /// </summary>
        public long gid => this.Entity.gid;
        public STree Root => this.Entity.World.Root;

        public bool Enable
        {
            get => _enable;
            set
            {
                if (_enable == value) return;
                _enable = value;
                if (value)
                    this.SetChange();
                World.System.Enable(this);
            }
        }

        public void SetChange()
        {
            if (_changeHandles == null || _setChanged || !_enable) return;
            _setChanged = true;
            World.System.Change(this);
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
                    _changeHandles[i].AddToRemoveWait();
            }
            if (_kvWatcherHandles != null)
            {
                for (int i = 0; i < _kvWatcherHandles.Count; i++)
                    _kvWatcherHandles[i].Dispose();
            }

            if (!isDisposeObject)
                Entity.RemoveFromComponents(this);
            
            World.System.Dispose(this.GetType(), this);
        }
    }
}
