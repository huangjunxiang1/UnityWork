using UnityEngine;

public class test1
{
    /// <summary>
    /// id
    /// </summary>
    public int id { get; }
    /// <summary>
    /// xxxx
    /// </summary>
    public int[] value2 { get; }
    /// <summary>
    /// xxxx
    /// </summary>
    public string des { get; }
    /// <summary>
    /// xxxx
    /// </summary>
    public string[] des2 { get; }
    /// <summary>
    /// xxxx
    /// </summary>
    public Vector2Int v2t { get; }
    /// <summary>
    /// xxxx
    /// </summary>
    public Vector2Int[] v2t2 { get; }
    /// <summary>
    /// xxxx
    /// </summary>
    public float f1 { get; }
    /// <summary>
    /// xxxx
    /// </summary>
    public float[] f2 { get; }
    /// <summary>
    /// x'x
    /// </summary>
    public bool b1 { get; }
    /// <summary>
    /// xx
    /// </summary>
    public bool[] b2 { get; }

    public test1(WBuffer buffer)
    {
        this.id = buffer.ReadInt();
        int len1 = buffer.ReadInt();
        this.value2 = new int[len1];
        for (int i = 0; i < len1; i++)
            this.value2[i] = buffer.ReadInt();
        this.des = buffer.ReadString();
        int len3 = buffer.ReadInt();
        this.des2 = new string[len3];
        for (int i = 0; i < len3; i++)
            this.des2[i] = buffer.ReadString();
        this.v2t = new Vector2Int(buffer.ReadInt(), buffer.ReadInt());
        int len5 = buffer.ReadInt();
        this.v2t2 = new Vector2Int[len5];
        for (int i = 0; i < len5; i++)
            this.v2t2[i] = new Vector2Int(buffer.ReadInt(), buffer.ReadInt());
        this.f1 = buffer.ReadFloat();
        int len7 = buffer.ReadInt();
        this.f2 = new float[len7];
        for (int i = 0; i < len7; i++)
            this.f2[i] = buffer.ReadFloat();
        this.b1 = buffer.ReadBool();
        int len9 = buffer.ReadInt();
        this.b2 = new bool[len9];
        for (int i = 0; i < len9; i++)
            this.b2[i] = buffer.ReadBool();
    }
}
