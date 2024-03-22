using Event;
using Game;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Core
{
    public class NetSystem 
    {
        public NetSystem(CoreWorld world) => this.world = world;

        CoreWorld world;
        SBaseNet clientToServerNet;//客户端的单链接
        ConcurrentDictionary<int, SBaseNet> sessions = new();

        void _onError(SBaseNet net, NetError code)
        {
            world.Event.RunEvent(new EC_NetError { code = (int)code });
        }
        void _onResponse(IMessage message)
        {
            //自动注册的事件一般是底层事件 所以先执行底层监听
            if (message.rpc > 0)
                world.Event.RunRPCEvent(message.rpc, (object)message);
            else
                world.Event.RunEvent((object)message);
        }

        public void Add(SBaseNet _net)
        {
            if (sessions.TryGetValue(_net.Session, out var n))
                n.DisConnect();
            _net.onError = _onError;
            _net.onMessage = _onResponse;
            sessions[_net.Session] = _net;
            _net.onDisconnect += n => this.DisConnect(n.Session);
        }
        /// <summary>
        /// 链接服务器IP
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ipEndPoint"></param>
        public async STask<bool> Connect(SBaseNet _net)
        {
            if (sessions.ContainsKey(_net.Session))
            {
                Loger.Error("会话已存在 id=" + _net.Session);
                return true;
            }
            clientToServerNet = _net;
            _net.onError = _onError;
            _net.onMessage = _onResponse;
            sessions[_net.Session] = _net;
            var b = await _net.Connect();
            if (b)
                _net.Work();
            return b;
        }
        /// <summary>
        /// 重连
        /// </summary>
        /// <param name="_net"></param>
        /// <returns></returns>
        public async STask<bool> ReConnect()
        {
            if (clientToServerNet == null) return false;
            var b = await clientToServerNet.Connect();
            if (b)
                clientToServerNet.Work();
            return b;
        }

        /// <summary>
        /// 非返回的消息发送
        /// </summary>
        /// <param name="message"></param>
        public void Send(IMessage message)
        {
            if (clientToServerNet == null)
            {
                Loger.Error("会话不存在 id=");
                return;
            }
            clientToServerNet.Send(message);
        }

        /// <summary>
        /// 异步返回的消息发送
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public STask<IMessage> SendAsync(IMessage message)
        {
            if (clientToServerNet == null)
            {
                Loger.Error("会话不存在 id=");
                return null;
            }
            return clientToServerNet.SendAsync(message);
        }

        /// <summary>
        /// 断开当前链接
        /// </summary>
        public void DisConnect()
        {
            if (clientToServerNet == null)
            {
                Loger.Error("会话不存在 id=");
                return;
            }
            clientToServerNet.DisConnect();
        }
        public void DisConnect(int session)
        {
            if (!sessions.TryRemove(session, out var net))
            {
                Loger.Error("会话不存在 id=");
                return;
            }
            net.DisConnect();
        }

        public void Dispose()
        {
            clientToServerNet?.DisConnect();
            clientToServerNet = null;
            foreach (var item in sessions)
                item.Value.DisConnect();
            sessions.Clear();
        }
    }
}
