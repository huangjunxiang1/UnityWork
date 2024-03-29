using Core;
using Event;
using main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game
{
    class Players : STree<Player>
    {
        public IPEndPoint ip;

        /*[Event]
        static async void awake(Awake<Players> t)
        {
            TcpListener tcp = new(t.t.ip);
            tcp.Start();
            while (true)
            {
                var client = await tcp.AcceptTcpClientAsync();

                uint rpc = (uint)Util.RandomInt();

                var p = new Player(rpc);
                t.t.AddChild(p);
                p.AddComponent<NetComponent>().SetSession(new STCP(client)).Session.Work();
            }
        }*/
        [Event]
        static void awake2(Awake<Players> t)
        {
            new SUDP(t.t.ip).Work();
            /*UdpClient udp = new(t.t.ip);
            //while (true)
            {
                var client = await tcp.AcceptTcpClientAsync();
                var ip = (IPEndPoint)client.Client.RemoteEndPoint;

                uint rpc = (uint)Util.RandomInt();

                var p = new Player(rpc);
                t.t.AddChild(p);
                p.AddComponent<NetComponent>().SetSession(new SUDP(t.t.ip)).Session.Work();
            }*/
        }
    }
    class Player : SObject
    {
        public Player(long rpc) : base(rpc) { }

        /*[Event]
        static void C2S_RoomList(EventWatcher<C2S_RoomList, NetComponent> t)
        {
            S2C_RoomList s = new();
            s.lst = t.t2.Entity.Parent.GetSibling<Room>().GetLst();
            t.t2.Send(s);
        }
        [Event]
        static void create(EventWatcher<C2S_CreateRoom, NetComponent> t)
        {
            S2C_CreateRoom s = new();

            var room = t.t2.Entity.Parent.GetSibling<Room>().CreateRoom(t.t.name);
            s.info = room.GetRoomInfo();

            t.t2.Send(s);
        }
        [Event]
        static void join(EventWatcher<C2S_JoinRoom, NetComponent> t)
        {
            if (!t.t2.Entity.Parent.GetSibling<Room>().TryGetChildGid(t.t.id, out var room)) return;

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
        static void dis(EventWatcher<C2S_DisRoom, NetComponent> t)
        {
            if (!t.t2.Entity.Parent.GetSibling<Room>().TryGetChildGid(t.t.id, out var room)) return;

            room.Dispose();

            S2C_DisRoom s = new();
            s.id = room.gid;

            t.t2.Send(s);
        }*/
    }
}
