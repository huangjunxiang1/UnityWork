using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Out<T> : __OutHandle where T : SComponent
{
    public Out(T t) { this.t = t; }
    public T t { get; }

    internal static void Invoke(Type type, SComponent c0)
    {
        T c;
        if (typeof(T) == type) c = c0 as T; else { if (!c0.Entity.TryGetComponent(out c) || !c.Enable) return; }
        c0.Entity.World.Event.RunEvent(new Out<T>(c));
    }
    internal static void Invoke2(SObject o, Dictionary<Type, __OutHandle> record)
    {
        if (record.ContainsKey(typeof(Out<T>))) return;
        if (!o.Entity.TryGetComponent<T>(out var c) || !c.Enable) return;
        record.Add(typeof(Out<T>), new Out<T>(c));
    }
    internal override void Invoke(SObject o) => o.World.Event.RunEvent(this);
}
public class Out<T, T2> : __OutHandle where T : SComponent where T2 : SComponent
{
    public Out(T t, T2 t2) { this.t = t; this.t2 = t2; }
    public T t { get; }
    public T2 t2 { get; }

    internal static void Invoke(Type type, SComponent c0)
    {
        T c;T2 c2;
        if (typeof(T) == type) c = c0 as T; else { if (!c0.Entity.TryGetComponent(out c) || !c.Enable) return; }
        if (typeof(T2) == type) c2 = c0 as T2; else { if (!c0.Entity.TryGetComponent(out c2) || !c2.Enable) return; }
        c0.Entity.World.Event.RunEvent(new Out<T, T2>(c, c2));
    }
    internal static void Invoke2(SObject o, Dictionary<Type, __OutHandle> record)
    {
        if (record.ContainsKey(typeof(Out<T, T2>))) return;
        if (!o.Entity.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!o.Entity.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        record.Add(typeof(Out<T, T2>), new Out<T, T2>(c, c2));
    }
    internal override void Invoke(SObject o) => o.World.Event.RunEvent(this);
}
public class Out<T, T2, T3> : __OutHandle where T : SComponent where T2 : SComponent where T3 : SComponent
{
    public Out(T t, T2 t2, T3 t3) { this.t = t; this.t2 = t2; this.t3 = t3; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }

    internal static void Invoke(Type type, SComponent c0)
    {
        T c;T2 c2;T3 c3;
        if (typeof(T) == type) c = c0 as T; else { if (!c0.Entity.TryGetComponent(out c) || !c.Enable) return; }
        if (typeof(T2) == type) c2 = c0 as T2; else { if (!c0.Entity.TryGetComponent(out c2) || !c2.Enable) return; }
        if (typeof(T3) == type) c3 = c0 as T3; else { if (!c0.Entity.TryGetComponent(out c3) || !c3.Enable) return; }
        c0.Entity.World.Event.RunEvent(new Out<T, T2, T3>(c, c2, c3));
    }
    internal static void Invoke2(SObject o, Dictionary<Type, __OutHandle> record)
    {
        if (record.ContainsKey(typeof(Out<T, T2, T3>))) return;
        if (!o.Entity.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!o.Entity.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.Entity.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        record.Add(typeof(Out<T, T2, T3>), new Out<T, T2, T3>(c, c2, c3));
    }
    internal override void Invoke(SObject o) => o.World.Event.RunEvent(this);
}
public class Out<T, T2, T3, T4> : __OutHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent
{
    public Out(T t, T2 t2, T3 t3, T4 t4) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }

