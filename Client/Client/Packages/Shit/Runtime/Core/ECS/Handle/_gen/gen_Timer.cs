using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Timer<T> : __Timer where T : SComponent
{
    Action<Timer<T>> action;
    static List<TimerItem> timers;
    internal override bool Disposed => this.t.Disposed;
    public Timer(T t) { this.t = t; }
    public T t { get; }

    internal static void SetTimerList(List<TimerItem> lst) => timers = lst;
    internal static void TryCreateHandle(SObject o)
    {
        for (int i = 0; i < timers.Count; i++)
        {
            var ti = timers[i];
            if (ti.attribute.delay < 0 || ti.attribute.count == 0 || ti.attribute.count < -1)
                Loger.Error($"Timer 参数错误 {ti.action.Method.ReflectedType.FullName} {ti.action.Method.Name}");

            if (!o.TryGetComponent<T>(out var c)) continue;
            var v = new Timer<T>(c);
            v.action = (Action<Timer<T>>)ti.action;
            v.obj = o;
            o.World.Timer.Add(ti.attribute.delay, ti.attribute.count, v.Invoke);
#if UNITY_EDITOR
            o._timers.Add(v);
#endif
        }
    }
    void Invoke()
    {
        if (this.Disposed)
        {
            this.obj.World.Timer.Remove(this.Invoke);
            return;
        }
        if (!t.Enable) return;
        action.Invoke(this);
    }
}
public class Timer<T, T2> : __Timer where T : SComponent where T2 : SComponent
{
    Action<Timer<T, T2>> action;
    static List<TimerItem> timers;
    internal override bool Disposed => this.t.Disposed || this.t2.Disposed;
    public Timer(T t, T2 t2) { this.t = t; this.t2 = t2; }
    public T t { get; }
    public T2 t2 { get; }

    internal static void SetTimerList(List<TimerItem> lst) => timers = lst;
    internal static void TryCreateHandle(SObject o)
    {
        for (int i = 0; i < timers.Count; i++)
        {
            var ti = timers[i];
            if (ti.attribute.delay < 0 || ti.attribute.count == 0 || ti.attribute.count < -1)
                Loger.Error($"Timer 参数错误 {ti.action.Method.ReflectedType.FullName} {ti.action.Method.Name}");

            if (!o.TryGetComponent<T>(out var c)) continue;
            if (!o.TryGetComponent<T2>(out var c2)) continue;
            var v = new Timer<T, T2>(c, c2);
            v.action = (Action<Timer<T, T2>>)ti.action;
            v.obj = o;
            o.World.Timer.Add(ti.attribute.delay, ti.attribute.count, v.Invoke);
#if UNITY_EDITOR
            o._timers.Add(v);
#endif
        }
    }
    void Invoke()
    {
        if (this.Disposed)
        {
            this.obj.World.Timer.Remove(this.Invoke);
            return;
        }
        if (!t.Enable || !t2.Enable) return;
        action.Invoke(this);
    }
}
public class Timer<T, T2, T3> : __Timer where T : SComponent where T2 : SComponent where T3 : SComponent
{
    Action<Timer<T, T2, T3>> action;
    static List<TimerItem> timers;
    internal override bool Disposed => this.t.Disposed || this.t2.Disposed || this.t3.Disposed;
    public Timer(T t, T2 t2, T3 t3) { this.t = t; this.t2 = t2; this.t3 = t3; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }

