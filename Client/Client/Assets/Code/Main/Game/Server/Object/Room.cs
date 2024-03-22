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
        public IPEndPoint ip = new(IPAddress.Any, 9528);

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

                var att = c.AddComponent<AttributeComponent>();
                att.Set((int)AttributeID.MoveSpeed, 5);
                att.Set((int)AttributeID.RotateSpeed, 20);

                c.AddComponent<NetComponent>().Session = session;

                c.AddComponent<MoveComponent>();
                c.AddComponent<UnitComponent>();
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

        [Event]
        void exit(EC_PlayerExit e)
        {
            if (this.TryGetChildRpc(e.rpc, out var c))
                c.Dispose();
        }
    }
    class UnitComponent : SComponent { }
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

            var att = this.GetComponent<AttributeComponent>();
            ui.attribute = new(att.Values);

            return ui;
        }

        [Event(RPC = true)]
        void get(C2S_SyncTransform t)
        {
            var f2 = math.normalize(t.dir);
            this.GetComponent<MoveComponent>().Direction = new float3(f2.x, 0, f2.y);
        }

        [Event]
        static void change(Change<TransformComponent, UnitComponent> t)
        {
            S2C_SyncTransform sync = new();
            sync.rpc = (uint)t.t.Entity.rpc;
            sync.p = t.t.position;
            sync.r = t.t.rotation.value;
            foreach (var item in t.t.Entity.Parent.As<RoomItem>().GetChildren())
            {
                item.GetComponent<NetComponent>().Send(sync);
            }
        }
    }
}
