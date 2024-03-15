using Game;
using PB;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public class SUDP : SBaseNet
    {
        public SUDP(IPEndPoint ip) : base(ip)
        {
            if (Socket.OSSupportsIPv4)
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            else if (Socket.OSSupportsIPv6)
                _socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
        }

        Socket _socket;
        int _sendLen;

        public override ServerType serverType => ServerType.UDP;

        public override async Task<bool> Connect()
        {
            if (_socket.Connected)
                return true;
            _socket.Connect(IP);
            await Task.CompletedTask;
            if (_socket.Connected)
                states = NetStates.Connect;
            return _socket.Connected;
        }

        public override void DisConnect()
        {
            if (!_socket.Connected)
                return;
            states = NetStates.None;
            _socket.Close();
            _socket.Dispose();
        }

        protected async override void ReceiveBuffer()
        {
            PBReader reader = new(new MemoryStream(_rBuffer, 0, _rBuffer.Length), 0, _rBuffer.Length);
            while (states != NetStates.None)
            {
                try
                {
                    int len;
                    try
                    {
                        await Task<int>.Factory.FromAsync(BeginReceive, EndReceive, null);

                        len = (_rBuffer[0] | _rBuffer[1] << 8) + 2;

                        if (len < 8)
                        {
                            Error(NetError.DataError, new Exception($"数据长度不对 len={len}"));
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        //被动断开链接
                        if (states != NetStates.None)
                            this.DisConnect();
                        Error(NetError.ReadError, ex);
                        break;
                    }

                    uint cmd = _rBuffer[4]
                        | (uint)_rBuffer[5] << 8
                        | (uint)_rBuffer[6] << 16
                        | (uint)_rBuffer[7] << 24;

                    byte checkCode = _rBuffer[2];
                    for (int i = 3; i < len; i++)
                        checkCode += _rBuffer[i];

                    if (checkCode != 0)
                    {
                        Error(NetError.DataError, new Exception($"数据校验不正确 cmd:[{(ushort)cmd},{cmd >> 16}]"));
                        break;
                    }

                    byte msgType = _rBuffer[3];
                    uint rpcid = 0;
                    if (msgType == 1)
                    {
                        rpcid = _rBuffer[8]
                            | (uint)_rBuffer[9] << 8
                            | (uint)_rBuffer[10] << 16
                            | (uint)_rBuffer[11] << 24;
                    }

                    try
                    {
                        Type t = Types.GetCMDType(cmd);
                        int index = msgType == 0 ? 8 : 12;
                        reader.SetMinMax(index, len);
                        reader.Seek(index);
                        var msg = (PB.PBMessage)Activator.CreateInstance(t);
                        msg.rpc = rpcid;
                        msg.Read(reader);

                        onReceive(msg);
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
        }

        protected override async void SendBuffer()
        {
            PBWriter writer = new(new MemoryStream(_sBuffer, 0, _sBuffer.Length, true, true));
            while (states != NetStates.None)
            {
                while (queues.TryDequeue(out var message))
                {
                    try
                    {
                        if (message.rpc > 0)
                            writer.Seek(12);
                        else
                            writer.Seek(8);

                        try
                        {
                            message.Write(writer);
                        }
                        catch (Exception e)
                        {
                            Loger.Error("序列化出错 ex=" + e);
                            continue;
                        }

                        int len = _sendLen = writer.Position;
                        if (len > ushort.MaxValue - 2)
                        {
                            Loger.Error($"数据过大 len={len}");
                            continue;
                        }

                        uint cmd = Types.GetCMDCode(message.GetType());
                        _sBuffer[0] = (byte)(len - 2);
                        _sBuffer[1] = (byte)((len - 2) >> 8);
                        _sBuffer[3] = (byte)(message.rpc > 0 ? 1 : 0);
                        _sBuffer[4] = (byte)cmd;
                        _sBuffer[5] = (byte)(cmd >> 8);
                        _sBuffer[6] = (byte)(cmd >> 16);
                        _sBuffer[7] = (byte)(cmd >> 24);
                        if (message.rpc > 0)
                        {
                            _sBuffer[8] = (byte)message.rpc;
                            _sBuffer[9] = (byte)(message.rpc >> 8);
                            _sBuffer[10] = (byte)(message.rpc >> 16);
                            _sBuffer[11] = (byte)(message.rpc >> 24);
                        }
                        byte checkCode = 0;
                        for (int i = 3; i < len; i++)
                            checkCode += _sBuffer[i];
                        _sBuffer[2] = (byte)(~checkCode + 1);

                        await Task<int>.Factory.FromAsync(BeginSend, EndSend, null);
                    }
                    catch (Exception e)
                    {
                        //被动断开链接
                        if (states != NetStates.None)
                            this.DisConnect();
                        Loger.Error("发送消息错误 ex=" + e);
                        return;
                    }
                }
                Thread.Sleep(1);
            }
        }

        IAsyncResult BeginSend(AsyncCallback callback, object state)
        {
            return _socket.BeginSendTo(_sBuffer, 0, _sendLen, SocketFlags.None, IP, callback, state);
        }
        int EndSend(IAsyncResult asyncResult)
        {
            return _socket.EndSend(asyncResult);
        }
        IAsyncResult BeginReceive(AsyncCallback callback, object state)
        {
            EndPoint ip = IP;
            return _socket.BeginReceiveFrom(_rBuffer, 0, _rBuffer.Length, SocketFlags.None, ref ip, callback, state);
        }
        int EndReceive(IAsyncResult asyncResult)
        {
            return _socket.EndReceive(asyncResult);
        }
    }
}
