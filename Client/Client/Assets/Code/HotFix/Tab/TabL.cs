using System.Collections.Generic;

public static class TabL
{
    public static test1[] test1Array { get; private set; }

    static Dictionary<int, test1> _maptest1 = new Dictionary<int, test1>();

    public static void Init(byte[] bytes)
    {
        WBuffer buffer = new WBuffer(bytes);

        int len0 = buffer.ReadInt();
        test1Array = new test1[len0];
        for (int i = 0; i < len0; i++)
        {
            var t = new test1(buffer);
            test1Array[i] = t;
            _maptest1.Add(t.id, t);
        }
    }

    public static test1 Gettest1(int key)
    {
        if (!_maptest1.TryGetValue(key, out var ret))
            Loger.Error("test1表没有key: " + key);
        return ret;
    }

}
