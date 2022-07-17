using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class DStreamBuffer : DBuffer
{
    public DStreamBuffer(Stream stream)
    {
        this.stream = stream;
    }

    Stream stream;
    byte[] tempBytes;

    public override int Position
    {
        get
        {
            long point = stream.Position;
            if (point > int.MaxValue)
                throw new Exception("长度超出限制");
            return (int)point;
        }
    }

    public override byte Readbyte()
    {
        return (byte)stream.ReadByte();
    }
    public override int Readint()
    {
        if (Compress)
        {
            uint v = readVarint32();
            return (int)((v >> 1) ^ -(v & 1));
        }
        else
        {
            return stream.ReadByte()
                 | stream.ReadByte() << 8
                 | stream.ReadByte() << 16
                 | stream.ReadByte() << 24;
        }
    }
    public override long Readlong()
    {
        if (Compress)
        {
            ulong v = readVarint64();
            return (long)(v >> 1) ^ -((long)v & 1);
        }
        else
        {
            long v = 0;
            for (int i = 0; i < sizeof(long); i++)
                v |= (long)stream.ReadByte() << (8 * i);
            return v;
        }
    }
    public override float Readfloat()
    {
        FixPoint fp = default;
        int value = stream.ReadByte()
                  | stream.ReadByte() << 8
                  | stream.ReadByte() << 16
                  | stream.ReadByte() << 24;
        fp.valueInt = value;
        return fp.valueFloat;
    }
    public override string Readstring()
    {
        int len = Readint();
        if (len == 0) return string.Empty;
        if (tempBytes == null || tempBytes.Length < len)
            tempBytes = new byte[len];
        stream.Read(tempBytes, 0, len);
        return Encoding.UTF8.GetString(tempBytes, 0, len);
    }

    public override void Write(byte v)
    {
        stream.WriteByte(v);
    }
    public override void Write(int v)
    {
        if (Compress)
        {
            writeVarint32((uint)((v >> 31) ^ (v << 1)));
        }
        else
        {
            stream.WriteByte((byte)v);
            stream.WriteByte((byte)(v >> 8));
            stream.WriteByte((byte)(v >> 16));
            stream.WriteByte((byte)(v >> 24));
        }
    }
    public override void Write(long v)
    {
        if (Compress)
        {
            writeVarint64((ulong)((v >> 63) ^ (v << 1)));
        }
        else
        {
            stream.WriteByte((byte)v);
            stream.WriteByte((byte)(v >> 8));
            stream.WriteByte((byte)(v >> 16));
            stream.WriteByte((byte)(v >> 24));
            stream.WriteByte((byte)(v >> 32));
            stream.WriteByte((byte)(v >> 40));
            stream.WriteByte((byte)(v >> 48));
            stream.WriteByte((byte)(v >> 56));
        }
    }
    public override void Write(float v)
    {
        FixPoint fp = default;
        fp.valueFloat = v;
        stream.WriteByte((byte)fp.valueInt);
        stream.WriteByte((byte)(fp.valueInt >> 8));
        stream.WriteByte((byte)(fp.valueInt >> 16));
        stream.WriteByte((byte)(fp.valueInt >> 24));
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
        if (tempBytes == null || tempBytes.Length < len)
            tempBytes = new byte[len];
        Encoding.UTF8.GetBytes(v, 0, v.Length, tempBytes, Position);
        stream.Write(tempBytes, 0, len);
    }

    public override byte[] ToBytes()
    {
        return ToBytes(0, Position);
    }
    public override byte[] ToBytes(int position, int length)
    {
        this.Seek(position);
        byte[] b = new byte[length];
        stream.Read(b, 0, length);
        return b;
    }
    public override void Seek(int index)
    {
        stream.Seek(index, SeekOrigin.Begin);
    }
    public override void Dispose()
    {
        base.Dispose();
        stream.Close();
        stream.Dispose();
    }

    void writeVarint32(uint v)
    {
        int byteCnt;
        if (v < 1 << 7) byteCnt = 1;
        else if (v < 1 << 14) byteCnt = 2;
        else if (v < 1 << 21) byteCnt = 3;
        else if (v < 1 << 28) byteCnt = 4;
        else byteCnt = 5;

        for (int i = 0; i < byteCnt - 1; i++)
        {
            stream.WriteByte((byte)(v | byteFlag));
            v >>= 7;
        }
        stream.WriteByte((byte)v);
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

        for (int i = 0; i < byteCnt - 1; i++)
        {
            stream.WriteByte((byte)(v | byteFlag));
            v >>= 7;
        }
        stream.WriteByte((byte)v);
    }
    uint readVarint32()
    {
        uint ret = 0;
        for (int i = 0; i < sizeof(uint); i++)
        {
            int v = stream.ReadByte();
            if (v < byteFlag)
            {
                ret |= (uint)(v << (7 * i));
                return ret;
            }
            else
                ret |= (uint)((v & 0x7F) << (7 * i));
        }
        return ret | (uint)(stream.ReadByte() << (7 * 4));
    }
    ulong readVarint64()
    {
        ulong ret = 0;

        for (int i = 0; i < sizeof(ulong); i++)
        {
            int v = stream.ReadByte();
            if (v < byteFlag)
            {
                ret |= (ulong)v << (7 * i);
                return ret;
            }
            else
                ret |= (ulong)(v & 0x7F) << (7 * i);
        }
        return ret | (((ulong)stream.ReadByte()) << (7 * 8));
    }
}
