using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Update<T> : __UpdateHandle where T : SComponent
{
    public Update(T t) { this.t = t; }
    public T t { get; }

    internal static __UpdateHandle TryCreateHandle(SObject o)
    {
        if (!o.TryGetComponent<T>(out var c)) return null;
        return new Update<T>(c);
    }
    internal override bool Disposed => t.Disposed;
    internal override void Invoke()
    {
        if (!t.Enable) return;
        t.World.Event.RunEventNoGCAndFaster(this);
    }
}
public class Update<T, T2> : __UpdateHandle where T : SComponent where T2 : SComponent
{
    public Update(T t, T2 t2) { this.t = t; this.t2 = t2; }
    public T t { get; }
    public T2 t2 { get; }

    internal static __UpdateHandle TryCreateHandle(SObject o)
    {
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        return new Update<T, T2>(c, c2);
    }
    internal override bool Disposed => t.Disposed || t2.Disposed;
    internal override void Invoke()
    {
        if (!t.Enable || !t2.Enable) return;
        t.World.Event.RunEventNoGCAndFaster(this);
    }
}
public class Update<T, T2, T3> : __UpdateHandle where T : SComponent where T2 : SComponent where T3 : SComponent
{
    public Update(T t, T2 t2, T3 t3) { this.t = t; this.t2 = t2; this.t3 = t3; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }

    internal static __UpdateHandle TryCreateHandle(SObject o)
    {
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        return new Update<T, T2, T3>(c, c2, c3);
    }
    internal override bool Disposed => t.Disposed || t2.Disposed || t3.Disposed;
    internal override void Invoke()
    {
        if (!t.Enable || !t2.Enable || !t3.Enable) return;
        t.World.Event.RunEventNoGCAndFaster(this);
    }
}
public class Update<T, T2, T3, T4> : __UpdateHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent
{
    public Update(T t, T2 t2, T3 t3, T4 t4) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }

    internal static __UpdateHandle TryCreateHandle(SObject o)
    {
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        if (!o.TryGetComponent<T4>(out var c4)) return null;
        return new Update<T, T2, T3, T4>(c, c2, c3, c4);
    }
    internal override bool Disposed => t.Disposed || t2.Disposed || t3.Disposed || t4.Disposed;
    internal override void Invoke()
    {
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable) return;
        t.World.Event.RunEventNoGCAndFaster(this);
    }
}
public class Update<T, T2, T3, T4, T5> : __UpdateHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent
{
    public Update(T t, T2 t2, T3 t3, T4 t4, T5 t5) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }

    internal static __UpdateHandle TryCreateHandle(SObject o)
    {
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        if (!o.TryGetComponent<T4>(out var c4)) return null;
        if (!o.TryGetComponent<T5>(out var c5)) return null;
        return new Update<T, T2, T3, T4, T5>(c, c2, c3, c4, c5);
    }
    internal override bool Disposed => t.Disposed || t2.Disposed || t3.Disposed || t4.Disposed || t5.Disposed;
    internal override void Invoke()
    {
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable) return;
        t.World.Event.RunEventNoGCAndFaster(this);
    }
}
public class Update<T, T2, T3, T4, T5, T6> : __UpdateHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent
{
    public Update(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }

    internal static __UpdateHandle TryCreateHandle(SObject o)
    {
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        if (!o.TryGetComponent<T4>(out var c4)) return null;
        if (!o.TryGetComponent<T5>(out var c5)) return null;
        if (!o.TryGetComponent<T6>(out var c6)) return null;
        return new Update<T, T2, T3, T4, T5, T6>(c, c2, c3, c4, c5, c6);
    }
    internal override bool Disposed => t.Disposed || t2.Disposed || t3.Disposed || t4.Disposed || t5.Disposed || t6.Disposed;
    internal override void Invoke()
    {
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable || !t6.Enable) return;
        t.World.Event.RunEventNoGCAndFaster(this);
    }
}
public class Update<T, T2, T3, T4, T5, T6, T7> : __UpdateHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent
{
    public Update(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }
    public T7 t7 { get; }

