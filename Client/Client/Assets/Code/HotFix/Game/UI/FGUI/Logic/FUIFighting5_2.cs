using Game;
using main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities.UniversalDelegates;

partial class FUIFighting5_2
{
    [Event]
    void join(S2C_PlayerJoinRoom t)
    {
        var v = t.info;
        SGameObject go = new(v.id);
        GameWorld.World.Scene.AddChild(go);
        _ = go.GameObject.LoadGameObjectAsync("3D/Model/Unit/chan.prefab");
        go.Transform.position = v.t.p;
        go.Transform.rotation = v.t.r;
        go.Attribute.Values = v.attribute;
    }
}
