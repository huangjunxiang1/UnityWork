using Core;
using game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    class TransformGetMessageComponent : SComponent
    {
        [Event(RPC = true)] S2C_SyncTransform p = new();

        [Event]
        static void awake(Awake<SGameObject> t)
        {
            t.t.AddComponent<TransformGetMessageComponent>();
        }
      
        [Event]
        static void Update(Update<TransformGetMessageComponent, TransformComponent, AttributeComponent, PlayingComponent> t)
        {
            if (math.distancesq(t.t.p.p, t.t2.position) > 0.01f)
                t.t4.Play("RUN00_F");
            else
                t.t4.Play("WAIT01");
            var speed = t.t3.Get((int)AttributeID.MoveSpeed);
            var speed2 = t.t3.Get((int)AttributeID.RotateSpeed);
            t.t2.position = math.lerp(t.t2.position, t.t.p.p, t.t.World.DeltaTime * speed);
            t.t2.rotation = math.slerp(t.t2.rotation, quaternion.LookRotation(math.mul(t.t.p.r, math.forward()), math.up()), t.t.World.DeltaTime * speed2);
        }
    }
    public class TransformClientInputComponent : SComponent
    {
        int2 last;
        [Event]
        static void update(Update<TransformClientInputComponent> t)
        {
            int2 dir = default;
            if (Input.GetKey(KeyCode.W))
                dir.y += 1;
            if (Input.GetKey(KeyCode.S))
                dir.y += -1;
            if (Input.GetKey(KeyCode.D))
                dir.x += 1;
            if (Input.GetKey(KeyCode.A))
                dir.x += -1;
            if (math.any(dir != t.t.last))
            {
                t.t.last = dir;
                C2S_SyncTransform p = new();
                p.dir = dir;
                GameWorld.World.Net.Send(p);
            }
        }
    }
}
