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
#if !Server
        public static NetComponent Inst { get; set; }
#endif
        public SBaseNet Session { get; set; }

        Dictionary<Type, STask<IMessage>> reqWaiter = new();

        void _onError(NetError code)
        {
            this.World.Thread.Post(() => this.World.Event.RunEvent(new EC_NetError { code = (int)code }));
        }
        void _onResponse(IMessage message)
        {
#if Server
            PrintField.Print($"收到消息 msg:[{message.GetType().Name}]  content:{{0}}", message);
#else
            PrintField.Print($"<Color=#00FF00>收到消息</Color> msg:[{message.GetType().Name}]  content:{{0}}", message);
#endif
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

        }
        public NetComponent SetSession(SBaseNet session)
        {
            this.Session?.DisConnect();
            this.Session = session;
            this.Session.onMessage += _onResponse;
            this.Session.onError += _onError;
            this.Session.onDisconnect += _onDisconnect;
            return this;
        }

        public void Send(IMessage message)
        {
#if Server
            PrintField.Print($"发送消息 cmd:[{message.GetType().Name}]  content:{{0}}", message);
#else
            PrintField.Print($"<Color=#FF0000>发送消息</Color> cmd:[{message.GetType().Name}]  content:{{0}}", message);
#endif
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

        [Event]
        static void dispose(Dispose<NetComponent> t) => t.t.Session?.DisConnect();
    }
}
