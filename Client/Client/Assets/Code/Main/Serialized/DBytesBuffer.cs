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
    public override int Readint()
    {
        if (Compress)
        {
            uint v = readVarint32();
            return (int)v;
        }
        else
        {
            fixed (byte* ptr = &bytes[Position])
            {
                point += sizeof(int);
                return *(int*)ptr;
            }
        }
    }
    public override long Readlong()
    {
        if (Compress)
        {
            ulong v = readVarint64();
            return (long)v;
        }
        else
        {
            fixed (byte* ptr = &bytes[Position])
            {
                point += sizeof(long);
                return *(long*)ptr;
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
    public override void Write(int v)
    {
        if (Compress)
        {
            writeVarint32((uint)v);
        }
        else
        {
            if (Position + sizeof(int) >= bytes.Length) ReSize(Math.Max(bytes.Length * 2, Position + sizeof(int)));

            fixed (byte* ptr = &bytes[Position])
                *(int*)ptr = v;
            point += sizeof(int);
        }
    }
    public override void Write(long v)
    {
        if (Compress)
        {
            writeVarint64((ulong)v);
        }
        else
        {
            if (Position + sizeof(long) >= bytes.Length) ReSize(Math.Max(bytes.Length * 2, Position + sizeof(long)));

            fixed (byte* ptr = &bytes[Position])
                *(long*)ptr = v;
            point += sizeof(long);
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
        byte[] bs = new byte[length];
        Array.Copy(bytes, position, bs, 0, length);
        return bs;
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
        byte[] bs = new byte[newSize];
        Array.Copy(bytes, 0, bs, 0, point);
        bytes = bs;
    }
    public override void Seek(int index)
    {
        point = index;
    }
    public override void Dispose()
    {

    }


    void writeVarint32(uint v)
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
    void writeVarint64(ulong v)
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
    uint readVarint32()
    {
        fixed (byte* ptr = &bytes[Position])
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
    }
    ulong readVarint64()
    {
        fixed (byte* ptr = &bytes[Position])
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
    }
}
