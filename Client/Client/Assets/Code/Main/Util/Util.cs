using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public class Util
{
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
}
