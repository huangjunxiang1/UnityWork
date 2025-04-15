using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal partial class EventWatcher<E, T> : __SystemHandle where T : SComponent
{
    internal List<Action<E, T>> sys = new(1);

    public override void Add(Delegate d) => sys.Add((Action<E, T>)d);
    public override void _invoke_eventWatcher(object o, SObject obj)
    {
        if (!obj.TryGetComponent<T>(out var c) || !c.Enable) return;
#if UNITY_EDITOR
        obj._EventWatcher.Add(this);
#endif
        for (int i = 0; i < sys.Count; i++)
        {
            try { sys[i].Invoke((E)o, c); }
            catch (Exception e) { Loger.Error(e); }
        }
    }
    public override Type _get_firstType() => typeof(T);
    internal override object GetActions() => sys;
}
internal partial class EventWatcher<E, T, T2> : __SystemHandle where T : SComponent where T2 : SComponent
{
    internal List<Action<E, T, T2>> sys = new(1);

    public override void Add(Delegate d) => sys.Add((Action<E, T, T2>)d);
    public override void _invoke_eventWatcher(object o, SObject obj)
    {
        if (!obj.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!obj.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
#if UNITY_EDITOR
        obj._EventWatcher.Add(this);
#endif
        for (int i = 0; i < sys.Count; i++)
        {
            try { sys[i].Invoke((E)o, c, c2); }
            catch (Exception e) { Loger.Error(e); }
        }
    }
    public override Type _get_firstType() => typeof(T);
    internal override object GetActions() => sys;
}
internal partial class EventWatcher<E, T, T2, T3> : __SystemHandle where T : SComponent where T2 : SComponent where T3 : SComponent
{
    internal List<Action<E, T, T2, T3>> sys = new(1);

    public override void Add(Delegate d) => sys.Add((Action<E, T, T2, T3>)d);
    public override void _invoke_eventWatcher(object o, SObject obj)
    {
        if (!obj.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!obj.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!obj.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
#if UNITY_EDITOR
        obj._EventWatcher.Add(this);
#endif
        for (int i = 0; i < sys.Count; i++)
        {
            try { sys[i].Invoke((E)o, c, c2, c3); }
            catch (Exception e) { Loger.Error(e); }
        }
    }
    public override Type _get_firstType() => typeof(T);
    internal override object GetActions() => sys;
}
internal partial class EventWatcher<E, T, T2, T3, T4> : __SystemHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent
{
    internal List<Action<E, T, T2, T3, T4>> sys = new(1);

    public override void Add(Delegate d) => sys.Add((Action<E, T, T2, T3, T4>)d);
    public override void _invoke_eventWatcher(object o, SObject obj)
    {
        if (!obj.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!obj.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!obj.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!obj.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
#if UNITY_EDITOR
        obj._EventWatcher.Add(this);
#endif
        for (int i = 0; i < sys.Count; i++)
        {
            try { sys[i].Invoke((E)o, c, c2, c3, c4); }
            catch (Exception e) { Loger.Error(e); }
        }
    }
    public override Type _get_firstType() => typeof(T);
    internal override object GetActions() => sys;
}
internal partial class EventWatcher<E, T, T2, T3, T4, T5> : __SystemHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent
{
    internal List<Action<E, T, T2, T3, T4, T5>> sys = new(1);

    public override void Add(Delegate d) => sys.Add((Action<E, T, T2, T3, T4, T5>)d);
    public override void _invoke_eventWatcher(object o, SObject obj)
    {
        if (!obj.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!obj.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!obj.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!obj.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!obj.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
#if UNITY_EDITOR
        obj._EventWatcher.Add(this);
#endif
        for (int i = 0; i < sys.Count; i++)
        {
            try { sys[i].Invoke((E)o, c, c2, c3, c4, c5); }
            catch (Exception e) { Loger.Error(e); }
        }
    }
    public override Type _get_firstType() => typeof(T);
    internal override object GetActions() => sys;
}
internal partial class EventWatcher<E, T, T2, T3, T4, T5, T6> : __SystemHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent
{
    internal List<Action<E, T, T2, T3, T4, T5, T6>> sys = new(1);

    public override void Add(Delegate d) => sys.Add((Action<E, T, T2, T3, T4, T5, T6>)d);
    public override void _invoke_eventWatcher(object o, SObject obj)
    {
        if (!obj.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!obj.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!obj.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!obj.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!obj.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        if (!obj.TryGetComponent<T6>(out var c6) || !c6.Enable) return;
#if UNITY_EDITOR
        obj._EventWatcher.Add(this);
#endif
        for (int i = 0; i < sys.Count; i++)
        {
            try { sys[i].Invoke((E)o, c, c2, c3, c4, c5, c6); }
            catch (Exception e) { Loger.Error(e); }
        }
    }
    public override Type _get_firstType() => typeof(T);
    internal override object GetActions() => sys;
}
internal partial class EventWatcher<E, T, T2, T3, T4, T5, T6, T7> : __SystemHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent
{
    internal List<Action<E, T, T2, T3, T4, T5, T6, T7>> sys = new(1);

