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
        float3 _t;
        public float3 Direction
        {
            get => _t;
            set
            {
                if (math.all(_t == value)) return;
                _t = value;
                this.SetChange();
            }
        }

        [Event]
        static void move(Update<MoveComponent, TransformComponent, AttributeComponent> t)
        {
            if (math.any(t.t.Direction != 0) && math.all(!math.isnan(t.t.Direction)))
            {
                var speed = t.t3.Get((int)AttributeID.MoveSpeed);
                var speed2 = t.t3.Get((int)AttributeID.RotateSpeed);
                t.t2.position += t.t.Direction * speed * t.t.World.DeltaTime;
                t.t2.rotation = math.slerp(t.t2.rotation, quaternion.LookRotation(t.t.Direction, math.up()), t.t.World.DeltaTime * speed2);
            }
        }
    }
}