    internal static void SetTimerList(List<TimerItem> lst) => timers = lst;
    internal static void TryCreateHandle(SObject o)
    {
        for (int i = 0; i < timers.Count; i++)
        {
            var ti = timers[i];
            if (ti.attribute.delay < 0 || ti.attribute.count == 0 || ti.attribute.count < -1)
                Loger.Error($"Timer 参数错误 {ti.action.Method.ReflectedType.FullName} {ti.action.Method.Name}");

            if (!o.TryGetComponent<T>(out var c)) continue;
            if (!o.TryGetComponent<T2>(out var c2)) continue;
            if (!o.TryGetComponent<T3>(out var c3)) continue;
            var v = new Timer<T, T2, T3>(c, c2, c3);
            v.action = (Action<Timer<T, T2, T3>>)ti.action;
            v.obj = o;
            o.World.Timer.Add(ti.attribute.delay, ti.attribute.count, v.Invoke);
#if UNITY_EDITOR
            o._timers.Add(v);
#endif
        }
    }
    void Invoke()
    {
        if (this.Disposed)
        {
            this.obj.World.Timer.Remove(this.Invoke);
            return;
        }
        if (!t.Enable || !t2.Enable || !t3.Enable) return;
        action.Invoke(this);
    }
}
public class Timer<T, T2, T3, T4> : __Timer where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent
{
    Action<Timer<T, T2, T3, T4>> action;
    static List<TimerItem> timers;
    internal override bool Disposed => this.t.Disposed || this.t2.Disposed || this.t3.Disposed || this.t4.Disposed;
    public Timer(T t, T2 t2, T3 t3, T4 t4) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }

    internal static void SetTimerList(List<TimerItem> lst) => timers = lst;
    internal static void TryCreateHandle(SObject o)
    {
        for (int i = 0; i < timers.Count; i++)
        {
            var ti = timers[i];
            if (ti.attribute.delay < 0 || ti.attribute.count == 0 || ti.attribute.count < -1)
                Loger.Error($"Timer 参数错误 {ti.action.Method.ReflectedType.FullName} {ti.action.Method.Name}");

            if (!o.TryGetComponent<T>(out var c)) continue;
            if (!o.TryGetComponent<T2>(out var c2)) continue;
            if (!o.TryGetComponent<T3>(out var c3)) continue;
            if (!o.TryGetComponent<T4>(out var c4)) continue;
            var v = new Timer<T, T2, T3, T4>(c, c2, c3, c4);
            v.action = (Action<Timer<T, T2, T3, T4>>)ti.action;
            v.obj = o;
            o.World.Timer.Add(ti.attribute.delay, ti.attribute.count, v.Invoke);
#if UNITY_EDITOR
            o._timers.Add(v);
#endif
        }
    }
    void Invoke()
    {
        if (this.Disposed)
        {
            this.obj.World.Timer.Remove(this.Invoke);
            return;
        }
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable) return;
        action.Invoke(this);
    }
}
public class Timer<T, T2, T3, T4, T5> : __Timer where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent
{
    Action<Timer<T, T2, T3, T4, T5>> action;
    static List<TimerItem> timers;
    internal override bool Disposed => this.t.Disposed || this.t2.Disposed || this.t3.Disposed || this.t4.Disposed || this.t5.Disposed;
    public Timer(T t, T2 t2, T3 t3, T4 t4, T5 t5) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }

    internal static void SetTimerList(List<TimerItem> lst) => timers = lst;
    internal static void TryCreateHandle(SObject o)
    {
        for (int i = 0; i < timers.Count; i++)
        {
            var ti = timers[i];
            if (ti.attribute.delay < 0 || ti.attribute.count == 0 || ti.attribute.count < -1)
                Loger.Error($"Timer 参数错误 {ti.action.Method.ReflectedType.FullName} {ti.action.Method.Name}");

            if (!o.TryGetComponent<T>(out var c)) continue;
            if (!o.TryGetComponent<T2>(out var c2)) continue;
            if (!o.TryGetComponent<T3>(out var c3)) continue;
            if (!o.TryGetComponent<T4>(out var c4)) continue;
            if (!o.TryGetComponent<T5>(out var c5)) continue;
            var v = new Timer<T, T2, T3, T4, T5>(c, c2, c3, c4, c5);
            v.action = (Action<Timer<T, T2, T3, T4, T5>>)ti.action;
            v.obj = o;
            o.World.Timer.Add(ti.attribute.delay, ti.attribute.count, v.Invoke);
#if UNITY_EDITOR
            o._timers.Add(v);
#endif
        }
    }
    void Invoke()
    {
        if (this.Disposed)
        {
            this.obj.World.Timer.Remove(this.Invoke);
            return;
        }
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable) return;
        action.Invoke(this);
    }
}
public class Timer<T, T2, T3, T4, T5, T6> : __Timer where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent
{
    Action<Timer<T, T2, T3, T4, T5, T6>> action;
    static List<TimerItem> timers;
    internal override bool Disposed => this.t.Disposed || this.t2.Disposed || this.t3.Disposed || this.t4.Disposed || this.t5.Disposed || this.t6.Disposed;
    public Timer(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }

