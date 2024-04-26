using System.Collections.Generic;
using System.Linq;
using System;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;

public readonly unsafe struct TabM_ST
{
    public readonly static SharedStatic<TabM_ST> Tab = SharedStatic<TabM_ST>.GetOrCreate<SharedStatic<TabM_ST>>();

    readonly UnsafeHashMap<int, int> _test2Map;
    public static void Init(DBuffer buffer)
    {
        Tab.Data.Dispose();
        fixed (Public_ST* ptr = &Tab.Data.Public) { *ptr = new Public_ST(buffer); }
        int len;
        len = buffer.Readint(); fixed (UnsafeList<_test2_ST>* ptr = &Tab.Data._test2Array) { *ptr = new UnsafeList<_test2_ST>(len, Allocator.Persistent); fixed (UnsafeHashMap<int, int>* ptr2 = &Tab.Data._test2Map) { *ptr2 = new UnsafeHashMap<int, int>(len, AllocatorManager.Persistent); for (int i = 0; i < len; i++) { _test2_ST st = new(buffer); ptr->Add(st); ptr2->Add(st.id, i); } } }
    }
    public void Dispose()
    {
        Public.cc.Dispose();
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
    public readonly Public_ST Public;

    public readonly UnsafeList<_test2_ST> _test2Array;
    public readonly bool Has_test2(int key) => _test2Map.ContainsKey(key);
    public readonly ref _test2_ST Get_test2(int key) => ref _test2Array.ElementAt(_test2Map[key]);
}
public readonly struct Public_ST
{
    /// <summary>
    /// 描述
    /// </summary>
    public readonly int aa;
    /// <summary>
    /// 
    /// </summary>
    public readonly UnsafeList<int> cc;
    public Public_ST(DBuffer buffer)
    {
        int len;
        this.aa = buffer.Readint();
        len = buffer.Readint(); this.cc = new UnsafeList<int>(len, AllocatorManager.Persistent); for (int i = 0; i < len; i++) this.cc.Add(buffer.Readint());
    }
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
        this.v2t = buffer.Readint2();
        len = buffer.Readint(); this.v2t2 = new UnsafeList<int2>(len, AllocatorManager.Persistent); for (int i = 0; i < len; i++) this.v2t2.Add(buffer.Readint2());
        this.f1 = buffer.Readfloat();
        len = buffer.Readint(); this.f2 = new UnsafeList<float>(len, AllocatorManager.Persistent); for (int i = 0; i < len; i++) this.f2.Add(buffer.Readfloat());
        this.b1 = buffer.Readbool();
        len = buffer.Readint(); this.b2 = new UnsafeList<bool>(len, AllocatorManager.Persistent); for (int i = 0; i < len; i++) this.b2.Add(buffer.Readbool());
    }
}
