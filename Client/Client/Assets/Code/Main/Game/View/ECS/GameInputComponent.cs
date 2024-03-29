using Core;
using game;
using System;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    public class GameInputComponent : SComponent
    {
        int2 last;
        [Event]
        static void update(Update<GameInputComponent> t)
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
                NetComponent.Inst.Send(p);
                t.t.SetChange();
            }
        }
        [Event]
        static void play(Change<MoveToComponent, PlayingComponent> t)
        {
            t.t2.Play("RUN00_F"); //t.t2.Play("WAIT01");
        }
        [Event]
        static void watcher(EventWatcher<S2C_SyncTransform, MoveToComponent> t)
        {
            t.t2.point = t.t.p;
            t.t2.rotation = t.t.r;
        }
    }
}
