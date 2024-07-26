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
        float3[] paths;
        [Sirenix.OdinInspector.ShowInInspector]
        int index;

        STask<bool> task;
        float3[] pool = new float3[1];
        bool moving = false;

        public float3 point
        {
            get => paths[0];
            set
            {
                if (math.all(value == paths[0])) return;
                paths[0] = value;
                this.SetChange();
            }
        }
        public quaternion rotation
        {
            get => _r;
            set
            {
                if (math.all(value.value == _r.value)) return;
                _r = value;
                this.SetChange();
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
            this.pool[0] = p;
            this.paths = pool;
            this.index = 0;
            this.rotation = r;
            this.moving = true;
            var old = task;
            task = null;
            old?.TrySetResult(false);
        }
        public void MoveTo(float3 p)
        {
            this.pool[0] = p;
            this.paths = pool;
            this.index = 0;
            this.moving = true;
            var old = task;
            task = null;
            old?.TrySetResult(false);
        }
        public void MoveTo(float3[] ps, quaternion r)
        {
            this.paths = ps;
            this.index = 0;
            this.rotation = r;
            this.moving = true;
            var old = task;
            task = null;
            old?.TrySetResult(false);
        }
        public void MoveTo(float3[] ps)
        {
            this.paths = ps;
            this.index = 0;
            this.moving = true;
            var old = task;
            task = null;
            old?.TrySetResult(false);
        }
        public STask<bool> MoveToAsync(float3 p, quaternion r)
        {
            this.pool[0] = p;
            this.paths = pool;
            this.index = 0;
            this.rotation = r;
            this.moving = true;
            var old = task;
            task = new();
            old?.TrySetResult(false);
            return task;
        }
        public STask<bool> MoveToAsync(float3 p)
        {
            this.pool[0] = p;
            this.paths = pool;
            this.index = 0;
            this.moving = true;
            var old = task;
            task = new();
            old?.TrySetResult(false);
            return task;
        }
        public STask<bool> MoveToAsync(float3[] ps, quaternion r)
        {
            this.paths = ps;
            this.index = 0;
            this.rotation = r;
            this.moving = true;
            var old = task;
            task = new();
            old?.TrySetResult(false);
            return task;
        }
        public STask<bool> MoveToAsync(float3[] ps)
        {
            this.paths = ps;
            this.index = 0;
            this.moving = true;
            var old = task;
            task = new();
            old?.TrySetResult(false);
            return task;
        }

        [Event]
        static void In(In<MoveToComponent, TransformComponent> t)
        {
            t.t.paths = t.t.pool;
            t.t.index = 0;
            t.t.point = t.t2.position;
            t.t.forward = t.t2.forward;
        }
        [Event]
        static void Update(Update<MoveToComponent, TransformComponent, KVComponent> t)
        {
            if (!t.t.moving) return;
            var speed = t.t3.Get((int)KType.MoveSpeed);
            var speed2 = t.t3.Get((int)KType.RotateSpeed);
            float3 now = t.t2.position;
            float3 p = t.t.paths[t.t.index];
            float moveStep = t.t.World.DeltaTime * speed;
            float distance = math.distance(p, now);
            while (distance < moveStep)
            {
                moveStep -= distance;
                if (t.t.index < t.t.paths.Length - 1)
                {
                    t.t.index++;
                    now = p;
                    p = t.t.paths[t.t.index];
                    distance = math.distance(p, now);
                }
                else
                {
                    moveStep = -1;
                    break;
                }
            }
            if (moveStep > 0)
            {
                var dir = math.normalize(p - now);
                var r = quaternion.LookRotation(dir, math.up());
                t.t2.rotation = math.slerp(t.t2.rotation, r, math.clamp(t.t.World.DeltaTime * speed2, 0, 1));
                t.t2.position = now + dir * moveStep;
            }
            else
            {
                t.t2.position = p;
                t.t2.rotation = math.slerp(t.t2.rotation, t.t.rotation, math.clamp(t.t.World.DeltaTime * speed2, 0, 1));
                t.t.moving = false;
                var old = t.t.task;
                t.t.task = null;
                old?.TrySetResult(true);
            }
        }
    }
}
