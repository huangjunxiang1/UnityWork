using Core;
using Event;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SocketManager
{
    static long rpc;

    public SBaseNet Session { get; private set; }

    Dictionary<Type, STask<IMessage>> reqWaiter = new();
    Dictionary<long, STask<IMessage>> rpcWaiter = new();

    void _onError(NetError code)
    {
        Game.ThreadSync.Post(s => Game.Event.RunEvent(new EC_NetError { code = (int)code }));
    }
    void _onResponse(IMessage message)
    {
        Game.ThreadSync.Post(s =>
        {
            Game.Data.Add(s);
            var message = (IMessage)s;
            Game.Event.RunEvent(new EC_AcceptedMessage { message = message });

            if (string.IsNullOrEmpty(message.error))
            {
                //自动注册的事件一般是底层事件 所以先执行底层监听
                if (message.actorId != 0)
                    Game.Event.RunEvent((object)message, actorId: message.actorId);
                else
                    Game.Event.RunEvent((object)message);
            }

            if (message.rpc == 0)
            {
                if (reqWaiter.TryGetValue(message.GetType(), out var v2))
                {
                    reqWaiter.Remove(message.GetType());
                    v2.TrySetResult(message);
                }
            }
            else
            {
                if (rpcWaiter.TryGetValue(message.rpc, out var task))
                {
                    rpcWaiter.Remove(message.rpc);
                    task.TrySetResult(message);
                }
            }
        }, message);
    }
    void _onDisconnect()
    {
        Game.ThreadSync.Post(static s => Game.Event.RunEvent(new EC_Disconnect()));
    }
    public SBaseNet SetSession(SBaseNet session, bool disConnect = true)
    {
        if (this.Session != null)
        {
            if (disConnect)
                this.Session.DisConnect();
            this.Session.onMessage -= _onResponse;
            this.Session.onError -= _onError;
            this.Session.onDisconnect -= _onDisconnect;
        }

        this.Session = session;
        if (this.Session != null)
        {
            this.Session.onMessage += _onResponse;
            this.Session.onError += _onError;
            this.Session.onDisconnect += _onDisconnect;
        }
        return session;
    }

    public void Send(IMessage message)
    {
        Game.Event.RunEvent(new EC_SendMesssage { message = message });
        Session.Send(message);
    }
    public STask<IMessage> SendAsync(IMessage message, bool ignoreError = true)
    {
        Type k = message.GetType();
        var v = MessageParser.GetResponseType(k);
        if (v == null)
        {
            Loger.Error("没有responseType类型 req=" + k);
            return null;
        }
        var ret = Wait(v);
        Send(message);
        return ret;
    }
    public STask<IMessage> Wait<T>() where T : IMessage => Wait(typeof(T));
    public STask<IMessage> Wait(Type type)
    {
        if (!reqWaiter.TryGetValue(type, out var v))
            reqWaiter[type] = v = new();
        return v;
    }
    public void SendRpc(IMessage message)
    {
        if (message.rpc == 0)
            message.rpc = ++rpc;
        Game.Event.RunEvent(new EC_SendMesssage { message = message });
        Session.Send(message);
    }
    public STask<IMessage> SendRpcAsync(IMessage message)
    {
        Type k = message.GetType();
        var v = MessageParser.GetResponseType(k);
        if (v == null)
        {
            Loger.Error("没有responseType类型 req=" + k);
            return null;
        }
        message.rpc = ++rpc;
        var ret = rpcWaiter[message.rpc] = new();
        SendRpc(message);
        return ret;
    }
    public void DisconnectOnNext()
    {
        if (Session == null)
            return;
        var s = Session;
        s.onMessage -= _onResponse;
        s.onError -= _onError;
        s.onDisconnect -= _onDisconnect;
        Session = null;
        Game.ThreadSync.Post(t => s.DisConnect());
    }
    public void Dispose()
    {
        Session?.DisConnect();
        Session = null;
    }
}
