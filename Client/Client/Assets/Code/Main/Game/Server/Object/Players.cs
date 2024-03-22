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
    class Players : STree
    {
        static uint _gen_rpc = 0;
        Dictionary<IPEndPoint, long> players = new();

        [Event]
        static void playerDispose(Dispose<Player> t)
        {
            t.t.Parent.As<Players>().players.Remove(t.t.tcp.IP);

            t.t.World.Event.RunRPCEvent(t.t.rpc, new EC_PlayerExit { rpc = t.t.rpc });
            t.t.World.Event.RunEvent(new EC_PlayerExit { rpc = t.t.rpc });
        }
        [Event]
        static async void awake(Awake<Players> t)
        {
            TcpListener tcp = new(IPAddress.Any, SettingM.serverPort);
            tcp.Start();
            while (true)
            {
                var client = await tcp.AcceptTcpClientAsync();
                var ip = (IPEndPoint)client.Client.RemoteEndPoint;

                if (t.t.players.TryGetValue(ip, out var rpcID))
                    t.t.RemoveRpc(rpcID);

                uint rpc = ++_gen_rpc;
                STCP s = new(client, t.t.World.Thread.threadId, rpc);
                s.Work();
                s.onDisconnect += n =>
                {
                    if (t.t.players.TryGetValue(n.IP, out var rpcID))
                        t.t.RemoveRpc(rpcID);
                };
                GameWorldServer.World.Net.Add(s);

                t.t.AddChild(new Player(s, rpc));
                t.t.players[ip] = rpc;
            }
        }
    }
    class Player : SObject
    {
        public Player(STCP tcp, long rpc) : base(rpc)
        {
            this.tcp = tcp;
        }
        public STCP tcp { get; }

        public override void Dispose()
        {
            tcp.DisConnect();
            base.Dispose();
        }

        [Event(RPC = true)]
        void get(C2S_RoomList e)
        {
            S2C_RoomList s = new();
            s.lst = this.Parent.GetSibling<Room>().GetLst();
            tcp.Send(s);
        }
        [Event(RPC = true)]
        void create(C2S_CreateRoom e)
        {
            S2C_CreateRoom s = new();

            var room = this.Parent.GetSibling<Room>().CreateRoom(e.name);
            s.info = room.GetRoomInfo();

            tcp.Send(s);
        }
        [Event(RPC = true)]
        void join(C2S_JoinRoom e)
        {
            if (!this.Parent.GetSibling<Room>().TryGetChildGid(e.id, out var room)) return;

            room.AddUnit(this.rpc, tcp);

            S2C_JoinRoom s = new();
            s.info = room.GetRoomInfo();
            s.units = room.GetUnitInfo2s();
            s.myid = this.rpc;
            tcp.Send(s);

        }
        [Event(RPC = true)]
        void dis(C2S_DisRoom e)
        {
            if (!this.Parent.GetSibling<Room>().TryGetChildGid(e.id, out var room)) return;

            room.AddUnit(this.rpc, tcp);
            room.Dispose();

            S2C_DisRoom s = new();
            s.id = room.gid;

            tcp.Send(s);
        }
    }
}
