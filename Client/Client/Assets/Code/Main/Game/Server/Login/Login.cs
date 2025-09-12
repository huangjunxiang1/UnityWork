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

public class Login : STree
{
    public IPEndPoint ip { get; set; }
    [InSystem]
    static async void Awake(Login t)
    {
        TcpListener tcp = new(t.ip);
        try
        {
            tcp.Start();
        }
        catch (Exception e)
        {
            Loger.Error("ip 绑定出错 =" + e);
        }
        while (true)
        {
            var client = await tcp.AcceptTcpClientAsync();
            Player o = new() { ActorId = (uint)Util.RandomInt() };
            t.AddChild(o);
            o.AddComponent(new NetComponent(false)).SetSession(new STCP(client)).Work();
        }
    }
    [EventWatcherSystem]
    static void login(C2S_Login a, NetComponent b)
    {
        if (string.IsNullOrEmpty(a.acc) || string.IsNullOrEmpty(a.pw)) return;
        var login = b.Entity.Parent.As<Login>();
        S2C_Login s = new();
        if (login.GetChildren().Find(c => c.As<Player>().acc == a.acc) != null)
        {
            s.error = "账号已被登录";
            b.Send(s);
            return;
        }
        login.TryGetChildByActorId(b.ActorId,out var c);
        c.As<Player>().acc = a.acc;
        b.Send(s);
    }
    [EventWatcherSystem]
    static void EC_Disconnect(EC_Disconnect a, Player b)
    {
        b.Dispose();
    }
}
public class Player : SObject
{
    public string acc;
}
