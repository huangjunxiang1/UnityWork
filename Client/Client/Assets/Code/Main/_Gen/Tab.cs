using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;

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

    static bool _init_test2Array; static TabM_test2[] __test2Array; static Dictionary<int, TabMapping> _map_test2;

    public static void Init(DBuffer buffer, bool isLoadAll)
    {
        dbbuff = buffer; loadAll = isLoadAll;
        int len = buffer.Readint(); buffer.Readint(); stringCache = new string[len]; stringIndex = new int[len]; for (int i = 0; i < len; i++) { stringIndex[i] = buffer.Position; buffer.Seek(buffer.Readint() + buffer.Position); }
        buffer.Readint();//data buff总长
        len = buffer.Readint(); _init_test2Array = false; __test2Array = new TabM_test2[len]; _map_test2 = new(len); for (int i = 0; i < len; i++) { int offset = buffer.Readint(); TabMapping map = new(buffer.Position, i); _map_test2.Add(buffer.Readint(), map); buffer.Seek(map.point + offset); }
        if (loadAll) { _ = _test2Array; }
    }

    public static TabM_test2[] _test2Array { get { if (!_init_test2Array) { _init_test2Array = true; foreach (var item in _map_test2.Keys) Get_test2(item); } return __test2Array; } }
    public static bool Has_test2(int key) => _map_test2.ContainsKey(key);
    public static TabM_test2 Get_test2(int key) { if (_map_test2.TryGetValue(key, out var value)) { if (__test2Array[value.index] == null) { dbbuff.Seek(value.point); __test2Array[value.index] = new(dbbuff, loadAll); } return __test2Array[value.index]; } Loger.Error("TabM_test2表没有key: " + key); return null; }
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
    public Vector2Int v2t { get; }
    /// <summary>
    /// xxxx
    /// </summary>
    public Vector2Int[] v2t2 => getv2t2();
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
    int _v2t2Idx; Vector2Int[] _v2t2Tmp; Vector2Int[] getv2t2() { if (_v2t2Tmp == null) { dbuff.Seek(_v2t2Idx); _v2t2Tmp = dbuff.ReadVector2Ints(); } return _v2t2Tmp; }
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
        buffer.Seek(buffer.Readint() + (this._longValue2Idx = buffer.Position));
        this._desIdx = buffer.Readint();
        buffer.Seek(buffer.Readint() + (this._des2Idx = buffer.Position));
        this.v2t = buffer.ReadVector2Int();
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

public readonly unsafe struct TabM_ST
{
    public readonly static SharedStatic<TabM_ST> Tab = SharedStatic<TabM_ST>.GetOrCreate<SharedStatic<TabM_ST>>();

    readonly UnsafeHashMap<int, int> _test2Map;
    public static void Init(DBuffer buffer)
    {
        Tab.Data.Dispose();
        int len;
        len = buffer.Readint(); fixed (UnsafeList<_test2_ST>* ptr = &Tab.Data._test2Array) { *ptr = new UnsafeList<_test2_ST>(len, Allocator.Persistent); fixed (UnsafeHashMap<int, int>* ptr2 = &Tab.Data._test2Map) { *ptr2 = new UnsafeHashMap<int, int>(len, AllocatorManager.Persistent); for (int i = 0; i < len; i++) { _test2_ST st = new(buffer); ptr->Add(st); ptr2->Add(st.id, i); } } }
    }
    public void Dispose()
    {
        for (int i = 0; i < _test2Array.Length; i++)
        {
            _test2Array[i].value2.Dispose();
            _test2Array[i].longValue2.Dispose();
            _test2Array[i].v2t2.Dispose();
            _test2Array[i].f2.Dispose();
            _test2Array[i].b2.Dispose();
        }
        _test2Array.Dispose();
        _test2Map.Dispose();
    }

    public readonly UnsafeList<_test2_ST> _test2Array;
    public readonly bool Has_test2(int key) => _test2Map.ContainsKey(key);
    public readonly ref _test2_ST Get_test2(int key) => ref _test2Array.ElementAt(_test2Map[key]);
}
public readonly struct _test2_ST
{
    /// <summary>
    /// id
    /// </summary>
    public readonly int id;
    /// <summary>
    /// xxxx
    /// </summary>
    public readonly UnsafeList<int> value2;
    /// <summary>
    /// xxx
    /// </summary>
    public readonly long longValue;
    /// <summary>
    /// xxx
    /// </summary>
    public readonly UnsafeList<long> longValue2;
    /// <summary>
    /// xxxx
    /// </summary>
    public readonly int2 v2t;
    /// <summary>
    /// xxxx
    /// </summary>
    public readonly UnsafeList<int2> v2t2;
    /// <summary>
    /// xxxx
    /// </summary>
    public readonly float f1;
    /// <summary>
    /// xxxx
    /// </summary>
    public readonly UnsafeList<float> f2;
    /// <summary>
    /// x'x
    /// </summary>
    public readonly bool b1;
    /// <summary>
    /// xx
    /// </summary>
    public readonly UnsafeList<bool> b2;
    public _test2_ST(DBuffer buffer)
    {
        int len;
        this.id = buffer.Readint();
        len = buffer.Readint(); this.value2 = new UnsafeList<int>(len, AllocatorManager.Persistent); for (int i = 0; i < len; i++) this.value2.Add(buffer.Readint());
        this.longValue = buffer.Readlong();
        len = buffer.Readint(); this.longValue2 = new UnsafeList<long>(len, AllocatorManager.Persistent); for (int i = 0; i < len; i++) this.longValue2.Add(buffer.Readlong());
        this.v2t = new int2(buffer.Readint(), buffer.Readint());
        len = buffer.Readint(); this.v2t2 = new UnsafeList<int2>(len, AllocatorManager.Persistent); for (int i = 0; i < len; i++) this.v2t2.Add(new int2(buffer.Readint(), buffer.Readint()));
        this.f1 = buffer.Readfloat();
        len = buffer.Readint(); this.f2 = new UnsafeList<float>(len, AllocatorManager.Persistent); for (int i = 0; i < len; i++) this.f2.Add(buffer.Readfloat());
        this.b1 = buffer.Readbool();
        len = buffer.Readint(); this.b2 = new UnsafeList<bool>(len, AllocatorManager.Persistent); for (int i = 0; i < len; i++) this.b2.Add(buffer.Readbool());
    }
}
