using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class STimerAttribute : Attribute
{
    public float Time { get; }
    public int Count { get; }

    public STimerAttribute(float time, int count)
    {
        Time = time;
        Count = count;
    }
}
