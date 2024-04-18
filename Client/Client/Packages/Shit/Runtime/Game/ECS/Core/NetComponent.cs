using Core;
using Event;
using PB;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    [Message(943545107, typeof(S2C_Ping))]
    partial class C2S_Ping : PB.PBMessage
    {
        public override void Read(PBReader reader) { }
        public override void Write(PBWriter writer) { }
    }

    [Message(942496515)]
    partial class S2C_Ping : PB.PBMessage
    {
        public override void Read(PBReader reader) { }
        public override void Write(PBWriter writer) { }
    }
    public class NetComponent : SComponent
    {
#if !Server
        public static NetComponent Inst { get; set; }
#endif
        public SBaseNet Session { get; private set; }

        Dictionary<Type, STask<IMessage>> reqWaiter = new();

        void _onError(NetError code)
        {
            this.World.Thread.Post(() => this.World.Event.RunEvent(new EC_NetError { code = (int)code }));
        }
        void _onResponse(IMessage message)
        {
            if (message is not C2S_Ping && message is not S2C_Ping)
            {
#if Server
                PrintField.Print($"收到消息 msg:[{message.GetType().Name}]  content:{{0}}", message);
#else
                PrintField.Print($"<Color=#00FF00>收到消息</Color> msg:[{message.GetType().Name}]  content:{{0}}", message);
#endif
            }
            this.World.Thread.Post(() =>
            {
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
            if (message is not C2S_Ping && message is not S2C_Ping)
            {
#if Server
                PrintField.Print($"发送消息 msg:[{message.GetType().Name}]  content:{{0}}", message);
#else
                PrintField.Print($"<Color=#FF0000>发送消息</Color> msg:[{message.GetType().Name}]  content:{{0}}", message);
#endif
            }
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
    public class PingComponent : SComponent
    {
        static C2S_Ping c_p = new();
        S2C_Ping s_p = new();
        int counter;

        public async void Ping()
        {
            NetComponent net = this.Entity.GetComponent<NetComponent>();
            while (true)
            {
                await Task.Delay(3000);
                if (!this.Disposed)
                {
                    if (counter > 5)
                    {
                        this.Dispose();
                        net.Session.DisConnect();
                        break;
                    }
                    net.Send(s_p);
                    counter++;
                }
                else break;
            }
        }
        
        [Event]
        static void watcher(EventWatcher<C2S_Ping, PingComponent> t)
        {
            t.t2.counter = 0;
        }
        [Event]
        static void ping(S2C_Ping ping)
        {
            NetComponent.Inst.Send(c_p);
        }
    }
}
