using Core;
using game;
using System;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    public class GameInputComponent : SComponent
    {
        float2 last;
        PlayerControl ctr;

        [Event]
        static void Awake(Awake<GameInputComponent> t)
        {
            t.t.ctr = new();
            t.t.ctr.PlayerMove.performed += t.t.input;
            t.t.ctr.PlayerMove.canceled += t.t.cancel;
        }
        [Event]
        static void Dispose(Dispose<GameInputComponent> t) => t.t.ctr.Dispose();

        void input(UnityEngine.InputSystem.InputAction.CallbackContext e)
        {
            setDir(e.ReadValue<Vector2>());
        }
        void cancel(UnityEngine.InputSystem.InputAction.CallbackContext e)
        {
            setDir(0);
        }

        void setDir(float2 dir)
        {
            if (math.any(dir != last))
            {
                last = dir;
                C2S_SyncTransform p = new();
                p.dir = dir;
                NetComponent.Inst.Send(p);
                this.SetChange();
            }
        }
       
        [Event]
        static void watcher(EventWatcher<S2C_SyncTransform, TransformComponent, PlayingComponent> t)
        {
            t.t2.position = t.t.p;
            t.t2.rotation = t.t.r;
            t.t3.Play(t.t.isMoving ? "RUN00_F" : "WAIT00");
        }
    }
}
