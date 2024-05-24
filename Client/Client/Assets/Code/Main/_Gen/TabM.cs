using System.Collections.Generic;
using System.Linq;
using System;
using Unity.Mathematics;

public struct TabMapping
{
    public TabMapping(int point, int index) { this.point = point; this.index = index; }
    public int point;
    public int index;
}
public static class TabM
{
    static DBuffer dbbuff; static bool loadAll; static string[] stringCache; static int[] stringIndex;
    internal static string __getstring(int index) { if (stringCache[index] == null) { dbbuff.Seek(stringIndex[index]); stringCache[index] = dbbuff.Readstring(); } return stringCache[index]; }
    internal static string[] __getstrings(int point) { dbbuff.Seek(point); int len = dbbuff.Readint(); if (len == 0) return Array.Empty<string>(); else { var arr = new string[len]; for (int i = 0; i < len; i++) { int index = dbbuff.Readint(); int pos = dbbuff.Position; arr[i] = __getstring(index); dbbuff.Seek(pos); } return arr; } }
    internal static string[][] __getstringss(int point) { dbbuff.Seek(point); int len = dbbuff.Readint(); if (len == 0) return Array.Empty<string[]>(); else { var arr = new string[len][]; for (int i = 0; i < len; i++) { int len2 = dbbuff.Readint(); if (len2 == 0) arr[i] = Array.Empty<string>(); else { arr[i] = new string[len2]; for (int j = 0; j < len2; j++) { int index = dbbuff.Readint(); int pos = dbbuff.Position; arr[i][j] = __getstring(index); dbbuff.Seek(pos); } } } return arr; } }

    static bool _init_test2Array; static TabM_test2[] __test2Array; static Dictionary<int, TabMapping> _map_test2; static Dictionary<long, List<TabM_test2>> _test2_longValueGroupMap; static Dictionary<int, List<TabM_test2>> _test2_intvGroupMap; static Dictionary<string, List<TabM_test2>> _test2_desGroupMap;

    public static void Init(DBuffer buffer, bool isLoadAll)
    {
        dbbuff = buffer; loadAll = isLoadAll;
        int len = buffer.Readint(); buffer.Readint(); stringCache = new string[len]; stringIndex = new int[len]; for (int i = 0; i < len; i++) { stringIndex[i] = buffer.Position; buffer.Seek(buffer.Readint() + buffer.Position); }
        buffer.Readint();//data buff总长
        Public = new(buffer, isLoadAll);
        len = buffer.Readint(); _init_test2Array = false; __test2Array = new TabM_test2[len]; _map_test2 = new(len); for (int i = 0; i < len; i++) { int offset = buffer.Readint(); TabMapping map = new(buffer.Position, i); _map_test2.Add(buffer.Readint(), map); buffer.Seek(map.point + offset); }
        if (loadAll) { _ = _test2Array; }
    }
    public static TabMPublic Public { get; private set; }

    public static TabM_test2[] _test2Array { get { if (!_init_test2Array) { _init_test2Array = true; foreach (var item in _map_test2.Keys) Get_test2(item); } return __test2Array; } }
    public static bool Has_test2(int key) => _map_test2.ContainsKey(key);
    public static TabM_test2 Get_test2(int key) { if (_map_test2.TryGetValue(key, out var value)) { if (__test2Array[value.index] == null) { dbbuff.Seek(value.point); __test2Array[value.index] = new(dbbuff, loadAll); } return __test2Array[value.index]; } Loger.Error("TabM_test2表没有key: " + key); return null; }
    public static List<TabM_test2> Get_test2_longValueGroup(long group) { if (_test2_longValueGroupMap == null) { _test2_longValueGroupMap = new(); for (int i = 0; i < _test2Array.Length; i++) { var t = _test2Array[i]; if (!_test2_longValueGroupMap.TryGetValue(t.longValue, out var vs)) _test2_longValueGroupMap[t.longValue] = vs = new(); vs.Add(t); } } if (!_test2_longValueGroupMap.TryGetValue(group, out var lst)) Loger.Error("TabM_test2表没有longValue组: " + group); return lst; }
    public static List<TabM_test2> Get_test2_intvGroup(int group) { if (_test2_intvGroupMap == null) { _test2_intvGroupMap = new(); for (int i = 0; i < _test2Array.Length; i++) { var t = _test2Array[i]; if (!_test2_intvGroupMap.TryGetValue(t.intv, out var vs)) _test2_intvGroupMap[t.intv] = vs = new(); vs.Add(t); } } if (!_test2_intvGroupMap.TryGetValue(group, out var lst)) Loger.Error("TabM_test2表没有intv组: " + group); return lst; }
    public static List<TabM_test2> Get_test2_desGroup(string group) { if (_test2_desGroupMap == null) { _test2_desGroupMap = new(); for (int i = 0; i < _test2Array.Length; i++) { var t = _test2Array[i]; if (!_test2_desGroupMap.TryGetValue(t.des, out var vs)) _test2_desGroupMap[t.des] = vs = new(); vs.Add(t); } } if (!_test2_desGroupMap.TryGetValue(group, out var lst)) Loger.Error("TabM_test2表没有des组: " + group); return lst; }
}
public class TabMPublic
{
    /// <summary>
    /// 描述
    /// </summary>
    public int aa { get; }
    /// <summary>
    /// 
    /// </summary>
    public string bb => TabM.__getstring(_bbIdx);
    /// <summary>
    /// 
    /// </summary>
    public int[] cc => getcc();
    /// <summary>
    /// 
    /// </summary>
    public string[] dd => getdd();

