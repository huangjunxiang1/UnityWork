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
    public class Room : STree
    {
        public static Room inst;
        public List<RoomInfo> GetLst()
        {
            var lst = new List<RoomInfo>();
            foreach (var item in this.GetChildren())
                lst.Add(item.As<RoomItem>().GetRoomInfo());
            return lst;
        }
        public RoomItem CreateRoom(string name)
        {
            RoomItem ri = new();
            ri.name = name;
            this.AddChild(ri);
            return ri;
        }

        [EventWatcherSystem]
        static void EC_Disconnect(EC_Disconnect a, NetComponent b, BelongRoom c)
        {
            foreach (var item in c.room.GetChildren())
            {
                if (item.ActorId != b.ActorId)
                {
                    item.GetComponent<NetComponent>().Send(new S2C_PlayerQuit { id = b.ActorId });
                }
            }
            b.Entity.Dispose();
        }
        [EventWatcherSystem]
        static void EC_Disconnect(C2S_PlayerQuit a, NetComponent b, BelongRoom c)
        {
            foreach (var item in c.room.GetChildren())
            {
                if (item.ActorId != b.ActorId)
                {
                    item.GetComponent<NetComponent>().Send(new S2C_PlayerQuit { id = b.ActorId });
                }
            }
            b.Entity.Dispose();
        }
        [EventWatcherSystem]
        static void C2S_RoomList(C2S_RoomList a, NetComponent b)
        {
            S2C_RoomList s = new();
            s.lst = inst.GetLst();
            b.Send(s);
        }
        [EventWatcherSystem]
        static void create(C2S_CreateRoom a, NetComponent b)
        {
            S2C_CreateRoom s = new();

            var r = inst.CreateRoom(a.name);
            s.info = r.GetRoomInfo();

            b.Send(s);
        }
        [EventWatcherSystem]
        static void join(C2S_JoinRoom a, NetComponent b, Player c)
        {
            if (!inst.TryGetChildByGid(a.id, out var r)) return;

            string acc = c.acc;

            r.As<RoomItem>().AddUnit(b.ActorId, acc, b.Session);

            S2C_JoinRoom s = new();
            s.info = r.As<RoomItem>().GetRoomInfo(true);
            s.units = r.As<RoomItem>().GetUnitInfo2s();
            s.myid = b.ActorId;
            b.Send(s);

            b.SetSession(null, false);//不断开链接 将就现在的用
            b.Entity.Dispose();
        }
        [EventWatcherSystem]
        static void dis(C2S_DisRoom a, NetComponent b)
        {
            if (!inst.TryGetChildByGid(a.id, out var r)) return;

            S2C_DisRoom s = new();
            s.id = r.gid;

            b.Send(s);
            r.Dispose();
        }
    }
    public class RoomItem : STree
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

        public Unit AddUnit(long actorId, string name, SBaseNet session)
        {
            if (!this.TryGetChildByActorId(actorId, out var o))
            {
                o = new Unit() { ActorId = actorId };
                o.As<Unit>().name = name;
                this.AddChild(o);

                o.AddComponent<TransformComponent>();
                o.AddComponent<SubRoom>();

                var att = o.AddComponent<KVComponent>();
                att.Set((int)KType.MoveSpeed, 5);
                att.Set((int)KType.RotateSpeed, 20);

                o.AddComponent(new NetComponent(false)).SetSession(session);

                o.AddComponent<MoveComponent>();
                o.AddComponent<BelongRoom>().room = this;
            }
            S2C_PlayerJoinRoom join = new();
            join.info = o.As<Unit>().GetUnitInfo2();
            foreach (var item in this.GetChildren())
            {
                if (item.ActorId != actorId)
                {
                    item.GetComponent<NetComponent>().Send(join);
                }
            }

            return o.As<Unit>();
        }
        public RoomInfo GetRoomInfo(bool getLink = false)
        {
            RoomInfo ri = new();

            ri.id = this.gid;
            ri.name = this.name;
            if (getLink)
                ri.link = new(this.linkMap);

            foreach (var item in this.GetChildren())
                ri.infos.Add(item.As<Unit>().GetUnitInfo());

            return ri;
        }
        public List<UnitInfo2> GetUnitInfo2s()
        {
            var lst = new List<UnitInfo2>();

            foreach (var item in this.GetChildren())
                lst.Add(item.As<Unit>().GetUnitInfo2());

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


        public UnitInfo GetUnitInfo()
        {
            UnitInfo ui = new();
            ui.id = this.ActorId;
            ui.name = name;
            return ui;
        }
        public UnitInfo2 GetUnitInfo2()
        {
            UnitInfo2 ui = new();
            ui.id = this.ActorId;
            var t = this.GetComponent<TransformComponent>();
            ui.t.p = t.position;
            ui.t.r = t.rotation.value;

            var att = this.GetComponent<KVComponent>();
            att.CopyTo(ui.attribute);

            return ui;
        }

        [EventWatcherSystem]
        static void get(C2S_SyncTransform a, MoveComponent b, TransformComponent c, BelongRoom d)
        {
            if (!math.all(a.dir == 0))
            {
                var f2 = math.normalize(a.dir);
                b.Direction = new float3(f2.x, 0, f2.y);
            }
            else
            {
                b.Direction = 0;
                S2C_SyncTransform sync = new();
                sync.actorId = b.ActorId;
                sync.p = c.position;
                sync.r = c.rotation.value;
                sync.isMoving = false;
                foreach (var item in d.room.GetChildren())
                {
                    item.GetComponent<NetComponent>().Send(sync);
                }
            }
        }

        [ChangeSystem]
        static void change(TransformComponent a, BelongRoom b)
        {
            S2C_SyncTransform sync = new();
            sync.actorId = a.ActorId;
            sync.p = a.position;
            sync.r = a.rotation.value;
            sync.isMoving = true;
            foreach (var item in b.room.GetChildren())
            {
                item.GetComponent<NetComponent>().Send(sync);
            }
        }
    }
    class SubRoom : SComponent
    {
        public int2 xy { get; private set; }//room xy

        [InSystem]
        static void In(SubRoom a, TransformComponent b)
        {
            var min = SettingM.SubRoomSize * a.xy + 1;
            var max = SettingM.SubRoomSize * (a.xy + 1);
            b.AABB = new Unity.Mathematics.Geometry.MinMaxAABB(new float3(min.x + 0.5f, 0, min.y + 0.5f), new float3(max.x - 0.5f, 0, max.y - 0.5f));
        }
        [OutSystem]
        static void Out(SubRoom a, TransformComponent b)
        {
            b.ResetAABB();
        }
        [ChangeSystem]
        static void change(TransformComponent a, SubRoom b, BelongRoom c)
        {
            int2 xy = new((int)a.position.x, (int)a.position.z);
            int2 roomxy = (int2)a.position.xz / SettingM.SubRoomSize;
            if (xy.Equals(roomxy * SettingM.SubRoomSize + new int2(1, SettingM.SubRoomSize.y / 2)))
            {
                var link = c.room.linkMap[(roomxy.y * SettingM.RoomSize.x + roomxy.x) * 4 + 0];
                link = c.room.linkMap[link.link];
                b.xy = link.xy;
                move(a, b.xy, link.dir);
            }
            else if (xy.Equals(roomxy * SettingM.SubRoomSize + new int2(SettingM.SubRoomSize.x / 2, 1)))
            {
                var link = c.room.linkMap[(roomxy.y * SettingM.RoomSize.x + roomxy.x) * 4 + 1];
                link = c.room.linkMap[link.link];
                b.xy = link.xy;
                move(a, b.xy, link.dir);
            }
            else if (xy.Equals(roomxy * SettingM.SubRoomSize + new int2(SettingM.SubRoomSize.x - 1, SettingM.SubRoomSize.y / 2)))
            {
                var link = c.room.linkMap[(roomxy.y * SettingM.RoomSize.x + roomxy.x) * 4 + 2];
                link = c.room.linkMap[link.link];
                b.xy = link.xy;
                move(a, b.xy, link.dir);
            }
            else if (xy.Equals(roomxy * SettingM.SubRoomSize + new int2(SettingM.SubRoomSize.x / 2, SettingM.SubRoomSize.y - 1)))
            {
                var link = c.room.linkMap[(roomxy.y * SettingM.RoomSize.x + roomxy.x) * 4 + 3];
                link = c.room.linkMap[link.link];
                b.xy = link.xy;
                move(a, b.xy, link.dir);
            }
        }
        static void move(TransformComponent t, int2 xy, int dir)
        {
            var min = SettingM.SubRoomSize * xy + 1;
            var max = SettingM.SubRoomSize * (xy + 1);
            t.AABB = new Unity.Mathematics.Geometry.MinMaxAABB(new float3(min.x + 0.5f, 0, min.y + 0.5f), new float3(max.x - 0.5f, 0, max.y - 0.5f));

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
