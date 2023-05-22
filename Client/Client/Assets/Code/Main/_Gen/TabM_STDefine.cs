using Unity.Collections;
using Unity.Mathematics;
using Unity.Entities;

public struct _test2_ST
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
    public BlobArray<int> value2;
    /// <summary>
    /// xxx
    /// </summary>
    [ReadOnly]
    public long longValue;
    /// <summary>
    /// xxx
    /// </summary>
    [ReadOnly]
    public BlobArray<long> longValue2;
    /// <summary>
    /// xxxx
    /// </summary>
    [ReadOnly]
    public int2 v2t;
    /// <summary>
    /// xxxx
    /// </summary>
    [ReadOnly]
    public BlobArray<int2> v2t2;
    /// <summary>
    /// xxxx
    /// </summary>
    [ReadOnly]
    public float f1;
    /// <summary>
    /// xxxx
    /// </summary>
    [ReadOnly]
    public BlobArray<float> f2;
    /// <summary>
    /// x'x
    /// </summary>
    [ReadOnly]
    public bool b1;
    /// <summary>
    /// xx
    /// </summary>
    [ReadOnly]
    public BlobArray<bool> b2;
}
