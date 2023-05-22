using Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Main;
using System.Net.Sockets;
using PB;
using System.Buffers;
using System.Threading;
using System.Collections.Concurrent;

namespace Game
{
    public class NetSystem
    {
        class Engine : MonoBehaviour
        {
            public NetSystem sys;
            private void Update()
            {
                sys.update();
            }
        }

        public NetSystem()
        {
            GameObject go = new GameObject($"[{nameof(NetSystem)}]");
            GameObject.DontDestroyOnLoad(go);
            go.AddComponent<Engine>().sys = this;
        }

        Engine engine;
        BaseNet net;
        readonly Dictionary<Type, Queue<TaskAwaiter<PB.IPBMessage>>> _requestTask = new();
        Queue<TaskAwaiter<PB.IPBMessage>> _swap = new();
        ConcurrentQueue<Data> msgs = new();

        void _onError(int error)
        {
            Loger.Error("Net Error Code:" + error);
            GameM.Event.RunEvent((int)EventIDM.NetError, error);
        }
        void _onResponse(uint actorId, PB.IPBMessage message)
        {
            var type = message.GetType();
            uint cmd = Types.GetCMDCode(type);

            if (actorId > 0)
            {
                //自动注册的事件一般是底层事件 所以先执行底层监听
                bool has = GameM.Event.RunMsgWithKey(cmd, actorId, message);

#if DebugEnable
                if (cmd != 1 << 16)
                    PrintField.Print($"收到消息 cmd: main={(ushort)cmd} sub={cmd >> 16}  content:{0}", message);
#endif
            }
            else
            {
                //自动注册的事件一般是底层事件 所以先执行底层监听
                bool has = GameM.Event.RunMsg(cmd, message);
#if DebugEnable
                if (!has && (message != null && !_requestTask.ContainsKey(type)))
                    Loger.Error($"没有注册的消息返回 cmd: main={(ushort)cmd} sub={cmd >> 16}  msg:{type.FullName}");
                if (cmd != 1 << 16)
                    PrintField.Print($"收到消息 cmd: main={(ushort)cmd} sub={cmd >> 16}  content:{0}", message);
#endif

                if (message != null)
                {
                    if (_requestTask.TryGetValue(type, out var queue))
                    {
                        //防止TrySetResult执行过程中 有另外的异步发送同时执行
                        _requestTask[type] = _swap;
                        while (queue.Count > 0)
                            queue.Dequeue().TrySetResult(message);
                        _swap = queue;
                    }
                }
            }
        }

        /// <summary>
        /// 链接服务器IP
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ipEndPoint"></param>
        public async TaskAwaiter<bool> Connect(ServerType type, IPEndPoint ipEndPoint)
        {
            net?.DisConnect();
            switch (type)
            {
                case ServerType.TCP:
                    net = new TCP(ipEndPoint);
                    break;
                case ServerType.UDP:
                default:
                    Loger.Error($"未识别的链接类型->{type}");
                    return false;
            }
            net.onReceive += (rpc, msg) =>
            {
                msgs.Enqueue(new Data { rpc = rpc, message = msg });
            };
            net.onError += _onError;
            var b = await net.Connect();
            if (b)
                net.Work();
            return b;
        }