    internal static void Invoke(Type type, SComponent c0)
    {
        T c;T2 c2;T3 c3;T4 c4;
        if (typeof(T) == type) c = c0 as T; else { if (!c0.Entity.TryGetComponent(out c) || !c.Enable) return; }
        if (typeof(T2) == type) c2 = c0 as T2; else { if (!c0.Entity.TryGetComponent(out c2) || !c2.Enable) return; }
        if (typeof(T3) == type) c3 = c0 as T3; else { if (!c0.Entity.TryGetComponent(out c3) || !c3.Enable) return; }
        if (typeof(T4) == type) c4 = c0 as T4; else { if (!c0.Entity.TryGetComponent(out c4) || !c4.Enable) return; }
        c0.Entity.World.Event.RunEvent(new Out<T, T2, T3, T4>(c, c2, c3, c4));
    }
    internal static void Invoke2(SObject o, Dictionary<Type, __OutHandle> record)
    {
        if (record.ContainsKey(typeof(Out<T, T2, T3, T4>))) return;
        if (!o.Entity.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!o.Entity.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.Entity.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.Entity.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        record.Add(typeof(Out<T, T2, T3, T4>), new Out<T, T2, T3, T4>(c, c2, c3, c4));
    }
    internal override void Invoke(SObject o) => o.World.Event.RunEvent(this);
}
public class Out<T, T2, T3, T4, T5> : __OutHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent
{
    public Out(T t, T2 t2, T3 t3, T4 t4, T5 t5) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }

    internal static void Invoke(Type type, SComponent c0)
    {
        T c;T2 c2;T3 c3;T4 c4;T5 c5;
        if (typeof(T) == type) c = c0 as T; else { if (!c0.Entity.TryGetComponent(out c) || !c.Enable) return; }
        if (typeof(T2) == type) c2 = c0 as T2; else { if (!c0.Entity.TryGetComponent(out c2) || !c2.Enable) return; }
        if (typeof(T3) == type) c3 = c0 as T3; else { if (!c0.Entity.TryGetComponent(out c3) || !c3.Enable) return; }
        if (typeof(T4) == type) c4 = c0 as T4; else { if (!c0.Entity.TryGetComponent(out c4) || !c4.Enable) return; }
        if (typeof(T5) == type) c5 = c0 as T5; else { if (!c0.Entity.TryGetComponent(out c5) || !c5.Enable) return; }
        c0.Entity.World.Event.RunEvent(new Out<T, T2, T3, T4, T5>(c, c2, c3, c4, c5));
    }
    internal static void Invoke2(SObject o, Dictionary<Type, __OutHandle> record)
    {
        if (record.ContainsKey(typeof(Out<T, T2, T3, T4, T5>))) return;
        if (!o.Entity.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!o.Entity.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.Entity.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.Entity.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!o.Entity.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        record.Add(typeof(Out<T, T2, T3, T4, T5>), new Out<T, T2, T3, T4, T5>(c, c2, c3, c4, c5));
    }
    internal override void Invoke(SObject o) => o.World.Event.RunEvent(this);
}
public class Out<T, T2, T3, T4, T5, T6> : __OutHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent
{
    public Out(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }

    internal static void Invoke(Type type, SComponent c0)
    {
        T c;T2 c2;T3 c3;T4 c4;T5 c5;T6 c6;
        if (typeof(T) == type) c = c0 as T; else { if (!c0.Entity.TryGetComponent(out c) || !c.Enable) return; }
        if (typeof(T2) == type) c2 = c0 as T2; else { if (!c0.Entity.TryGetComponent(out c2) || !c2.Enable) return; }
        if (typeof(T3) == type) c3 = c0 as T3; else { if (!c0.Entity.TryGetComponent(out c3) || !c3.Enable) return; }
        if (typeof(T4) == type) c4 = c0 as T4; else { if (!c0.Entity.TryGetComponent(out c4) || !c4.Enable) return; }
        if (typeof(T5) == type) c5 = c0 as T5; else { if (!c0.Entity.TryGetComponent(out c5) || !c5.Enable) return; }
        if (typeof(T6) == type) c6 = c0 as T6; else { if (!c0.Entity.TryGetComponent(out c6) || !c6.Enable) return; }
        c0.Entity.World.Event.RunEvent(new Out<T, T2, T3, T4, T5, T6>(c, c2, c3, c4, c5, c6));
    }
    internal static void Invoke2(SObject o, Dictionary<Type, __OutHandle> record)
    {
        if (record.ContainsKey(typeof(Out<T, T2, T3, T4, T5, T6>))) return;
        if (!o.Entity.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!o.Entity.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.Entity.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.Entity.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!o.Entity.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        if (!o.Entity.TryGetComponent<T6>(out var c6) || !c6.Enable) return;
        record.Add(typeof(Out<T, T2, T3, T4, T5, T6>), new Out<T, T2, T3, T4, T5, T6>(c, c2, c3, c4, c5, c6));
    }
    internal override void Invoke(SObject o) => o.World.Event.RunEvent(this);
}
public class Out<T, T2, T3, T4, T5, T6, T7> : __OutHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent
{
    public Out(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }
    public T7 t7 { get; }

