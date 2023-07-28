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
        public abstract void Error(NetError error,Exception ex);
        public void Work()
        {
            new Thread(_work) { IsBackground = true }.Start();
        }

        protected abstract void SendBuffer();
        protected abstract void ReceiveBuffer();

        void _work()
        {
            SendBuffer();
            ReceiveBuffer();
        }
    }
}
