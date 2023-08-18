using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
#if UNITY_2019_4_OR_NEWER
using UnityEngine;
#endif

/// <summary>
/// int和long采用Varint编码
/// </summary>
public class DBuffer : IDisposable
{
    public DBuffer(Stream stream)
    {
        this.stream = stream;
    }
    public DBuffer(int length) : this(new byte[length]) { }
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
#if UNITY_2019_4_OR_NEWER
    public Vector2 ReadVector2()
    {
        return new Vector2(Readfloat(), Readfloat());
    }
    public Vector2Int ReadVector2Int()
    {
        return new Vector2Int(Readint(), Readint());
    }
    public Vector3 ReadVector3()
    {
        return new Vector3(Readfloat(), Readfloat(), Readfloat());
    }
    public Vector3Int ReadVector3Int()
    {
        return new Vector3Int(Readint(), Readint(), Readint());
    }
    public Color ReadColor()
    {
        return new Color(Readfloat(), Readfloat(), Readfloat(), Readfloat());
    }
    public Color32 ReadColor32()
    {
        return new Color32(Readbyte(), Readbyte(), Readbyte(), Readbyte());
    }
#endif

    public bool[] Readbools()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        bool[] arr = new bool[len];
        for (int i = 0; i < len; i++)
            arr[i] = Readbool();
        return arr;
    }
    public bool[][] Readboolss()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        bool[][] arr = new bool[len][];
        for (int i = 0; i < len; i++)
        {
            int len2 = Readint();
            arr[i] = new bool[len2];
            for (int j = 0; j < len2; j++)
                arr[i][j] = Readbool();
        }
        return arr;
    }
    public int[] Readints()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        int[] arr = new int[len];
        for (int i = 0; i < len; i++)
            arr[i] = Readint();
        return arr;
    }
    public int[][] Readintss()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        int[][] arr = new int[len][];
        for (int i = 0; i < len; i++)
        {
            int len2 = Readint();
            arr[i] = new int[len2];
            for (int j = 0; j < len2; j++)
                arr[i][j] = Readint();
        }
        return arr;
    }
    public uint[] Readuints()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        uint[] arr = new uint[len];
        for (int i = 0; i < len; i++)
            arr[i] = Readuint();
        return arr;
    }
    public uint[][] Readuintss()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        uint[][] arr = new uint[len][];
        for (int i = 0; i < len; i++)
        {
            int len2 = Readint();
            arr[i] = new uint[len2];
            for (int j = 0; j < len2; j++)
                arr[i][j] = Readuint();
        }
        return arr;
    }
    public long[] Readlongs()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        long[] arr = new long[len];
        for (int i = 0; i < len; i++)
            arr[i] = Readlong();
        return arr;
    }
    public long[][] Readlongss()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        long[][] arr = new long[len][];
        for (int i = 0; i < len; i++)
        {
            int len2 = Readint();
            arr[i] = new long[len2];
            for (int j = 0; j < len2; j++)
                arr[i][j] = Readlong();
        }
        return arr;
    }
    public ulong[] Readulongs()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        ulong[] arr = new ulong[len];
        for (int i = 0; i < len; i++)
            arr[i] = Readulong();
        return arr;
    }
    public ulong[][] Readulongss()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        ulong[][] arr = new ulong[len][];
        for (int i = 0; i < len; i++)
        {
            int len2 = Readint();
            arr[i] = new ulong[len2];
            for (int j = 0; j < len2; j++)
                arr[i][j] = Readulong();
        }
        return arr;
    }
    public float[] Readfloats()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        float[] arr = new float[len];
        for (int i = 0; i < len; i++)
            arr[i] = Readfloat();
        return arr;
    }
    public float[][] Readfloatss()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        float[][] arr = new float[len][];
        for (int i = 0; i < len; i++)
        {
            int len2 = Readint();
            arr[i] = new float[len2];
            for (int j = 0; j < len2; j++)
                arr[i][j] = Readfloat();
        }
        return arr;
    }
    public string[] Readstrings()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        string[] arr = new string[len];
        for (int i = 0; i < len; i++)
            arr[i] = Readstring();
        return arr;
    }
    public string[][] Readstringss()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        string[][] arr = new string[len][];
        for (int i = 0; i < len; i++)
        {
            int len2 = Readint();
            arr[i] = new string[len2];
            for (int j = 0; j < len2; j++)
                arr[i][j] = Readstring();
        }
        return arr;
    }
    public byte[] Readbytes()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        byte[] arr = new byte[len];
        for (int i = 0; i < len; i++)
            arr[i] = Readbyte();
        return arr;
    }
    public bool ReadHeaderInfo()
    {
        this.Compress = false;
        if (this.Readint() != Verify)
        {
#if UNITY_2019_4_OR_NEWER
            Loger.Error("不是DBuffer数据");
#else
            Console.WriteLine("不是DBuffer数据");
#endif
            return false;
        }
        this.Compress = Readbool();
        return true;
    }