    DBuffer dbuff;
    int _bbIdx;
    int _ccIdx; int[] _ccTmp; int[] getcc() { if (_ccTmp == null) { dbuff.Seek(_ccIdx); _ccTmp = dbuff.Readints(); } return _ccTmp; }
    int _ddIdx; string[] _ddTmp; string[] getdd() => _ddTmp ??= TabM.__getstrings(_ddIdx);

    public TabMPublic(DBuffer buffer, bool loadAll = false)
    {
        dbuff = buffer;
        this.aa = buffer.Readint();
        this._bbIdx = buffer.Readint();
        buffer.Seek(buffer.Readint() + (this._ccIdx = buffer.Position));
        buffer.Seek(buffer.Readint() + (this._ddIdx = buffer.Position));
        if (loadAll) { _ = bb; _ = cc; _ = dd; }
    }
}
public class TabM_test2
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
    /// xxx
    /// </summary>
    public long longValue { get; }
    /// <summary>
    /// x
    /// </summary>
    public int intv { get; }
    /// <summary>
    /// xxx
    /// </summary>
    public long[] longValue2 => getlongValue2();
    /// <summary>
    /// xxxx
    /// </summary>
    public string des => TabM.__getstring(_desIdx);
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
    /// <summary>
    /// hh
    /// </summary>
    public int[][] arrs => getarrs();
    /// <summary>
    /// hh
    /// </summary>
    public string[][] arr2s => getarr2s();

    DBuffer dbuff;
    int _value2Idx; int[] _value2Tmp; int[] getvalue2() { if (_value2Tmp == null) { dbuff.Seek(_value2Idx); _value2Tmp = dbuff.Readints(); } return _value2Tmp; }
    int _longValue2Idx; long[] _longValue2Tmp; long[] getlongValue2() { if (_longValue2Tmp == null) { dbuff.Seek(_longValue2Idx); _longValue2Tmp = dbuff.Readlongs(); } return _longValue2Tmp; }
    int _desIdx;
    int _des2Idx; string[] _des2Tmp; string[] getdes2() => _des2Tmp ??= TabM.__getstrings(_des2Idx);
    int _v2t2Idx; int2[] _v2t2Tmp; int2[] getv2t2() { if (_v2t2Tmp == null) { dbuff.Seek(_v2t2Idx); _v2t2Tmp = dbuff.Readint2s(); } return _v2t2Tmp; }
    int _f2Idx; float[] _f2Tmp; float[] getf2() { if (_f2Tmp == null) { dbuff.Seek(_f2Idx); _f2Tmp = dbuff.Readfloats(); } return _f2Tmp; }
    int _b2Idx; bool[] _b2Tmp; bool[] getb2() { if (_b2Tmp == null) { dbuff.Seek(_b2Idx); _b2Tmp = dbuff.Readbools(); } return _b2Tmp; }
    int _arrsIdx; int[][] _arrsTmp; int[][] getarrs() { if (_arrsTmp == null) { dbuff.Seek(_arrsIdx); _arrsTmp = dbuff.Readintss(); } return _arrsTmp; }
    int _arr2sIdx; string[][] _arr2sTmp; string[][] getarr2s() => _arr2sTmp ??= TabM.__getstringss(_arr2sIdx);

    public TabM_test2(DBuffer buffer, bool loadAll = false)
    {
        dbuff = buffer;
        this.id = buffer.Readint();
        buffer.Seek(buffer.Readint() + (this._value2Idx = buffer.Position));
        this.longValue = buffer.Readlong();
        this.intv = buffer.Readint();
        buffer.Seek(buffer.Readint() + (this._longValue2Idx = buffer.Position));
        this._desIdx = buffer.Readint();
        buffer.Seek(buffer.Readint() + (this._des2Idx = buffer.Position));
        this.v2t = buffer.Readint2();
        buffer.Seek(buffer.Readint() + (this._v2t2Idx = buffer.Position));
        this.f1 = buffer.Readfloat();
        buffer.Seek(buffer.Readint() + (this._f2Idx = buffer.Position));
        this.b1 = buffer.Readbool();
        buffer.Seek(buffer.Readint() + (this._b2Idx = buffer.Position));
        buffer.Seek(buffer.Readint() + (this._arrsIdx = buffer.Position));
        buffer.Seek(buffer.Readint() + (this._arr2sIdx = buffer.Position));
        if (loadAll) { _ = value2; _ = longValue2; _ = des; _ = des2; _ = v2t2; _ = f2; _ = b2; _ = arrs; _ = arr2s; }
    }
}
