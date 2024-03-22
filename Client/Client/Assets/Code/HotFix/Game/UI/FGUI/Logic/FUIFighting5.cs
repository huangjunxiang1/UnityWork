using Cinemachine;
using Core;
using FairyGUI;
using Game;
using main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

partial class FUIFighting5
{
    bool connected = false;
    protected override void OnEnter(params object[] data)
    {
        base.OnEnter(data);

        this._create.onClick.Add(on_create);
        this._rooms.onClickItem.Add(on_click);
        this._server.onClick.Add(on_server);
        this._cServer.onClick.Add(on_cServer);
        this._ref.onClick.Add(on_ref);

        this._rooms.numItems = 0;
    }
    protected override void OnExit()
    {
        base.OnExit();
    }

    void review()
    {
        var info = GameWorld.World.Data.Get<S2C_RoomList>();
        this._rooms.numItems = info.lst.Count;
        for (int i = 0; i < this._rooms.numItems; i++)
        {
            var c = this._rooms.GetChildAt(i);
            var d = info.lst[i];

            c.text = d.name + " 人数:" + d.infos.Count;
        }
    }
    async void on_create()
    {
        if (string.IsNullOrEmpty(this._name.text)|| !connected) return;
        C2S_CreateRoom c = new();
        c.name = this._name.text;
        await GameWorld.World.Net.SendAsync(c);
        on_ref();
    }
    async void on_click(EventContext e)
    {
        int index = this._rooms.GetChildIndex((GObject)e.data);
        var d = GameWorld.World.Data.Get<S2C_RoomList>().lst[index];
        var c = new C2S_JoinRoom();
        c.id = d.id;
        var s = (S2C_JoinRoom)await GameWorld.World.Net.SendAsync(c);
        int id = 10001;
        await GameWorld.World.Scene.InScene(id, TabL.GetScene(id).type, TabL.GetScene(id).name);
        for (int i = 0; i < s.units.Count; i++)
        {
            var v = s.units[i];
            SGameObject go = new(v.id);
            GameWorld.World.Scene.AddChild(go);
            _ = go.GameObject.LoadGameObjectAsync("3D/Model/Unit/chan.prefab");
            go.Transform.position = v.t.p;
            go.Transform.rotation = v.t.r;
            if (go.rpc == s.myid)
                go.AddComponent<TransformClientInputComponent>();
            go.Attribute.Values = v.attribute;
        }
        await UI.Inst.OpenAsync<FUIFighting5_2>();
    }
    async void on_server()
    {
        GameWorldServer.Close();
        await GameWorldServer.Init();
        var net = new STCP(Util.ToIPEndPoint("127.0.0.1", SettingM.serverPort));
        await GameWorld.World.Net.Connect(net);
        connected = true;
        on_ref();
    }
    async void on_cServer()
    {
        var net = new STCP(Util.ToIPEndPoint("127.0.0.1", SettingM.serverPort));
        await GameWorld.World.Net.Connect(net);
        connected = true;
        on_ref();
    }
    async void on_ref()
    {
        await GameWorld.World.Net.SendAsync(new C2S_RoomList());
        this._server.enabled = false;
        this._cServer.enabled = false;
        this.review();
    }
}
