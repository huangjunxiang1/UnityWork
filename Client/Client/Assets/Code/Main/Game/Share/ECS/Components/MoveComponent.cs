using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

namespace Game
{
    public class MoveComponent : SComponent
    {
        [Sirenix.OdinInspector.ShowInInspector]
        float3 _t;
        public float3 Direction
        {
            get => _t;
            set
            {
                if (math.all(_t == value)) return;
                _t = value;
                this.SetChangeFlag();
            }
        }

        [UpdateSystem]
        static void Update(MoveComponent a, TransformComponent b, KVComponent c)
        {
            if (math.any(a.Direction != 0) && math.all(!math.isnan(a.Direction)))
            {
                var speed = c.Get((int)KType.MoveSpeed);
                var speed2 = c.Get((int)KType.RotateSpeed);
                b.position += a.Direction * speed * a.World.DeltaTime;
                b.rotation = math.slerp(b.rotation, quaternion.LookRotation(a.Direction, math.up()), math.clamp(a.World.DeltaTime * speed2, 0, 1));
            }
        }
    }
}
