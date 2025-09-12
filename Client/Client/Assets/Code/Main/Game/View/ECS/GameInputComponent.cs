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

        [InSystem]
        static void In(GameInputComponent t)
        {
            t.ctr = new();
            t.ctr.PlayerMove.performed += t.input;
            t.ctr.PlayerMove.canceled += t.cancel;
        }
        [OutSystem]
        static void Out(GameInputComponent t) => t.ctr.Dispose();

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
                this.SetChangeFlag();
            }
        }

        [EventWatcherSystem]
        static void watcher(S2C_SyncTransform a, TransformComponent b)
        {
            b.position = a.p;
            b.rotation = a.r;
        }
        [EventWatcherSystem]
        static void watcher(S2C_SyncTransform a, PlayingComponent c)
        {
            c.Play(a.isMoving ? "RUN00_F" : "WAIT00");
        }
    }
}