        /// <summary>
        /// 非返回的消息发送
        /// </summary>
        /// <param name="message"></param>
        public void Send(IPBMessage message)
        {
            Send(0, message);
        }
        public void Send(uint actorId, IPBMessage message)
        {
            PBWriter writer = PBBuffPool.Get();
            try
            {
                message.Write(writer);

                int len = writer.Position;
                int headerLen;
                if (actorId > 0)
                    headerLen = 12;
                else
                    headerLen = 8;
                int clen = len + headerLen;

                if (clen > ushort.MaxValue)
                {
                    Loger.Error($"数据过大 len={clen}");
                    return;
                }

                var bs = ArrayPool<byte>.Shared.Rent(ushort.MaxValue);
                try
                {
                    writer.Seek(0);
                    writer.Stream.Read(bs, headerLen, len);

                    uint cmd = Types.GetCMDCode(message.GetType());
                    bs[0] = (byte)(clen - 2);
                    bs[1] = (byte)((clen - 2) >> 8);
                    bs[3] = (byte)(actorId > 0 ? 1 : 0);
                    bs[4] = (byte)cmd;
                    bs[5] = (byte)(cmd >> 8);
                    bs[6] = (byte)(cmd >> 16);
                    bs[7] = (byte)(cmd >> 24);
                    if (actorId > 0)
                    {
                        bs[8] = (byte)actorId;
                        bs[9] = (byte)(actorId >> 8);
                        bs[10] = (byte)(actorId >> 16);
                        bs[11] = (byte)(actorId >> 24);
                    }
                    byte checkCode = 0;
                    for (int i = 3; i < clen; i++)
                        checkCode += bs[i];
                    bs[2] = (byte)(~checkCode + 1);

                    net.Send(bs, 0, clen);

                    if (cmd != 1 << 16)
                        PrintField.Print($"发送消息 cmd: main={(ushort)cmd} sub={cmd >> 16}  content:{0}", message);
                }
                catch (Exception)
                {
                    ArrayPool<byte>.Shared.Return(bs);
                    throw;
                }
            }
            catch (Exception e)
            {
                Loger.Error(e);
            }
            finally
            {
                PBBuffPool.Return(writer);
            }
        }

        /// <summary>
        /// 异步返回的消息发送
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public TaskAwaiter<PB.IPBMessage> SendAsync(IPBMessage request)
        {
            return SendAsync(0, request);
        }
        public TaskAwaiter<PB.IPBMessage> SendAsync(uint actorId, IPBMessage request)
        {
            Type t;
#if ILRuntime
            if (request is ILRuntime.Runtime.Enviorment.CrossBindingAdaptorType ilRequest)
                t = ilRequest.ILInstance.Type.ReflectionType;
            else
#endif
            t = request.GetType();

            var rsp = Types.GetResponseType(t);
            if (rsp == null)
            {
                Loger.Error("没有responseType类型 req=" + t);
                return null;
            }
            if (!_requestTask.TryGetValue(rsp, out Queue<TaskAwaiter<PB.IPBMessage>> queue))
            {
                queue = new Queue<TaskAwaiter<PB.IPBMessage>>();
                _requestTask[rsp] = queue;
            }
            TaskAwaiter<PB.IPBMessage> task = new();
            queue.Enqueue(task);
            Send(actorId, request);
            return task;
        }

        public TaskAwaiter<PB.IPBMessage> SendAsync(IPBMessage request, TaskManager taskManager)
        {
            return SendAsync(0, request, taskManager);
        }
        public TaskAwaiter<PB.IPBMessage> SendAsync(uint actorId, IPBMessage request, TaskManager taskManager)
        {
            Type t;
#if ILRuntime
            if (request is ILRuntime.Runtime.Enviorment.CrossBindingAdaptorType ilRequest)
                t = ilRequest.ILInstance.Type.ReflectionType;
            else
#endif
            t = request.GetType();

            var rsp = Types.GetResponseType(t);
            if (rsp == null)
            {
                Loger.Error("没有responseType类型 req=" + t);
                return null;
            }
            if (!_requestTask.TryGetValue(rsp, out Queue<TaskAwaiter<PB.IPBMessage>> queue))
            {
                queue = new Queue<TaskAwaiter<PB.IPBMessage>>();
                _requestTask[rsp] = queue;
            }
            TaskAwaiter<PB.IPBMessage> task = taskManager.Create<PB.IPBMessage>();
            queue.Enqueue(task);
            Send(actorId, request);
            return task;
        }

        /// <summary>
        /// 断开当前链接
        /// </summary>
        public void DisConnect()
        {
            net?.DisConnect();
            net = null;
        }

        public void Dispose()
        {
            DisConnect();
            _requestTask.Clear();
            GameObject.DestroyImmediate(engine);
        }

        void update()
        {
            var tick = DateTime.Now.Ticks;
            while (msgs.TryDequeue(out var item))
            {
                _onResponse(item.rpc, item.message);
                //逻辑处理大于10 ms 放到下一帧处理
                if (DateTime.Now.Ticks - tick > 100000)
                    break;
            }
        }

        struct Data
        {
            public uint rpc;
            public PB.IPBMessage message;
        }
    }
}
