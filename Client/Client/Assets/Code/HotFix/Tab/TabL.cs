using System.Collections.Generic;

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
