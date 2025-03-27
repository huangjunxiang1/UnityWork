using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class In<T> : __InHandle where T : SComponent
{
    public In(T t) { this.t = t; }
    public T t { get; }

    internal static void Invoke(SObject o)
    {
        if (!o.World.Event.HasEvent(typeof(In<T>))) return;

        if (!o.TryGetComponent<T>(out var c) || !c.Enable) return;
 #if UNITY_EDITOR
        o.World.Event.GetEventQueue(typeof(In<T>), out var v);
        o._In.AddRange(v);
 #endif
        o.World.Event.RunEventNoGCAndFaster(new In<T>(c));
    }
}
public class In<T, T2> : __InHandle where T : SComponent where T2 : SComponent
{
    public In(T t, T2 t2) { this.t = t; this.t2 = t2; }
    public T t { get; }
    public T2 t2 { get; }

    internal static void Invoke(SObject o)
    {
        if (!o.World.Event.HasEvent(typeof(In<T, T2>))) return;

        if (!o.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!o.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
 #if UNITY_EDITOR
        o.World.Event.GetEventQueue(typeof(In<T, T2>), out var v);
        o._In.AddRange(v);
 #endif
        o.World.Event.RunEventNoGCAndFaster(new In<T, T2>(c, c2));
    }
}
public class In<T, T2, T3> : __InHandle where T : SComponent where T2 : SComponent where T3 : SComponent
{
    public In(T t, T2 t2, T3 t3) { this.t = t; this.t2 = t2; this.t3 = t3; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }

    internal static void Invoke(SObject o)
    {
        if (!o.World.Event.HasEvent(typeof(In<T, T2, T3>))) return;

        if (!o.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!o.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
 #if UNITY_EDITOR
        o.World.Event.GetEventQueue(typeof(In<T, T2, T3>), out var v);
        o._In.AddRange(v);
 #endif
        o.World.Event.RunEventNoGCAndFaster(new In<T, T2, T3>(c, c2, c3));
    }
}
public class In<T, T2, T3, T4> : __InHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent
{
    public In(T t, T2 t2, T3 t3, T4 t4) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }

    internal static void Invoke(SObject o)
    {
        if (!o.World.Event.HasEvent(typeof(In<T, T2, T3, T4>))) return;

        if (!o.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!o.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
 #if UNITY_EDITOR
        o.World.Event.GetEventQueue(typeof(In<T, T2, T3, T4>), out var v);
        o._In.AddRange(v);
 #endif
        o.World.Event.RunEventNoGCAndFaster(new In<T, T2, T3, T4>(c, c2, c3, c4));
    }
}
public class In<T, T2, T3, T4, T5> : __InHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent
{
    public In(T t, T2 t2, T3 t3, T4 t4, T5 t5) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }

    internal static void Invoke(SObject o)
    {
        if (!o.World.Event.HasEvent(typeof(In<T, T2, T3, T4, T5>))) return;

        if (!o.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!o.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!o.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
 #if UNITY_EDITOR
        o.World.Event.GetEventQueue(typeof(In<T, T2, T3, T4, T5>), out var v);
        o._In.AddRange(v);
 #endif
        o.World.Event.RunEventNoGCAndFaster(new In<T, T2, T3, T4, T5>(c, c2, c3, c4, c5));
    }
}
public class In<T, T2, T3, T4, T5, T6> : __InHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent
{
    public In(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }

    internal static void Invoke(SObject o)
    {
        if (!o.World.Event.HasEvent(typeof(In<T, T2, T3, T4, T5, T6>))) return;

        if (!o.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!o.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!o.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        if (!o.TryGetComponent<T6>(out var c6) || !c6.Enable) return;
 #if UNITY_EDITOR
        o.World.Event.GetEventQueue(typeof(In<T, T2, T3, T4, T5, T6>), out var v);
        o._In.AddRange(v);
 #endif
        o.World.Event.RunEventNoGCAndFaster(new In<T, T2, T3, T4, T5, T6>(c, c2, c3, c4, c5, c6));
    }
}
public class In<T, T2, T3, T4, T5, T6, T7> : __InHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent
{
    public In(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }
    public T7 t7 { get; }

