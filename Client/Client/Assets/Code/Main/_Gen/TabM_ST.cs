using Unity.Collections;
using Unity.Mathematics;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

public unsafe struct TabM_ST
{
    public readonly NativeArray<_test2_ST> _test2Array;

    readonly NativeHashMap<int, _test2_ST> _test2Map;
    public void Init(DBuffer buffer)
    {
        int len0 = buffer.Readint();
        fixed (NativeArray<_test2_ST>* ptr = &_test2Array) *ptr = new NativeArray<_test2_ST>(len0, Allocator.Persistent);
        fixed (NativeHashMap<int, _test2_ST>* ptr = &_test2Map) *ptr = new NativeHashMap<int, _test2_ST>(len0, AllocatorManager.Persistent);
        for (int i = 0; i < len0; i++)
        {
            _test2_ST st = new _test2_ST(buffer);
            UnsafeUtility.WriteArrayElement(_test2Array.GetUnsafePtr(), i, st);
            _test2Map.Add(st.id, st);
        }
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

    [return: ReadOnly] public _test2_ST Get_test2(int key) => _test2Map[key];
}
public partial struct _test2_ST
{
    public _test2_ST(DBuffer buffer)
    {
        this.id = buffer.Readint();
        int len1 = buffer.Readint();
        this.value2 = new UnsafeList<int>(len1, AllocatorManager.Persistent);
        for (int i = 0; i < len1; i++) this.value2.Add(buffer.Readint());
        this.longValue = buffer.Readlong();
        int len3 = buffer.Readint();
        this.longValue2 = new UnsafeList<long>(len3, AllocatorManager.Persistent);
        for (int i = 0; i < len3; i++) this.longValue2.Add(buffer.Readlong());
        this.v2t = new int2(buffer.Readint(), buffer.Readint());
        int len5 = buffer.Readint();
        this.v2t2 = new UnsafeList<int2>(len5, AllocatorManager.Persistent);
        for (int i = 0; i < len5; i++) this.v2t2.Add(new int2(buffer.Readint(), buffer.Readint()));
        this.f1 = buffer.Readfloat();
        int len7 = buffer.Readint();
        this.f2 = new UnsafeList<float>(len7, AllocatorManager.Persistent);
        for (int i = 0; i < len7; i++) this.f2.Add(buffer.Readfloat());
        this.b1 = buffer.Readbool();
        int len9 = buffer.Readint();
        this.b2 = new UnsafeList<bool>(len9, AllocatorManager.Persistent);
        for (int i = 0; i < len9; i++) this.b2.Add(buffer.Readbool());
    }
}
