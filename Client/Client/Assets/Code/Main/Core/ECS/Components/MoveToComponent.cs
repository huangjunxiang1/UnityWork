using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

namespace Game
{
    public class MoveToComponent : SComponent
    {
        [Sirenix.OdinInspector.ShowInInspector]
        quaternion _r = quaternion.identity;
        [Sirenix.OdinInspector.ShowInInspector]
        IList<float3> _paths;
        [Sirenix.OdinInspector.ShowInInspector]
        int _index = -1;
        [Sirenix.OdinInspector.ShowInInspector]
        int _endIndex;

        SValueTask<bool> _task;
        float3[] _pool = new float3[1];

        public float3 point
        {
            get => _paths != null && _paths.Count > 0 ? _paths[0] : 0;
            set
            {
                if (_paths == null || _paths.Count < 1) return;
                if (math.all(value == _paths[0])) return;
                _paths[0] = value;
                this.SetChangeFlag();
            }
        }
        public quaternion rotation
        {
            get => _r;
            set
            {
                if (math.all(value.value == _r.value)) return;
                _r = value;
                this.SetChangeFlag();
            }
        }
        [Sirenix.OdinInspector.ShowInInspector]
        public float3 forward
        {
            get => math.mul(_r, math.forward());
            set => rotation = quaternion.LookRotation(value, math.up());
        }

        public void MoveTo(float3 p, quaternion r)
        {
            this._pool[0] = p;
            this.setPaths(this._pool, r, 0, 0);
        }
        public void MoveTo(float3 p)
        {
            this._pool[0] = p;
            TransformComponent t = this.Entity.GetComponent<TransformComponent>();
            var r = quaternion.LookRotation(p - (t == null ? 0 : t.position), math.up());
            this.setPaths(this._pool, r, 0, 0);
        }
        public void MoveTo(IList<float3> ps, quaternion r, int startIndex = 0, int endIndex = -1)
        {
            if (endIndex == -1) endIndex = ps.Count - 1;
            this.setPaths(ps, r, startIndex, endIndex);
        }
        public void MoveTo(IList<float3> ps, int startIndex = 0, int endIndex = -1)
        {
            if (endIndex == -1) endIndex = ps.Count - 1;
            quaternion r;
            if (startIndex >= endIndex)
            {
                TransformComponent t = this.Entity.GetComponent<TransformComponent>();
                r = quaternion.LookRotation(ps[endIndex] - (t == null ? 0 : t.position), math.up());
            }
            else
                r = quaternion.LookRotation(ps[endIndex] - ps[endIndex - 1], math.up());
            this.setPaths(ps, r, startIndex, endIndex);
        }

        public SValueTask<bool> MoveToAsync(float3 p, quaternion r)
        {
            this._pool[0] = p;
            this.setPaths(this._pool, r, 0, 0, true);
            return _task;
        }
        public SValueTask<bool> MoveToAsync(float3 p)
        {
            this._pool[0] = p;
            TransformComponent t = this.Entity.GetComponent<TransformComponent>();
            var r = quaternion.LookRotation(p - (t == null ? 0 : t.position), math.up());
            this.setPaths(this._pool, r, 0, 0, true);
            return _task;
        }
        public SValueTask<bool> MoveToAsync(IList<float3> ps, quaternion r, int startIndex = 0, int endIndex = -1)
        {
            if (endIndex == -1) endIndex = ps.Count - 1;
            this.setPaths(ps, r, startIndex, endIndex, true);
            return _task;
        }
        public SValueTask<bool> MoveToAsync(IList<float3> ps, int startIndex = 0, int endIndex = -1)
        {
            if (endIndex == -1) endIndex = ps.Count - 1;
            quaternion r;
            if (startIndex >= endIndex)
            {
                TransformComponent t = this.Entity.GetComponent<TransformComponent>();
                r = quaternion.LookRotation(ps[endIndex] - (t == null ? 0 : t.position), math.up());
            }
            else
                r = quaternion.LookRotation(ps[endIndex] - ps[endIndex - 1], math.up());
            this.setPaths(ps, r, startIndex, endIndex, true);
            return _task;
        }

        public void Stop()
        {
            var old = this._task;
            this._task = default;
            this._index = -1;
            old.TrySetResult(false);
        }

        void setPaths(IList<float3> paths, quaternion r, int startIndex, int endIndex, bool newTask = false)
        {
            if (math.any(math.isnan(r.value)))
            {
                TransformComponent t = this.Entity.GetComponent<TransformComponent>();
                r = t == null ? this._r : t.rotation;
            }
            this._paths = paths;
            this._index = startIndex;
            this._endIndex = endIndex;
            this._r = r;
            var old = _task;
            _task = newTask ? SValueTask<bool>.Create() : default;
            old.TrySetResult(false);
        }

        [UpdateSystem]
        static void Update(MoveToComponent a, TransformComponent b, KVComponent c)
        {
            if (a._index == -1) return;
            var speed = c.Get((int)KType.MoveSpeed);
            var speed2 = c.Get((int)KType.RotateSpeed);
            float3 now = b.position;
            float3 next = a._paths[a._index];
            float moveStep = a.World.DeltaTime * speed;
            float distance = math.distance(next, now);
            while (distance < moveStep)
            {
                moveStep -= distance;
                if (a._index < a._endIndex)
                {
                    a._index++;
                    now = next;
                    next = a._paths[a._index];
                    distance = math.distance(next, now);
                }
                else
                {
                    moveStep = -1;
                    break;
                }
            }
            if (moveStep > 0)
            {
                var dir = math.normalize(next - now);
                var r = quaternion.LookRotation(dir, math.up());
                b.rotation = math.slerp(b.rotation, r, math.clamp(a.World.DeltaTime * speed2, 0, 1));
                b.position = now + dir * moveStep;
            }
            else
            {
                b.position = next;
                b.rotation = math.slerp(b.rotation, a.rotation, math.clamp(a.World.DeltaTime * speed2, 0, 1));
                if (math.abs(math.angle(b.rotation, a.rotation)) < 0.1f)
                {
                    var old = a._task;
                    a._task = default;
                    a._index = -1;
                    old.TrySetResult(true);
                }
            }
        }
        [DisposeSystem]
        static void dispose(MoveToComponent move)
        {
            move._task.TryCancel();
        }
    }
}
