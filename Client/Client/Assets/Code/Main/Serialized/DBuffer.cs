using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public unsafe class DBuffer
{
    public DBuffer(byte[] data)
    {
        this.bytes = data;
    }
    public DBuffer(int capacity = 20)
    {
        capacity = Math.Max(capacity, 1);
        this.bytes = new byte[capacity];
    }

    const byte byteFlag = 128;
    public readonly static byte[] EmptyBytes = new byte[0];

    byte[] bytes;

    public int Position { get; set; }

    /// <summary>
    /// 数据是否压缩
    /// </summary>
    public bool Compress { get; set; } = true;

    public bool ReadBool()
    {
        return bytes[Position++] == 1;
    }
    public int ReadInt()
    {
        fixed (byte* ptr = &bytes[Position])
        {
            if (Compress)
            {
                int ret = 0;
                for (int i = 0; i < sizeof(int); i++)
                {
                    byte v = ptr[i];
                    if (v < byteFlag)
                    {
                        ret |= v << (7 * i);
                        Position += i + 1;
                        return ret;
                    }
                    else
                        ret |= (v & 0x7F) << (7 * i);
                }
                Position += sizeof(int) + 1;
                return ret | (ptr[sizeof(int) + 1] << (7 * 4));
            }
            else
            {
                Position += sizeof(int);
                return (ptr[0]
                    | ptr[1] << 8
                    | ptr[2] << 16
                    | ptr[3] << 24);
            }
        }
    }
    public uint ReadUint()
    {
        return (uint)ReadInt();
    }
    public long ReadLong()
    {
        fixed (byte* ptr = &bytes[Position])
        {
            if (Compress)
            {
                long ret = 0;

                for (int i = 0; i < sizeof(long); i++)
                {
                    byte v = ptr[i];
                    if (v < byteFlag)
                    {
                        ret |= (long)v << (7 * i);
                        Position += i + 1;
                        return ret;
                    }
                    else
                        ret |= (long)(v & 0x7F) << (7 * i);
                }
                Position += sizeof(ulong) + 1;
                return ret | ((long)ptr[sizeof(ulong) + 1] << (7 * 8));
            }
            else
            {
                Position += sizeof(ulong);
                return (ptr[0]
                    | ptr[1] << 8
                    | ptr[2] << 16
                    | ptr[3] << 24
                    | ptr[4] << 32
                    | ptr[5] << 40
                    | ptr[6] << 48
                    | ptr[7] << 56);
            }
        }
    }
    public ulong ReadUlong()
    {
        return (ulong)ReadLong();
    }
    public float ReadFloat()
    {
        fixed (byte* ptr = &bytes[Position])
        {
            FloatInt fi = default;
            fi.valueUint =(uint)(ptr[0]
                               | ptr[1] << 8
                               | ptr[2] << 16
                               | ptr[3] << 24);
            Position += sizeof(float);
            return fi.valueFloat;
        }
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
        fixed (byte* ptr = &bytes[Position], ptr2 = ret)
        {
            for (int i = 0; i < len; i++)
                ptr2[i] = ptr[i];
        }
        Position += len;
        return ret;
    }

    public void Write(bool v)
    {
        if (Position >= bytes.Length) ReSize(Math.Max(bytes.Length * 2, Position + sizeof(bool)));
        bytes[Position++] = v ? (byte)1 : (byte)0;
    }
    public void Write(int v)
    {
        Write((uint)v);
    }
    public void Write(uint uv)
    {
        if (Compress)
        {
            int byteCnt;
            if (uv < 1 << 7) byteCnt = 1;
            else if (uv < 1 << 14) byteCnt = 2;
            else if (uv < 1 << 21) byteCnt = 3;
            else if (uv < 1 << 28) byteCnt = 4;
            else byteCnt = 5;

            if (Position + byteCnt >= bytes.Length) ReSize(Math.Max(bytes.Length * 2, Position + byteCnt));

            fixed (byte* ptr = &bytes[Position])
            {
                for (int i = 0; i < byteCnt - 1; i++)
                {
                    ptr[i] = (byte)(uv | byteFlag);
                    uv >>= 7;
                }
                ptr[byteCnt - 1] = (byte)uv;
                Position += byteCnt;
            }
        }
        else
        {
            if (Position + sizeof(uint) >= bytes.Length) ReSize(Math.Max(bytes.Length * 2, Position + sizeof(uint)));

            fixed (byte* ptr = &bytes[Position])
            {
                ptr[0] = (byte)uv;
                ptr[1] = (byte)(uv >> 8);
                ptr[2] = (byte)(uv >> 16);
                ptr[3] = (byte)(uv >> 24);
            }
            Position += sizeof(uint);
        }
    }
    public void Write(long v)
    {
        Write((ulong)v);
    }
    public void Write(ulong uv)
    {
        if (Compress)
        {
            int byteCnt;
            if (uv < 1ul << 7) byteCnt = 1;
            else if (uv < 1ul << 14) byteCnt = 2;
            else if (uv < 1ul << 21) byteCnt = 3;
            else if (uv < 1ul << 28) byteCnt = 4;
            else if (uv < 1ul << 35) byteCnt = 5;
            else if (uv < 1ul << 42) byteCnt = 6;
            else if (uv < 1ul << 49) byteCnt = 7;
            else if (uv < 1ul << 56) byteCnt = 8;
            else byteCnt = 9;

            if (Position + byteCnt >= bytes.Length) ReSize(Math.Max(bytes.Length * 2, Position + byteCnt));

            fixed (byte* ptr = &bytes[Position])
            {
                for (int i = 0; i < byteCnt - 1; i++)
                {
                    ptr[i] = (byte)(uv | byteFlag);
                    uv >>= 7;
                }
                ptr[byteCnt - 1] = (byte)uv;
                Position += byteCnt;
            }
        }
        else
        {
            if (Position + sizeof(ulong) >= bytes.Length) ReSize(Math.Max(bytes.Length * 2, Position + sizeof(ulong)));

            fixed (byte* ptr = &bytes[Position])
            {
                ptr[0] = (byte)uv;
                ptr[1] = (byte)(uv >> 8);
                ptr[2] = (byte)(uv >> 16);
                ptr[3] = (byte)(uv >> 24);
                ptr[4] = (byte)(uv >> 32);
                ptr[5] = (byte)(uv >> 40);
                ptr[6] = (byte)(uv >> 48);
                ptr[7] = (byte)(uv >> 56);
            }
            Position += sizeof(ulong);
        }
    }
    public void Write(float v)
    {
        if (Position + sizeof(float) >= bytes.Length) ReSize(Math.Max(bytes.Length * 2, Position + sizeof(float)));
        FloatInt fi = default;
        fi.valueFloat = v;
        fixed (byte* ptr = &bytes[Position])
        {
            ptr[0] = (byte)fi.valueUint;
            ptr[1] = (byte)(fi.valueUint >> 8);
            ptr[2] = (byte)(fi.valueUint >> 16);
            ptr[3] = (byte)(fi.valueUint >> 24);
        }
        Position += sizeof(float);
    }
    public void Write(string v)
    {
        if (string.IsNullOrEmpty(v))
        {
            Write(0);
            return;
        }

        int len = Encoding.UTF8.GetByteCount(v);
        if (Position + sizeof(int) + 1 + len >= bytes.Length) ReSize(Math.Max(bytes.Length * 2, Position + sizeof(int) + 1 + len));
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
        if (Position + sizeof(int) + 1 + length >= bytes.Length) ReSize(Math.Max(bytes.Length * 2, Position + length));
        Write(length);

        fixed (byte* ptr = &bytes[Position], ptr2 = &v[index])
        {
            for (int i = 0; i < length; i++)
                ptr[i] = ptr2[i];
        }
    }

    public byte[] GetBytes()
    {
        return bytes;
    }
    public byte[] ToBytes()
    {
        byte[] b = new byte[Position];
        fixed (byte* ptr = bytes, ptr2 = b)
        {
            int len = Position;
            for (int i = 0; i < len; i++)
                ptr2[i] = ptr[i];
        }
        return b;
    }

    public void ReSize(int newSize)
    {
#if DebugEnable
        if (bytes != null && newSize <= bytes.Length)
        {
            Loger.Error("newSize is too short");
            return;
        }
#endif
        byte[] b = new byte[newSize];
        fixed (byte* ptr = bytes, ptr2 = b)
        {
            int len = bytes.Length;
            for (int i = 0; i < len; i++)
                ptr2[i] = ptr[i];
        }
        bytes = b;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct FloatInt
    {
        [FieldOffset(0)]
        public float valueFloat;
        [FieldOffset(0)]
        public uint valueUint;
    }
}