using System;
using System.Runtime.InteropServices;
#if UNITY_2019_4_OR_NEWER
using UnityEngine;
#endif

public abstract class DBuffer : IDisposable
{
    protected const byte byteFlag = 128;
    public readonly static byte[] EmptyBytes = new byte[0];

    public virtual int Position { get; }

    /// <summary>
    /// 数据是否压缩
    /// </summary>
    public bool Compress { get; set; } = true;

    public abstract byte Readbyte();
    public bool Readbool() { return Readbyte() == 1; }
    public abstract int Readint();
    public uint Readuint() { return (uint)Readint(); }
    public abstract long Readlong();
    public ulong Readulong() { return (ulong)Readlong(); }
    public abstract float Readfloat();
    public abstract string Readstring();
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
#endif

    public bool[] Readbools()
    {
        int len = Readint();
        bool[] result = new bool[len];
        for (int i = 0; i < len; i++)
            result[i] = Readbool();
        return result;
    }
    public int[] Readints()
    {
        int len = Readint();
        int[] result = new int[len];
        for (int i = 0; i < len; i++)
            result[i] = Readint();
        return result;
    }
    public uint[] Readuints()
    {
        int len = Readint();
        uint[] result = new uint[len];
        for (int i = 0; i < len; i++)
            result[i] = Readuint();
        return result;
    }
    public long[] Readlongs()
    {
        int len = Readint();
        long[] result = new long[len];
        for (int i = 0; i < len; i++)
            result[i] = Readlong();
        return result;
    }
    public ulong[] Readulongs()
    {
        int len = Readint();
        ulong[] result = new ulong[len];
        for (int i = 0; i < len; i++)
            result[i] = Readulong();
        return result;
    }
    public float[] Readfloats()
    {
        int len = Readint();
        float[] result = new float[len];
        for (int i = 0; i < len; i++)
            result[i] = Readfloat();
        return result;
    }
    public string[] Readstrings()
    {
        int len = Readint();
        string[] result = new string[len];
        for (int i = 0; i < len; i++)
            result[i] = Readstring();
        return result;
    }
    public byte[] Readbytes()
    {
        int len = Readint();
        byte[] result = new byte[len];
        for (int i = 0; i < len; i++)
            result[i] = Readbyte();
        return result;
    }
#if UNITY_2019_4_OR_NEWER
    public Vector2[] ReadVector2s()
    {
        int len = Readint();
        Vector2[] result = new Vector2[len];
        for (int i = 0; i < len; i++)
            result[i] = ReadVector2();
        return result;
    }
    public Vector2Int[] ReadVector2Ints()
    {
        int len = Readint();
        Vector2Int[] result = new Vector2Int[len];
        for (int i = 0; i < len; i++)
            result[i] = ReadVector2Int();
        return result;
    }
    public Vector3[] ReadVector3s()
    {
        int len = Readint();
        Vector3[] result = new Vector3[len];
        for (int i = 0; i < len; i++)
            result[i] = ReadVector3();
        return result;
    }
    public Vector3Int[] ReadVector3Ints()
    {
        int len = Readint();
        Vector3Int[] result = new Vector3Int[len];
        for (int i = 0; i < len; i++)
            result[i] = ReadVector3Int();
        return result;
    }
#endif

    public abstract void Write(byte v);
    public void Write(bool v) { Write(v ? (byte)1 : (byte)0); }
    public abstract void Write(int v);
    public void Write(uint v) { Write((int)v); }
    public abstract void Write(long v);
    public void Write(ulong v) { Write((long)v); }
    public abstract void Write(float v);
    public abstract void Write(string v);

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
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
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
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
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
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
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
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
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
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
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
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
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
        Write(length);
        for (int i = 0; i < length; i++)
            Write(v[index + i]);
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
#endif

    public abstract byte[] ToBytes();
    public abstract byte[] ToBytes(int position, int length);

    public abstract void Seek(int index);

    public virtual void Dispose()
    {

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