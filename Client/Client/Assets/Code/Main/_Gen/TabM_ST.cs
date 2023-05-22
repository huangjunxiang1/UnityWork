using Unity.Collections;
using Unity.Mathematics;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

public unsafe struct TabM_ST : IComponentData
{
    public readonly NativeArray<BlobAssetReference<_test2_ST>> _test2Array;

    readonly NativeHashMap<int, BlobAssetReference<_test2_ST>> _test2Map;
    public void Init(DBuffer buffer)
    {
        int len0 = buffer.Readint();
        fixed (NativeArray<BlobAssetReference<_test2_ST>>* ptr = &_test2Array) *ptr = new NativeArray<BlobAssetReference<_test2_ST>>(len0, Allocator.Persistent);
        fixed (NativeHashMap<int, BlobAssetReference<_test2_ST>>* ptr = &_test2Map) *ptr = new NativeHashMap<int, BlobAssetReference<_test2_ST>>(len0, AllocatorManager.Persistent);
        for (int i = 0; i < len0; i++)
        {
            var blobBuilder = new BlobBuilder(Allocator.Temp);
            ref var t = ref blobBuilder.ConstructRoot<_test2_ST>();
            t.id = buffer.Readint();
            int len1 = buffer.Readint();
            var tmp1 = blobBuilder.Allocate(ref t.value2, len1);
            for (int j = 0; j < len1; j++) tmp1[j] = buffer.Readint();
            t.longValue = buffer.Readlong();
            int len3 = buffer.Readint();
            var tmp3 = blobBuilder.Allocate(ref t.longValue2, len3);
            for (int j = 0; j < len3; j++) tmp3[j] = buffer.Readlong();
            t.v2t = new int2(buffer.Readint(), buffer.Readint());
            int len5 = buffer.Readint();
            var tmp5 = blobBuilder.Allocate(ref t.v2t2, len5);
            for (int j = 0; j < len5; j++) tmp5[j] = new int2(buffer.Readint(), buffer.Readint());
            t.f1 = buffer.Readfloat();
            int len7 = buffer.Readint();
            var tmp7 = blobBuilder.Allocate(ref t.f2, len7);
            for (int j = 0; j < len7; j++) tmp7[j] = buffer.Readfloat();
            t.b1 = buffer.Readbool();
            int len9 = buffer.Readint();
            var tmp9 = blobBuilder.Allocate(ref t.b2, len9);
            for (int j = 0; j < len9; j++) tmp9[j] = buffer.Readbool();
            BlobAssetReference<_test2_ST> bar = blobBuilder.CreateBlobAssetReference<_test2_ST>(Allocator.Persistent);
            UnsafeUtility.WriteArrayElement(_test2Array.GetUnsafePtr(), i, bar);
            _test2Map.Add(t.id, bar);
            blobBuilder.Dispose();
        }
    }
    public void Dispose()
    {
        for (int i = 0; i < _test2Array.Length; i++) _test2Array[i].Dispose();
        _test2Array.Dispose();
        _test2Map.Dispose();
    }

    [return: ReadOnly] public ref _test2_ST Get_test2(int key) => ref _test2Map[key].Value;
}
