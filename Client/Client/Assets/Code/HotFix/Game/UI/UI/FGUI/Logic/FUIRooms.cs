using FairyGUI;
using Game;
using main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

partial class FUIRooms
{
    protected override async STask OnTask(params object[] data)
    {
        await NetComponent.Inst.SendAsync(new C2S_RoomList());
    }
    protected override void OnEnter(params object[] data)
    {
        this._ref.onClick.Add(refRoom);
        this._create.onClick.Add(create);
        this._rooms.onClickItem.Add(join);

        this.review();
    }

    void review()
    {
        var info = Client.Data.Get<S2C_RoomList>();
        this._rooms.numItems = info.lst.Count;
        for (int i = 0; i < this._rooms.numItems; i++)
        {
            var c = this._rooms.GetChildAt(i);
            var d = info.lst[i];
            c.text = $"[{d.name}] 人数:{d.infos.Count}";
        }
    }
    async void refRoom()
    {
        await NetComponent.Inst.SendAsync(new C2S_RoomList());
        this.review();
    }
    async void create()
    {
        if (string.IsNullOrEmpty(this._roomName.text)) return;
        C2S_CreateRoom c = new();
        c.name = this._roomName.text;
        await NetComponent.Inst.SendAsync(c);
        await NetComponent.Inst.SendAsync(new C2S_RoomList());
        this.review();
    }
    async void join(EventContext e)
    {
        int index = this._rooms.GetChildIndex((GObject)e.data);
        var d = Client.Data.Get<S2C_RoomList>().lst[index];
        var c = new C2S_JoinRoom();
        c.id = d.id;
        var s = (S2C_JoinRoom)await NetComponent.Inst.SendAsync(c);
        int id = 10001;
        await Client.Scene.InScene(id, TabL.GetScene(id).type, TabL.GetScene(id).name);
        for (int i = 0; i < s.units.Count; i++)
        {
            var v = s.units[i];
            SGameObject go = new() { rpc = v.id };
            Client.Scene.AddChild(go);
            go.GameObject.SetGameObject("3D/Model/Unit/chan.prefab");
            go.Transform.position = v.t.p;
            go.Transform.rotation = v.t.r;
            if (go.rpc == s.myid)
                go.AddComponent<GameInputComponent>();
            go.KV.Set(v.attribute);
            go.AddComponent<ColliderClickComponent>();
        }
        await Client.UI.OpenAsync<FUIGame>();
    }
}