#if UNITY_2019_4_OR_NEWER
    public Vector2[] ReadVector2s()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        Vector2[] arr = new Vector2[len];
        for (int i = 0; i < len; i++)
            arr[i] = ReadVector2();
        return arr;
    }
    public Vector2[][] ReadVector2ss()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        Vector2[][] arr = new Vector2[len][];
        for (int i = 0; i < len; i++)
        {
            int len2 = Readint();
            arr[i] = new Vector2[len2];
            for (int j = 0; j < len2; j++)
                arr[i][j] = ReadVector2();
        }
        return arr;
    }
    public Vector2Int[] ReadVector2Ints()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        Vector2Int[] arr = new Vector2Int[len];
        for (int i = 0; i < len; i++)
            arr[i] = ReadVector2Int();
        return arr;
    }
    public Vector2Int[][] ReadVector2Intss()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        Vector2Int[][] arr = new Vector2Int[len][];
        for (int i = 0; i < len; i++)
        {
            int len2 = Readint();
            arr[i] = new Vector2Int[len2];
            for (int j = 0; j < len2; j++)
                arr[i][j] = ReadVector2Int();
        }
        return arr;
    }
    public Vector3[] ReadVector3s()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        Vector3[] arr = new Vector3[len];
        for (int i = 0; i < len; i++)
            arr[i] = ReadVector3();
        return arr;
    }
    public Vector3[][] ReadVector3ss()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        Vector3[][] arr = new Vector3[len][];
        for (int i = 0; i < len; i++)
        {
            int len2 = Readint();
            arr[i] = new Vector3[len2];
            for (int j = 0; j < len2; j++)
                arr[i][j] = ReadVector3();
        }
        return arr;
    }
    public Vector3Int[] ReadVector3Ints()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        Vector3Int[] arr = new Vector3Int[len];
        for (int i = 0; i < len; i++)
            arr[i] = ReadVector3Int();
        return arr;
    }
    public Vector3Int[][] ReadVector3Intss()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        Vector3Int[][] arr = new Vector3Int[len][];
        for (int i = 0; i < len; i++)
        {
            int len2 = Readint();
            arr[i] = new Vector3Int[len2];
            for (int j = 0; j < len2; j++)
                arr[i][j] = ReadVector3Int();
        }
        return arr;
    }
    public Color[] ReadColors()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        Color[] arr = new Color[len];
        for (int i = 0; i < len; i++)
            arr[i] = ReadColor();
        return arr;
    }
    public Color[][] ReadColorss()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        Color[][] arr = new Color[len][];
        for (int i = 0; i < len; i++)
        {
            int len2 = Readint();
            arr[i] = new Color[len2];
            for (int j = 0; j < len2; j++)
                arr[i][j] = ReadColor();
        }
        return arr;
    }
    public Color32[] ReadColor32s()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        Color32[] arr = new Color32[len];
        for (int i = 0; i < len; i++)
            arr[i] = ReadColor32();
        return arr;
    }
    public Color32[][] ReadColor32ss()
    {
        int len = Readint();
        checkArrayLengthLimit(len);
        Color32[][] arr = new Color32[len][];
        for (int i = 0; i < len; i++)
        {
            int len2 = Readint();
            arr[i] = new Color32[len2];
            for (int j = 0; j < len2; j++)
                arr[i][j] = ReadColor32();
        }
        return arr;
    }
#endif

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

#if UNITY_2019_4_OR_NEWER
    public void Write(Vector2 v)
    {
        Write(v.x);
        Write(v.y);
    }
    public void Write(Vector2Int v)
    {
        Write(v.x);
        Write(v.y);
    }
    public void Write(Vector3 v)
    {
        Write(v.x);
        Write(v.y);
        Write(v.z);
    }
    public void Write(Vector3Int v)
    {
        Write(v.x);
        Write(v.y);
        Write(v.z);
    }
    public void Write(Color v)
    {
        Write(v.r);
        Write(v.g);
        Write(v.b);
        Write(v.a);
    }
    public void Write(Color32 v)
    {
        Write(v.r);
        Write(v.g);
        Write(v.b);
        Write(v.a);
    }
#endif
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
    }

#if UNITY_2019_4_OR_NEWER
    public void Write(Vector2[] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        Write(v, 0, v.Length);
    }
    public void Write(Vector2[] v, int index, int length)
    {
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
    }
    public void Write(Vector2[][] v)
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
    public void Write(Vector2Int[] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        Write(v, 0, v.Length);
    }
    public void Write(Vector2Int[] v, int index, int length)
    {
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
    }
    public void Write(Vector2Int[][] v)
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
    public void Write(Vector3[] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        Write(v, 0, v.Length);
    }
    public void Write(Vector3[] v, int index, int length)
    {
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
    }
    public void Write(Vector3[][] v)
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
    public void Write(Vector3Int[] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        Write(v, 0, v.Length);
    }
    public void Write(Vector3Int[] v, int index, int length)
    {
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
    }
    public void Write(Vector3Int[][] v)
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
    public void Write(Color[] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        Write(v, 0, v.Length);
    }
    public void Write(Color[] v, int index, int length)
    {
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
    }
    public void Write(Color[][] v)
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
    public void Write(Color32[] v)
    {
        if (v == null)
        {
            Write(0);
            return;
        }
        Write(v, 0, v.Length);
    }
    public void Write(Color32[] v, int index, int length)
    {
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
    }
    public void Write(Color32[][] v)
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
#endif

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
    [Conditional("DebugEnable")]
    void checkArrayLengthLimit(int len)
    {
        if (len > ushort.MaxValue)
            throw new Exception($"长度超过{ushort.MaxValue}的限制 当前长度={len}");
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