    internal static void Invoke(Type type, SComponent c0)
    {
        T c;T2 c2;T3 c3;T4 c4;T5 c5;T6 c6;T7 c7;
        if (typeof(T) == type) c = c0 as T; else { if (!c0.Entity.TryGetComponent(out c) || !c.Enable) return; }
        if (typeof(T2) == type) c2 = c0 as T2; else { if (!c0.Entity.TryGetComponent(out c2) || !c2.Enable) return; }
        if (typeof(T3) == type) c3 = c0 as T3; else { if (!c0.Entity.TryGetComponent(out c3) || !c3.Enable) return; }
        if (typeof(T4) == type) c4 = c0 as T4; else { if (!c0.Entity.TryGetComponent(out c4) || !c4.Enable) return; }
        if (typeof(T5) == type) c5 = c0 as T5; else { if (!c0.Entity.TryGetComponent(out c5) || !c5.Enable) return; }
        if (typeof(T6) == type) c6 = c0 as T6; else { if (!c0.Entity.TryGetComponent(out c6) || !c6.Enable) return; }
        if (typeof(T7) == type) c7 = c0 as T7; else { if (!c0.Entity.TryGetComponent(out c7) || !c7.Enable) return; }
        c0.Entity.World.Event.RunEvent(new Out<T, T2, T3, T4, T5, T6, T7>(c, c2, c3, c4, c5, c6, c7));
    }
    internal static void Invoke2(SObject o, Dictionary<Type, __OutHandle> record)
    {
        if (record.ContainsKey(typeof(Out<T, T2, T3, T4, T5, T6, T7>))) return;
        if (!o.Entity.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!o.Entity.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.Entity.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.Entity.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!o.Entity.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        if (!o.Entity.TryGetComponent<T6>(out var c6) || !c6.Enable) return;
        if (!o.Entity.TryGetComponent<T7>(out var c7) || !c7.Enable) return;
        record.Add(typeof(Out<T, T2, T3, T4, T5, T6, T7>), new Out<T, T2, T3, T4, T5, T6, T7>(c, c2, c3, c4, c5, c6, c7));
    }
    internal override void Invoke(SObject o) => o.World.Event.RunEvent(this);
}
public class Out<T, T2, T3, T4, T5, T6, T7, T8> : __OutHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent
{
    public Out(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }
    public T7 t7 { get; }
    public T8 t8 { get; }

