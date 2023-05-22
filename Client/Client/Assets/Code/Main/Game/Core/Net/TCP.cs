using PB;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

namespace Main
{
    public class TCP : BaseNet
    {
        public TCP(IPEndPoint ip) : base(ip)
        {
            client = new TcpClient();
            client.NoDelay = true;
            client.ReceiveTimeout = 3000;
            client.ReceiveBufferSize = ushort.MaxValue;
            client.SendTimeout = 3000;
            client.SendBufferSize = ushort.MaxValue;
        }

        TcpClient client;
        bool isConnecting = false;
        Task connectTask;

        public override ServerType serverType => ServerType.TCP;

        public override async TaskAwaiter<bool> Connect()
        {
            if (client.Connected)
                return true;
            if (isConnecting)
            {
                await connectTask;
                return client.Connected;
            }
            isConnecting = true;
            await (connectTask = client.ConnectAsync(IP.Address, IP.Port));
            connectTask = null;
            isConnecting = false;
            if (client.Connected)
                states = NetStates.Connect;
            return client.Connected;
        }

        public override void DisConnect()
        {
            if (!client.Connected)
                return;
            client.Close();
            client.Dispose();
            states = NetStates.None;
        }

        public override void Error(NetError error, Exception ex)
        {
            //除了解析出错 其他都是非正常出错
            if (error != NetError.ParseError)
            {
                client.Dispose();
                states = NetStates.None;
            }
            Loger.Error($"网络错误 error={ex} \n stack={Loger.GetStackTrace()}");
            ThreadSynchronizationContext.Instance.Post(() => onError?.Invoke((int)error));
        }

        protected override async TaskAwaiter SendBuffer()
        {
            while (states != NetStates.None)
            {
                while (queues.TryDequeue(out var d))
                {
                    try
                    {
                        await client.GetStream().WriteAsync(d.bs, d.index, d.length);
                    }
                    catch (Exception ex)
                    {
                        //被动断开链接
                        if (states != NetStates.None)
                            this.DisConnect();
                        Loger.Error("发送消息错误 ex=" + ex);
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(d.bs);
                    }
                }
                Thread.Sleep(1);
            }
        }

        protected override async TaskAwaiter ReceiveBuffer()
        {
            var bs = ArrayPool<byte>.Shared.Rent(ushort.MaxValue);
            while (states != NetStates.None)
            {
                try
                {
                    int len;
                    try
                    {
                        await client.GetStream().ReadAsync(bs, 0, 2);
                        len = (bs[0] | bs[1] << 8) + 2;

                        if (len < 8)
                        {
                            Error(NetError.DataError, new Exception($"数据长度不对 len={len}"));
                            break;
                        }

                        await client.GetStream().ReadAsync(bs, 2, len - 2);
                    }
                    catch (Exception ex)
                    {
                        //被动断开链接
                        if (states != NetStates.None)
                            this.DisConnect();
                        Error(NetError.ReadError, ex);
                        break;
                    }

                    uint cmd = bs[4]
                        | (uint)bs[5] << 8
                        | (uint)bs[6] << 16
                        | (uint)bs[7] << 24;

                    byte checkCode = bs[2];
                    for (int i = 3; i < len; i++)
                        checkCode += bs[i];

                    if (checkCode != 0)
                    {
                        Error(NetError.DataError, new Exception($"数据校验不正确 cmd:main={(ushort)cmd} sub={cmd >> 16}"));
                        break;
                    }

                    byte msgType = bs[3];
                    uint rpcid = 0;
                    if (msgType == 1)
                    {
                        rpcid = bs[8]
                            | (uint)bs[9] << 8
                            | (uint)bs[10] << 16
                            | (uint)bs[11] << 24;
                    }

                    try
                    {
                        Type t = Types.GetCMDType(cmd);
                        int index = msgType == 0 ? 8 : 12;
                        PBReader reader = new PBReader(new MemoryStream(bs, index, len - index), 0, len - index);
                        var msg = (PB.IPBMessage)Activator.CreateInstance(t);
                        msg.Read(reader);

                        onReceive(rpcid, msg);
                    }
                    catch (Exception ex)
                    {
                        Error(NetError.ParseError, ex);
                    }
                }
                catch (Exception ex)
                {
                    Error(NetError.UnKnown, ex);
                    break;
                }
            }
            ArrayPool<byte>.Shared.Return(bs);
        }
    }
}
