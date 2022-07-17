using System;
using System.Runtime.InteropServices;
#if UNITY_2019_4_OR_NEWER
using UnityEngine;
#endif

/// <summary>
/// int和long采用Varint编码的zigzag编码格式
/// </summary>
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
    public abstract long Readlong();
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
        bool[] arr = new bool[len];
        if (len > 0)
        {
            int c1 = (len - 1) / 8 + 1;
            for (int i = 0; i < c1; i++)
            {
                int c2 = i < c1 - 1 ? 8 : ((len - 1) % 8 + 1);
                byte bv = Readbyte();
                for (int j = 0; j < c2; j++)
                    arr[i * 8 + j] = (bv & (byte)(1 << j)) == 1;
            }
        }
        return arr;
    }
    public int[] Readints()
    {
        int len = Readint();
        int[] arr = new int[len];
        for (int i = 0; i < len; i++)
            arr[i] = Readint();
        return arr;
    }
    public long[] Readlongs()
    {
        int len = Readint();
        long[] arr = new long[len];
        for (int i = 0; i < len; i++)
            arr[i] = Readlong();
        return arr;
    }
    public float[] Readfloats()
    {
        int len = Readint();
        float[] arr = new float[len];
        for (int i = 0; i < len; i++)
            arr[i] = Readfloat();
        return arr;
    }
    public string[] Readstrings()
    {
        int len = Readint();
        string[] arr = new string[len];
        for (int i = 0; i < len; i++)
            arr[i] = Readstring();
        return arr;
    }
    public byte[] Readbytes()
    {
        int len = Readint();
        byte[] arr = new byte[len];
        for (int i = 0; i < len; i++)
            arr[i] = Readbyte();
        return arr;
    }
#if UNITY_2019_4_OR_NEWER
    public Vector2[] ReadVector2s()
    {
        int len = Readint();
        Vector2[] arr = new Vector2[len];
        for (int i = 0; i < len; i++)
            arr[i] = ReadVector2();
        return arr;
    }
    public Vector2Int[] ReadVector2Ints()
    {
        int len = Readint();
        Vector2Int[] arr = new Vector2Int[len];
        for (int i = 0; i < len; i++)
            arr[i] = ReadVector2Int();
        return arr;
    }
    public Vector3[] ReadVector3s()
    {
        int len = Readint();
        Vector3[] arr = new Vector3[len];
        for (int i = 0; i < len; i++)
            arr[i] = ReadVector3();
        return arr;
    }
    public Vector3Int[] ReadVector3Ints()
    {
        int len = Readint();
        Vector3Int[] arr = new Vector3Int[len];
        for (int i = 0; i < len; i++)
            arr[i] = ReadVector3Int();
        return arr;
    }
    public Color[] ReadColors()
    {
        int len = Readint();
        Color[] arr = new Color[len];
        for (int i = 0; i < len; i++)
            arr[i] = ReadColor();
        return arr;
    }
    public Color32[] ReadColor32s()
    {
        int len = Readint();
        Color32[] arr = new Color32[len];
        for (int i = 0; i < len; i++)
            arr[i] = ReadColor32();
        return arr;
    }
#endif

    public abstract void Write(byte v);
    public void Write(bool v) { Write(v ? (byte)1 : (byte)0); }
    public abstract void Write(int v);
    public abstract void Write(long v);
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
        if (length == 0)
        {
            Write(0);
            return;
        }
        Write(length);
        int c1 = (length - 1) / 8 + 1;
        for (int i = 0; i < c1; i++)
        {
            byte bv = 0;
            int c2 = i < c1 - 1 ? 8 : ((length - 1) % 8 + 1);
            for (int j = 0; j < c2; j++)
            {
                if (v[index + i * 8 + j])
                    bv |= (byte)(1 << j);
            }
            Write(bv);
        }
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