    public override void Add(Delegate d) => sys.Add((Action<E, T, T2, T3, T4, T5, T6, T7>)d);
    public override void _invoke_eventWatcher(object o, SObject obj)
    {
        if (!obj.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!obj.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!obj.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!obj.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!obj.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        if (!obj.TryGetComponent<T6>(out var c6) || !c6.Enable) return;
        if (!obj.TryGetComponent<T7>(out var c7) || !c7.Enable) return;
#if UNITY_EDITOR
        obj._EventWatcher.Add(this);
#endif
        for (int i = 0; i < sys.Count; i++)
        {
            try { sys[i].Invoke((E)o, c, c2, c3, c4, c5, c6, c7); }
            catch (Exception e) { Loger.Error(e); }
        }
    }
    public override Type _get_firstType() => typeof(T);
    internal override object GetActions() => sys;
}
internal partial class EventWatcher<E, T, T2, T3, T4, T5, T6, T7, T8> : __SystemHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent
{
    internal List<Action<E, T, T2, T3, T4, T5, T6, T7, T8>> sys = new(1);

    public override void Add(Delegate d) => sys.Add((Action<E, T, T2, T3, T4, T5, T6, T7, T8>)d);
    public override void _invoke_eventWatcher(object o, SObject obj)
    {
        if (!obj.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!obj.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!obj.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!obj.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!obj.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        if (!obj.TryGetComponent<T6>(out var c6) || !c6.Enable) return;
        if (!obj.TryGetComponent<T7>(out var c7) || !c7.Enable) return;
        if (!obj.TryGetComponent<T8>(out var c8) || !c8.Enable) return;
#if UNITY_EDITOR
        obj._EventWatcher.Add(this);
#endif
        for (int i = 0; i < sys.Count; i++)
        {
            try { sys[i].Invoke((E)o, c, c2, c3, c4, c5, c6, c7, c8); }
            catch (Exception e) { Loger.Error(e); }
        }
    }
    public override Type _get_firstType() => typeof(T);
    internal override object GetActions() => sys;
}
internal partial class EventWatcher<E, T, T2, T3, T4, T5, T6, T7, T8, T9> : __SystemHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent
{
    internal List<Action<E, T, T2, T3, T4, T5, T6, T7, T8, T9>> sys = new(1);

    public override void Add(Delegate d) => sys.Add((Action<E, T, T2, T3, T4, T5, T6, T7, T8, T9>)d);
    public override void _invoke_eventWatcher(object o, SObject obj)
    {
        if (!obj.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!obj.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!obj.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!obj.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!obj.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        if (!obj.TryGetComponent<T6>(out var c6) || !c6.Enable) return;
        if (!obj.TryGetComponent<T7>(out var c7) || !c7.Enable) return;
        if (!obj.TryGetComponent<T8>(out var c8) || !c8.Enable) return;
        if (!obj.TryGetComponent<T9>(out var c9) || !c9.Enable) return;
#if UNITY_EDITOR
        obj._EventWatcher.Add(this);
#endif
        for (int i = 0; i < sys.Count; i++)
        {
            try { sys[i].Invoke((E)o, c, c2, c3, c4, c5, c6, c7, c8, c9); }
            catch (Exception e) { Loger.Error(e); }
        }
    }
    public override Type _get_firstType() => typeof(T);
    internal override object GetActions() => sys;
}
internal partial class EventWatcher<E, T, T2, T3, T4, T5, T6, T7, T8, T9, T10> : __SystemHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent where T10 : SComponent
{
    internal List<Action<E, T, T2, T3, T4, T5, T6, T7, T8, T9, T10>> sys = new(1);

    public override void Add(Delegate d) => sys.Add((Action<E, T, T2, T3, T4, T5, T6, T7, T8, T9, T10>)d);
    public override void _invoke_eventWatcher(object o, SObject obj)
    {
        if (!obj.TryGetComponent<T>(out var c) || !c.Enable) return;
        if (!obj.TryGetComponent<T2>(out var c2) || !c2.Enable) return;
        if (!obj.TryGetComponent<T3>(out var c3) || !c3.Enable) return;
        if (!obj.TryGetComponent<T4>(out var c4) || !c4.Enable) return;
        if (!obj.TryGetComponent<T5>(out var c5) || !c5.Enable) return;
        if (!obj.TryGetComponent<T6>(out var c6) || !c6.Enable) return;
        if (!obj.TryGetComponent<T7>(out var c7) || !c7.Enable) return;
        if (!obj.TryGetComponent<T8>(out var c8) || !c8.Enable) return;
        if (!obj.TryGetComponent<T9>(out var c9) || !c9.Enable) return;
        if (!obj.TryGetComponent<T10>(out var c10) || !c10.Enable) return;
#if UNITY_EDITOR
        obj._EventWatcher.Add(this);
#endif
        for (int i = 0; i < sys.Count; i++)
        {
            try { sys[i].Invoke((E)o, c, c2, c3, c4, c5, c6, c7, c8, c9, c10); }
            catch (Exception e) { Loger.Error(e); }
        }
    }
    public override Type _get_firstType() => typeof(T);
    internal override object GetActions() => sys;
}
