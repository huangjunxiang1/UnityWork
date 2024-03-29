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
    protected override void OnExit()
    {
        base.OnExit();
        Client.Scene.InLoginScene();
    }
    [Event]
    void join(S2C_PlayerJoinRoom t)
    {
        var v = t.info;
        SGameObject go = new(v.id);
        Client.Scene.AddChild(go);
        go.GameObject.SetGameObject("3D/Model/Unit/chan.prefab");
        go.Transform.position = v.t.p;
        go.Transform.rotation = v.t.r;
        go.KV.Values = v.attribute;
    }
}
