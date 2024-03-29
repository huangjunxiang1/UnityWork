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

namespace Game
{
    class Room : STree<RoomItem>
    {
        public IPEndPoint ip;

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
        static async void awake(Awake<Room> t)
        {
            TcpListener tcp = new(t.t.ip);
            tcp.Start();
            while (true)
            {
                var client = await tcp.AcceptTcpClientAsync();
                SObject o = new((uint)Util.RandomInt());
                Server.World.Root.AddChild(o);
                o.AddComponent<NetComponent>().SetSession(new STCP(client)).Session.Work();
            }
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

            room.AddUnit(t.t2.rpc, t.t2.Session);

            S2C_JoinRoom s = new();
            s.info = room.GetRoomInfo();
            s.units = room.GetUnitInfo2s();
            s.myid = t.t2.rpc;
            t.t2.Send(s);

            t.t2.Session = null;//不断开链接 将就现在的用
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
    class RoomItem : STree<Unit>
    {
        public string name;

        public Unit AddUnit(long rpc, SBaseNet session)
        {
            if (!this.TryGetChildRpc(rpc, out var c))
            {
                c = new Unit(rpc);
                this.AddChild(c);

                c.AddComponent<TransformComponent>();

                var att = c.AddComponent<KVComponent>();
                att.Set((int)KType.MoveSpeed, 5);
                att.Set((int)KType.RotateSpeed, 20);

                c.AddComponent<NetComponent>().Session = session;

                c.AddComponent<MoveComponent>();
                c.AddComponent<BelongRoom>().room = this;
            }
            S2C_PlayerJoinRoom join = new();
            join.info = c.GetUnitInfo2();
            foreach (var item in this.GetChildren())
            {
                if (item.rpc != rpc)
                {
                    item.GetComponent<NetComponent>().Send(join);
                }
            }

            return c;
        }
        public RoomInfo GetRoomInfo()
        {
            RoomInfo ri = new();

            ri.id = this.gid;
            ri.name = this.name;

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
    class Unit : SObject
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
        static void get(EventWatcher<C2S_SyncTransform, MoveComponent> t)
        {
            var f2 = math.normalize(t.t.dir);
            t.t2.Direction = new float3(f2.x, 0, f2.y);
        }

        [Event]
        static void change(Change<TransformComponent, BelongRoom> t)
        {
            S2C_SyncTransform sync = new();
            sync.rpc = (uint)t.t.rpc;
            sync.p = t.t.position;
            sync.r = t.t.rotation.value;
            foreach (var item in t.t2.room.GetChildren())
            {
                item.GetComponent<NetComponent>().Send(sync);
            }
        }
    }
}