    internal static void Invoke(SObject o)
    {
        if (!o.World.Event.HasEvent(typeof(In<T, T2, T3, T4, T5, T6, T7>))) return;

        if (!o.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!o.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!o.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        if (!o.TryGetComponent<T6>(out var c6) || !c6.Enable) return;
        if (!o.TryGetComponent<T7>(out var c7) || !c7.Enable) return;
 #if UNITY_EDITOR
        o.World.Event.GetEventQueue(typeof(In<T, T2, T3, T4, T5, T6, T7>), out var v);
        o._In.AddRange(v);
 #endif
        o.World.Event.RunEventNoGCAndFaster(new In<T, T2, T3, T4, T5, T6, T7>(c, c2, c3, c4, c5, c6, c7));
    }
}
public class In<T, T2, T3, T4, T5, T6, T7, T8> : __InHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent
{
    public In(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }
    public T7 t7 { get; }
    public T8 t8 { get; }

    internal static void Invoke(SObject o)
    {
        if (!o.World.Event.HasEvent(typeof(In<T, T2, T3, T4, T5, T6, T7, T8>))) return;

        if (!o.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!o.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!o.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        if (!o.TryGetComponent<T6>(out var c6) || !c6.Enable) return;
        if (!o.TryGetComponent<T7>(out var c7) || !c7.Enable) return;
        if (!o.TryGetComponent<T8>(out var c8) || !c8.Enable) return;
 #if UNITY_EDITOR
        o.World.Event.GetEventQueue(typeof(In<T, T2, T3, T4, T5, T6, T7, T8>), out var v);
        o._In.AddRange(v);
 #endif
        o.World.Event.RunEventNoGCAndFaster(new In<T, T2, T3, T4, T5, T6, T7, T8>(c, c2, c3, c4, c5, c6, c7, c8));
    }
}
public class In<T, T2, T3, T4, T5, T6, T7, T8, T9> : __InHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent
{
    public In(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; this.t9 = t9; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }
    public T7 t7 { get; }
    public T8 t8 { get; }
    public T9 t9 { get; }

    internal static void Invoke(SObject o)
    {
        if (!o.World.Event.HasEvent(typeof(In<T, T2, T3, T4, T5, T6, T7, T8, T9>))) return;

        if (!o.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!o.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!o.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        if (!o.TryGetComponent<T6>(out var c6) || !c6.Enable) return;
        if (!o.TryGetComponent<T7>(out var c7) || !c7.Enable) return;
        if (!o.TryGetComponent<T8>(out var c8) || !c8.Enable) return;
        if (!o.TryGetComponent<T9>(out var c9) || !c9.Enable) return;
 #if UNITY_EDITOR
        o.World.Event.GetEventQueue(typeof(In<T, T2, T3, T4, T5, T6, T7, T8, T9>), out var v);
        o._In.AddRange(v);
 #endif
        o.World.Event.RunEventNoGCAndFaster(new In<T, T2, T3, T4, T5, T6, T7, T8, T9>(c, c2, c3, c4, c5, c6, c7, c8, c9));
    }
}
public class In<T, T2, T3, T4, T5, T6, T7, T8, T9, T10> : __InHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent where T10 : SComponent
{
    public In(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; this.t9 = t9; this.t10 = t10; }
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

    internal static void Invoke(SObject o)
    {
        if (!o.World.Event.HasEvent(typeof(In<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>))) return;

        if (!o.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!o.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!o.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        if (!o.TryGetComponent<T6>(out var c6) || !c6.Enable) return;
        if (!o.TryGetComponent<T7>(out var c7) || !c7.Enable) return;
        if (!o.TryGetComponent<T8>(out var c8) || !c8.Enable) return;
        if (!o.TryGetComponent<T9>(out var c9) || !c9.Enable) return;
        if (!o.TryGetComponent<T10>(out var c10) || !c10.Enable) return;
 #if UNITY_EDITOR
        o.World.Event.GetEventQueue(typeof(In<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>), out var v);
        o._In.AddRange(v);
 #endif
        o.World.Event.RunEventNoGCAndFaster(new In<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>(c, c2, c3, c4, c5, c6, c7, c8, c9, c10));
    }
}
