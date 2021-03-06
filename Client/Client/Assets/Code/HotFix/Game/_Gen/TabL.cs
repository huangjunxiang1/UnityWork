using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class TabL
{
    static DBuffer dbbuff;

    static Dictionary<int, Mapping> _mapSceneIdx;
    static TabScene[] _SceneArray;
    static Dictionary<int, TabScene> _mapScene;
    public static TabScene[] SceneArray
    {
        get
        {
            if (_SceneArray == null)
            {
                int[] keys = _mapSceneIdx.Keys.ToArray();
                int len = keys.Length;
                _SceneArray = new TabScene[_mapSceneIdx.Count];
                for (int i = 0; i < len; i++)
                {
                    int k = keys[i];
                    Mapping v = _mapSceneIdx[k];
                    if (_mapScene.TryGetValue(k, out TabScene value))
                        _SceneArray[v.index] = value;
                    else
                    {
                        dbbuff.Seek(v.point);
                        TabScene tmp = new TabScene(dbbuff);
                        _mapScene[k] = tmp;
                        _SceneArray[v.index] = tmp;
                    }
                }
            }
            return _SceneArray;
        }
    }

    static Dictionary<int, Mapping> _map_test1Idx;
    static Tab_test1[] __test1Array;
    static Dictionary<int, Tab_test1> _map_test1;
    public static Tab_test1[] _test1Array
    {
        get
        {
            if (__test1Array == null)
            {
                int[] keys = _map_test1Idx.Keys.ToArray();
                int len = keys.Length;
                __test1Array = new Tab_test1[_map_test1Idx.Count];
                for (int i = 0; i < len; i++)
                {
                    int k = keys[i];
                    Mapping v = _map_test1Idx[k];
                    if (_map_test1.TryGetValue(k, out Tab_test1 value))
                        __test1Array[v.index] = value;
                    else
                    {
                        dbbuff.Seek(v.point);
                        Tab_test1 tmp = new Tab_test1(dbbuff);
                        _map_test1[k] = tmp;
                        __test1Array[v.index] = tmp;
                    }
                }
            }
            return __test1Array;
        }
    }


    public static void Init(DBuffer buffer)
    {
        dbbuff = buffer;

        int len0 = buffer.Readint();
        _mapSceneIdx = new Dictionary<int, Mapping>(len0);
        _mapScene = new Dictionary<int, TabScene>(len0);
        for (int i = 0; i < len0; i++)
        {
            int offset = buffer.Readint();
            Mapping map = new Mapping();
            map.point = buffer.Position;
            map.index = i;
            _mapSceneIdx.Add(buffer.Readint(), map);
            buffer.Seek(map.point + offset);
        }

        int len1 = buffer.Readint();
        _map_test1Idx = new Dictionary<int, Mapping>(len1);
        _map_test1 = new Dictionary<int, Tab_test1>(len1);
        for (int i = 0; i < len1; i++)
        {
            int offset = buffer.Readint();
            Mapping map = new Mapping();
            map.point = buffer.Position;
            map.index = i;
            _map_test1Idx.Add(buffer.Readint(), map);
            buffer.Seek(map.point + offset);
        }

    }

    public static TabScene GetScene(int key)
    {
        if (_mapScene.TryGetValue(key, out var value))
            return value;
        if (_mapSceneIdx.TryGetValue(key, out Mapping map))
        {
            dbbuff.Seek(map.point);
            TabScene tmp = new TabScene(dbbuff);
            _mapScene[key] = tmp;
            return tmp;
        }
        Loger.Error("TabScene?????????key: " + key);
        return null;
    }

    public static Tab_test1 Get_test1(int key)
    {
        if (_map_test1.TryGetValue(key, out var value))
            return value;
        if (_map_test1Idx.TryGetValue(key, out Mapping map))
        {
            dbbuff.Seek(map.point);
            Tab_test1 tmp = new Tab_test1(dbbuff);
            _map_test1[key] = tmp;
            return tmp;
        }
        Loger.Error("Tab_test1?????????key: " + key);
        return null;
    }


    class Mapping
    {
        public int point;
        public int index;
    }
}

public class TabScene
{
    DBuffer dbuff;

    /// <summary>
    /// id
    /// </summary>
    public int id { get; }

    int _nameIdx;
    string _nameTmp;
    /// <summary>
    /// xxxx
    /// </summary>
    public string name
    {
        get
        {
            if (_nameTmp == null)
            {
                dbuff.Seek(_nameIdx);
                _nameTmp = dbuff.Readstring();
            }
            return _nameTmp;
        }
    }

    public TabScene(DBuffer buffer)
    {
        dbuff = buffer;
        this.id = buffer.Readint();
        this._nameIdx = buffer.Position;
        buffer.Seek(buffer.Readint() + buffer.Position);
    }
}

public class Tab_test1
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

    public Tab_test1(DBuffer buffer)
    {
        dbuff = buffer;
        this.id = buffer.Readint();
        int next1 = buffer.Readint() + buffer.Position;
        this._value2Idx = buffer.Position;
        buffer.Seek(next1);
        this._desIdx = buffer.Position;
        buffer.Seek(buffer.Readint() + buffer.Position);
        int next3 = buffer.Readint() + buffer.Position;
        this._des2Idx = buffer.Position;
        buffer.Seek(next3);
        this.v2t = buffer.ReadVector2Int();
        int next5 = buffer.Readint() + buffer.Position;
        this._v2t2Idx = buffer.Position;
        buffer.Seek(next5);
        this.f1 = buffer.Readfloat();
        int next7 = buffer.Readint() + buffer.Position;
        this._f2Idx = buffer.Position;
        buffer.Seek(next7);
        this.b1 = buffer.Readbool();
        int next9 = buffer.Readint() + buffer.Position;
        this._b2Idx = buffer.Position;
        buffer.Seek(next9);
    }
}
