using Core;
using Game;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

class PlayerComponent : SComponent
{
    public WorldScene scene;
    public int2 xy = int.MinValue;
    public int2 center = int.MinValue;

    PlayerControl ctr = new();
    PlayingComponent playing;
    MoveComponent move;

    [InSystem]
    static void In(PlayerComponent player, PlayingComponent playing, MoveComponent move, GameObjectComponent gc)
    {
        player.ctr.PlayerMove.performed += player.input;
        player.ctr.PlayerMove.canceled += player.cancel;
        player.playing = playing;
        player.move = move;
        BaseCamera.SetCamera(new LockingCamera(Camera.main.GetComponent<CinemachineBrain>()));
        BaseCamera.Current.Init(gc.gameRoot);
    }
    [OutSystem]
    static void Out(PlayerComponent player, PlayingComponent c, MoveComponent move)
    {
        player.ctr.PlayerMove.performed -= player.input;
        player.ctr.PlayerMove.canceled -= player.cancel;
    }

    [OutSystem]
    static void Out(PlayerComponent player)
    {
        player.ctr.Dispose();
    }
    [ChangeSystem]
    static void Change(TransformComponent transform, PlayerComponent player)
    {
        int2 xy = Hex.PositionToHex(transform.position);
        if (!xy.Equals(player.xy))
        {
            player.xy = xy;
            var center = getCenterGrid(xy);
            if (!player.center.Equals(center))
            {
                player.center = center;
                player.scene.CreateRoundGrid(player.center);
            }
        }
    }
    static public int2 getCenterGrid(int2 xy)
    {
        xy += Hex.HexQuad;
        int2 dis = Hex.HexQuad * 2 + 1;
        int2 n = xy / dis;
        return new int2(xy.x < 0 ? n.x - 1 : n.x, xy.y < 0 ? n.y - 1 : n.y) * dis;
    }



    void input(UnityEngine.InputSystem.InputAction.CallbackContext e)
    {
        playing.Play("RUN00_F");
        float2 dir = e.ReadValue<Vector2>();
        move.Direction = new float3(dir.x, 0, dir.y);
    }
    void cancel(UnityEngine.InputSystem.InputAction.CallbackContext e)
    {
        playing.Play("WAIT00");
        move.Direction = 0;
    }
}
