using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Timer
{
    static readonly DateTime _dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    static long _timeOffset;


    public static long ServerTime
    {
        get { return ClientTime - _timeOffset; }
        set { _timeOffset = ClientTime - value; }
    }
    public static long ClientTime
    {
        get { return (DateTime.Now.Ticks - _dt1970.Ticks) / 10000; }
    }

    static Action Call = null;
    public static void Add(float time,Action call)
    {
        Call += call;
    }
    public static void Remove(Action call)
    {
        Call -= call;
    }

    public static void Update()
    {
        Call?.Invoke();
    }
}
