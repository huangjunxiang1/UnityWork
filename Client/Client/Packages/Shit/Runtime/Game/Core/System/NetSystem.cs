﻿using Game;
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
        readonly Dictionary<Type, Queue<TaskAwaiter<PB.PBMessage>>> _requestTask = new();
        Queue<TaskAwaiter<PB.PBMessage>> _swap = new();
        ConcurrentQueue<PBMessage> msgs = new();

        void _onError(int error)
        {
            Loger.Error("Net Error Code:" + error);
            GameM.Event.RunEvent(new EC_NetError { code = error });
        }
        void _onResponse(PB.PBMessage message)
        {
            var type = message.GetType();
            uint cmd = Types.GetCMDCode(type);

#if DebugEnable
            PrintField.Print($"<Color=#00FF00>收到消息</Color> cmd:[{(ushort)cmd},{cmd >> 16}]  content:{{0}}", message);
#endif
            if (message.rpc > 0)
            {
                //自动注册的事件一般是底层事件 所以先执行底层监听
                GameM.Event.RunRPCEvent(message.rpc, message);
            }
            else
            {
                //自动注册的事件一般是底层事件 所以先执行底层监听
                GameM.Event.RunEvent(message);

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
            net.onReceive += msg =>
            {
                msgs.Enqueue(msg);
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
        public void Send(PBMessage message)
        {
#if DebugEnable
            uint cmd = Types.GetCMDCode(message.GetType());
            PrintField.Print($"<Color=#FF0000>发送消息</Color> cmd:[{(ushort)cmd},{cmd >> 16}]  content:{{0}}", message);
#endif
            net.Send(message);
        }

        /// <summary>
        /// 异步返回的消息发送
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public TaskAwaiter<PB.PBMessage> SendAsync(PBMessage request)
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
            if (!_requestTask.TryGetValue(rsp, out Queue<TaskAwaiter<PB.PBMessage>> queue))
            {
                queue = new Queue<TaskAwaiter<PB.PBMessage>>();
                _requestTask[rsp] = queue;
            }
            TaskAwaiter<PB.PBMessage> task = new();
            queue.Enqueue(task);
            Send(request);
            return task.MakeAutoCancel(true);
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
                _onResponse(item);
                //逻辑处理大于10 ms 放到下一帧处理
                if (DateTime.Now.Ticks - tick > 100000)
                    break;
            }
        }
    }
}