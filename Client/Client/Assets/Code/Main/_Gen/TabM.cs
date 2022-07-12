using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class TabM
{
    static DBuffer dbbuff;

    static Dictionary<int, Mapping> _map_test2Idx;
    static Tab_test2[] __test2Array;
    static Dictionary<int, Tab_test2> _map_test2;
    public static Tab_test2[] _test2Array
    {
        get
        {
            if (__test2Array == null)
            {
                int[] keys = _map_test2Idx.Keys.ToArray();
                int len = keys.Length;
                __test2Array = new Tab_test2[_map_test2Idx.Count];
                for (int i = 0; i < len; i++)
                {
                    int k = keys[i];
                    Mapping v = _map_test2Idx[k];
                    if (_map_test2.TryGetValue(k, out Tab_test2 value))
                        __test2Array[v.index] = value;
                    else
                    {
                        dbbuff.Seek(v.point);
                        Tab_test2 tmp = new Tab_test2(dbbuff);
                        _map_test2[k] = tmp;
                        __test2Array[v.index] = tmp;
                    }
                }
            }
            return __test2Array;
        }
    }


    public static void Init(DBuffer buffer)
    {
        dbbuff = buffer;

        int len0 = buffer.Readint();
        _map_test2Idx = new Dictionary<int, Mapping>(len0);
        _map_test2 = new Dictionary<int, Tab_test2>(len0);
        for (int i = 0; i < len0; i++)
        {
            int offset = buffer.Readint();
            Mapping map = new Mapping();
            map.point = buffer.Position;
            map.index = i;
            _map_test2Idx.Add(buffer.Readint(), map);
            buffer.Seek(map.point + offset);
        }

    }

    public static Tab_test2 Get_test2(int key)
    {
        if (_map_test2.TryGetValue(key, out var value))
            return value;
        if (_map_test2Idx.TryGetValue(key, out Mapping map))
        {
            dbbuff.Seek(map.point);
            Tab_test2 tmp = new Tab_test2(dbbuff);
            _map_test2[key] = tmp;
            return tmp;
        }
        Loger.Error("Tab_test2表没有key: " + key);
        return null;
    }


    struct Mapping
    {
        public int point;
        public int index;
    }
}

public class Tab_test2
{
    DBuffer dbuff;

    /// <summary>
    /// id
    /// </summary>
    public int id { get; }

    int _value2Idx;
    int[] _value2Tmp;
    /// <summary>
    /// xxxx
    /// </summary>
    public int[] value2
    {
        get
        {
            if (_value2Tmp == null)
            {
                dbuff.Seek(_value2Idx);
                _value2Tmp = dbuff.Readints();
            }
            return _value2Tmp;
        }
    }

    /// <summary>
    /// xxx
    /// </summary>
    public long longValue { get; }

    int _longValue2Idx;
    long[] _longValue2Tmp;
    /// <summary>
    /// xxx
    /// </summary>
    public long[] longValue2
    {
        get
        {
            if (_longValue2Tmp == null)
            {
                dbuff.Seek(_longValue2Idx);
                _longValue2Tmp = dbuff.Readlongs();
            }
            return _longValue2Tmp;
        }
    }

    int _desIdx;
    string _desTmp;
    /// <summary>
    /// xxxx
    /// </summary>
    public string des
    {
        get
        {
            if (_desTmp == null)
            {
                dbuff.Seek(_desIdx);
                _desTmp = dbuff.Readstring();
            }
            return _desTmp;
        }
    }

    int _des2Idx;
    string[] _des2Tmp;
    /// <summary>
    /// xxxx
    /// </summary>
    public string[] des2
    {
        get
        {
            if (_des2Tmp == null)
            {
                dbuff.Seek(_des2Idx);
                _des2Tmp = dbuff.Readstrings();
            }
            return _des2Tmp;
        }
    }

    /// <summary>
    /// xxxx
    /// </summary>
    public Vector2Int v2t { get; }

    int _v2t2Idx;
    Vector2Int[] _v2t2Tmp;
    /// <summary>
    /// xxxx
    /// </summary>
    public Vector2Int[] v2t2
    {
        get
        {
            if (_v2t2Tmp == null)
            {
                dbuff.Seek(_v2t2Idx);
                _v2t2Tmp = dbuff.ReadVector2Ints();
            }
            return _v2t2Tmp;
        }
    }

    /// <summary>
    /// xxxx
    /// </summary>
    public float f1 { get; }

    int _f2Idx;
    float[] _f2Tmp;
    /// <summary>
    /// xxxx
    /// </summary>
    public float[] f2
    {
        get
        {
            if (_f2Tmp == null)
            {
                dbuff.Seek(_f2Idx);
                _f2Tmp = dbuff.Readfloats();
            }
            return _f2Tmp;
        }
    }

    /// <summary>
    /// x'x
    /// </summary>
    public bool b1 { get; }

    int _b2Idx;
    bool[] _b2Tmp;
    /// <summary>
    /// xx
    /// </summary>
    public bool[] b2
    {
        get
        {
            if (_b2Tmp == null)
            {
                dbuff.Seek(_b2Idx);
                _b2Tmp = dbuff.Readbools();
            }
            return _b2Tmp;
        }
    }

    public Tab_test2(DBuffer buffer)
    {
        dbuff = buffer;
        this.id = buffer.Readint();
        int next1 = buffer.Readint() + buffer.Position;
        this._value2Idx = buffer.Position;
        buffer.Seek(next1);
        this.longValue = buffer.Readlong();
        int next3 = buffer.Readint() + buffer.Position;
        this._longValue2Idx = buffer.Position;
        buffer.Seek(next3);
        this._desIdx = buffer.Position;
        buffer.Seek(buffer.Readint() + buffer.Position);
        int next5 = buffer.Readint() + buffer.Position;
        this._des2Idx = buffer.Position;
        buffer.Seek(next5);
        this.v2t = buffer.ReadVector2Int();
        int next7 = buffer.Readint() + buffer.Position;
        this._v2t2Idx = buffer.Position;
        buffer.Seek(next7);
        this.f1 = buffer.Readfloat();
        int next9 = buffer.Readint() + buffer.Position;
        this._f2Idx = buffer.Position;
        buffer.Seek(next9);
        this.b1 = buffer.Readbool();
        int next11 = buffer.Readint() + buffer.Position;
        this._b2Idx = buffer.Position;
        buffer.Seek(next11);
    }
}