    internal static void Invoke(Type type, SComponent c0)
    {
        T c;T2 c2;T3 c3;T4 c4;T5 c5;T6 c6;T7 c7;T8 c8;
        if (typeof(T) == type) c = c0 as T; else { if (!c0.Entity.TryGetComponent(out c) || !c.Enable) return; }
        if (typeof(T2) == type) c2 = c0 as T2; else { if (!c0.Entity.TryGetComponent(out c2) || !c2.Enable) return; }
        if (typeof(T3) == type) c3 = c0 as T3; else { if (!c0.Entity.TryGetComponent(out c3) || !c3.Enable) return; }
        if (typeof(T4) == type) c4 = c0 as T4; else { if (!c0.Entity.TryGetComponent(out c4) || !c4.Enable) return; }
        if (typeof(T5) == type) c5 = c0 as T5; else { if (!c0.Entity.TryGetComponent(out c5) || !c5.Enable) return; }
        if (typeof(T6) == type) c6 = c0 as T6; else { if (!c0.Entity.TryGetComponent(out c6) || !c6.Enable) return; }
        if (typeof(T7) == type) c7 = c0 as T7; else { if (!c0.Entity.TryGetComponent(out c7) || !c7.Enable) return; }
        if (typeof(T8) == type) c8 = c0 as T8; else { if (!c0.Entity.TryGetComponent(out c8) || !c8.Enable) return; }
        c0.Entity.World.Event.RunEvent(new Out<T, T2, T3, T4, T5, T6, T7, T8>(c, c2, c3, c4, c5, c6, c7, c8));
    }
    internal static void Invoke2(SObject o, Dictionary<Type, __OutHandle> record)
    {
        if (record.ContainsKey(typeof(Out<T, T2, T3, T4, T5, T6, T7, T8>))) return;
        if (!o.Entity.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!o.Entity.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.Entity.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.Entity.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!o.Entity.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        if (!o.Entity.TryGetComponent<T6>(out var c6) || !c6.Enable) return;
        if (!o.Entity.TryGetComponent<T7>(out var c7) || !c7.Enable) return;
        if (!o.Entity.TryGetComponent<T8>(out var c8) || !c8.Enable) return;
        record.Add(typeof(Out<T, T2, T3, T4, T5, T6, T7, T8>), new Out<T, T2, T3, T4, T5, T6, T7, T8>(c, c2, c3, c4, c5, c6, c7, c8));
    }
    internal override void Invoke(SObject o) => o.World.Event.RunEvent(this);
}
public class Out<T, T2, T3, T4, T5, T6, T7, T8, T9> : __OutHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent
{
    public Out(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; this.t9 = t9; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }
    public T7 t7 { get; }
    public T8 t8 { get; }
    public T9 t9 { get; }

    internal static void Invoke(Type type, SComponent c0)
    {
        T c;T2 c2;T3 c3;T4 c4;T5 c5;T6 c6;T7 c7;T8 c8;T9 c9;
        if (typeof(T) == type) c = c0 as T; else { if (!c0.Entity.TryGetComponent(out c) || !c.Enable) return; }
        if (typeof(T2) == type) c2 = c0 as T2; else { if (!c0.Entity.TryGetComponent(out c2) || !c2.Enable) return; }
        if (typeof(T3) == type) c3 = c0 as T3; else { if (!c0.Entity.TryGetComponent(out c3) || !c3.Enable) return; }
        if (typeof(T4) == type) c4 = c0 as T4; else { if (!c0.Entity.TryGetComponent(out c4) || !c4.Enable) return; }
        if (typeof(T5) == type) c5 = c0 as T5; else { if (!c0.Entity.TryGetComponent(out c5) || !c5.Enable) return; }
        if (typeof(T6) == type) c6 = c0 as T6; else { if (!c0.Entity.TryGetComponent(out c6) || !c6.Enable) return; }
        if (typeof(T7) == type) c7 = c0 as T7; else { if (!c0.Entity.TryGetComponent(out c7) || !c7.Enable) return; }
        if (typeof(T8) == type) c8 = c0 as T8; else { if (!c0.Entity.TryGetComponent(out c8) || !c8.Enable) return; }
        if (typeof(T9) == type) c9 = c0 as T9; else { if (!c0.Entity.TryGetComponent(out c9) || !c9.Enable) return; }
        c0.Entity.World.Event.RunEvent(new Out<T, T2, T3, T4, T5, T6, T7, T8, T9>(c, c2, c3, c4, c5, c6, c7, c8, c9));
    }
    internal static void Invoke2(SObject o, Dictionary<Type, __OutHandle> record)
    {
        if (record.ContainsKey(typeof(Out<T, T2, T3, T4, T5, T6, T7, T8, T9>))) return;
        if (!o.Entity.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!o.Entity.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.Entity.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.Entity.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!o.Entity.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        if (!o.Entity.TryGetComponent<T6>(out var c6) || !c6.Enable) return;
        if (!o.Entity.TryGetComponent<T7>(out var c7) || !c7.Enable) return;
        if (!o.Entity.TryGetComponent<T8>(out var c8) || !c8.Enable) return;
        if (!o.Entity.TryGetComponent<T9>(out var c9) || !c9.Enable) return;
        record.Add(typeof(Out<T, T2, T3, T4, T5, T6, T7, T8, T9>), new Out<T, T2, T3, T4, T5, T6, T7, T8, T9>(c, c2, c3, c4, c5, c6, c7, c8, c9));
    }
    internal override void Invoke(SObject o) => o.World.Event.RunEvent(this);
}
public class Out<T, T2, T3, T4, T5, T6, T7, T8, T9, T10> : __OutHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent where T10 : SComponent
{
    public Out(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; this.t9 = t9; this.t10 = t10; }
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

