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
        float3 _p;
        [Sirenix.OdinInspector.ShowInInspector]
        quaternion _r = quaternion.identity;

        STask task;

        public float3 point
        {
            get => _p;
            set
            {
                if (math.all(value == _p)) return;
                _p = value;
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

        public STask MoveTo(float3 p, quaternion r)
        {
            this.point = p;
            this.rotation = r;
            return task = new();
        }
        public STask MoveTo(float3 p)
        {
            this.point = p;
            return task = new();
        }

        [Event]
        static void In(In<MoveToComponent, TransformComponent> t)
        {
            t.t.point = t.t2.position;
            t.t.forward = t.t2.forward;
        }
        [Event]
        static void Update(Update<MoveToComponent, TransformComponent, KVComponent> t)
        {
            var speed = t.t3.Get((int)KType.MoveSpeed);
            var speed2 = t.t3.Get((int)KType.RotateSpeed);
            float distance = math.distance(t.t.point, t.t2.position);
            float moveStep = t.t.World.DeltaTime * speed;
            if (distance > moveStep)
            {
                var dir = math.normalize(t.t.point - t.t2.position);
                t.t2.position += dir * moveStep;
                var r = quaternion.LookRotation(dir, math.up());
                t.t2.rotation = math.slerp(t.t2.rotation, r, t.t.World.DeltaTime * speed2);
            }
            else
            {
                t.t2.position = t.t.point;
                t.t2.rotation = math.slerp(t.t2.rotation, t.t.rotation, t.t.World.DeltaTime * speed2);
                var s = t.t.task;
                t.t.task = null;
                s?.TrySetResult();
            }
        }
    }
}