    internal static __UpdateHandle TryCreateHandle(SObject o)
    {
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        if (!o.TryGetComponent<T4>(out var c4)) return null;
        if (!o.TryGetComponent<T5>(out var c5)) return null;
        if (!o.TryGetComponent<T6>(out var c6)) return null;
        if (!o.TryGetComponent<T7>(out var c7)) return null;
        return new Update<T, T2, T3, T4, T5, T6, T7>(c, c2, c3, c4, c5, c6, c7);
    }
    internal override bool Disposed => t.Disposed || t2.Disposed || t3.Disposed || t4.Disposed || t5.Disposed || t6.Disposed || t7.Disposed;
    internal override void Invoke()
    {
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable || !t6.Enable || !t7.Enable) return;
        t.World.Event.RunEventNoGCAndFaster(this);
    }
}
public class Update<T, T2, T3, T4, T5, T6, T7, T8> : __UpdateHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent
{
    public Update(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }
    public T7 t7 { get; }
    public T8 t8 { get; }

    internal static __UpdateHandle TryCreateHandle(SObject o)
    {
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        if (!o.TryGetComponent<T4>(out var c4)) return null;
        if (!o.TryGetComponent<T5>(out var c5)) return null;
        if (!o.TryGetComponent<T6>(out var c6)) return null;
        if (!o.TryGetComponent<T7>(out var c7)) return null;
        if (!o.TryGetComponent<T8>(out var c8)) return null;
        return new Update<T, T2, T3, T4, T5, T6, T7, T8>(c, c2, c3, c4, c5, c6, c7, c8);
    }
    internal override bool Disposed => t.Disposed || t2.Disposed || t3.Disposed || t4.Disposed || t5.Disposed || t6.Disposed || t7.Disposed || t8.Disposed;
    internal override void Invoke()
    {
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable || !t6.Enable || !t7.Enable || !t8.Enable) return;
        t.World.Event.RunEventNoGCAndFaster(this);
    }
}
public class Update<T, T2, T3, T4, T5, T6, T7, T8, T9> : __UpdateHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent
{
    public Update(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; this.t9 = t9; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }
    public T7 t7 { get; }
    public T8 t8 { get; }
    public T9 t9 { get; }

    internal static __UpdateHandle TryCreateHandle(SObject o)
    {
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        if (!o.TryGetComponent<T4>(out var c4)) return null;
        if (!o.TryGetComponent<T5>(out var c5)) return null;
        if (!o.TryGetComponent<T6>(out var c6)) return null;
        if (!o.TryGetComponent<T7>(out var c7)) return null;
        if (!o.TryGetComponent<T8>(out var c8)) return null;
        if (!o.TryGetComponent<T9>(out var c9)) return null;
        return new Update<T, T2, T3, T4, T5, T6, T7, T8, T9>(c, c2, c3, c4, c5, c6, c7, c8, c9);
    }
    internal override bool Disposed => t.Disposed || t2.Disposed || t3.Disposed || t4.Disposed || t5.Disposed || t6.Disposed || t7.Disposed || t8.Disposed || t9.Disposed;
    internal override void Invoke()
    {
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable || !t6.Enable || !t7.Enable || !t8.Enable || !t9.Enable) return;
        t.World.Event.RunEventNoGCAndFaster(this);
    }
}
public class Update<T, T2, T3, T4, T5, T6, T7, T8, T9, T10> : __UpdateHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent where T10 : SComponent
{
    public Update(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; this.t9 = t9; this.t10 = t10; }
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

    internal static __UpdateHandle TryCreateHandle(SObject o)
    {
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        if (!o.TryGetComponent<T4>(out var c4)) return null;
        if (!o.TryGetComponent<T5>(out var c5)) return null;
        if (!o.TryGetComponent<T6>(out var c6)) return null;
        if (!o.TryGetComponent<T7>(out var c7)) return null;
        if (!o.TryGetComponent<T8>(out var c8)) return null;
        if (!o.TryGetComponent<T9>(out var c9)) return null;
        if (!o.TryGetComponent<T10>(out var c10)) return null;
        return new Update<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>(c, c2, c3, c4, c5, c6, c7, c8, c9, c10);
    }
    internal override bool Disposed => t.Disposed || t2.Disposed || t3.Disposed || t4.Disposed || t5.Disposed || t6.Disposed || t7.Disposed || t8.Disposed || t9.Disposed || t10.Disposed;
    internal override void Invoke()
    {
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable || !t6.Enable || !t7.Enable || !t8.Enable || !t9.Enable || !t10.Enable) return;
        t.World.Event.RunEventNoGCAndFaster(this);
    }
}