    internal static void SetTimerList(List<TimerItem> lst) => timers = lst;
    internal static void TryCreateHandle(SObject o)
    {
        for (int i = 0; i < timers.Count; i++)
        {
            var ti = timers[i];
            if (ti.attribute.delay < 0 || ti.attribute.count == 0 || ti.attribute.count < -1)
                Loger.Error($"Timer 参数错误 {ti.action.Method.ReflectedType.FullName} {ti.action.Method.Name}");

            if (!o.TryGetComponent<T>(out var c)) continue;
            if (!o.TryGetComponent<T2>(out var c2)) continue;
            if (!o.TryGetComponent<T3>(out var c3)) continue;
            if (!o.TryGetComponent<T4>(out var c4)) continue;
            if (!o.TryGetComponent<T5>(out var c5)) continue;
            if (!o.TryGetComponent<T6>(out var c6)) continue;
            var v = new Timer<T, T2, T3, T4, T5, T6>(c, c2, c3, c4, c5, c6);
            v.action = (Action<Timer<T, T2, T3, T4, T5, T6>>)ti.action;
            v.obj = o;
            o.World.Timer.Add(ti.attribute.delay, ti.attribute.count, v.Invoke);
#if UNITY_EDITOR
            o._timers.Add(v);
#endif
        }
    }
    void Invoke()
    {
        if (this.Disposed)
        {
            this.obj.World.Timer.Remove(this.Invoke);
            return;
        }
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable || !t6.Enable) return;
        action.Invoke(this);
    }
}
public class Timer<T, T2, T3, T4, T5, T6, T7> : __Timer where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent
{
    Action<Timer<T, T2, T3, T4, T5, T6, T7>> action;
    static List<TimerItem> timers;
    internal override bool Disposed => this.t.Disposed || this.t2.Disposed || this.t3.Disposed || this.t4.Disposed || this.t5.Disposed || this.t6.Disposed || this.t7.Disposed;
    public Timer(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }
    public T7 t7 { get; }

