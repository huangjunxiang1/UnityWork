using System.Collections.Generic;

public static class TabM
{
    public static _test2[] _test2Array { get; private set; }

    static Dictionary<int, _test2> _map_test2 = new Dictionary<int, _test2>();

    public static void Init(byte[] bytes)
    {
        WBuffer buffer = new WBuffer(bytes);

        int len0 = buffer.ReadInt();
        _test2Array = new _test2[len0];
        for (int i = 0; i < len0; i++)
        {
            var t = new _test2(buffer);
            _test2Array[i] = t;
            _map_test2.Add(t.id, t);
        }
    }

    public static _test2 Get_test2(int key)
    {
        if (!_map_test2.TryGetValue(key, out var ret))
            Loger.Error("_test2表没有key: " + key);
        return ret;
    }

}
