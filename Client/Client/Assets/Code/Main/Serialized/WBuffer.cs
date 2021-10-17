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

    const byte maxMask = 128;
    const byte sizeInt = sizeof(int) + 1;
    const byte sizeLong = sizeof(long) + 2;

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
        uint ret = 0;

        for (int i = 0; i < sizeInt; i++)
        {
            byte v = bytes[Position++];
            if (v < maxMask)
            {
                ret |= (uint)v << (7 * i);
                break;
            }
            else
                ret |= (uint)(v & 0x7F) << (7 * i);
        }

        return (int)ret;
    }
    public long ReadLong()
    {
        ulong ret = 0;

        for (int i = 0; i < sizeLong; i++)
        {
            byte v = bytes[Position++];
            if (v < maxMask)
            {
                ret |= (ulong)v << (7 * i);
                break;
            }
            else
                ret |= (ulong)(v & 0x7F) << (7 * i);
        }

        return (long)ret;
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
        uint uv = (uint)v;
        do
        {
            if (Position >= bytes.Length) ReSize(bytes.Length * 2);
            bytes[Position] = (byte)(uv & ~maxMask);
            uv >>= 7;
            if (uv > 0) bytes[Position] |= maxMask;
            Position++;
        } while (uv != 0);
    }
    public void Write(long v)
    {
        ulong uv = (ulong)v;
        do
        {
            if (Position >= bytes.Length) ReSize(bytes.Length * 2);
            bytes[Position] = (byte)((byte)uv & ~maxMask);
            uv >>= 7;
            if (uv > 0) bytes[Position] |= maxMask;
            Position++;
        } while (uv != 0);
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
        if (Position + sizeInt + 1 + len >= bytes.Length) ReSize(bytes.Length * 2 + sizeInt + 1 + len);
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
        if (Position + (length + sizeInt) >= bytes.Length) ReSize(bytes.Length * 2 + length + sizeInt);
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