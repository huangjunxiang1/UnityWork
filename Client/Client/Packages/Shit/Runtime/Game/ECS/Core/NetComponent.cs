using Core;
using Event;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class NetComponent : SComponent
    {
        public NetComponent(bool isClient = true)
        {
            this.isClient = isClient;
#if !Server
            if (isClient)
                Inst = this;
#endif
        }
#if !Server
        public static NetComponent Inst { get; private set; }
#endif

        [ShowInInspector]
        string IP => Session?.IP.ToString();
        static long rpc; 

        public SBaseNet Session { get; private set; }
        public bool isClient { get; }

        Dictionary<Type, (bool, STask<IMessage>)> reqWaiter = new();
        Dictionary<long, STask<IMessage>> rpcWaiter = new();

        void _onError(NetError code)
        {
            this.World.Thread.Post(s => this.World.Event.RunEvent(new EC_NetError { code = (int)code }));
        }
        void _onResponse(IMessage message)
        {
            this.World.Thread.Post(s =>
            {
#if !Server
                Client.Data.Add(s);
#endif
                var message = (IMessage)s;
                this.World.Event.RunEvent(new EC_AcceptedMessage { message = message });

                if (string.IsNullOrEmpty(message.error))
                {
                    //自动注册的事件一般是底层事件 所以先执行底层监听
                    if (message.actorId != 0)
                        this.World.Event.RunEvent((object)message, actorId: message.actorId);
                    else if (this.ActorId != 0)
                        this.World.Event.RunEvent((object)message, actorId: this.ActorId);
                    else
                        this.World.Event.RunEvent((object)message);
                }

                if (message.rpc == 0)
                {
                    if (reqWaiter.TryGetValue(message.GetType(), out var v2))
                    {
                        reqWaiter.Remove(message.GetType());
                        if (v2.Item1 || !string.IsNullOrEmpty(message.error))
                            v2.Item2.TrySetResult(message);
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
            this.World.Thread.Post(s =>
            {
                var v = new EC_Disconnect { rpc = this.ActorId };
                if (this.ActorId != 0)
                    this.World.Event.RunEvent(v, actorId: this.ActorId);
                else
                    this.World.Event.RunEvent(v);
            });
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
            this.World.Event.RunEvent(new EC_SendMesssage { message = message });
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
            var ret = reqWaiter[v] = (ignoreError, new());
            Send(message);
            return ret.Item2;
        }
        public void SendRpc(IMessage message)
        {
            if (message.rpc == 0)
                message.rpc = ++rpc;
            this.World.Event.RunEvent(new EC_SendMesssage { message = message });
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
            var s = Session;
            s.onMessage -= _onResponse;
            s.onError -= _onError;
            s.onDisconnect -= _onDisconnect;
            Session = null;
            this.World.Thread.Post(t => s.DisConnect());
        }

        [OutSystem]
        static void Dispose(NetComponent t)
        {
            if (t.Disposed)
                t.Session?.DisConnect();
        }
    }
}
