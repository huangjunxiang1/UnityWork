using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Event;
using game;
using main;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    public class Room : STree<RoomItem>
    {
        public List<RoomInfo> GetLst()
        {
            var lst = new List<RoomInfo>();
            foreach (var item in this.GetChildren())
                lst.Add(item.GetRoomInfo());
            return lst;
        }
        public RoomItem CreateRoom(string name)
        {
            RoomItem ri = new();
            ri.name = name;
            this.AddChild(ri);
            return ri;
        }

        [Event]
        static void EC_Disconnect(EventWatcher<EC_Disconnect, NetComponent, BelongRoom> t)
        {
            foreach (var item in t.t3.room.GetChildren())
            {
                if (item.rpc != t.t2.rpc)
                {
                    item.GetComponent<NetComponent>().Send(new S2C_PlayerQuit { id = t.t2.rpc });
                }
            }
            t.t2.Entity.Dispose();
        }
        [Event]
        static void EC_Disconnect(EventWatcher<C2S_PlayerQuit, NetComponent, BelongRoom> t)
        {
            foreach (var item in t.t3.room.GetChildren())
            {
                if (item.rpc != t.t2.rpc)
                {
                    item.GetComponent<NetComponent>().Send(new S2C_PlayerQuit { id = t.t2.rpc });
                }
            }
            t.t2.Entity.Dispose();
        }
        [Event]
        void C2S_RoomList(EventWatcher<C2S_RoomList, NetComponent> t)
        {
            S2C_RoomList s = new();
            s.lst = this.GetLst();
            t.t2.Send(s);
        }
        [Event]
        void create(EventWatcher<C2S_CreateRoom, NetComponent> t)
        {
            S2C_CreateRoom s = new();

            var room = this.CreateRoom(t.t.name);
            s.info = room.GetRoomInfo();

            t.t2.Send(s);
        }
        [Event]
        void join(EventWatcher<C2S_JoinRoom, NetComponent> t)
        {
            if (!this.TryGetChildGid(t.t.id, out var room)) return;

            string acc = ((Player)t.t2.Entity).acc;

            room.AddUnit(t.t2.rpc, acc, t.t2.Session);

            S2C_JoinRoom s = new();
            s.info = room.GetRoomInfo(true);
            s.units = room.GetUnitInfo2s();
            s.myid = t.t2.rpc;
            t.t2.Send(s);

            t.t2.SetSession(null, false);//不断开链接 将就现在的用
            t.t2.Entity.Dispose();
        }
        [Event]
        void dis(EventWatcher<C2S_DisRoom, NetComponent> t)
        {
            if (!this.TryGetChildGid(t.t.id, out var room)) return;

            S2C_DisRoom s = new();
            s.id = room.gid;

            t.t2.Send(s);
            room.Dispose();
        }
    }
    public class RoomItem : STree<Unit>
    {
        public string name;

        public RoomItem() : base()
        {
            List<RoomLinkItem> links = new List<RoomLinkItem>();
            for (int x = 0; x < SettingM.RoomSize.x; x++)
            {
                for (int y = 0; y < SettingM.RoomSize.y; y++)
                {
                    for (int d = 0; d < 4; d++)
                    {
                        RoomLinkItem k = new();
                        k.index = (y * SettingM.RoomSize.x + x) * 4 + d;
                        k.xy = new int2(x, y);
                        k.dir = d;
                        links.Add(k); 
                    }
                }
            }
            for (int i = 0; i < links.Count - 1; i++)
            {
                int index = Util.RandomInt(0, links.Count - 1);
                (links[index], links[i]) = (links[i], links[index]);
            }
            for (int i = 0; i < links.Count; i += 2)
            {
                links[i].link = links[i + 1].index;
                links[i + 1].link = links[i].index;

                links[i].colorIndex = links[i + 1].colorIndex = i / 2;
            }
            for (int i = 0; i < links.Count; i++)
                linkMap[links[i].index] = links[i];
        }
        public Dictionary<int, RoomLinkItem> linkMap = new();

        public Unit AddUnit(long rpc, string name, SBaseNet session)
        {
            if (!this.TryGetChildRpc(rpc, out var o))
            {
                o = new Unit(rpc);
                o.name = name;
                this.AddChild(o);

                o.AddComponent<TransformComponent>();
                o.AddComponent<SubRoom>();

                var att = o.AddComponent<KVComponent>();
                att.Set((int)KType.MoveSpeed, 5);
                att.Set((int)KType.RotateSpeed, 20);

                o.AddComponent<NetComponent>().SetSession(session);
                o.AddComponent<PingComponent>().Ping();

                o.AddComponent<MoveComponent>();
                o.AddComponent<BelongRoom>().room = this;
            }
            S2C_PlayerJoinRoom join = new();
            join.info = o.GetUnitInfo2();
            foreach (var item in this.GetChildren())
            {
                if (item.rpc != rpc)
                {
                    item.GetComponent<NetComponent>().Send(join);
                }
            }

            return o;
        }
        public RoomInfo GetRoomInfo(bool getLink = false)
        {
            RoomInfo ri = new();

            ri.id = this.gid;
            ri.name = this.name;
            if (getLink)
                ri.link = new(this.linkMap);

            foreach (var item in this.GetChildren())
                ri.infos.Add(item.GetUnitInfo());

            return ri;
        }
        public List<UnitInfo2> GetUnitInfo2s()
        {
            var lst = new List<UnitInfo2>();

            foreach (var item in this.GetChildren())
                lst.Add(item.GetUnitInfo2());

            return lst;
        }
    }
    class BelongRoom : SComponent
    {
        public RoomItem room;
    }
    public class Unit : SObject
    {
        public string name;

        public Unit(long rpc) : base(rpc) { }

        public UnitInfo GetUnitInfo()
        {
            UnitInfo ui = new();
            ui.id = this.rpc;
            ui.name = name;
            return ui;
        }
        public UnitInfo2 GetUnitInfo2()
        {
            UnitInfo2 ui = new();
            ui.id = this.rpc;
            var t = this.GetComponent<TransformComponent>();
            ui.t.p = t.position;
            ui.t.r = t.rotation.value;

            var att = this.GetComponent<KVComponent>();
            ui.attribute = new(att.Values);

            return ui;
        }

        [Event]
        static void get(EventWatcher<C2S_SyncTransform, MoveComponent, TransformComponent, BelongRoom> t)
        {
            if (!math.all(t.t.dir == 0))
            {
                var f2 = math.normalize(t.t.dir);
                t.t2.Direction = new float3(f2.x, 0, f2.y);
            }
            else
            {
                t.t2.Direction = 0;
                S2C_SyncTransform sync = new();
                sync.rpc = (uint)t.t2.rpc;
                sync.p = t.t3.position;
                sync.r = t.t3.rotation.value;
                sync.isMoving = false;
                foreach (var item in t.t4.room.GetChildren())
                {
                    item.GetComponent<NetComponent>().Send(sync);
                }
            }
        }

        [Event]
        static void change(Change<TransformComponent, BelongRoom> t)
        {
            S2C_SyncTransform sync = new();
            sync.rpc = (uint)t.t.rpc;
            sync.p = t.t.position;
            sync.r = t.t.rotation.value;
            sync.isMoving = true;
            foreach (var item in t.t2.room.GetChildren())
            {
                item.GetComponent<NetComponent>().Send(sync);
            }
        }
    }
    class SubRoom : SComponent
    {
        public int2 xy { get; private set; }//room xy

        [Event]
        static void bound(Awake<SubRoom, TransformComponent> t)
        {
            var min = SettingM.SubRoomSize * t.t.xy + 1;
            var max = SettingM.SubRoomSize * (t.t.xy + 1);
            t.t2.Bound = new float3x2(new float3(min.x + 0.5f, 0, min.y + 0.5f), new float3(max.x - 0.5f, 0, max.y - 0.5f));
        }
        [Event]
        static void Dispose(Dispose<SubRoom, TransformComponent> t)
        {
            t.t2.ResetBound();
        }
        [Event]
        static void change(Change<TransformComponent, SubRoom, BelongRoom> t)
        {
            int2 xy = new((int)t.t.position.x, (int)t.t.position.z);
            int2 roomxy = (int2)t.t.position.xz / SettingM.SubRoomSize;
            if (xy.Equals(roomxy * SettingM.SubRoomSize + new int2(1, SettingM.SubRoomSize.y / 2)))
            {
                var link = t.t3.room.linkMap[(roomxy.y * SettingM.RoomSize.x + roomxy.x) * 4 + 0];
                link = t.t3.room.linkMap[link.link];
                t.t2.xy = link.xy;
                move(t.t, t.t2.xy, link.dir);
            }
            else if (xy.Equals(roomxy * SettingM.SubRoomSize + new int2(SettingM.SubRoomSize.x / 2, 1)))
            {
                var link = t.t3.room.linkMap[(roomxy.y * SettingM.RoomSize.x + roomxy.x) * 4 + 1];
                link = t.t3.room.linkMap[link.link];
                t.t2.xy = link.xy;
                move(t.t, t.t2.xy, link.dir);
            }
            else if (xy.Equals(roomxy * SettingM.SubRoomSize + new int2(SettingM.SubRoomSize.x - 1, SettingM.SubRoomSize.y / 2)))
            {
                var link = t.t3.room.linkMap[(roomxy.y * SettingM.RoomSize.x + roomxy.x) * 4 + 2];
                link = t.t3.room.linkMap[link.link];
                t.t2.xy = link.xy;
                move(t.t, t.t2.xy, link.dir);
            }
            else if (xy.Equals(roomxy * SettingM.SubRoomSize + new int2(SettingM.SubRoomSize.x / 2, SettingM.SubRoomSize.y - 1)))
            {
                var link = t.t3.room.linkMap[(roomxy.y * SettingM.RoomSize.x + roomxy.x) * 4 + 3];
                link = t.t3.room.linkMap[link.link];
                t.t2.xy = link.xy;
                move(t.t, t.t2.xy, link.dir);
            }
        }
        static void move(TransformComponent t, int2 xy, int dir)
        {
            var min = SettingM.SubRoomSize * xy + 1;
            var max = SettingM.SubRoomSize * (xy + 1);
            t.Bound = new float3x2(new float3(min.x + 0.5f, 0, min.y + 0.5f), new float3(max.x - 0.5f, 0, max.y - 0.5f));

            int2 n = SettingM.SubRoomSize * xy;
            if (dir == 0)
                t.position = new float3(n.x, 0, n.y) + new float3(2, 0, SettingM.SubRoomSize.y / 2f) + new float3(0.5f, 0, 0.5f);
            else if (dir == 1)
                t.position = new float3(n.x, 0, n.y) + new float3(SettingM.SubRoomSize.x / 2f, 0, 2) + new float3(0.5f, 0, 0.5f);
            else if (dir == 2)
                t.position = new float3(n.x, 0, n.y) + new float3(SettingM.SubRoomSize.x - 2, 0, SettingM.SubRoomSize.y / 2f) + new float3(0.5f, 0, 0.5f);
            else if (dir == 3)
                t.position = new float3(n.x, 0, n.y) + new float3(SettingM.SubRoomSize.x / 2f, 0, SettingM.SubRoomSize.y - 2) + new float3(0.5f, 0, 0.5f);
        }
    }
}
