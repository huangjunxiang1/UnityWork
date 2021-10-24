using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WBuffer
{
    public WBuffer(byte[] data)
    {
        this.bytes = data;
    }
    public WBuffer(int capacity = 20)
    {
        capacity = Math.Max(capacity, 1);
        this.bytes = new byte[capacity];
    }

    const byte byteFlag = 128;
    static readonly byte[] _temp4Bytes = new byte[4];
    public readonly static byte[] EmptyBytes = new byte[0];

    byte[] bytes;

    public int Position { get; set; }

    public bool ReadBool()
    {
        return bytes[Position++] == 1;
    }
    public int ReadInt()
    {
        return (int)ReadUint();
    }
    public uint ReadUint()
    {
        uint ret = 0;

        for (int i = 0; i < sizeof(int); i++)
        {
            byte v = bytes[Position++];
            if (v < byteFlag)
            {
                ret |= (uint)v << (7 * i);
                return ret;
            }
            else
                ret |= (uint)(v & 0x7F) << (7 * i);
        }
        return ret | ((uint)bytes[Position++] << (7 * 4));
    }
    public long ReadLong()
    {
        return (long)ReadUlong();
    }
    public ulong ReadUlong()
    {
        ulong ret = 0; 

        for (int i = 0; i < sizeof(long); i++)
        {
            byte v = bytes[Position++];
            if (v < byteFlag)
            {
                ret |= (ulong)v << (7 * i);
                return ret;
            }
            else
                ret |= (ulong)(v & 0x7F) << (7 * i);
        }
        return ret | ((ulong)bytes[Position++] << (7 * 8));
    }
    public float ReadFloat()
    {
        _temp4Bytes[0] = bytes[Position + 0];
        _temp4Bytes[1] = bytes[Position + 1];
        _temp4Bytes[2] = bytes[Position + 2];
        _temp4Bytes[3] = bytes[Position + 3];
        Position += 4;
        return BitConverter.ToSingle(_temp4Bytes, 0);
    }
    public string ReadString()
    {
        int len = ReadInt();
        if (len == 0) return string.Empty;
        string s = Encoding.UTF8.GetString(bytes, Position, len);
        Position += len;
        return s;
    }
    public byte[] ReadBytes()
    {
        int len = ReadInt();
        if (len == 0) return EmptyBytes;

        byte[] ret = new byte[len];
        for (int i = 0; i < len; i++)
            ret[i] = bytes[Position++];
        return ret;
    }

    public void Write(bool v)
    {
        if (Position >= bytes.Length) ReSize(bytes.Length * 2);
        bytes[Position++] = v ? (byte)1 : (byte)0;
    }
    public void Write(int v)
    {
        Write((uint)v);
    }
    public void Write(uint uv)
    {
        int byteCnt;
        if (uv < 1 << 7) byteCnt = 1;
        else if (uv < 1 << 14) byteCnt = 2;
        else if (uv < 1 << 21) byteCnt = 3;
        else if (uv < 1 << 28) byteCnt = 4;
        else byteCnt = 5;

        if (Position + byteCnt >= bytes.Length) ReSize(Math.Max(bytes.Length * 2, Position + byteCnt + 1));
        for (int i = 0; i < byteCnt - 1; i++)
        {
            bytes[Position++] = (byte)(uv | byteFlag);
            uv >>= 7;
        }
        bytes[Position++] = (byte)uv;
    }
    public void Write(long v)
    {
        Write((ulong)v);
    }
    public void Write(ulong uv)
    {
        int byteCnt;
        ulong t = 1;
        if (uv < t << 7) byteCnt = 1;
        else if (uv < t << 14) byteCnt = 2;
        else if (uv < t << 21) byteCnt = 3;
        else if (uv < t << 28) byteCnt = 4;
        else if (uv < t << 35) byteCnt = 5;
        else if (uv < t << 42) byteCnt = 6;
        else if (uv < t << 49) byteCnt = 7;
        else if (uv < t << 56) byteCnt = 8;
        else byteCnt = 9;

        if (Position + byteCnt >= bytes.Length) ReSize(Math.Max(bytes.Length * 2, Position + byteCnt + 1));
        for (int i = 0; i < byteCnt - 1; i++)
        {
            bytes[Position++] = (byte)(uv | byteFlag);
            uv >>= 7;
        }
        bytes[Position++] = (byte)uv;
    }
    public void Write(float v)
    {
        if (Position + 4 >= bytes.Length) ReSize(bytes.Length * 2 + 4);
        byte[] bs = BitConverter.GetBytes(v);
        bytes[Position + 0] = bs[0];
        bytes[Position + 1] = bs[1];
        bytes[Position + 2] = bs[2];
        bytes[Position + 3] = bs[3];
        Position += 4;
    }
    public void Write(string v)
    {
        if (string.IsNullOrEmpty(v))
        {
            Write(0);
            return;
        }

        int len = Encoding.UTF8.GetByteCount(v);
        if (Position + sizeof(int) + 1 + len >= bytes.Length) ReSize(bytes.Length * 2 + sizeof(int) + 1 + len);
        Write(len);
        Encoding.UTF8.GetBytes(v, 0, v.Length, bytes, Position);
        Position += len;
    }
    public void Write(byte[] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        Write(v, 0, v.Length);
    }
    public void Write(byte[] v, int index, int length)
    {
        if (Position + sizeof(int) + 1 + length >= bytes.Length) ReSize(bytes.Length * 2 + sizeof(int) + 1 + length);
        Write(length);
        int len = index + length;
        for (; index < len; index++)
            bytes[Position++] = v[index];
    }

    public byte[] GetBytes()
    {
        return bytes;
    }
    public byte[] ToBytes()
    {
        byte[] b = new byte[Position];

        for (int i = 0; i < Position; i++)
            b[i] = bytes[i];

        return b;
    }

    public void ReSize(int newSize)
    {
        byte[] b = new byte[newSize];
        for (int i = 0; i < bytes.Length; i++)
            b[i] = bytes[i];
        bytes = b;
    }
}