    internal static void SetTimerList(List<TimerItem> lst) => timers = lst;
    internal static void TryCreateHandle(SObject o)
    {
        for (int i = 0; i < timers.Count; i++)
        {
            var ti = timers[i];
            if (ti.attribute.delay < 0 || ti.attribute.count == 0 || ti.attribute.count < -1)
                Loger.Error($"Timer 参数错误 {ti.action.Method.ReflectedType.FullName} {ti.action.Method.Name}");

            if (!o.TryGetComponent<T>(out var c)) continue;
            if (!o.TryGetComponent<T2>(out var c2)) continue;
            if (!o.TryGetComponent<T3>(out var c3)) continue;
            if (!o.TryGetComponent<T4>(out var c4)) continue;
            if (!o.TryGetComponent<T5>(out var c5)) continue;
            if (!o.TryGetComponent<T6>(out var c6)) continue;
            if (!o.TryGetComponent<T7>(out var c7)) continue;
            var v = new Timer<T, T2, T3, T4, T5, T6, T7>(c, c2, c3, c4, c5, c6, c7);
            v.action = (Action<Timer<T, T2, T3, T4, T5, T6, T7>>)ti.action;
            v.obj = o;
            o.World.Timer.Add(ti.attribute.delay, ti.attribute.count, v.Invoke);
#if UNITY_EDITOR
            o._timers.Add(v);
#endif
        }
    }
    void Invoke()
    {
        if (this.Disposed)
        {
            this.obj.World.Timer.Remove(this.Invoke);
            return;
        }
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable || !t6.Enable || !t7.Enable) return;
        action.Invoke(this);
    }
}
public class Timer<T, T2, T3, T4, T5, T6, T7, T8> : __Timer where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent
{
    Action<Timer<T, T2, T3, T4, T5, T6, T7, T8>> action;
    static List<TimerItem> timers;
    internal override bool Disposed => this.t.Disposed || this.t2.Disposed || this.t3.Disposed || this.t4.Disposed || this.t5.Disposed || this.t6.Disposed || this.t7.Disposed || this.t8.Disposed;
    public Timer(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }
    public T7 t7 { get; }
    public T8 t8 { get; }

    internal static void SetTimerList(List<TimerItem> lst) => timers = lst;
    internal static void TryCreateHandle(SObject o)
    {
        for (int i = 0; i < timers.Count; i++)
        {
            var ti = timers[i];
            if (ti.attribute.delay < 0 || ti.attribute.count == 0 || ti.attribute.count < -1)
                Loger.Error($"Timer 参数错误 {ti.action.Method.ReflectedType.FullName} {ti.action.Method.Name}");

            if (!o.TryGetComponent<T>(out var c)) continue;
            if (!o.TryGetComponent<T2>(out var c2)) continue;
            if (!o.TryGetComponent<T3>(out var c3)) continue;
            if (!o.TryGetComponent<T4>(out var c4)) continue;
            if (!o.TryGetComponent<T5>(out var c5)) continue;
            if (!o.TryGetComponent<T6>(out var c6)) continue;
            if (!o.TryGetComponent<T7>(out var c7)) continue;
            if (!o.TryGetComponent<T8>(out var c8)) continue;
            var v = new Timer<T, T2, T3, T4, T5, T6, T7, T8>(c, c2, c3, c4, c5, c6, c7, c8);
            v.action = (Action<Timer<T, T2, T3, T4, T5, T6, T7, T8>>)ti.action;
            v.obj = o;
            o.World.Timer.Add(ti.attribute.delay, ti.attribute.count, v.Invoke);
#if UNITY_EDITOR
            o._timers.Add(v);
#endif
        }
    }
    void Invoke()
    {
        if (this.Disposed)
        {
            this.obj.World.Timer.Remove(this.Invoke);
            return;
        }
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable || !t6.Enable || !t7.Enable || !t8.Enable) return;
        action.Invoke(this);
    }
}
public class Timer<T, T2, T3, T4, T5, T6, T7, T8, T9> : __Timer where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent
{
    Action<Timer<T, T2, T3, T4, T5, T6, T7, T8, T9>> action;
    static List<TimerItem> timers;
    internal override bool Disposed => this.t.Disposed || this.t2.Disposed || this.t3.Disposed || this.t4.Disposed || this.t5.Disposed || this.t6.Disposed || this.t7.Disposed || this.t8.Disposed || this.t9.Disposed;
    public Timer(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; this.t9 = t9; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }
    public T7 t7 { get; }
    public T8 t8 { get; }
    public T9 t9 { get; }

