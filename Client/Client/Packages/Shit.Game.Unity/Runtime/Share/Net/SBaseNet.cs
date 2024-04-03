using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Game
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
        public SBaseNet(IPEndPoint ip)
        {
            this.IP = ip;
            this.Session = Util.RandomInt();
        }

        protected ConcurrentQueue<IMessage> sendQueues = new();

        protected byte[] _sBuffer = new byte[ushort.MaxValue];
        protected byte[] _rBuffer = new byte[ushort.MaxValue];
        protected static byte[] _ping = new byte[8] { 6, 0, 21, 0, 0, 0, 232, 3 };

        public IPEndPoint IP { get; set; }
        public int Session { get; }
        public NetStates states { get; protected set; }
        public abstract ServerType serverType { get; }
        public Action<NetError> onError;
        public Action<IMessage> onMessage;
        public Action onDisconnect;

        public abstract Task<bool> Connect();
        public abstract void DisConnect();
        public void Send(IMessage message)
        {
            sendQueues.Enqueue(message);
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
            onError.Invoke(error);
        }
        protected void ReceiveMessage(IMessage message)
        {
            this.onMessage.Invoke(message);
        }

        void _work()
        {
            ReceiveBuffer();
            SendBuffer();
        }
    }
}
