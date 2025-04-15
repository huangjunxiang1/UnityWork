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
            this.Enable = true;
            var old = _task;
            _task = null;
            old?.TrySetResult(false);
        }
        public void MoveTo(float3 p)
        {
            this._pool[0] = p;
            this._paths = _pool;
            this._index = 0;
            TransformComponent t = this.Entity.GetComponent<TransformComponent>();
            this._r = quaternion.LookRotation(p - (t == null ? 0 : t.position), math.up());
            this.Enable = true;
            var old = _task;
            _task = null;
            old?.TrySetResult(false);
        }
        public void MoveTo(float3[] ps, quaternion r)
        {
            this._paths = ps;
            this._index = 0;
            this._r = r;
            this.Enable = true;
            var old = _task;
            _task = null;
            old?.TrySetResult(false);
        }
        public void MoveTo(float3[] ps)
        {
            this._paths = ps;
            this._index = 0;
            TransformComponent t = this.Entity.GetComponent<TransformComponent>();
            this._r = quaternion.LookRotation(ps[^1] - (t == null ? 0 : t.position), math.up());
            this.Enable = true;
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
            this.Enable = true;
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
            TransformComponent t = this.Entity.GetComponent<TransformComponent>();
            this._r = quaternion.LookRotation(p - (t == null ? 0 : t.position), math.up());
            this.Enable = true;
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
            this.Enable = true;
            var old = _task;
            _task = new();
            old?.TrySetResult(false);
            return _task;
        }
        public STask<bool> MoveToAsync(float3[] ps)
        {
            this._paths = ps;
            this._index = 0;
            TransformComponent t = this.Entity.GetComponent<TransformComponent>();
            this._r = quaternion.LookRotation(ps[^1] - (t == null ? 0 : t.position), math.up());
            this.Enable = true;
            var old = _task;
            _task = new();
            old?.TrySetResult(false);
            return _task;
        }

        [AwakeSystem]
        static void Awake(MoveToComponent t)
        {
            t.Enable = false;
        }
        [UpdateSystem]
        static void Update(MoveToComponent a, TransformComponent b, KVComponent c)
        {
            var speed = c.Get((int)KType.MoveSpeed);
            var speed2 = c.Get((int)KType.RotateSpeed);
            float3 now = b.position;
            float3 next = a._paths[a._index];
            float moveStep = a.World.DeltaTime * speed;
            float distance = math.distance(next, now);
            while (distance < moveStep)
            {
                moveStep -= distance;
                if (a._index < a._paths.Length - 1)
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
                    a.Enable = false;
                    var old = a._task;
                    a._task = null;
                    old?.TrySetResult(true);
                }
            }
        }
    }
}
