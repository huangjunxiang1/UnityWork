using System.Collections.Generic;
using UnityEngine;

public static class TabL
{
    public static Scene[] SceneArray { get; private set; }
    public static _test1[] _test1Array { get; private set; }

    static Dictionary<int, Scene> _mapScene = new Dictionary<int, Scene>();
    static Dictionary<int, _test1> _map_test1 = new Dictionary<int, _test1>();

    public static void Init(byte[] bytes)
    {
        WBuffer buffer = new WBuffer(bytes);

        int len0 = buffer.ReadInt();
        SceneArray = new Scene[len0];
        for (int i = 0; i < len0; i++)
        {
            var t = new Scene(buffer);
            SceneArray[i] = t;
            _mapScene.Add(t.id, t);
        }
        int len1 = buffer.ReadInt();
        _test1Array = new _test1[len1];
        for (int i = 0; i < len1; i++)
        {
            var t = new _test1(buffer);
            _test1Array[i] = t;
            _map_test1.Add(t.id, t);
        }
    }

    public static Scene GetScene(int key)
    {
        if (!_mapScene.TryGetValue(key, out var ret))
            Loger.Error("Scene表没有key: " + key);
        return ret;
    }
    public static _test1 Get_test1(int key)
    {
        if (!_map_test1.TryGetValue(key, out var ret))
            Loger.Error("_test1表没有key: " + key);
        return ret;
    }

}

public class Scene
{
    /// <summary>
    /// id
    /// </summary>
    public int id { get; }
    /// <summary>
    /// xxxx
    /// </summary>
    public string name { get; }

    public Scene(WBuffer buffer)
    {
        this.id = buffer.ReadInt();
        this.name = buffer.ReadString();
    }
}

public class _test1
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

    public _test1(WBuffer buffer)
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
