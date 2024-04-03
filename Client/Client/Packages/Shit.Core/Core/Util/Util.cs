using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public static class Util
{
    static Random random = new Random((int)DateTime.Now.Ticks);

    public static long ToUtc(DateTime time)
    {
        TimeSpan ts = time - new DateTime(1970, 1, 1).ToLocalTime();
        return (long)ts.TotalMilliseconds;
    }
    public static DateTime ToDateTime(long utc)
    {
        DateTime t = new DateTime(1970, 1, 1).ToLocalTime();
        return t.AddMilliseconds(utc);
    }
    public static IPEndPoint ToIPEndPoint(string host, int port)
    {
        return new IPEndPoint(IPAddress.Parse(host), port);
    }
    public static IPEndPoint ToIPEndPoint(string address)
    {
        int index = address.LastIndexOf(':');
        string host = address.Substring(0, index);
        string p = address.Substring(index + 1);
        int port = int.Parse(p);
        return ToIPEndPoint(host, port);
    }

    public static int RandomInt() => random.Next();
    public static int RandomInt(int min, int max) => random.Next(min, max);
    public static float RandomFloat(float min, float max) => min + (max - min) * RandomFloat01();
    public static float RandomFloat01() => RandomInt(0, int.MaxValue) / (float)int.MaxValue;
    public static long RandomLong() => (((long)random.Next()) << 32) | (random.Next() & 0xffffffff);
}
