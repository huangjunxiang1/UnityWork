using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class TabL
{
    static DBuffer dbbuff;
    static bool debug;

    static Dictionary<int, TabMapping> _mapSceneIdx;
    static TabScene[] _SceneArray;
    static Dictionary<int, TabScene> _mapScene;
    public static TabScene[] SceneArray
    {
        get
        {
            if (_SceneArray == null)
            {
                bool isDebug = debug;
                int[] keys = _mapSceneIdx.Keys.ToArray();
                int len = keys.Length;
                _SceneArray = new TabScene[_mapSceneIdx.Count];
                for (int i = 0; i < len; i++)
                {
                    int k = keys[i];
                    TabMapping v = _mapSceneIdx[k];
                    if (_mapScene.TryGetValue(k, out TabScene value))
                        _SceneArray[v.index] = value;
                    else
                    {
                        dbbuff.Seek(v.point);
                        TabScene tmp = new TabScene(dbbuff, isDebug);
                        _mapScene[k] = tmp;
                        _SceneArray[v.index] = tmp;
                    }
                }
                _mapSceneIdx = null;
            }
            return _SceneArray;
        }
    }

    static Dictionary<int, TabMapping> _map_test1Idx;
    static Tab_test1[] __test1Array;
    static Dictionary<int, Tab_test1> _map_test1;
    public static Tab_test1[] _test1Array
    {
        get
        {
            if (__test1Array == null)
            {
                bool isDebug = debug;
                int[] keys = _map_test1Idx.Keys.ToArray();
                int len = keys.Length;
                __test1Array = new Tab_test1[_map_test1Idx.Count];
                for (int i = 0; i < len; i++)
                {
                    int k = keys[i];
                    TabMapping v = _map_test1Idx[k];
                    if (_map_test1.TryGetValue(k, out Tab_test1 value))
                        __test1Array[v.index] = value;
                    else
                    {
                        dbbuff.Seek(v.point);
                        Tab_test1 tmp = new Tab_test1(dbbuff, isDebug);
                        _map_test1[k] = tmp;
                        __test1Array[v.index] = tmp;
                    }
                }
                _map_test1Idx = null;
            }
            return __test1Array;
        }
    }

    public static void Init(DBuffer buffer, bool isDebug)
    {
        dbbuff = buffer;
        debug = isDebug;

        int len0 = buffer.Readint();
        _mapSceneIdx = new Dictionary<int, TabMapping>(len0);
        _mapScene = new Dictionary<int, TabScene>(len0);
        _SceneArray = null;
        for (int i = 0; i < len0; i++)
        {
            int offset = buffer.Readint();
            TabMapping map = new TabMapping();
            map.point = buffer.Position;
            map.index = i;
            _mapSceneIdx.Add(buffer.Readint(), map);
            buffer.Seek(map.point + offset);
        }
        int len1 = buffer.Readint();
        _map_test1Idx = new Dictionary<int, TabMapping>(len1);
        _map_test1 = new Dictionary<int, Tab_test1>(len1);
        __test1Array = null;
        for (int i = 0; i < len1; i++)
        {
            int offset = buffer.Readint();
            TabMapping map = new TabMapping();
            map.point = buffer.Position;
            map.index = i;
            _map_test1Idx.Add(buffer.Readint(), map);
            buffer.Seek(map.point + offset);
        }
        if (isDebug)
        {
            _ = SceneArray;
            _ = _test1Array;
        }
    }
    public static TabScene GetScene(int key)
    {
        if (_mapScene.TryGetValue(key, out var value))
            return value;
        if (_mapSceneIdx != null && _mapSceneIdx.TryGetValue(key, out TabMapping map))
        {
            dbbuff.Seek(map.point);
            TabScene tmp = new TabScene(dbbuff);
            _mapScene[key] = tmp;
            return tmp;
        }
        Loger.Error("TabScene表没有key: " + key);
        return null;
    }
    public static Tab_test1 Get_test1(int key)
    {
        if (_map_test1.TryGetValue(key, out var value))
            return value;
        if (_map_test1Idx != null && _map_test1Idx.TryGetValue(key, out TabMapping map))
        {
            dbbuff.Seek(map.point);
            Tab_test1 tmp = new Tab_test1(dbbuff);
            _map_test1[key] = tmp;
            return tmp;
        }
        Loger.Error("Tab_test1表没有key: " + key);
        return null;
    }
}
public partial class TabScene
{
    DBuffer dbuff;

