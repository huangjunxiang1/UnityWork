using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Unity.Mathematics;
using UnityEngine.UIElements;

/// <summary>
/// int和long采用Varint编码
/// </summary>
public class DBuffer : IDisposable
{
    public DBuffer(Stream stream)
    {
        this.stream = stream;
    }
    public DBuffer(int length) : this(new MemoryStream(length)) { }
    public DBuffer(byte[] bytes) : this(new MemoryStream(bytes, 0, bytes.Length, true, true)) { }
    public DBuffer(byte[] bytes, int index, int length) : this(new MemoryStream(bytes, index, length, true, true)) { }

    Stream stream;

    public const int Verify = 20220702;

    public int Position
    {
        get
        {
            long point = stream.Position;
            if (point > int.MaxValue)
                throw new Exception("长度超出限制");
            return (int)point;
        }
    }
    public int Length
    {
        get
        {
            return (int)stream.Length;
        }
    }
    public Stream Stream { get { return stream; } }

    /// <summary>
    /// 数据是否压缩
    /// </summary>
    public bool Compress { get; set; } = true;
    public string Hash { get; private set; }

    public byte Readbyte()
    {
        return (byte)stream.ReadByte();
    }
    public bool Readbool() { return Readbyte() == 1; }
    public int Readint()
    {
        if (Compress)
        {
            uint v = readVarint32();
            return (int)v;
        }
        else
        {
            return stream.ReadByte()
                 | stream.ReadByte() << 8
                 | stream.ReadByte() << 16
                 | stream.ReadByte() << 24;
        }
    }
    public uint Readuint()
    {
        if (Compress)
        {
            uint v = readVarint32();
            return v;
        }
        else
        {
            return (uint)(stream.ReadByte()
                 | stream.ReadByte() << 8
                 | stream.ReadByte() << 16
                 | stream.ReadByte() << 24);
        }
    }
    public long Readlong()
    {
        if (Compress)
        {
            ulong v = readVarint64();
            return (long)v;
        }
        else
        {
            long v = 0;
            for (int i = 0; i < sizeof(long); i++)
                v |= (long)stream.ReadByte() << (8 * i);
            return v;
        }
    }
    public ulong Readulong()
    {
        if (Compress)
        {
            return readVarint64();
        }
        else
        {
            ulong v = 0;
            for (int i = 0; i < sizeof(long); i++)
                v |= (ulong)stream.ReadByte() << (8 * i);
            return v;
        }
    }
    public float Readfloat()
    {
        FixPoint fp = default;
        int value = stream.ReadByte()
                  | stream.ReadByte() << 8
                  | stream.ReadByte() << 16
                  | stream.ReadByte() << 24;
        fp.valueInt = value;
        return fp.valueFloat;
    }
    public string Readstring()
    {
        int len = Readint();
        if (len == 0) return string.Empty;

        checkArrayLengthLimit(len);
        byte[] buffer = ArrayPool<byte>.Shared.Rent(len);
        try
        {
            stream.Read(buffer, 0, len);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
        return Encoding.UTF8.GetString(buffer, 0, len);
    }
    public float2 Readfloat2()
    {
        return new float2(Readfloat(), Readfloat());
    }
    public int2 Readint2()
    {
        return new int2(Readint(), Readint());
    }
    public float3 Readfloat3()
    {
        return new float3(Readfloat(), Readfloat(), Readfloat());
    }
    public int3 Readint3()
    {
        return new int3(Readint(), Readint(), Readint());
    }
    public float4 Readfloat4()
    {
        return new float4(Readfloat(), Readfloat(), Readfloat(), Readfloat());
    }
    public int4 Readint4()
    {
        return new int4(Readint(), Readint(), Readint(), Readint());
    }

    public bool[] Readbools()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<bool>();
        checkArrayLengthLimit(len);
        bool[] arr = new bool[len];
        for (int i = 0; i < len; i++) arr[i] = Readbool();
        return arr;
    }
    public bool[][] Readboolss()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<bool[]>();
        checkArrayLengthLimit(len);
        bool[][] arr = new bool[len][];
        for (int i = 0; i < len; i++) arr[i] = Readbools();
        return arr;
    }
    public int[] Readints()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<int>();
        checkArrayLengthLimit(len);
        int[] arr = new int[len];
        for (int i = 0; i < len; i++) arr[i] = Readint();
        return arr;
    }
    public int[][] Readintss()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<int[]>();
        checkArrayLengthLimit(len);
        int[][] arr = new int[len][];
        for (int i = 0; i < len; i++) arr[i] = Readints();
        return arr;
    }
    public uint[] Readuints()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<uint>();
        checkArrayLengthLimit(len);
        uint[] arr = new uint[len];
        for (int i = 0; i < len; i++) arr[i] = Readuint();
        return arr;
    }
    public uint[][] Readuintss()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<uint[]>();
        checkArrayLengthLimit(len);
        uint[][] arr = new uint[len][];
        for (int i = 0; i < len; i++) arr[i] = Readuints();
        return arr;
    }
    public long[] Readlongs()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<long>();
        checkArrayLengthLimit(len);
        long[] arr = new long[len];
        for (int i = 0; i < len; i++) arr[i] = Readlong();
        return arr;
    }
    public long[][] Readlongss()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<long[]>();
        checkArrayLengthLimit(len);
        long[][] arr = new long[len][];
        for (int i = 0; i < len; i++) arr[i] = Readlongs();
        return arr;
    }
    public ulong[] Readulongs()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<ulong>();
        checkArrayLengthLimit(len);
        ulong[] arr = new ulong[len];
        for (int i = 0; i < len; i++) arr[i] = Readulong();
        return arr;
    }
    public ulong[][] Readulongss()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<ulong[]>();
        checkArrayLengthLimit(len);
        ulong[][] arr = new ulong[len][];
        for (int i = 0; i < len; i++) arr[i] = Readulongs();
        return arr;
    }
    public float[] Readfloats()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<float>();
        checkArrayLengthLimit(len);
        float[] arr = new float[len];
        for (int i = 0; i < len; i++) arr[i] = Readfloat();
        return arr;
    }
    public float[][] Readfloatss()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<float[]>();
        checkArrayLengthLimit(len);
        float[][] arr = new float[len][];
        for (int i = 0; i < len; i++) arr[i] = Readfloats();
        return arr;
    }
    public string[] Readstrings()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<string>();
        checkArrayLengthLimit(len);
        string[] arr = new string[len];
        for (int i = 0; i < len; i++) arr[i] = Readstring();
        return arr;
    }
    public string[][] Readstringss()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<string[]>();
        checkArrayLengthLimit(len);
        string[][] arr = new string[len][];
        for (int i = 0; i < len; i++) arr[i] = Readstrings();
        return arr;
    }
    public byte[] Readbytes()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<byte>();
        checkArrayLengthLimit(len);
        byte[] arr = new byte[len];
        for (int i = 0; i < len; i++) arr[i] = Readbyte();
        return arr;
    }
    public bool ReadHeaderInfo()
    {
        this.Compress = false;
        if (this.Readint() != Verify)
        {
#if UNITY_2019_4_OR_NEWER
            Loger.Error("is not DBuffer data");
#else
            Console.WriteLine("is not DBuffer data");
#endif
            return false;
        }
        var compress = Readbool();
        char[] hash = new char[6];
        for (int i = 0; i < hash.Length; i++)
        {
            var b = this.Readbyte() % 36;
            //用ascll码转换成数字和小写字母
            if (b < 10) b = 48 + b;
            else b = 97 + b - 10;
            hash[i] = Convert.ToChar(b);
        }
        this.Hash = new string(hash);
        this.Compress = compress;
        return true;
    }
    public float2[] Readfloat2s()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<float2>();
        checkArrayLengthLimit(len);
        float2[] arr = new float2[len];
        for (int i = 0; i < len; i++) arr[i] = Readfloat2();
        return arr;
    }
    public float2[][] Readfloat2ss()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<float2[]>();
        checkArrayLengthLimit(len);
        float2[][] arr = new float2[len][];
        for (int i = 0; i < len; i++) arr[i] = Readfloat2s();
        return arr;
    }
    public int2[] Readint2s()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<int2>();
        checkArrayLengthLimit(len);
        int2[] arr = new int2[len];
        for (int i = 0; i < len; i++) arr[i] = Readint2();
        return arr;
    }
    public int2[][] Readint2ss()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<int2[]>();
        checkArrayLengthLimit(len);
        int2[][] arr = new int2[len][];
        for (int i = 0; i < len; i++) arr[i] = Readint2s();
        return arr;
    }
    public float3[] Readfloat3s()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<float3>();
        checkArrayLengthLimit(len);
        float3[] arr = new float3[len];
        for (int i = 0; i < len; i++) arr[i] = Readfloat3();
        return arr;
    }
    public float3[][] Readfloat3ss()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<float3[]>();
        checkArrayLengthLimit(len);
        float3[][] arr = new float3[len][];
        for (int i = 0; i < len; i++) arr[i] = Readfloat3s();
        return arr;
    }
    public int3[] Readint3s()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<int3>();
        checkArrayLengthLimit(len);
        int3[] arr = new int3[len];
        for (int i = 0; i < len; i++) arr[i] = Readint3();
        return arr;
    }
    public int3[][] Readint3ss()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<int3[]>();
        checkArrayLengthLimit(len);
        int3[][] arr = new int3[len][];
        for (int i = 0; i < len; i++) arr[i] = Readint3s();
        return arr;
    }
    public float4[] Readfloat4s()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<float4>();
        checkArrayLengthLimit(len);
        float4[] arr = new float4[len];
        for (int i = 0; i < len; i++) arr[i] = Readfloat4();
        return arr;
    }
    public float4[][] Readfloat4ss()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<float4[]>();
        checkArrayLengthLimit(len);
        float4[][] arr = new float4[len][];
        for (int i = 0; i < len; i++) arr[i] = Readfloat4s();
        return arr;
    }
    public int4[] Readint4s()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<int4>();
        checkArrayLengthLimit(len);
        int4[] arr = new int4[len];
        for (int i = 0; i < len; i++) arr[i] = Readint4();
        return arr;
    }
    public int4[][] Readint4ss()
    {
        int len = Readint();
        if (len == 0) return Array.Empty<int4[]>();
        checkArrayLengthLimit(len);
        int4[][] arr = new int4[len][];
        for (int i = 0; i < len; i++) arr[i] = Readint4s();
        return arr;
    }

    public void Write(byte v)
    {
        stream.WriteByte(v);
    }
    public void Write(bool v) { Write(v ? (byte)1 : (byte)0); }
    public void Write(int v)
    {
        if (Compress)
        {
            writeVarint32((uint)v);
        }
        else
        {
            stream.WriteByte((byte)v);
            stream.WriteByte((byte)(v >> 8));
            stream.WriteByte((byte)(v >> 16));
            stream.WriteByte((byte)(v >> 24));
        }
    }
    public void Write(uint v)
    {
        if (Compress)
        {
            writeVarint32(v);
        }
        else
        {
            stream.WriteByte((byte)v);
            stream.WriteByte((byte)(v >> 8));
            stream.WriteByte((byte)(v >> 16));
            stream.WriteByte((byte)(v >> 24));
        }
    }
    public void Write(long v)
    {
        if (Compress)
        {
            writeVarint64((ulong)v);
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
    public void Write(ulong v)
    {
        if (Compress)
        {
            writeVarint64(v);
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
    public void Write(float v)
    {
        FixPoint fp = default;
        fp.valueFloat = v;
        stream.WriteByte((byte)fp.valueInt);
        stream.WriteByte((byte)(fp.valueInt >> 8));
        stream.WriteByte((byte)(fp.valueInt >> 16));
        stream.WriteByte((byte)(fp.valueInt >> 24));
    }
    public void Write(string v)
    {
        if (string.IsNullOrEmpty(v))
        {
            Write(0);
            return;
        }

        int len = Encoding.UTF8.GetByteCount(v);
        checkArrayLengthLimit(len);
        Write(len);

        byte[] buffer = ArrayPool<byte>.Shared.Rent(len);
        try
        {
            Encoding.UTF8.GetBytes(v, 0, v.Length, buffer, 0);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
        stream.Write(buffer, 0, len);
    }
    public void Write(float2 v)
    {
        Write(v.x);
        Write(v.y);
    }
    public void Write(int2 v)
    {
        Write(v.x);
        Write(v.y);
    }
    public void Write(float3 v)
    {
        Write(v.x);
        Write(v.y);
        Write(v.z);
    }
    public void Write(int3 v)
    {
        Write(v.x);
        Write(v.y);
        Write(v.z);
    }
    public void Write(float4 v)
    {
        Write(v.x);
        Write(v.y);
        Write(v.z);
        Write(v.w);
    }
    public void Write(int4 v)
    {
        Write(v.x);
        Write(v.y);
        Write(v.z);
        Write(v.w);
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
        checkArrayLengthLimit(length);
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
    }
    public void Write(bool[] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        Write(v, 0, v.Length);
    }
    public void Write(bool[] v, int index, int length)
    {
        checkArrayLengthLimit(length);
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
    }
    public void Write(bool[][] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        int len = v.Length;
        checkArrayLengthLimit(len);
        Write(len);
        for (int i = 0; i < len; i++)
            Write(v[i]);
    }
    public void Write(int[] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        Write(v, 0, v.Length);
    }
    public void Write(int[] v, int index, int length)
    {
        checkArrayLengthLimit(length);
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
    }
    public void Write(int[][] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        int len = v.Length;
        checkArrayLengthLimit(len);
        Write(len);
        for (int i = 0; i < len; i++)
            Write(v[i]);
    }
    public void Write(uint[] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        Write(v, 0, v.Length);
    }
    public void Write(uint[] v, int index, int length)
    {
        checkArrayLengthLimit(length);
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
    }
    public void Write(uint[][] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        int len = v.Length;
        checkArrayLengthLimit(len);
        Write(len);
        for (int i = 0; i < len; i++)
            Write(v[i]);
    }
    public void Write(long[] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        Write(v, 0, v.Length);
    }
    public void Write(long[] v, int index, int length)
    {
        checkArrayLengthLimit(length);
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
    }
    public void Write(long[][] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        int len = v.Length;
        checkArrayLengthLimit(len);
        Write(len);
        for (int i = 0; i < len; i++)
            Write(v[i]);
    }
    public void Write(ulong[] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        Write(v, 0, v.Length);
    }
    public void Write(ulong[] v, int index, int length)
    {
        checkArrayLengthLimit(length);
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
    }
    public void Write(ulong[][] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        int len = v.Length;
        checkArrayLengthLimit(len);
        Write(len);
        for (int i = 0; i < len; i++)
            Write(v[i]);
    }
    public void Write(float[] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        Write(v, 0, v.Length);
    }
    public void Write(float[] v, int index, int length)
    {
        checkArrayLengthLimit(length);
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
    }
    public void Write(float[][] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        int len = v.Length;
        checkArrayLengthLimit(len);
        Write(len);
        for (int i = 0; i < len; i++)
            Write(v[i]);
    }
    public void Write(string[] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        Write(v, 0, v.Length);
    }
    public void Write(string[] v, int index, int length)
    {
        checkArrayLengthLimit(length);
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
    }
    public void Write(string[][] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        int len = v.Length;
        checkArrayLengthLimit(len);
        Write(len);
        for (int i = 0; i < len; i++)
            Write(v[i]);
    }
    public void Write(float2[] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        Write(v, 0, v.Length);
    }
    public void Write(float2[] v, int index, int length)
    {
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
    }
    public void Write(float2[][] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        int len = v.Length;
        checkArrayLengthLimit(len);
        Write(len);
        for (int i = 0; i < len; i++)
            Write(v[i]);
    }
    public void Write(int2[] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        Write(v, 0, v.Length);
    }
    public void Write(int2[] v, int index, int length)
    {
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
    }
    public void Write(int2[][] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        int len = v.Length;
        checkArrayLengthLimit(len);
        Write(len);
        for (int i = 0; i < len; i++)
            Write(v[i]);
    }
    public void Write(float3[] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        Write(v, 0, v.Length);
    }
    public void Write(float3[] v, int index, int length)
    {
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
    }
    public void Write(float3[][] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        int len = v.Length;
        checkArrayLengthLimit(len);
        Write(len);
        for (int i = 0; i < len; i++)
           Write(v[i]);
    }
    public void Write(int3[] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        Write(v, 0, v.Length);
    }
    public void Write(int3[] v, int index, int length)
    {
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
    }
    public void Write(int3[][] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        int len = v.Length;
        checkArrayLengthLimit(len);
        Write(len);
        for (int i = 0; i < len; i++)
            Write(v[i]);
    }
    public void Write(float4[] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        Write(v, 0, v.Length);
    }
    public void Write(float4[] v, int index, int length)
    {
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
    }
    public void Write(float4[][] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        int len = v.Length;
        checkArrayLengthLimit(len);
        Write(len);
        for (int i = 0; i < len; i++)
            Write(v[i]);
    }
    public void Write(int4[] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        Write(v, 0, v.Length);
    }
    public void Write(int4[] v, int index, int length)
    {
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
    }
    public void Write(int4[][] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        int len = v.Length;
        checkArrayLengthLimit(len);
        Write(len);
        for (int i = 0; i < len; i++)
            Write(v[i]);
    }
    public void Write(DBuffer buff)
    {
        int size = buff.Position;
        buff.Seek(0);
        Write(size);

        byte[] buffer = ArrayPool<byte>.Shared.Rent(size);
        try
        {
            buff.stream.Read(buffer, 0, size);
            stream.Write(buffer, 0, size);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
    public void WriteHeaderInfo()
    {
        var c = this.Compress;
        this.Compress = false;
        this.Write(Verify);
        this.Compress = c;
        this.Write(this.Compress);
        int pos = this.Position;
        this.Seek(pos + 6);
        byte[] hash = new byte[6];
        for (int i = 11; i < this.Length; i++)
            hash[(i - 11) % 6] ^= this.Readbyte();
        this.Seek(pos);
        for (int i = 0; i < hash.Length; i++)
            this.Write(hash[i]);
    }

    public byte[] ToBytes()
    {
        return ToBytes(0, Position);
    }
    public byte[] ToBytes(int position, int length)
    {
        this.Seek(position);
        byte[] b = new byte[length];
        stream.Read(b, 0, length);
        return b;
    }

    public void Seek(int index)
    {
        stream.Seek(index, SeekOrigin.Begin);
    }

    public virtual void Dispose()
    {
        stream.Dispose();
    }

    void writeVarint32(uint v)
    {
        while (v > 127)
        {
            stream.WriteByte((byte)(v | 128));
            v >>= 7;
        }
        stream.WriteByte((byte)v);
    }
    void writeVarint64(ulong v)
    {
        while (v > 127)
        {
            stream.WriteByte((byte)(v | 128));
            v >>= 7;
        }
        stream.WriteByte((byte)v);
    }
    uint readVarint32()
    {
        uint ret = 0;
        for (int i = 0; i < 4; i++)
        {
            int v = stream.ReadByte();
            if (v < 128)
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

        for (int i = 0; i < 9; i++)
        {
            int v = stream.ReadByte();
            if (v < 128)
            {
                ret |= (ulong)v << (7 * i);
                return ret;
            }
            else
                ret |= (ulong)(v & 0x7F) << (7 * i);
        }
        return ret | (((ulong)stream.ReadByte()) << (7 * 9));
    }
    [Conditional(ConstDefCore.DebugEnableString)]
    void checkArrayLengthLimit(int len)
    {
        /*if (len > int.MaxValue)
            throw new Exception($"长度超过{ushort.MaxValue}的限制 当前长度={len}");*/
    }

    [StructLayout(LayoutKind.Explicit)]
    protected struct FixPoint
    {
        [FieldOffset(0)]
        public float valueFloat;
        [FieldOffset(0)]
        public int valueInt;
    }
}