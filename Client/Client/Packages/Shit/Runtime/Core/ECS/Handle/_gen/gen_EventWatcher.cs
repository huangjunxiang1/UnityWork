using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EventWatcher<T, T2> : __EventWatcher where T : class where T2 : SComponent
{
    public EventWatcher(T t, T2 t2) { this.t = t; this.t2 = t2; }
    public T t { get; }
    public T2 t2 { get; }

    internal static void Invoke(object e, SObject o, int type)
    {
        if (!o.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        o.World.Event.RunEvent(new EventWatcher<T, T2>((T)e, c2), type: type);
    }
}
public class EventWatcher<T, T2, T3> : __EventWatcher where T : class where T2 : SComponent where T3 : SComponent
{
    public EventWatcher(T t, T2 t2, T3 t3) { this.t = t; this.t2 = t2; this.t3 = t3; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }

    internal static void Invoke(object e, SObject o, int type)
    {
        if (!o.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        o.World.Event.RunEvent(new EventWatcher<T, T2, T3>((T)e, c2, c3), type: type);
    }
}
public class EventWatcher<T, T2, T3, T4> : __EventWatcher where T : class where T2 : SComponent where T3 : SComponent where T4 : SComponent
{
    public EventWatcher(T t, T2 t2, T3 t3, T4 t4) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }

    internal static void Invoke(object e, SObject o, int type)
    {
        if (!o.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        o.World.Event.RunEvent(new EventWatcher<T, T2, T3, T4>((T)e, c2, c3, c4), type: type);
    }
}
public class EventWatcher<T, T2, T3, T4, T5> : __EventWatcher where T : class where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent
{
    public EventWatcher(T t, T2 t2, T3 t3, T4 t4, T5 t5) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }

    internal static void Invoke(object e, SObject o, int type)
    {
        if (!o.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!o.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        o.World.Event.RunEvent(new EventWatcher<T, T2, T3, T4, T5>((T)e, c2, c3, c4, c5), type: type);
    }
}
public class EventWatcher<T, T2, T3, T4, T5, T6> : __EventWatcher where T : class where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent
{
    public EventWatcher(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }

    internal static void Invoke(object e, SObject o, int type)
    {
        if (!o.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!o.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        if (!o.TryGetComponent<T6>(out var c6) || !c6.Enable) return;
        o.World.Event.RunEvent(new EventWatcher<T, T2, T3, T4, T5, T6>((T)e, c2, c3, c4, c5, c6), type: type);
    }
}
public class EventWatcher<T, T2, T3, T4, T5, T6, T7> : __EventWatcher where T : class where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent
{
    public EventWatcher(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }
    public T7 t7 { get; }

    internal static void Invoke(object e, SObject o, int type)
    {
        if (!o.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!o.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        if (!o.TryGetComponent<T6>(out var c6) || !c6.Enable) return;
        if (!o.TryGetComponent<T7>(out var c7) || !c7.Enable) return;
        o.World.Event.RunEvent(new EventWatcher<T, T2, T3, T4, T5, T6, T7>((T)e, c2, c3, c4, c5, c6, c7), type: type);
    }
}
public class EventWatcher<T, T2, T3, T4, T5, T6, T7, T8> : __EventWatcher where T : class where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent
{
    public EventWatcher(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }
    public T7 t7 { get; }
    public T8 t8 { get; }

    internal static void Invoke(object e, SObject o, int type)
    {
        if (!o.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!o.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        if (!o.TryGetComponent<T6>(out var c6) || !c6.Enable) return;
        if (!o.TryGetComponent<T7>(out var c7) || !c7.Enable) return;
        if (!o.TryGetComponent<T8>(out var c8) || !c8.Enable) return;
        o.World.Event.RunEvent(new EventWatcher<T, T2, T3, T4, T5, T6, T7, T8>((T)e, c2, c3, c4, c5, c6, c7, c8), type: type);
    }
}
public class EventWatcher<T, T2, T3, T4, T5, T6, T7, T8, T9> : __EventWatcher where T : class where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent
{
    public EventWatcher(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; this.t9 = t9; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }
    public T7 t7 { get; }
    public T8 t8 { get; }
    public T9 t9 { get; }

    internal static void Invoke(object e, SObject o, int type)
    {
        if (!o.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!o.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        if (!o.TryGetComponent<T6>(out var c6) || !c6.Enable) return;
        if (!o.TryGetComponent<T7>(out var c7) || !c7.Enable) return;
        if (!o.TryGetComponent<T8>(out var c8) || !c8.Enable) return;
        if (!o.TryGetComponent<T9>(out var c9) || !c9.Enable) return;
        o.World.Event.RunEvent(new EventWatcher<T, T2, T3, T4, T5, T6, T7, T8, T9>((T)e, c2, c3, c4, c5, c6, c7, c8, c9), type: type);
    }
}
public class EventWatcher<T, T2, T3, T4, T5, T6, T7, T8, T9, T10> : __EventWatcher where T : class where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent where T10 : SComponent
{
    public EventWatcher(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; this.t9 = t9; this.t10 = t10; }
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

    internal static void Invoke(object e, SObject o, int type)
    {
        if (!o.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!o.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!o.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!o.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        if (!o.TryGetComponent<T6>(out var c6) || !c6.Enable) return;
        if (!o.TryGetComponent<T7>(out var c7) || !c7.Enable) return;
        if (!o.TryGetComponent<T8>(out var c8) || !c8.Enable) return;
        if (!o.TryGetComponent<T9>(out var c9) || !c9.Enable) return;
        if (!o.TryGetComponent<T10>(out var c10) || !c10.Enable) return;
        o.World.Event.RunEvent(new EventWatcher<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>((T)e, c2, c3, c4, c5, c6, c7, c8, c9, c10), type: type);
    }
}
