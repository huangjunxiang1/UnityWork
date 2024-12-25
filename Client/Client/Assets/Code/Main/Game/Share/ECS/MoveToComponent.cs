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
        float3[] _paths;
        [Sirenix.OdinInspector.ShowInInspector]
        int _index;

        STask<bool> _task;
        float3[] _pool = new float3[1];

        public float3 point
        {
            get => _paths[0];
            set
            {
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
            this._paths = _pool;
            this._index = 0;
            this._r = r;
            var old = _task;
            _task = null;
            old?.TrySetResult(false);
        }
        public void MoveTo(float3 p)
        {
            this._pool[0] = p;
            this._paths = _pool;
            this._index = 0;
            var old = _task;
            _task = null;
            old?.TrySetResult(false);
        }
        public void MoveTo(float3[] ps, quaternion r)
        {
            this._paths = ps;
            this._index = 0;
            this._r = r;
            var old = _task;
            _task = null;
            old?.TrySetResult(false);
        }
        public void MoveTo(float3[] ps)
        {
            this._paths = ps;
            this._index = 0;
            var old = _task;
            _task = null;
            old?.TrySetResult(false);
        }
        public STask<bool> MoveToAsync(float3 p, quaternion r)
        {
            this._pool[0] = p;
            this._paths = _pool;
            this._index = 0;
            this._r = r;
            var old = _task;
            _task = new();
            old?.TrySetResult(false);
            return _task;
        }
        public STask<bool> MoveToAsync(float3 p)
        {
            this._pool[0] = p;
            this._paths = _pool;
            this._index = 0;
            var old = _task;
            _task = new();
            old?.TrySetResult(false);
            return _task;
        }
        public STask<bool> MoveToAsync(float3[] ps, quaternion r)
        {
            this._paths = ps;
            this._index = 0;
            this._r = r;
            var old = _task;
            _task = new();
            old?.TrySetResult(false);
            return _task;
        }
        public STask<bool> MoveToAsync(float3[] ps)
        {
            this._paths = ps;
            this._index = 0;
            var old = _task;
            _task = new();
            old?.TrySetResult(false);
            return _task;
        }

        [Event]
        static void In(In<MoveToComponent, TransformComponent> t)
        {
            t.t._paths = t.t._pool;
            t.t._index = 0;
            t.t.point = t.t2.position;
            t.t._r = t.t2.rotation;
        }
        [Event]
        static void Update(Update<MoveToComponent, TransformComponent, KVComponent> t)
        {
            var speed = t.t3.Get((int)KType.MoveSpeed);
            var speed2 = t.t3.Get((int)KType.RotateSpeed);
            float3 now = t.t2.position;
            float3 next = t.t._paths[t.t._index];
            float moveStep = t.t.World.DeltaTime * speed;
            float distance = math.distance(next, now);
            while (distance < moveStep)
            {
                moveStep -= distance;
                if (t.t._index < t.t._paths.Length - 1)
                {
                    t.t._index++;
                    now = next;
                    next = t.t._paths[t.t._index];
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
                t.t2.rotation = math.slerp(t.t2.rotation, r, math.clamp(t.t.World.DeltaTime * speed2, 0, 1));
                t.t2.position = now + dir * moveStep;
            }
            else
            {
                t.t2.position = next;
                t.t2.rotation = math.slerp(t.t2.rotation, t.t.rotation, math.clamp(t.t.World.DeltaTime * speed2, 0, 1));
                var old = t.t._task;
                t.t._task = null;
                old?.TrySetResult(true);
            }
        }
    }
}
