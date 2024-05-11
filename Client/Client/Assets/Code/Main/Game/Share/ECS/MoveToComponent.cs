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

        [Event]
        static void Update(Update<MoveToComponent, TransformComponent, KVComponent> t)
        {
            var speed = t.t3.Get((int)KType.MoveSpeed);
            var speed2 = t.t3.Get((int)KType.RotateSpeed);
            t.t2.position = math.lerp(t.t2.position, t.t.point, t.t.World.DeltaTime * speed);
            t.t2.rotation = math.slerp(t.t2.rotation, t.t.rotation, t.t.World.DeltaTime * speed2);
        }
    }
}
