using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Dispose<T> : __DisposeHandle
{
    public Dispose(T o) => t = o;
    public T t { get; }

    internal static void Invoke(object oo, CoreWorld world)
    {
        if (typeof(SComponent).IsAssignableFrom(typeof(T)))
        {
            var o = (SComponent)oo;
            world.Event.RunEvent(new Dispose<T>((T)(object)o));
        }
        else
            world.Event.RunEvent(new Dispose<T>((T)oo));
    }
}
public class Dispose<T1, T2> : __DisposeHandle where T1 : SComponent where T2 : SComponent
{
    public Dispose(T1 t1, T2 t2) { this.t = t1; this.t2 = t2; }
    public T1 t { get; }
    public T2 t2 { get; }

    static void Invoke(object oo, CoreWorld world)
    {
        var c = (T1)oo;
        var o = c.Entity;
        if (o.Disposed || !world.Event.HasEvent(typeof(Dispose<T1, T2>))) return;

        if (!o.TryGetComponent<T2>(out var c2)) return;
        world.Event.RunEvent(new Dispose<T1, T2>(c, c2));
    }
}
public class Dispose<T1, T2, T3> : __DisposeHandle where T1 : SComponent where T2 : SComponent where T3 : SComponent
{
    public Dispose(T1 t1, T2 t2, T3 t3) { this.t = t1; this.t2 = t2; this.t3 = t3; }
    public T1 t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }

    static void Invoke(object oo, CoreWorld world)
    {
        var c = (T1)oo;
        var o = c.Entity;
        if (o.Disposed || !world.Event.HasEvent(typeof(Dispose<T1, T2, T3>))) return;

        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        world.Event.RunEvent(new Dispose<T1, T2, T3>(c, c2, c3));
    }
}
public class Dispose<T1, T2, T3, T4> : __DisposeHandle where T1 : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent
{
    public Dispose(T1 t1, T2 t2, T3 t3, T4 t4) { this.t = t1; this.t2 = t2; this.t3 = t3; this.t4 = t4; }
    public T1 t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }

    static void Invoke(object oo, CoreWorld world)
    {
        var c = (T1)oo;
        var o = c.Entity;
        if (o.Disposed || !world.Event.HasEvent(typeof(Dispose<T1, T2, T3, T4>))) return;

        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        if (!o.TryGetComponent<T4>(out var c4)) return;
        world.Event.RunEvent(new Dispose<T1, T2, T3, T4>(c, c2, c3, c4));
    }
}
public class Dispose<T1, T2, T3, T4, T5> : __DisposeHandle where T1 : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent
{
    public Dispose(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) { this.t = t1; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; }
    public T1 t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }

    static void Invoke(object oo, CoreWorld world)
    {
        var c = (T1)oo;
        var o = c.Entity;
        if (o.Disposed || !world.Event.HasEvent(typeof(Dispose<T1, T2, T3, T4, T5>))) return;

        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        if (!o.TryGetComponent<T4>(out var c4)) return;
        if (!o.TryGetComponent<T5>(out var c5)) return;
        world.Event.RunEvent(new Dispose<T1, T2, T3, T4, T5>(c, c2, c3, c4, c5));
    }
}
