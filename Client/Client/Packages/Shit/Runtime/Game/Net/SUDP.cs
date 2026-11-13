using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

public class SUDP : SBaseNet
{
    public SUDP(IPEndPoint ip) : base(ip)
    {
        if (Socket.OSSupportsIPv4)
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        else if (Socket.OSSupportsIPv6)
            _socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
    }
    public SUDP(UdpClient udp) : base((IPEndPoint)udp.Client.RemoteEndPoint)
    {
        _socket = udp.Client;
        states = NetStates.Connect;
    }

    Socket _socket;

    public override ServerType serverType => ServerType.UDP;

    public override async Task<bool> Connect()
    {
        if (_socket.Connected)
            return true;
        await _socket.ConnectAsync(IP);
        if (_socket.Connected)
            states = NetStates.Connect;
        return _socket.Connected;
    }

    public override void DisConnect()
    {
        if (states == NetStates.None)
            return;
        states = NetStates.None;
        _socket.Dispose();
        onDisconnect.Invoke();
    }

    protected async override void ReceiveBuffer()
    {
        var ms = new MemoryStream(_rBuffer, 0, _rBuffer.Length);
        var segment = new ArraySegment<byte>(_rBuffer);

        while (states != NetStates.None)
        {
            try
            {
                int received;
                try
                {
                    received = await _socket.ReceiveAsync(segment, SocketFlags.None).ConfigureAwait(false);
                }
                catch (ObjectDisposedException) { break; }
                catch (SocketException ex)
                {
                    if (states == NetStates.None) break;
                    Error(NetError.ReadError, ex);
                    break;
                }
                catch (Exception ex)
                {
                    Error(NetError.UnKnown, ex);
                    break;
                }

                if (received < 3) continue;

                int len = (_rBuffer[0] | _rBuffer[1] << 8) + 2;

                if (len < 8 || len != received)
                {
                    Error(NetError.DataError, new Exception($"长度不匹配 len={len} received={received}"));
                    continue;
                }

                int cmd = BitConverter.ToInt16(_rBuffer, 3);
                byte checkCode = _rBuffer[2];
                for (int i = 3; i < len; i++)
                    checkCode += _rBuffer[i];

                if (checkCode != 0)
                {
                    Error(NetError.DataError, new Exception($"数据校验不正确 cmd:[{cmd}]"));
                    continue;
                }

                try
                {
                    Type t = MessageParser.GetCMDType(cmd);
                    var message = (Message)Activator.CreateInstance(t);
                    message.rpc = BitConverter.ToInt64(_rBuffer, 5);
                    message.actorId = BitConverter.ToInt64(_rBuffer, 13);
                    int strLen = BitConverter.ToInt16(_rBuffer, 21);
                    message.error = BitConverter.ToString(_rBuffer, 23, strLen);
                    ms.Seek(23 + strLen, SeekOrigin.Begin);
                    Serializer.NonGeneric.Serialize(ms, message);

                    this.ReceiveMessage(message);
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
        var ms = new MemoryStream(_sBuffer, 0, _sBuffer.Length, true, true);

        while (states != NetStates.None)
        {
            while (sendQueues.TryDequeue(out var message))
            {
                try
                {
                    int cmd = MessageParser.GetCMDCode(message.GetType());

                    _sBuffer[3] = (byte)cmd;
                    _sBuffer[4] = (byte)(cmd >> 8);

                    _sBuffer[5] = (byte)message.rpc;
                    _sBuffer[6] = (byte)(message.rpc >> 8);
                    _sBuffer[7] = (byte)(message.rpc >> 16);
                    _sBuffer[8] = (byte)(message.rpc >> 24);
                    _sBuffer[9] = (byte)(message.rpc >> 32);
                    _sBuffer[10] = (byte)(message.rpc >> 40);
                    _sBuffer[11] = (byte)(message.rpc >> 48);
                    _sBuffer[12] = (byte)(message.rpc >> 56);

                    _sBuffer[13] = (byte)message.actorId;
                    _sBuffer[14] = (byte)(message.actorId >> 8);
                    _sBuffer[15] = (byte)(message.actorId >> 16);
                    _sBuffer[16] = (byte)(message.actorId >> 24);
                    _sBuffer[17] = (byte)(message.actorId >> 32);
                    _sBuffer[18] = (byte)(message.actorId >> 40);
                    _sBuffer[19] = (byte)(message.actorId >> 48);
                    _sBuffer[20] = (byte)(message.actorId >> 56);

                    _sBuffer[21] = _sBuffer[22] = 0;

                    try
                    {
                        ms.Seek(23, SeekOrigin.Begin);
                        Serializer.NonGeneric.Deserialize(message.GetType(), ms, message);
                    }
                    catch (Exception e)
                    {
                        Loger.Error("序列化出错 ex=" + e);
                        continue;
                    }

                    var len = ms.Position;
                    if (len > ushort.MaxValue)
                    {
                        Loger.Error($"数据过大 len={len}  class={message.GetType().FullName}");
                        continue;
                    }

                    _sBuffer[0] = (byte)(len - 2);
                    _sBuffer[1] = (byte)((len - 2) >> 8);

                    byte checkCode = 0;
                    for (int i = 3; i < len; i++)
                        checkCode += _sBuffer[i];
                    _sBuffer[2] = (byte)(~checkCode + 1);

                    var sendSegment = new ArraySegment<byte>(_sBuffer, 0, (int)len);
                    await _socket.SendAsync(sendSegment, SocketFlags.None).ConfigureAwait(false);
                }
                catch (ObjectDisposedException) { return; }
                catch (SocketException ex)
                {
                    if (states != NetStates.None)
                        DisConnect();
                    return;
                }
                catch (Exception e)
                {
                    //被动断开链接
                    if (states != NetStates.None)
                        this.DisConnect();
                    Loger.Error("Send message error :" + e);
                    return;
                }
            }
            await Task.Delay(1).ConfigureAwait(false);
        }
    }
}