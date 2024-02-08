using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public abstract class SComponent : IDispose, IEvent, ITimer
    {
        bool _enable = true;
        bool _eventEnable = true;
        bool _timerEnable = true;
        internal bool _timerRigisterd = false;
        internal bool _isChanged = false;

        public SObject Entity { get; internal set; }
        public bool Disposed { get; private set; }

        public bool Enable
        {
            get => _enable && !Disposed;
            set
            {
                if (_enable == value) return;
                _enable = value;
                if (value)
                    this.SetChange();
                var ps = ArrayCache<object>.Get(1);
                ps[0] = this;
                GameM.Event.RunEvent(Types.InstanceGenericObject(typeof(Enable<>), this.GetType(), ps));
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
        public bool TimerEnable { get => _timerEnable && Entity != null && Entity.TimerEnable; set => _timerEnable = value; }

        public void MoveToEntity(SObject entity)
        {
            if (this.Disposed)
            {
                Loger.Error($"组件已经销毁 ={this}");
                return;
            }
            if (entity == null || entity.Disposed)
            {
                Loger.Error($"实体已经销毁 ={entity}");
                return;
            }
            if (Entity == null)
            {
                Loger.Error($"不在实体上 ={this}");
                entity.AddComponentInternal(this);
                return;
            }
            if (SSystem.isAutoAddComponent(this.GetType()))
            {
                Loger.Error($"有AddComponentIf标记的组件 不能手动修改 c={this.GetType()}");
                return;
            }
            if (entity == Entity)
                return;
            var old = Entity;
            this.dispose(1);
            entity.AddComponentInternal(this, true);
            var ps = ArrayCache<object>.Get(2);
            ps[0] = this;
            ps[1] = old;
            GameM.Event.RunEvent(Types.InstanceGenericObject(typeof(Move<>), this.GetType(), ps));
        }
        public void SetChange()
        {
            if (_isChanged || !this.Enable) return;
            _isChanged = true;
            SSystem.SetChange(this);
        }
        public void Dispose()
        {
            if (this.Disposed)
            {
                Loger.Error("重复Dispose->" + this);
                return;
            }
            this.dispose(0);
        }
        public override string ToString() => $"this={base.ToString()} from={(Entity == null ? "Null" : Entity.ToString())}";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">0=主动dispose 1=move 2=entity dispose</param>
        internal void dispose(int type)
        {
            if (type == 0)
            {
                this.Disposed = true;
                SSystem.UnRigisteComponent(this);
                ((IEvent)this).RemoveAllEvent(this.Entity.rpc);
                if (this._timerRigisterd)
                    ((ITimer)this).RemoveTimer();
                Entity.RemoveFromComponents(this);
                var ps = ArrayCache<object>.Get(1);
                ps[0] = this;
                GameM.Event.RunEvent(Types.InstanceGenericObject(typeof(Dispose<>), this.GetType(), ps));
            }
            else if (type == 1)
            {
                SSystem.UnRigisteComponent(this);
                if (this.Entity.rpc != 0)
                    ((IEvent)this).RemoveRPCEvent(this.Entity.rpc);
                Entity.RemoveFromComponents(this);
            }
            else if (type == 2)
            {
                this.Disposed = true;
                ((IEvent)this).RemoveAllEvent(this.Entity.rpc);
                if (this._timerRigisterd)
                    ((ITimer)this).RemoveTimer();
                var ps = ArrayCache<object>.Get(1);
                ps[0] = this;
                GameM.Event.RunEvent(Types.InstanceGenericObject(typeof(Dispose<>), this.GetType(), ps));
            }
        }
    }
}
