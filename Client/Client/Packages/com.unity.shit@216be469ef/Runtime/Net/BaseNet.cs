using PB;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Main
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
    public abstract class BaseNet
    {
        public BaseNet(IPEndPoint ip)
        {
            IP = ip;
        }

        protected ConcurrentQueue<PBMessage> queues = new ConcurrentQueue<PBMessage>();
        protected byte[] _sBuffer = new byte[ushort.MaxValue];
        protected byte[] _rBuffer = new byte[ushort.MaxValue];

        public IPEndPoint IP { get; set; }
        public NetStates states { get; protected set; }
        public Action<PB.PBMessage> onReceive;
        public Action<int> onError;
        public abstract ServerType serverType { get; }

        public abstract Task<bool> Connect();
        public abstract void DisConnect();
        public void Send(PB.PBMessage message)
        {
            queues.Enqueue(message);
        }
        public void Error(NetError error,Exception ex)
        {
            //除了解析出错 其他都是非正常出错
            if (error != NetError.ParseError)
                this.DisConnect();
            Loger.Error($"网络错误 error={ex} \n stack={Loger.GetStackTrace()}");
            ThreadSynchronizationContext.Instance.Post(() => onError?.Invoke((int)error));
        }
        public void Work()
        {
            new Thread(_work) { IsBackground = true }.Start();
        }

        protected abstract void ReceiveBuffer();
        protected abstract void SendBuffer();

        void _work()
        {
            ReceiveBuffer();
            SendBuffer();
        }
    }
}
