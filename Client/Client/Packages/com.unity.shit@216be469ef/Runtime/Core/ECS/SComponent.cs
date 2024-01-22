using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public abstract class SComponent
    {
        bool _enable = true;

        public SObject Entity { get; internal set; }
        public bool Disposed { get; private set; }
        public bool Enable
        {
            get => _enable;
            set
            {
                if (value == _enable) return;
                _enable = value;
                this.Change();
            }
        }

        public void MoveToEntity(SObject entity)
        {
            if (this.Disposed)
            {
                Loger.Error($"组件已经销毁 ={this}");
                return;
            }
            if (entity.Disposed)
            {
                Loger.Error($"实体已经销毁 ={entity}");
                return;
            }
            if (Entity == null)
            {
                Loger.Error($"组件已经不在实体 ={this}");
                return;
            }
            if (entity == Entity)
                return;
            this.dispose();
            this.Entity = null;
            this.Disposed = false;
            entity.AddComponent(this);
        }
        public void Change()
        {
            SSystem.SetChange(this);
        }
        public void Dispose()
        {
            if (this.Disposed)
            {
                Loger.Error("重复Dispose->" + this.GetType().FullName);
                return;
            }
            this.dispose();
        }
        internal void dispose(bool removeFromEntity = true)
        {
            this.Disposed = true;
            SSystem.UnRigisteComponent(this);
            if (removeFromEntity)
                Entity.RemoveFromComponents(this);
            SSystem.Run<DisposeAttribute>(this);
        }
    }
}
