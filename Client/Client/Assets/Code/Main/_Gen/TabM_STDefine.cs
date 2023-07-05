using Unity.Collections;
using Unity.Mathematics;
using Unity.Collections.LowLevel.Unsafe;

public partial struct _test2_ST
{
    /// <summary>
    /// id
    /// </summary>
    [ReadOnly]
    public int id;
    /// <summary>
    /// xxxx
    /// </summary>
    [ReadOnly]
    public UnsafeList<int> value2;
    /// <summary>
    /// xxx
    /// </summary>
    [ReadOnly]
    public long longValue;
    /// <summary>
    /// xxx
    /// </summary>
    [ReadOnly]
    public UnsafeList<long> longValue2;
    /// <summary>
    /// xxxx
    /// </summary>
    [ReadOnly]
    public int2 v2t;
    /// <summary>
    /// xxxx
    /// </summary>
    [ReadOnly]
    public UnsafeList<int2> v2t2;
    /// <summary>
    /// xxxx
    /// </summary>
    [ReadOnly]
    public float f1;
    /// <summary>
    /// xxxx
    /// </summary>
    [ReadOnly]
    public UnsafeList<float> f2;
    /// <summary>
    /// x'x
    /// </summary>
    [ReadOnly]
    public bool b1;
    /// <summary>
    /// xx
    /// </summary>
    [ReadOnly]
    public UnsafeList<bool> b2;
}
