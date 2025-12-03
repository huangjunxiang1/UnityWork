using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public static partial class Util
{
    static Util()
    {
        int tick = (int)DateTime.Now.Ticks;
        int pid = Process.GetCurrentProcess().Id;
        int seed = (tick >> 17) ^ (pid << 15);
        random = new Random(seed);
    }
    static Random random;

    public static long ToUtc(DateTime time)
    {
        TimeSpan ts = time - DateTime.UnixEpoch.ToLocalTime();
        return (long)ts.TotalMilliseconds;
    }
    public static DateTime ToDateTime(long utc)
    {
        DateTime t = DateTime.UnixEpoch.ToLocalTime();
        return t.AddMilliseconds(utc);
    }
    public static IPEndPoint ToIPEndPoint(string host, int port)
    {
        return new IPEndPoint(IPAddress.Parse(host), port);
    }
    public static IPEndPoint ToIPEndPoint(string address)
    {
        int index = address.LastIndexOf(':');
        string host = address[..index];
        string p = address[(index + 1)..];
        int port = int.Parse(p);
        return ToIPEndPoint(host, port);
    }

    public static int RandomInt() => random.Next();
    public static int RandomInt(int min, int max) => random.Next(min, max);
    public static float RandomFloat(float min, float max) => min + (max - min) * RandomFloat01();
    public static float RandomFloat01() => RandomInt(0, int.MaxValue) / (float)int.MaxValue;
    public static long RandomLong() => (((long)random.Next()) << 32) | (random.Next() & 0xffffffff);
}
