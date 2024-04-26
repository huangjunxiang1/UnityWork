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
        public NetComponent(bool isClient)
        {
            this.isClient = isClient;
            if (isClient)
                Inst = this;
        }
#if !Server
        public static NetComponent Inst { get; private set; }
#endif
        public SBaseNet Session { get; private set; }
        public bool isClient { get; }

        Dictionary<Type, STask<IMessage>> reqWaiter = new();

        void _onError(NetError code)
        {
            this.World.Thread.Post(() => this.World.Event.RunEvent(new EC_NetError { code = (int)code }));
        }
        void _onResponse(IMessage message)
        {
            this.World.Thread.Post(() =>
            {
                this.World.Event.RunEvent(new EC_AcceptedMessage { message = message });
                //自动注册的事件一般是底层事件 所以先执行底层监听
                if (message.rpc != 0)
                    this.World.Event.RunRPCEvent(message.rpc, (object)message);
                else if (this.rpc != 0)
                    this.World.Event.RunRPCEvent(this.rpc, (object)message);
                else
                {
                    this.World.Event.RunEvent((object)message);

                    if (reqWaiter.TryGetValue(message.GetType(), out var task))
                    {
                        reqWaiter.Remove(message.GetType());
                        task.TrySetResult(message);
                    }
                }
            });
        }
        void _onDisconnect()
        {
            this.World.Thread.Post(() =>
            {
                var v = new EC_Disconnect { rpc = this.rpc };
                this.World.Event.RunRPCEvent(this.rpc, v);
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
        public STask<IMessage> SendAsync(IMessage message)
        {
            Type k = message.GetType();
            var v = MessageParser.GetResponseType(k);
            if (v == null)
            {
                Loger.Error("没有responseType类型 req=" + k);
                return null;
            }
            if (!reqWaiter.TryGetValue(v, out var task))
                reqWaiter[v] = task = new();
            Send(message);
            return task;
        }
        public void DisconnectOnNext()
        {
            var s = Session;
            s.onMessage -= _onResponse;
            s.onError -= _onError;
            s.onDisconnect -= _onDisconnect;
            Session = null;
            this.World.Thread.Post(s.DisConnect);
        }

        [Event]
        static void dispose(Dispose<NetComponent> t) => t.t.Session?.DisConnect();
    }
}
