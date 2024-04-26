using System.Collections.Generic;
using System.Linq;
using System;
using Unity.Mathematics;

public static class TabL
{
    static DBuffer dbbuff; static bool loadAll; static string[] stringCache; static int[] stringIndex;
    internal static string __getstring(int index) { if (stringCache[index] == null) { dbbuff.Seek(stringIndex[index]); stringCache[index] = dbbuff.Readstring(); } return stringCache[index]; }
    internal static string[] __getstrings(int point) { dbbuff.Seek(point); int len = dbbuff.Readint(); if (len == 0) return Array.Empty<string>(); else { var arr = new string[len]; for (int i = 0; i < len; i++) { int index = dbbuff.Readint(); int pos = dbbuff.Position; arr[i] = __getstring(index); dbbuff.Seek(pos); } return arr; } }
    internal static string[][] __getstringss(int point) { dbbuff.Seek(point); int len = dbbuff.Readint(); if (len == 0) return Array.Empty<string[]>(); else { var arr = new string[len][]; for (int i = 0; i < len; i++) { int len2 = dbbuff.Readint(); if (len2 == 0) arr[i] = Array.Empty<string>(); else { arr[i] = new string[len2]; for (int j = 0; j < len2; j++) { int index = dbbuff.Readint(); int pos = dbbuff.Position; arr[i][j] = __getstring(index); dbbuff.Seek(pos); } } } return arr; } }

    static bool _initSceneArray; static TabLScene[] _SceneArray; static Dictionary<int, TabMapping> _mapScene;
    static bool _init_test1Array; static TabL_test1[] __test1Array; static Dictionary<int, TabMapping> _map_test1;

    public static void Init(DBuffer buffer, bool isLoadAll)
    {
        dbbuff = buffer; loadAll = isLoadAll;
        int len = buffer.Readint(); buffer.Readint(); stringCache = new string[len]; stringIndex = new int[len]; for (int i = 0; i < len; i++) { stringIndex[i] = buffer.Position; buffer.Seek(buffer.Readint() + buffer.Position); }
        buffer.Readint();//data buff总长
        Public = new(buffer, isLoadAll);
        len = buffer.Readint(); _initSceneArray = false; _SceneArray = new TabLScene[len]; _mapScene = new(len); for (int i = 0; i < len; i++) { int offset = buffer.Readint(); TabMapping map = new(buffer.Position, i); _mapScene.Add(buffer.Readint(), map); buffer.Seek(map.point + offset); }
        len = buffer.Readint(); _init_test1Array = false; __test1Array = new TabL_test1[len]; _map_test1 = new(len); for (int i = 0; i < len; i++) { int offset = buffer.Readint(); TabMapping map = new(buffer.Position, i); _map_test1.Add(buffer.Readint(), map); buffer.Seek(map.point + offset); }
        if (loadAll) { _ = SceneArray; _ = _test1Array; }
    }
    public static TabLPublic Public { get; private set; }

    public static TabLScene[] SceneArray { get { if (!_initSceneArray) { _initSceneArray = true; foreach (var item in _mapScene.Keys) GetScene(item); } return _SceneArray; } }
    public static bool HasScene(int key) => _mapScene.ContainsKey(key);
    public static TabLScene GetScene(int key) { if (_mapScene.TryGetValue(key, out var value)) { if (_SceneArray[value.index] == null) { dbbuff.Seek(value.point); _SceneArray[value.index] = new(dbbuff, loadAll); } return _SceneArray[value.index]; } Loger.Error("TabLScene表没有key: " + key); return null; }

