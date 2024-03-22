using Event;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public enum NetStates
    {
        None,
        Connect,
    }
    public enum ServerType
    {
        TCP,
        UDP
    }
    public enum NetError
    {
        UnKnown,
        ReadError,
        ParseError,
        DataError,
    }
    public abstract class SBaseNet
    {
        public SBaseNet(IPEndPoint ip, int syncThreadId = 0, uint rpc = 0)
        {
            this.IP = ip;
            this.SyncThreadID = syncThreadId == 0 ? Thread.CurrentThread.ManagedThreadId : syncThreadId;
            this.rpc = rpc;
            this.Session = Util.RandomInt();

            tsc = ThreadSynchronizationContext.GetOrCreate(this.SyncThreadID);
        }

        ThreadSynchronizationContext tsc;
        protected ConcurrentQueue<IMessage> sendQueues = new();
        ConcurrentDictionary<Type, ConcurrentQueue<STask<IMessage>>> RequestTask = new();

        protected byte[] _sBuffer = new byte[ushort.MaxValue];
        protected byte[] _rBuffer = new byte[ushort.MaxValue];

        public IPEndPoint IP { get; set; }
        public int SyncThreadID { get; }
        public uint rpc { get; }
        public int Session { get; }
        public NetStates states { get; protected set; }
        public abstract ServerType serverType { get; }
        public Action<SBaseNet, NetError> onError;
        public Action<IMessage> onMessage;
        public Action<SBaseNet> onDisconnect;

        public abstract Task<bool> Connect();
        public abstract void DisConnect();
        public void Send(IMessage message)
        {
#if DebugEnable
            uint cmd = MessageParser.GetCMDCode(message.GetType());
            PrintField.Print($"<Color=#FF0000>发送消息</Color> cmd:[{(ushort)cmd},{cmd >> 16}]  content:{{0}}", message);
#endif
            sendQueues.Enqueue(message);
        }
        public STask<IMessage> SendAsync(IMessage request)
        {
            Type k = request.GetType();
            var v = MessageParser.GetResponseType(k);
            if (v == null)
            {
                Loger.Error("没有responseType类型 req=" + k);
                return null;
            }
            if (!RequestTask.TryGetValue(v, out var queue))
                RequestTask[v] = queue = ObjectPool.Get<ConcurrentQueue<STask<IMessage>>>();
            STask<IMessage> task = new();
            queue.Enqueue(task);
            Send(request);
            return task.MakeAutoCancel();
        }
        public void Work()
        {
            new Thread(_work) { IsBackground = true }.Start();
        }

        protected abstract void ReceiveBuffer();
        protected abstract void SendBuffer();
        protected void Error(NetError error, Exception ex)
        {
            //除了解析出错 其他都是非正常出错
            if (error != NetError.ParseError)
                this.DisConnect();
            Loger.Error($"网络错误 error={ex} \n stack={Loger.GetStackTrace()}");
            onError?.Invoke(this, error);
        }
        protected void ReceiveMessage(IMessage message)
        {
            if (message.rpc == 0 && this.rpc != 0) message.rpc = this.rpc;
#if DebugEnable
            var type = message.GetType();
            uint cmd = MessageParser.GetCMDCode(type);
            PrintField.Print($"<Color=#00FF00>收到消息</Color> cmd:[{(ushort)cmd},{cmd >> 16}]  content:{{0}}", message);
#endif
            tsc.Post(() =>
            {
                this.onMessage?.Invoke(message);
                if (RequestTask.TryRemove(message.GetType(), out var queue))
                {
                    while (queue.TryDequeue(out var v))
                        v.TrySetResult(message);
                    ObjectPool.Return(queue);
                }
            });
        }

        void _work()
        {
            ReceiveBuffer();
            SendBuffer();
        }
    }
}
