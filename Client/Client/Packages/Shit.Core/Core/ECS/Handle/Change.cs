using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Change<T1> : __ChangeHandle where T1 : SComponent
{
    public Change(T1 v1) => this.t = v1;
    public T1 t { get; }

    internal static void TryCreateHandle(SObject target)
    {
        if (!target.TryGetComponent<T1>(out var c)) return;
        var v = new Change<T1>(c);
        c._changeHandles ??= ObjectPool.Get<List<__ChangeHandle>>();
        c._changeHandles.Add(v);
    }
    internal override void Invoke(CoreWorld world)
    {
        t._setChanged = false;
        if (!t.Enable) return;
        world.Event.RunEventNoGC(this);
    }
}
public class Change<T1, T2> : __ChangeHandle where T1 : SComponent where T2 : SComponent
{
    public Change(T1 t1, T2 t2)
    {
        this.t = t1;
        this.t2 = t2;
    }
    public T1 t { get; }
    public T2 t2 { get; }

    internal static void TryCreateHandle(SObject target)
    {
        if (!target.TryGetComponent<T1>(out var c)) return;
        if (!target.TryGetComponent<T2>(out var c2)) return;
        var v = new Change<T1, T2>(c, c2);
        c._changeHandles ??= ObjectPool.Get<List<__ChangeHandle>>();
        c._changeHandles.Add(v);
    }
    internal override void Invoke(CoreWorld world)
    {
        t._setChanged = t2._setChanged = false;
        if (!t.Enable || !t2.Enable) return;
        world.Event.RunEventNoGC(this);
    }
}
public class Change<T1, T2, T3> : __ChangeHandle where T1 : SComponent where T2 : SComponent where T3 : SComponent
{
    public Change(T1 t1, T2 t2, T3 t3)
    {
        this.t = t1;
        this.t2 = t2;
        this.t3 = t3;
    }
    public T1 t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }

    internal static void TryCreateHandle(SObject target)
    {
        if (!target.TryGetComponent<T1>(out var c)) return;
        if (!target.TryGetComponent<T2>(out var c2)) return;
        if (!target.TryGetComponent<T3>(out var c3)) return;
        var v = new Change<T1, T2, T3>(c, c2, c3);
        c._changeHandles ??= ObjectPool.Get<List<__ChangeHandle>>();
        c._changeHandles.Add(v);
    }
    internal override void Invoke(CoreWorld world)
    {
        t._setChanged = t2._setChanged = t3._setChanged = false;
        if (!t.Enable || !t2.Enable || !t3.Enable) return;
        world.Event.RunEventNoGC(this);
    }
}
public class Change<T1, T2, T3, T4> : __ChangeHandle where T1 : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent
{
    public Change(T1 t1, T2 t2, T3 t3, T4 t4)
    {
        this.t = t1;
        this.t2 = t2;
        this.t3 = t3;
        this.t4 = t4;
    }
    public T1 t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }

    internal static void TryCreateHandle(SObject target)
    {
        if (!target.TryGetComponent<T1>(out var c)) return;
        if (!target.TryGetComponent<T2>(out var c2)) return;
        if (!target.TryGetComponent<T3>(out var c3)) return;
        if (!target.TryGetComponent<T4>(out var c4)) return;
        var v = new Change<T1, T2, T3, T4>(c, c2, c3, c4);
        c._changeHandles ??= ObjectPool.Get<List<__ChangeHandle>>();
        c._changeHandles.Add(v);
    }
    internal override void Invoke(CoreWorld world)
    {
        t._setChanged = t2._setChanged = t3._setChanged = t4._setChanged = false;
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable) return;
        world.Event.RunEventNoGC(this);
    }
}
public class Change<T1, T2, T3, T4, T5> : __ChangeHandle where T1 : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent
{
    public Change(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
    {
        this.t = t1;
        this.t2 = t2;
        this.t3 = t3;
        this.t4 = t4;
        this.t5 = t5;
    }
    public T1 t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }

    internal static void TryCreateHandle(SObject target)
    {
        if (!target.TryGetComponent<T1>(out var c)) return;
        if (!target.TryGetComponent<T2>(out var c2)) return;
        if (!target.TryGetComponent<T3>(out var c3)) return;
        if (!target.TryGetComponent<T4>(out var c4)) return;
        if (!target.TryGetComponent<T5>(out var c5)) return;
        var v = new Change<T1, T2, T3, T4, T5>(c, c2, c3, c4, c5);
        c._changeHandles ??= ObjectPool.Get<List<__ChangeHandle>>();
        c._changeHandles.Add(v);
    }
    internal override void Invoke(CoreWorld world)
    {
        t._setChanged = t2._setChanged = t3._setChanged = t4._setChanged = t5._setChanged = false;
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable) return;
        world.Event.RunEventNoGC(this);
    }
}