    public static TabL_test1[] _test1Array { get { if (!_init_test1Array) { _init_test1Array = true; foreach (var item in _map_test1.Keys) Get_test1(item); } return __test1Array; } }
    public static bool Has_test1(int key) => _map_test1.ContainsKey(key);
    public static TabL_test1 Get_test1(int key) { if (_map_test1.TryGetValue(key, out var value)) { if (__test1Array[value.index] == null) { dbbuff.Seek(value.point); __test1Array[value.index] = new(dbbuff, loadAll); } return __test1Array[value.index]; } Loger.Error("TabL_test1表没有key: " + key); return null; }
}
public class TabLScene
{
    /// <summary>
    /// id
    /// </summary>
    public int id { get; }
    /// <summary>
    /// xxxx
    /// </summary>
    public string name => TabL.__getstring(_nameIdx);
    /// <summary>
    /// 类型
    /// </summary>
    public int type { get; }

    DBuffer dbuff;
    int _nameIdx;

    public TabLScene(DBuffer buffer, bool loadAll = false)
    {
        dbuff = buffer;
        this.id = buffer.Readint();
        this._nameIdx = buffer.Readint();
        this.type = buffer.Readint();
        if (loadAll) { _ = name; }
    }
}
public class TabLPublic
{

    DBuffer dbuff;

    public TabLPublic(DBuffer buffer, bool loadAll = false)
    {
        dbuff = buffer;
        if (loadAll) { }
    }
}
public class TabL_test1
{
    /// <summary>
    /// id
    /// </summary>
    public int id { get; }
    /// <summary>
    /// xxxx
    /// </summary>
    public int[] value2 => getvalue2();
    /// <summary>
    /// xxxx
    /// </summary>
    public string des => TabL.__getstring(_desIdx);
    /// <summary>
    /// xxxx
    /// </summary>
    public string[] des2 => getdes2();
    /// <summary>
    /// xxxx
    /// </summary>
    public int2 v2t { get; }
    /// <summary>
    /// xxxx
    /// </summary>
    public int2[] v2t2 => getv2t2();
    /// <summary>
    /// xxxx
    /// </summary>
    public float f1 { get; }
    /// <summary>
    /// xxxx
    /// </summary>
    public float[] f2 => getf2();
    /// <summary>
    /// x'x
    /// </summary>
    public bool b1 { get; }
    /// <summary>
    /// xx
    /// </summary>
    public bool[] b2 => getb2();

    DBuffer dbuff;
    int _value2Idx; int[] _value2Tmp; int[] getvalue2() { if (_value2Tmp == null) { dbuff.Seek(_value2Idx); _value2Tmp = dbuff.Readints(); } return _value2Tmp; }
    int _desIdx;
    int _des2Idx; string[] _des2Tmp; string[] getdes2() => _des2Tmp ??= TabL.__getstrings(_des2Idx);
    int _v2t2Idx; int2[] _v2t2Tmp; int2[] getv2t2() { if (_v2t2Tmp == null) { dbuff.Seek(_v2t2Idx); _v2t2Tmp = dbuff.Readint2s(); } return _v2t2Tmp; }
    int _f2Idx; float[] _f2Tmp; float[] getf2() { if (_f2Tmp == null) { dbuff.Seek(_f2Idx); _f2Tmp = dbuff.Readfloats(); } return _f2Tmp; }
    int _b2Idx; bool[] _b2Tmp; bool[] getb2() { if (_b2Tmp == null) { dbuff.Seek(_b2Idx); _b2Tmp = dbuff.Readbools(); } return _b2Tmp; }

    public TabL_test1(DBuffer buffer, bool loadAll = false)
    {
        dbuff = buffer;
        this.id = buffer.Readint();
        buffer.Seek(buffer.Readint() + (this._value2Idx = buffer.Position));
        this._desIdx = buffer.Readint();
        buffer.Seek(buffer.Readint() + (this._des2Idx = buffer.Position));
        this.v2t = buffer.Readint2();
        buffer.Seek(buffer.Readint() + (this._v2t2Idx = buffer.Position));
        this.f1 = buffer.Readfloat();
        buffer.Seek(buffer.Readint() + (this._f2Idx = buffer.Position));
        this.b1 = buffer.Readbool();
        buffer.Seek(buffer.Readint() + (this._b2Idx = buffer.Position));
        if (loadAll) { _ = value2; _ = des; _ = des2; _ = v2t2; _ = f2; _ = b2; }
    }
}
