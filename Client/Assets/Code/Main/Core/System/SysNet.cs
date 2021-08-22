using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Core
{
    public enum NetType
    {
        TCP,
        KCP,
    }
    public static class SysNet 
    {
        public class NetEntity
        {
            public NetEntity(NetType type,string ip,int port)
            {
                this.Type = type;
                this.IP = ip;
                this.Port = port;
            }
            public NetType Type { get; private set; }
            public string IP { get; private set; }
            public int Port { get; private set; }
        }

        public static Dictionary<NetType, NetEntity> NetMap { get; } = new Dictionary<NetType, NetEntity>();

        public static NetEntity Connect(NetType type, string ip, int port)
        {
            if (NetMap.TryGetValue(type, out NetEntity ret))
                return ret;
            NetEntity net = new NetEntity(type, ip, port);
            NetMap[type] = net;
            return net;
        }
        public static void DisConnect()
        {

        }
        public static void Send(byte[] bytes, int index, int length)
        {

        }
    }
}
