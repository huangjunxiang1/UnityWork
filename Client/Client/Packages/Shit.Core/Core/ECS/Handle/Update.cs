using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Update<T1> : __UpdateHandle where T1 : SComponent
{
    public Update(T1 c) { t = c; }
    public T1 t { get; private set; }

    internal static __UpdateHandle TryCreateHandle(SObject target)
    {
        if (!target.TryGetComponent<T1>(out var c)) return null;
        return new Update<T1>(c);
    }
    internal override bool IsValid() => !t.Disposed;
    internal override void Invoke(CoreWorld world)
    {
        if (!t.Enable) return;
        world.Event.RunEventNoGC(this);
    }
}
public class Update<T1, T2> : __UpdateHandle where T1 : SComponent where T2 : SComponent
{
    public Update(T1 c, T2 c2) { t = c; t2 = c2; }
    public T1 t { get; private set; }
    public T2 t2 { get; private set; }

    internal static __UpdateHandle TryCreateHandle(SObject target)
    {
        if (!target.TryGetComponent<T1>(out var c)) return null;
        if (!target.TryGetComponent<T2>(out var c2)) return null;
        return new Update<T1, T2>(c, c2);
    }
    internal override bool IsValid() => !t.Disposed && !t2.Disposed;
    internal override void Invoke(CoreWorld world)
    {
        if (!t.Enable || !t2.Enable) return;
        world.Event.RunEventNoGC(this);
    }
}
public class Update<T1, T2, T3> : __UpdateHandle where T1 : SComponent where T2 : SComponent where T3 : SComponent
{
    public Update(T1 c, T2 c2, T3 c3) { t = c; t2 = c2; t3 = c3; }
    public T1 t { get; private set; }
    public T2 t2 { get; private set; }
    public T3 t3 { get; private set; }

    internal static __UpdateHandle TryCreateHandle(SObject target)
    {
        if (!target.TryGetComponent<T1>(out var c)) return null;
        if (!target.TryGetComponent<T2>(out var c2)) return null;
        if (!target.TryGetComponent<T3>(out var c3)) return null;
        return new Update<T1, T2, T3>(c, c2, c3);
    }
    internal override bool IsValid() => !t.Disposed && !t2.Disposed && !t3.Disposed;
    internal override void Invoke(CoreWorld world)
    {
        if (!t.Enable || !t2.Enable || !t3.Enable) return;
        world.Event.RunEventNoGC(this);
    }
}
public class Update<T1, T2, T3, T4> : __UpdateHandle where T1 : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent
{
    public Update(T1 c, T2 c2, T3 c3, T4 c4) { t = c; t2 = c2; t3 = c3; t4 = c4; }
    public T1 t { get; private set; }
    public T2 t2 { get; private set; }
    public T3 t3 { get; private set; }
    public T4 t4 { get; private set; }

    internal static __UpdateHandle TryCreateHandle(SObject target)
    {
        if (!target.TryGetComponent<T1>(out var c)) return null;
        if (!target.TryGetComponent<T2>(out var c2)) return null;
        if (!target.TryGetComponent<T3>(out var c3)) return null;
        if (!target.TryGetComponent<T4>(out var c4)) return null;
        return new Update<T1, T2, T3, T4>(c, c2, c3, c4);
    }
    internal override bool IsValid() => !t.Disposed && !t2.Disposed && !t3.Disposed && !t4.Disposed;
    internal override void Invoke(CoreWorld world)
    {
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable) return;
        world.Event.RunEventNoGC(this);
    }
}
public class Update<T1, T2, T3, T4, T5> : __UpdateHandle where T1 : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent
{
    public Update(T1 c, T2 c2, T3 c3, T4 c4, T5 c5) { t = c; t2 = c2; t3 = c3; t4 = c4; t5 = c5; }
    public T1 t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }

    internal static __UpdateHandle TryCreateHandle(SObject target)
    {
        if (!target.TryGetComponent<T1>(out var c)) return null;
        if (!target.TryGetComponent<T2>(out var c2)) return null;
        if (!target.TryGetComponent<T3>(out var c3)) return null;
        if (!target.TryGetComponent<T4>(out var c4)) return null;
        if (!target.TryGetComponent<T5>(out var c5)) return null;
        return new Update<T1, T2, T3, T4, T5>(c, c2, c3, c4, c5);
    }
    internal override bool IsValid() => !t.Disposed && !t2.Disposed && !t3.Disposed && !t4.Disposed && !t5.Disposed;
    internal override void Invoke(CoreWorld world)
    {
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable) return;
        world.Event.RunEventNoGC(this);
    }
}