    int _nameIdx;
    string _nameTmp;
    string getname()
    {
        if (_nameTmp == null)
        {
            dbuff.Seek(_nameIdx);
            _nameTmp = dbuff.Readstring();
        }
        return _nameTmp;
    }

    public TabScene(DBuffer buffer, bool isDebug = false)
    {
        dbuff = buffer;
        this.id = buffer.Readint();
        this._nameIdx = buffer.Position;
        buffer.Seek(buffer.Readint() + buffer.Position);
        this.type = buffer.Readint();
        if (isDebug)
        {
            _ = this.name;
        }
    }
}
public partial class Tab_test1
{
    DBuffer dbuff;

    int _value2Idx;
    int[] _value2Tmp;
    int[] getvalue2()
    {
        if (_value2Tmp == null)
        {
            dbuff.Seek(_value2Idx);
            _value2Tmp = dbuff.Readints();
        }
        return _value2Tmp;
    }

    int _desIdx;
    string _desTmp;
    string getdes()
    {
        if (_desTmp == null)
        {
            dbuff.Seek(_desIdx);
            _desTmp = dbuff.Readstring();
        }
        return _desTmp;
    }

    int _des2Idx;
    string[] _des2Tmp;
    string[] getdes2()
    {
        if (_des2Tmp == null)
        {
            dbuff.Seek(_des2Idx);
            _des2Tmp = dbuff.Readstrings();
        }
        return _des2Tmp;
    }

    int _v2t2Idx;
    Vector2Int[] _v2t2Tmp;
    Vector2Int[] getv2t2()
    {
        if (_v2t2Tmp == null)
        {
            dbuff.Seek(_v2t2Idx);
            _v2t2Tmp = dbuff.ReadVector2Ints();
        }
        return _v2t2Tmp;
    }

    int _f2Idx;
    float[] _f2Tmp;
    float[] getf2()
    {
        if (_f2Tmp == null)
        {
            dbuff.Seek(_f2Idx);
            _f2Tmp = dbuff.Readfloats();
        }
        return _f2Tmp;
    }

    int _b2Idx;
    bool[] _b2Tmp;
    bool[] getb2()
    {
        if (_b2Tmp == null)
        {
            dbuff.Seek(_b2Idx);
            _b2Tmp = dbuff.Readbools();
        }
        return _b2Tmp;
    }

    public Tab_test1(DBuffer buffer, bool isDebug = false)
    {
        dbuff = buffer;
        this.id = buffer.Readint();
        buffer.Seek(buffer.Readint() + (this._value2Idx = buffer.Position));
        this._desIdx = buffer.Position;
        buffer.Seek(buffer.Readint() + buffer.Position);
        buffer.Seek(buffer.Readint() + (this._des2Idx = buffer.Position));
        this.v2t = buffer.ReadVector2Int();
        buffer.Seek(buffer.Readint() + (this._v2t2Idx = buffer.Position));
        this.f1 = buffer.Readfloat();
        buffer.Seek(buffer.Readint() + (this._f2Idx = buffer.Position));
        this.b1 = buffer.Readbool();
        buffer.Seek(buffer.Readint() + (this._b2Idx = buffer.Position));
        if (isDebug)
        {
            _ = this.value2;
            _ = this.des;
            _ = this.des2;
            _ = this.v2t2;
            _ = this.f2;
            _ = this.b2;
        }
    }
}
