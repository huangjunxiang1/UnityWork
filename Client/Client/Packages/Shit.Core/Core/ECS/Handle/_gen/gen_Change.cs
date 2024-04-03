using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Change<T> : __ChangeHandle where T : SComponent
{
    public Change(T t) { this.t = t; }
    public T t { get; }

    internal static void TryCreateHandle(SObject o)
    {
        if (!o.TryGetComponent<T>(out var c)) return;
        var v = new Change<T>(c);
        if (c._changeHandles == null) c._changeHandles = ObjectPool.Get<List<__ChangeHandle>>();
        c._changeHandles.Add(v);
        o.World.System.AddToChangeWaitInvoke(v);
    }
    internal override void AddToRemoveWait()
    {
        t.World.System.AddToChangeWaitRemove(t);
    }
    internal override void Invoke()
    {
        t._setChanged = false;
        if (this.Disposed = (t.Disposed)) return;
        if (!t.Enable) return;
        t.World.Event.RunEventNoGCAndFaster(this);
    }
}
public class Change<T, T2> : __ChangeHandle where T : SComponent where T2 : SComponent
{
    public Change(T t, T2 t2) { this.t = t; this.t2 = t2; }
    public T t { get; }
    public T2 t2 { get; }

    internal static void TryCreateHandle(SObject o)
    {
        if (!o.TryGetComponent<T>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        var v = new Change<T, T2>(c, c2);
        if (c._changeHandles == null) c._changeHandles = ObjectPool.Get<List<__ChangeHandle>>();
        c._changeHandles.Add(v);
        o.World.System.AddToChangeWaitInvoke(v);
    }
    internal override void AddToRemoveWait()
    {
        t.World.System.AddToChangeWaitRemove(t);
    }
    internal override void Invoke()
    {
        t._setChanged = t2._setChanged = false;
        if (this.Disposed = (t.Disposed || t2.Disposed)) return;
        if (!t.Enable || !t2.Enable) return;
        t.World.Event.RunEventNoGCAndFaster(this);
    }
}
public class Change<T, T2, T3> : __ChangeHandle where T : SComponent where T2 : SComponent where T3 : SComponent
{
    public Change(T t, T2 t2, T3 t3) { this.t = t; this.t2 = t2; this.t3 = t3; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }

    internal static void TryCreateHandle(SObject o)
    {
        if (!o.TryGetComponent<T>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        var v = new Change<T, T2, T3>(c, c2, c3);
        if (c._changeHandles == null) c._changeHandles = ObjectPool.Get<List<__ChangeHandle>>();
        c._changeHandles.Add(v);
        o.World.System.AddToChangeWaitInvoke(v);
    }
    internal override void AddToRemoveWait()
    {
        t.World.System.AddToChangeWaitRemove(t);
    }
    internal override void Invoke()
    {
        t._setChanged = t2._setChanged = t3._setChanged = false;
        if (this.Disposed = (t.Disposed || t2.Disposed || t3.Disposed)) return;
        if (!t.Enable || !t2.Enable || !t3.Enable) return;
        t.World.Event.RunEventNoGCAndFaster(this);
    }
}
public class Change<T, T2, T3, T4> : __ChangeHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent
{
    public Change(T t, T2 t2, T3 t3, T4 t4) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }

    internal static void TryCreateHandle(SObject o)
    {
        if (!o.TryGetComponent<T>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        if (!o.TryGetComponent<T4>(out var c4)) return;
        var v = new Change<T, T2, T3, T4>(c, c2, c3, c4);
        if (c._changeHandles == null) c._changeHandles = ObjectPool.Get<List<__ChangeHandle>>();
        c._changeHandles.Add(v);
        o.World.System.AddToChangeWaitInvoke(v);
    }
    internal override void AddToRemoveWait()
    {
        t.World.System.AddToChangeWaitRemove(t);
    }
    internal override void Invoke()
    {
        t._setChanged = t2._setChanged = t3._setChanged = t4._setChanged = false;
        if (this.Disposed = (t.Disposed || t2.Disposed || t3.Disposed || t4.Disposed)) return;
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable) return;
        t.World.Event.RunEventNoGCAndFaster(this);
    }
}
public class Change<T, T2, T3, T4, T5> : __ChangeHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent
{
    public Change(T t, T2 t2, T3 t3, T4 t4, T5 t5) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }

