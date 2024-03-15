using System;
using System.Collections.Generic;
using PB;
using System.Collections.Concurrent;
using Event;

namespace Core
{
    public class NetSystem
    {
        SBaseNet net;
        readonly Dictionary<Type, Queue<STask<PB.PBMessage>>> _requestTask = new();
        ConcurrentQueue<PBMessage> msgs = new();

        void _onError(int error)
        {
            Loger.Error("Net Error Code:" + error);
            GameM.Event.RunEvent(new EC_NetError { code = error });
        }
        void _onResponse(PB.PBMessage message)
        {
            var type = message.GetType();
            uint cmd = Game.Types.GetCMDCode(type);

            PrintField.Print($"<Color=#00FF00>收到消息</Color> cmd:[{(ushort)cmd},{cmd >> 16}]  content:{{0}}", message);

            if (message.rpc > 0)
            {
                //自动注册的事件一般是底层事件 所以先执行底层监听
                GameM.Event.RunRPCEvent(message.rpc, message);
            }
            else
            {
                GameM.Data.Add(message);
                //自动注册的事件一般是底层事件 所以先执行底层监听
                GameM.Event.RunEvent(message);

                if (_requestTask.TryGetValue(type, out var queue))
                {
                    _requestTask.Remove(type);
                    while (queue.Count > 0)
                        queue.Dequeue().TrySetResult(message);
                    ObjectPool.Return(queue);
                }
            }
        }

        /// <summary>
        /// 链接服务器IP
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ipEndPoint"></param>
        public async STask<bool> Connect(SBaseNet _net)
        {
            net?.DisConnect();
            net = _net;
            net.onReceive += msg => msgs.Enqueue(msg);
            net.onError += _onError;
            var b = await net.Connect();
            if (b)
                net.Work();
            return b;
        }
        /// <summary>
        /// 重连
        /// </summary>
        /// <param name="_net"></param>
        /// <returns></returns>
        public async STask<bool> ReConnect(SBaseNet _net)
        {
            if (net == null) return false;
            var b = await net.Connect();
            if (b)
                net.Work();
            return b;
        }

        /// <summary>
        /// 非返回的消息发送
        /// </summary>
        /// <param name="message"></param>
        public void Send(PBMessage message)
        {
#if DebugEnable
            uint cmd = Game.Types.GetCMDCode(message.GetType());
            PrintField.Print($"<Color=#FF0000>发送消息</Color> cmd:[{(ushort)cmd},{cmd >> 16}]  content:{{0}}", message);
#endif
            net.Send(message);
        }

        /// <summary>
        /// 异步返回的消息发送
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public STask<PB.PBMessage> SendAsync(PBMessage request)
        {
            Type k = request.GetType();
            var v = Game.Types.GetResponseType(k);
            if (v == null)
            {
                Loger.Error("没有responseType类型 req=" + k);
                return null;
            }
            if (!_requestTask.TryGetValue(v, out var queue))
                _requestTask[v] = ObjectPool.Get<Queue<STask<PB.PBMessage>>>();
            STask<PB.PBMessage> task = new();
            queue.Enqueue(task);
            Send(request);
            return task.MakeAutoCancel();
        }

        /// <summary>
        /// 断开当前链接
        /// </summary>
        public void DisConnect()
        {
            net?.DisConnect();
            net = null;
        }

        internal void Dispose()
        {
            DisConnect();
            _requestTask.Clear();
        }

        internal void Update(long tick)
        {
            while (msgs.TryDequeue(out var item))
            {
                _onResponse(item);
                //逻辑处理大于10 ms 放到下一帧处理
                if (DateTime.Now.Ticks - tick > 100000)
                    break;
            }
        }
    }
}