    internal static void Invoke(Type type, SComponent c0)
    {
        T c;T2 c2;T3 c3;T4 c4;T5 c5;T6 c6;T7 c7;T8 c8;T9 c9;T10 c10;
        if (typeof(T) == type) c = c0 as T; else { if (!c0.Entity.TryGetComponent(out c) || !c.Enable) return; }
        if (typeof(T2) == type) c2 = c0 as T2; else { if (!c0.Entity.TryGetComponent(out c2) || !c2.Enable) return; }
        if (typeof(T3) == type) c3 = c0 as T3; else { if (!c0.Entity.TryGetComponent(out c3) || !c3.Enable) return; }
        if (typeof(T4) == type) c4 = c0 as T4; else { if (!c0.Entity.TryGetComponent(out c4) || !c4.Enable) return; }
        if (typeof(T5) == type) c5 = c0 as T5; else { if (!c0.Entity.TryGetComponent(out c5) || !c5.Enable) return; }
        if (typeof(T6) == type) c6 = c0 as T6; else { if (!c0.Entity.TryGetComponent(out c6) || !c6.Enable) return; }
        if (typeof(T7) == type) c7 = c0 as T7; else { if (!c0.Entity.TryGetComponent(out c7) || !c7.Enable) return; }
        if (typeof(T8) == type) c8 = c0 as T8; else { if (!c0.Entity.TryGetComponent(out c8) || !c8.Enable) return; }
        if (typeof(T9) == type) c9 = c0 as T9; else { if (!c0.Entity.TryGetComponent(out c9) || !c9.Enable) return; }
        if (typeof(T10) == type) c10 = c0 as T10; else { if (!c0.Entity.TryGetComponent(out c10) || !c10.Enable) return; }
        c0.Entity.World.Event.RunEvent(new Out<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>(c, c2, c3, c4, c5, c6, c7, c8, c9, c10));
    }
    internal static void Invoke2(SObject o, Dictionary<Type, __OutHandle> record)
    {
        if (record.ContainsKey(typeof(Out<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>))) return;
        if (!o.Entity.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!o.Entity.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.Entity.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.Entity.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!o.Entity.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        if (!o.Entity.TryGetComponent<T6>(out var c6) || !c6.Enable) return;
        if (!o.Entity.TryGetComponent<T7>(out var c7) || !c7.Enable) return;
        if (!o.Entity.TryGetComponent<T8>(out var c8) || !c8.Enable) return;
        if (!o.Entity.TryGetComponent<T9>(out var c9) || !c9.Enable) return;
        if (!o.Entity.TryGetComponent<T10>(out var c10) || !c10.Enable) return;
        record.Add(typeof(Out<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>), new Out<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>(c, c2, c3, c4, c5, c6, c7, c8, c9, c10));
    }
    internal override void Invoke(SObject o) => o.World.Event.RunEvent(this);
}
