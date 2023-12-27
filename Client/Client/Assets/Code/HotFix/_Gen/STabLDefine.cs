using UnityEngine;

public partial class STabLScene
{
    /// <summary>
    /// id
    /// </summary>
    public int id { get; }
    /// <summary>
    /// xxxx
    /// </summary>
    public string name => getname();
    /// <summary>
    /// 类型
    /// </summary>
    public int type { get; }
}

public partial class STabL_test1
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
    public string des => getdes();
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
}
