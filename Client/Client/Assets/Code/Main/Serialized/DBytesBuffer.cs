using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public unsafe class DBytesBuffer: DBuffer
{
    public DBytesBuffer(byte[] data)
    {
        this.bytes = data;
    }
    public DBytesBuffer(int capacity = 20)
    {
        capacity = Math.Max(capacity, 1);
        this.bytes = new byte[capacity];
    }

    public override int Position
    {
        get { return point; }
    }

    byte[] bytes;
    int point;

    public override byte Readbyte()
    {
        return bytes[point++];
    }
    public override uint Readuint()
    {
        fixed (byte* ptr = &bytes[Position])
        {
            if (Compress)
            {
                uint ret = 0;
                for (int i = 0; i < sizeof(uint); i++)
                {
                    byte v = ptr[i];
                    if (v < byteFlag)
                    {
                        ret |= (uint)(v << (7 * i));
                        point += i + 1;
                        return ret;
                    }
                    else
                        ret |= (uint)((v & 0x7F) << (7 * i));
                }
                point += sizeof(uint) + 1;
                return ret | (uint)((ptr[sizeof(uint)] << (7 * 4)));
            }
            else
            {
                point += sizeof(uint);
                return *(uint*)ptr;
            }
        }
    }
    public override ulong Readulong()
    {
        fixed (byte* ptr = &bytes[Position])
        {
            if (Compress)
            {
                ulong ret = 0;

                for (int i = 0; i < sizeof(ulong); i++)
                {
                    byte v = ptr[i];
                    if (v < byteFlag)
                    {
                        ret |= (ulong)v << (7 * i);
                        point += i + 1;
                        return ret;
                    }
                    else
                        ret |= (ulong)(v & 0x7F) << (7 * i);
                }
                point += sizeof(ulong) + 1;
                return ret | ((ulong)ptr[sizeof(ulong)] << (7 * 8));
            }
            else
            {
                point += sizeof(ulong);
                return *(ulong*)ptr;
            }
        }
    }
    public override float Readfloat()
    {
        fixed (byte* ptr = &bytes[Position])
        {
            FixPoint fp = default;
            fp.valueInt = *(int*)ptr;
            point += sizeof(int);
            return fp.valueFloat;
        }
    }
    public override string Readstring()
    {
        int len = Readint();
        if (len == 0) return string.Empty;
        string s = Encoding.UTF8.GetString(bytes, Position, len);
        point += len;
        return s;
    }

    public override void Write(byte v)
    {
        if (Position >= bytes.Length) ReSize(Math.Max(bytes.Length * 2, Position + sizeof(byte)));
        bytes[point++] = v;
    }
    public override void Write(uint v)
    {
        if (Compress)
        {
            int byteCnt;
            if (v < 1 << 7) byteCnt = 1;
            else if (v < 1 << 14) byteCnt = 2;
            else if (v < 1 << 21) byteCnt = 3;
            else if (v < 1 << 28) byteCnt = 4;
            else byteCnt = 5;

            if (Position + byteCnt >= bytes.Length) ReSize(Math.Max(bytes.Length * 2, Position + byteCnt));

            fixed (byte* ptr = &bytes[Position])
            {
                for (int i = 0; i < byteCnt - 1; i++)
                {
                    ptr[i] = (byte)(v | byteFlag);
                    v >>= 7;
                }
                ptr[byteCnt - 1] = (byte)v;
                point += byteCnt;
            }
        }
        else
        {
            if (Position + sizeof(uint) >= bytes.Length) ReSize(Math.Max(bytes.Length * 2, Position + sizeof(uint)));

            fixed (byte* ptr = &bytes[Position])
                *(uint*)ptr = v;
            point += sizeof(uint);
        }
    }
    public override void Write(ulong v)
    {
        if (Compress)
        {
            int byteCnt;
            if (v < 1L << 7) byteCnt = 1;
            else if (v < 1L << 14) byteCnt = 2;
            else if (v < 1L << 21) byteCnt = 3;
            else if (v < 1L << 28) byteCnt = 4;
            else if (v < 1L << 35) byteCnt = 5;
            else if (v < 1L << 42) byteCnt = 6;
            else if (v < 1L << 49) byteCnt = 7;
            else if (v < 1L << 56) byteCnt = 8;
            else byteCnt = 9;

            if (Position + byteCnt >= bytes.Length) ReSize(Math.Max(bytes.Length * 2, Position + byteCnt));

            fixed (byte* ptr = &bytes[Position])
            {
                for (int i = 0; i < byteCnt - 1; i++)
                {
                    ptr[i] = (byte)(v | byteFlag);
                    v >>= 7;
                }
                ptr[byteCnt - 1] = (byte)v;
                point += byteCnt;
            }
        }
        else
        {
            if (Position + sizeof(ulong) >= bytes.Length) ReSize(Math.Max(bytes.Length * 2, Position + sizeof(ulong)));

            fixed (byte* ptr = &bytes[Position])
                *(ulong*)ptr = v;
            point += sizeof(ulong);
        }
    }
    public override void Write(float v)
    {
        if (Position + sizeof(int) >= bytes.Length) ReSize(Math.Max(bytes.Length * 2, Position + sizeof(int)));
        FixPoint fp = default;
        fp.valueFloat = v;
        fixed (byte* ptr = &bytes[Position])
            *(int*)ptr = fp.valueInt;
        point += sizeof(int);
    }
    public override void Write(string v)
    {
        if (string.IsNullOrEmpty(v))
        {
            Write(0);
            return;
        }

        int len = Encoding.UTF8.GetByteCount(v);
        Write(len);
        if (Position + len >= bytes.Length) ReSize(Math.Max(bytes.Length * 2, Position + len));
        Encoding.UTF8.GetBytes(v, 0, v.Length, bytes, Position);
        point += len;
    }

    public byte[] GetNativeBytes()
    {
        return bytes;
    }
    public override byte[] ToBytes()
    {
        return ToBytes(0, Position);
    }
    public override byte[] ToBytes(int position, int length)
    {
        byte[] b = new byte[length];
        fixed (byte* ptr = bytes, ptr2 = b)
        {
            for (int i = 0; i < length; i++)
                ptr2[i] = ptr[position + i];
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
    public override void Seek(int index)
    {
        point = index;
    }
    public override void Dispose()
    {

    }
}
