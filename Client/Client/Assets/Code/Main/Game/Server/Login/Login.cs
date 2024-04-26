using Core;
using Event;
using Game;
using main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class Login : STree<Player>
{
    public IPEndPoint ip { get; set; }
    [Event]
    static async void awake(Awake<Login> t)
    {
        TcpListener tcp = new(t.t.ip);
        tcp.Start();
        while (true)
        {
            var client = await tcp.AcceptTcpClientAsync();
            Player o = new((uint)Util.RandomInt());
            t.t.AddChild(o);
            o.AddComponent(new NetComponent(false)).SetSession(new STCP(client)).Work();
        }
    }
    [Event]
    static void login(EventWatcher<C2S_Login, NetComponent> t)
    {
        if (string.IsNullOrEmpty(t.t.acc) || string.IsNullOrEmpty(t.t.pw)) return;
        var login = t.t2.Entity.Parent.As<Login>();
        S2C_Login s = new();
        if (login.GetChildren().Find(c => c.acc == t.t.acc) != null)
        {
            s.error = "账号已被登录";
            t.t2.Send(s);
            return;
        }
        login.TryGetChildRpc(t.t2.rpc,out var c);
        c.acc = t.t.acc;
        t.t2.Send(s);
    }
    [Event]
    static void EC_Disconnect(EventWatcher<EC_Disconnect, NetComponent, Player> t)
    {
        t.t2.Entity.Dispose();
    }
}
public class Player : SObject
{
    public Player(uint rpc) : base(rpc) { }
    public string acc;
}