    internal static void SetTimerList(List<TimerItem> lst) => timers = lst;
    internal static void TryCreateHandle(SObject o)
    {
        for (int i = 0; i < timers.Count; i++)
        {
            var ti = timers[i];
            if (ti.attribute.delay < 0 || ti.attribute.count == 0 || ti.attribute.count < -1)
                Loger.Error($"Timer 参数错误 {ti.action.Method.ReflectedType.FullName} {ti.action.Method.Name}");

            if (!o.TryGetComponent<T>(out var c)) continue;
            if (!o.TryGetComponent<T2>(out var c2)) continue;
            if (!o.TryGetComponent<T3>(out var c3)) continue;
            if (!o.TryGetComponent<T4>(out var c4)) continue;
            if (!o.TryGetComponent<T5>(out var c5)) continue;
            if (!o.TryGetComponent<T6>(out var c6)) continue;
            if (!o.TryGetComponent<T7>(out var c7)) continue;
            if (!o.TryGetComponent<T8>(out var c8)) continue;
            if (!o.TryGetComponent<T9>(out var c9)) continue;
            var v = new Timer<T, T2, T3, T4, T5, T6, T7, T8, T9>(c, c2, c3, c4, c5, c6, c7, c8, c9);
            v.action = (Action<Timer<T, T2, T3, T4, T5, T6, T7, T8, T9>>)ti.action;
            v.obj = o;
            o.World.Timer.Add(ti.attribute.delay, ti.attribute.count, v.Invoke);
#if UNITY_EDITOR
            o._timers.Add(v);
#endif
        }
    }
    void Invoke()
    {
        if (this.Disposed)
        {
            this.obj.World.Timer.Remove(this.Invoke);
            return;
        }
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable || !t6.Enable || !t7.Enable || !t8.Enable || !t9.Enable) return;
        action.Invoke(this);
    }
}
public class Timer<T, T2, T3, T4, T5, T6, T7, T8, T9, T10> : __Timer where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent where T10 : SComponent
{
    Action<Timer<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>> action;
    static List<TimerItem> timers;
    internal override bool Disposed => this.t.Disposed || this.t2.Disposed || this.t3.Disposed || this.t4.Disposed || this.t5.Disposed || this.t6.Disposed || this.t7.Disposed || this.t8.Disposed || this.t9.Disposed || this.t10.Disposed;
    public Timer(T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10) { this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; this.t9 = t9; this.t10 = t10; }
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

    internal static void SetTimerList(List<TimerItem> lst) => timers = lst;
    internal static void TryCreateHandle(SObject o)
    {
        for (int i = 0; i < timers.Count; i++)
        {
            var ti = timers[i];
            if (ti.attribute.delay < 0 || ti.attribute.count == 0 || ti.attribute.count < -1)
                Loger.Error($"Timer 参数错误 {ti.action.Method.ReflectedType.FullName} {ti.action.Method.Name}");

            if (!o.TryGetComponent<T>(out var c)) continue;
            if (!o.TryGetComponent<T2>(out var c2)) continue;
            if (!o.TryGetComponent<T3>(out var c3)) continue;
            if (!o.TryGetComponent<T4>(out var c4)) continue;
            if (!o.TryGetComponent<T5>(out var c5)) continue;
            if (!o.TryGetComponent<T6>(out var c6)) continue;
            if (!o.TryGetComponent<T7>(out var c7)) continue;
            if (!o.TryGetComponent<T8>(out var c8)) continue;
            if (!o.TryGetComponent<T9>(out var c9)) continue;
            if (!o.TryGetComponent<T10>(out var c10)) continue;
            var v = new Timer<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>(c, c2, c3, c4, c5, c6, c7, c8, c9, c10);
            v.action = (Action<Timer<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>>)ti.action;
            v.obj = o;
            o.World.Timer.Add(ti.attribute.delay, ti.attribute.count, v.Invoke);
#if UNITY_EDITOR
            o._timers.Add(v);
#endif
        }
    }
    void Invoke()
    {
        if (this.Disposed)
        {
            this.obj.World.Timer.Remove(this.Invoke);
            return;
        }
        if (!t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable || !t6.Enable || !t7.Enable || !t8.Enable || !t9.Enable || !t10.Enable) return;
        action.Invoke(this);
    }
}
