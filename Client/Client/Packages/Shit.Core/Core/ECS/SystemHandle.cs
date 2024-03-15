using System.Collections.Generic;
using Core;

public abstract class __SystemHandle { }
public abstract class __AwakeHandle : __SystemHandle { }
public abstract class __DisposeHandle : __SystemHandle { }
public abstract class __EnableHandle : __SystemHandle { }
public abstract class __ChangeHandle : __SystemHandle
{
    internal bool Disposed { get; private set; }
    internal bool Dispose() => this.Disposed = true;
    internal abstract void Invoke(World world);
}
public abstract class __UpdateHandle : __SystemHandle
{
    internal abstract bool IsValid();
    internal abstract void Invoke(World world);
}
public class Awake<T> : __AwakeHandle
{
    public Awake(T o) => t = o;
    public T t { get; }

    internal static void Invoke(object oo, World world)
    {
        if (typeof(SComponent).IsAssignableFrom(typeof(T)))
        {
            var o = (SObject)oo;
            if (!o.TryGetComponent(typeof(T), out var c)) return;
            world.Event.RunEvent(new Awake<T>((T)(object)c));
        }
        else
            world.Event.RunEvent(new Awake<T>((T)oo));
    }
}
public class Awake<T1, T2> : __AwakeHandle where T1 : SComponent where T2 : SComponent
{
    public Awake(T1 t1, T2 t2) { this.t = t1; this.t2 = t2; }
    public T1 t { get; }
    public T2 t2 { get; }

    static void Invoke(object oo, World world)
    {
        if (!world.Event.HasEvent(typeof(Awake<T1, T2>))) return;

        var o = (SObject)oo;
        if (!o.TryGetComponent<T1>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        world.Event.RunEvent(new Awake<T1, T2>(c, c2));
    }
}
public class Awake<T1, T2, T3> : __AwakeHandle where T1 : SComponent where T2 : SComponent where T3 : SComponent
{
    public Awake(T1 t1, T2 t2, T3 t3) { this.t = t1; this.t2 = t2; this.t3 = t3; }
    public T1 t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }

    static void Invoke(object oo, World world)
    {
        if (!world.Event.HasEvent(typeof(Awake<T1, T2,T3>))) return;

        var o = (SObject)oo;
        if (!o.TryGetComponent<T1>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        world.Event.RunEvent(new Awake<T1, T2, T3>(c, c2, c3));
    }
}
public class Awake<T1, T2, T3, T4> : __AwakeHandle where T1 : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent
{
    public Awake(T1 t1, T2 t2, T3 t3, T4 t4) { this.t = t1; this.t2 = t2; this.t3 = t3; this.t4 = t4; }
    public T1 t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }

    static void Invoke(object oo, World world)
    {
        if (!world.Event.HasEvent(typeof(Awake<T1, T2, T3, T4>))) return;

        var o = (SObject)oo;
        if (!o.TryGetComponent<T1>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        if (!o.TryGetComponent<T4>(out var c4)) return;
        world.Event.RunEvent(new Awake<T1, T2, T3, T4>(c, c2, c3, c4));
    }
}
public class Awake<T1, T2, T3, T4, T5> : __AwakeHandle where T1 : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent
{
    public Awake(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) { this.t = t1; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; }
    public T1 t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }

    static void Invoke(object oo, World world)
    {
        if (!world.Event.HasEvent(typeof(Awake<T1, T2, T3, T4, T5>))) return;

        var o = (SObject)oo;
        if (!o.TryGetComponent<T1>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        if (!o.TryGetComponent<T4>(out var c4)) return;
        if (!o.TryGetComponent<T5>(out var c5)) return;
        world.Event.RunEvent(new Awake<T1, T2, T3, T4, T5>(c, c2, c3, c4, c5));
    }
}
public class Dispose<T> : __DisposeHandle
{
    public Dispose(T o) => t = o;
    public T t { get; }

    internal static void Invoke(object oo, World world)
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

    static void Invoke(object oo, World world)
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

    static void Invoke(object oo, World world)
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

    static void Invoke(object oo, World world)
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

    static void Invoke(object oo, World world)
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
public class Enable<T> : __EnableHandle where T : SComponent
{
    public Enable(T o) => t = o;
    public T t { get; }

    internal static void Invoke(SComponent o, World world) => world.Event.RunEvent(new Enable<T>((T)o));
}
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
    internal override void Invoke(World world)
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
        c2._changeHandles ??= ObjectPool.Get<List<__ChangeHandle>>();
        c2._changeHandles.Add(v);
    }
    internal override void Invoke(World world)
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
        c2._changeHandles ??= ObjectPool.Get<List<__ChangeHandle>>();
        c2._changeHandles.Add(v);
        c3._changeHandles ??= ObjectPool.Get<List<__ChangeHandle>>();
        c3._changeHandles.Add(v);
    }
    internal override void Invoke(World world)
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
        c2._changeHandles ??= ObjectPool.Get<List<__ChangeHandle>>();
        c2._changeHandles.Add(v);
        c3._changeHandles ??= ObjectPool.Get<List<__ChangeHandle>>();
        c3._changeHandles.Add(v);
        c4._changeHandles ??= ObjectPool.Get<List<__ChangeHandle>>();
        c4._changeHandles.Add(v);
    }
    internal override void Invoke(World world)
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
        c2._changeHandles ??= ObjectPool.Get<List<__ChangeHandle>>();
        c2._changeHandles.Add(v);
        c3._changeHandles ??= ObjectPool.Get<List<__ChangeHandle>>();
        c3._changeHandles.Add(v);
        c4._changeHandles ??= ObjectPool.Get<List<__ChangeHandle>>();
        c4._changeHandles.Add(v);
        c5._changeHandles ??= ObjectPool.Get<List<__ChangeHandle>>();
        c5._changeHandles.Add(v);
    }
    internal override void Invoke(World world)
    {
        t._setChanged = t2._setChanged = t3._setChanged = t4._setChanged = t5._setChanged = false;
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable) return;
        world.Event.RunEventNoGC(this);
    }
}
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
    internal override void Invoke(World world)
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
    internal override void Invoke(World world)
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
    internal override void Invoke(World world)
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
    internal override void Invoke(World world)
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
    internal override void Invoke(World world)
    {
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable) return;
        world.Event.RunEventNoGC(this);
    }
}
