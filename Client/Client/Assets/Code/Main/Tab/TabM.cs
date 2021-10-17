using System.Collections.Generic;

public static class TabM
{
    public static test2[] test2Array { get; private set; }

    static Dictionary<int, test2> _maptest2 = new Dictionary<int, test2>();

    public static void Init(byte[] bytes)
    {
        WBuffer buffer = new WBuffer(bytes);

        int len0 = buffer.ReadInt();
        test2Array = new test2[len0];
        for (int i = 0; i < len0; i++)
        {
            var t = new test2(buffer);
            test2Array[i] = t;
            _maptest2.Add(t.id, t);
        }
    }

    public static test2 Gettest2(int key)
    {
        if (!_maptest2.TryGetValue(key, out var ret))
            Loger.Error("test2表没有key: " + key);
        return ret;
    }

}
