using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SystemAttribute : SAttribute { }
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class AwakeSystem : SystemAttribute { }
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class DisposeSystem : SystemAttribute { }
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class InSystem : SystemAttribute { }
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class OutSystem : SystemAttribute { }
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class ChangeSystem : SystemAttribute { }
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class AnyChangeSystem : SystemAttribute { }
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class BeforeUpdateSystem : SystemAttribute { }
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class UpdateSystem : SystemAttribute { }
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class LateUpdateSystem : SystemAttribute { }
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class EventWatcherSystem : SystemAttribute { }
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class KVWatcherSystem : SystemAttribute
{
    public int K { get; set; }
    public KVWatcherSystem(int k) { this.K = k; }
}
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class Timer : SAttribute
{
    public float delay { get; set; }
    public int count { get; set; }
    public Timer(float delay, int count)
    {
        this.delay = delay;
        this.count = count;
    }
}