    internal static void TryCreateHandle(SObject o)
    {
        if (!o.TryGetComponent<T>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        if (!o.TryGetComponent<T4>(out var c4)) return;
        if (!o.TryGetComponent<T5>(out var c5)) return;
        var v = new Change<T, T2, T3, T4, T5>(c, c2, c3, c4, c5);
        if (c._changeHandles == null) c._changeHandles = ObjectPool.Get<List<__ChangeHandle>>();
        c._changeHandles.Add(v);
        o.World.System.AddToChangeWaitInvoke(v);
    }
    internal override void AddToRemoveWait()
    {
        t.World.System.AddToChangeWaitRemove(t);
    }
    internal override void Invoke()
    {
        t._setChanged = t2._setChanged = t3._setChanged = t4._setChanged = t5._setChanged = false;
        if (this.Disposed = (t.Disposed || t2.Disposed || t3.Disposed || t4.Disposed || t5.Disposed)) return;
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable) return;
        t.World.Event.RunEventNoGCAndFaster(this);
    }
}
public class Change<T, T2, T3, T4, T5, T6> : __ChangeHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent
{
    public Change(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }

    internal static void TryCreateHandle(SObject o)
    {
        if (!o.TryGetComponent<T>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        if (!o.TryGetComponent<T4>(out var c4)) return;
        if (!o.TryGetComponent<T5>(out var c5)) return;
        if (!o.TryGetComponent<T6>(out var c6)) return;
        var v = new Change<T, T2, T3, T4, T5, T6>(c, c2, c3, c4, c5, c6);
        if (c._changeHandles == null) c._changeHandles = ObjectPool.Get<List<__ChangeHandle>>();
        c._changeHandles.Add(v);
        o.World.System.AddToChangeWaitInvoke(v);
    }
    internal override void AddToRemoveWait()
    {
        t.World.System.AddToChangeWaitRemove(t);
    }
    internal override void Invoke()
    {
        t._setChanged = t2._setChanged = t3._setChanged = t4._setChanged = t5._setChanged = t6._setChanged = false;
        if (this.Disposed = (t.Disposed || t2.Disposed || t3.Disposed || t4.Disposed || t5.Disposed || t6.Disposed)) return;
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable || !t6.Enable) return;
        t.World.Event.RunEventNoGCAndFaster(this);
    }
}
public class Change<T, T2, T3, T4, T5, T6, T7> : __ChangeHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent
{
    public Change(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }
    public T7 t7 { get; }

    internal static void TryCreateHandle(SObject o)
    {
        if (!o.TryGetComponent<T>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        if (!o.TryGetComponent<T4>(out var c4)) return;
        if (!o.TryGetComponent<T5>(out var c5)) return;
        if (!o.TryGetComponent<T6>(out var c6)) return;
        if (!o.TryGetComponent<T7>(out var c7)) return;
        var v = new Change<T, T2, T3, T4, T5, T6, T7>(c, c2, c3, c4, c5, c6, c7);
        if (c._changeHandles == null) c._changeHandles = ObjectPool.Get<List<__ChangeHandle>>();
        c._changeHandles.Add(v);
        o.World.System.AddToChangeWaitInvoke(v);
    }
    internal override void AddToRemoveWait()
    {
        t.World.System.AddToChangeWaitRemove(t);
    }
    internal override void Invoke()
    {
        t._setChanged = t2._setChanged = t3._setChanged = t4._setChanged = t5._setChanged = t6._setChanged = t7._setChanged = false;
        if (this.Disposed = (t.Disposed || t2.Disposed || t3.Disposed || t4.Disposed || t5.Disposed || t6.Disposed || t7.Disposed)) return;
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable || !t6.Enable || !t7.Enable) return;
        t.World.Event.RunEventNoGCAndFaster(this);
    }
}
public class Change<T, T2, T3, T4, T5, T6, T7, T8> : __ChangeHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent
{
    public Change(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }
    public T7 t7 { get; }
    public T8 t8 { get; }

    internal static void TryCreateHandle(SObject o)
    {
        if (!o.TryGetComponent<T>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        if (!o.TryGetComponent<T4>(out var c4)) return;
        if (!o.TryGetComponent<T5>(out var c5)) return;
        if (!o.TryGetComponent<T6>(out var c6)) return;
        if (!o.TryGetComponent<T7>(out var c7)) return;
        if (!o.TryGetComponent<T8>(out var c8)) return;
        var v = new Change<T, T2, T3, T4, T5, T6, T7, T8>(c, c2, c3, c4, c5, c6, c7, c8);
        if (c._changeHandles == null) c._changeHandles = ObjectPool.Get<List<__ChangeHandle>>();
        c._changeHandles.Add(v);
        o.World.System.AddToChangeWaitInvoke(v);
    }
    internal override void AddToRemoveWait()
    {
        t.World.System.AddToChangeWaitRemove(t);
    }
    internal override void Invoke()
    {
        t._setChanged = t2._setChanged = t3._setChanged = t4._setChanged = t5._setChanged = t6._setChanged = t7._setChanged = t8._setChanged = false;
        if (this.Disposed = (t.Disposed || t2.Disposed || t3.Disposed || t4.Disposed || t5.Disposed || t6.Disposed || t7.Disposed || t8.Disposed)) return;
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable || !t6.Enable || !t7.Enable || !t8.Enable) return;
        t.World.Event.RunEventNoGCAndFaster(this);
    }
}
public class Change<T, T2, T3, T4, T5, T6, T7, T8, T9> : __ChangeHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent
{
    public Change(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; this.t9 = t9; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }
    public T7 t7 { get; }
    public T8 t8 { get; }
    public T9 t9 { get; }

    internal static void TryCreateHandle(SObject o)
    {
        if (!o.TryGetComponent<T>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        if (!o.TryGetComponent<T4>(out var c4)) return;
        if (!o.TryGetComponent<T5>(out var c5)) return;
        if (!o.TryGetComponent<T6>(out var c6)) return;
        if (!o.TryGetComponent<T7>(out var c7)) return;
        if (!o.TryGetComponent<T8>(out var c8)) return;
        if (!o.TryGetComponent<T9>(out var c9)) return;
        var v = new Change<T, T2, T3, T4, T5, T6, T7, T8, T9>(c, c2, c3, c4, c5, c6, c7, c8, c9);
        if (c._changeHandles == null) c._changeHandles = ObjectPool.Get<List<__ChangeHandle>>();
        c._changeHandles.Add(v);
        o.World.System.AddToChangeWaitInvoke(v);
    }
    internal override void AddToRemoveWait()
    {
        t.World.System.AddToChangeWaitRemove(t);
    }
    internal override void Invoke()
    {
        t._setChanged = t2._setChanged = t3._setChanged = t4._setChanged = t5._setChanged = t6._setChanged = t7._setChanged = t8._setChanged = t9._setChanged = false;
        if (this.Disposed = (t.Disposed || t2.Disposed || t3.Disposed || t4.Disposed || t5.Disposed || t6.Disposed || t7.Disposed || t8.Disposed || t9.Disposed)) return;
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable || !t6.Enable || !t7.Enable || !t8.Enable || !t9.Enable) return;
        t.World.Event.RunEventNoGCAndFaster(this);
    }
}
public class Change<T, T2, T3, T4, T5, T6, T7, T8, T9, T10> : __ChangeHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent where T10 : SComponent
{
    public Change(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; this.t9 = t9; this.t10 = t10; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }
    public T7 t7 { get; }
    public T8 t8 { get; }
    public T9 t9 { get; }
    public T10 t10 { get; }

    internal static void TryCreateHandle(SObject o)
    {
        if (!o.TryGetComponent<T>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        if (!o.TryGetComponent<T4>(out var c4)) return;
        if (!o.TryGetComponent<T5>(out var c5)) return;
        if (!o.TryGetComponent<T6>(out var c6)) return;
        if (!o.TryGetComponent<T7>(out var c7)) return;
        if (!o.TryGetComponent<T8>(out var c8)) return;
        if (!o.TryGetComponent<T9>(out var c9)) return;
        if (!o.TryGetComponent<T10>(out var c10)) return;
        var v = new Change<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>(c, c2, c3, c4, c5, c6, c7, c8, c9, c10);
        if (c._changeHandles == null) c._changeHandles = ObjectPool.Get<List<__ChangeHandle>>();
        c._changeHandles.Add(v);
        o.World.System.AddToChangeWaitInvoke(v);
    }
    internal override void AddToRemoveWait()
    {
        t.World.System.AddToChangeWaitRemove(t);
    }
    internal override void Invoke()
    {
        t._setChanged = t2._setChanged = t3._setChanged = t4._setChanged = t5._setChanged = t6._setChanged = t7._setChanged = t8._setChanged = t9._setChanged = t10._setChanged = false;
        if (this.Disposed = (t.Disposed || t2.Disposed || t3.Disposed || t4.Disposed || t5.Disposed || t6.Disposed || t7.Disposed || t8.Disposed || t9.Disposed || t10.Disposed)) return;
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable || !t6.Enable || !t7.Enable || !t8.Enable || !t9.Enable || !t10.Enable) return;
        t.World.Event.RunEventNoGCAndFaster(this);
    }
}
