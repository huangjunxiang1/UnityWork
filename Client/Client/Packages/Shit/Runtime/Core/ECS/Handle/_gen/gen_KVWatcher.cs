using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal partial class KVWatcher<T> : __KVWatcher where T : SComponent
{
    internal Dictionary<int, List<Action<T>>> sys = new();

    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
    {
        if (!o.TryGetComponent<KVComponent>(out var kv)) return null;
        if (!o.TryGetComponent<T>(out var c)) return null;
        var v = new ComponentFilter<T>() { system = this, kv = kv, t = c };
        if (!c.Enable) v.EnableCounter += 1;
        return v;
    }
    public override void Add(int key, Delegate d)
    {
        if (!sys.TryGetValue(key, out var lst))
            sys[key] = lst = new(1);
        lst.Add((Action<T>)d);
    }
    public override void _invoke_kvWatcher(int key, ComponentFilter cf)
    {
        if (!sys.TryGetValue(key, out var lst))
            return;
        var cs = (ComponentFilter<T>)cf;
        for (int i = 0; i < lst.Count; i++)
            lst[i].Invoke(cs.t);
    }
}
internal partial class KVWatcher<T, T2> : __KVWatcher where T : SComponent where T2 : SComponent
{
    internal Dictionary<int, List<Action<T, T2>>> sys = new();

    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
    {
        if (!o.TryGetComponent<KVComponent>(out var kv)) return null;
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        var v = new ComponentFilter<T, T2>() { system = this, kv = kv, t = c, t2 = c2 };
        if (!c.Enable) v.EnableCounter += 1;
        if (!c2.Enable) v.EnableCounter += 1;
        return v;
    }
    public override void Add(int key, Delegate d)
    {
        if (!sys.TryGetValue(key, out var lst))
            sys[key] = lst = new(1);
        lst.Add((Action<T, T2>)d);
    }
    public override void _invoke_kvWatcher(int key, ComponentFilter cf)
    {
        if (!sys.TryGetValue(key, out var lst))
            return;
        var cs = (ComponentFilter<T, T2>)cf;
        for (int i = 0; i < lst.Count; i++)
            lst[i].Invoke(cs.t, cs.t2);
    }
}
internal partial class KVWatcher<T, T2, T3> : __KVWatcher where T : SComponent where T2 : SComponent where T3 : SComponent
{
    internal Dictionary<int, List<Action<T, T2, T3>>> sys = new();

    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
    {
        if (!o.TryGetComponent<KVComponent>(out var kv)) return null;
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        var v = new ComponentFilter<T, T2, T3>() { system = this, kv = kv, t = c, t2 = c2, t3 = c3 };
        if (!c.Enable) v.EnableCounter += 1;
        if (!c2.Enable) v.EnableCounter += 1;
        if (!c3.Enable) v.EnableCounter += 1;
        return v;
    }
    public override void Add(int key, Delegate d)
    {
        if (!sys.TryGetValue(key, out var lst))
            sys[key] = lst = new(1);
        lst.Add((Action<T, T2, T3>)d);
    }
    public override void _invoke_kvWatcher(int key, ComponentFilter cf)
    {
        if (!sys.TryGetValue(key, out var lst))
            return;
        var cs = (ComponentFilter<T, T2, T3>)cf;
        for (int i = 0; i < lst.Count; i++)
            lst[i].Invoke(cs.t, cs.t2, cs.t3);
    }
}
internal partial class KVWatcher<T, T2, T3, T4> : __KVWatcher where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent
{
    internal Dictionary<int, List<Action<T, T2, T3, T4>>> sys = new();

    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
    {
        if (!o.TryGetComponent<KVComponent>(out var kv)) return null;
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        if (!o.TryGetComponent<T4>(out var c4)) return null;
        var v = new ComponentFilter<T, T2, T3, T4>() { system = this, kv = kv, t = c, t2 = c2, t3 = c3, t4 = c4 };
        if (!c.Enable) v.EnableCounter += 1;
        if (!c2.Enable) v.EnableCounter += 1;
        if (!c3.Enable) v.EnableCounter += 1;
        if (!c4.Enable) v.EnableCounter += 1;
        return v;
    }
    public override void Add(int key, Delegate d)
    {
        if (!sys.TryGetValue(key, out var lst))
            sys[key] = lst = new(1);
        lst.Add((Action<T, T2, T3, T4>)d);
    }
    public override void _invoke_kvWatcher(int key, ComponentFilter cf)
    {
        if (!sys.TryGetValue(key, out var lst))
            return;
        var cs = (ComponentFilter<T, T2, T3, T4>)cf;
        for (int i = 0; i < lst.Count; i++)
            lst[i].Invoke(cs.t, cs.t2, cs.t3, cs.t4);
    }
}
internal partial class KVWatcher<T, T2, T3, T4, T5> : __KVWatcher where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent
{
    internal Dictionary<int, List<Action<T, T2, T3, T4, T5>>> sys = new();

    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
    {
        if (!o.TryGetComponent<KVComponent>(out var kv)) return null;
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        if (!o.TryGetComponent<T4>(out var c4)) return null;
        if (!o.TryGetComponent<T5>(out var c5)) return null;
        var v = new ComponentFilter<T, T2, T3, T4, T5>() { system = this, kv = kv, t = c, t2 = c2, t3 = c3, t4 = c4, t5 = c5 };
        if (!c.Enable) v.EnableCounter += 1;
        if (!c2.Enable) v.EnableCounter += 1;
        if (!c3.Enable) v.EnableCounter += 1;
        if (!c4.Enable) v.EnableCounter += 1;
        if (!c5.Enable) v.EnableCounter += 1;
        return v;
    }
    public override void Add(int key, Delegate d)
    {
        if (!sys.TryGetValue(key, out var lst))
            sys[key] = lst = new(1);
        lst.Add((Action<T, T2, T3, T4, T5>)d);
    }
    public override void _invoke_kvWatcher(int key, ComponentFilter cf)
    {
        if (!sys.TryGetValue(key, out var lst))
            return;
        var cs = (ComponentFilter<T, T2, T3, T4, T5>)cf;
        for (int i = 0; i < lst.Count; i++)
            lst[i].Invoke(cs.t, cs.t2, cs.t3, cs.t4, cs.t5);
    }
}
internal partial class KVWatcher<T, T2, T3, T4, T5, T6> : __KVWatcher where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent
{
    internal Dictionary<int, List<Action<T, T2, T3, T4, T5, T6>>> sys = new();

    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
    {
        if (!o.TryGetComponent<KVComponent>(out var kv)) return null;
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        if (!o.TryGetComponent<T4>(out var c4)) return null;
        if (!o.TryGetComponent<T5>(out var c5)) return null;
        if (!o.TryGetComponent<T6>(out var c6)) return null;
        var v = new ComponentFilter<T, T2, T3, T4, T5, T6>() { system = this, kv = kv, t = c, t2 = c2, t3 = c3, t4 = c4, t5 = c5, t6 = c6 };
        if (!c.Enable) v.EnableCounter += 1;
        if (!c2.Enable) v.EnableCounter += 1;
        if (!c3.Enable) v.EnableCounter += 1;
        if (!c4.Enable) v.EnableCounter += 1;
        if (!c5.Enable) v.EnableCounter += 1;
        if (!c6.Enable) v.EnableCounter += 1;
        return v;
    }
    public override void Add(int key, Delegate d)
    {
        if (!sys.TryGetValue(key, out var lst))
            sys[key] = lst = new(1);
        lst.Add((Action<T, T2, T3, T4, T5, T6>)d);
    }
    public override void _invoke_kvWatcher(int key, ComponentFilter cf)
    {
        if (!sys.TryGetValue(key, out var lst))
            return;
        var cs = (ComponentFilter<T, T2, T3, T4, T5, T6>)cf;
        for (int i = 0; i < lst.Count; i++)
            lst[i].Invoke(cs.t, cs.t2, cs.t3, cs.t4, cs.t5, cs.t6);
    }
}
internal partial class KVWatcher<T, T2, T3, T4, T5, T6, T7> : __KVWatcher where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent
{
    internal Dictionary<int, List<Action<T, T2, T3, T4, T5, T6, T7>>> sys = new();

    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
    {
        if (!o.TryGetComponent<KVComponent>(out var kv)) return null;
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        if (!o.TryGetComponent<T4>(out var c4)) return null;
        if (!o.TryGetComponent<T5>(out var c5)) return null;
        if (!o.TryGetComponent<T6>(out var c6)) return null;
        if (!o.TryGetComponent<T7>(out var c7)) return null;
        var v = new ComponentFilter<T, T2, T3, T4, T5, T6, T7>() { system = this, kv = kv, t = c, t2 = c2, t3 = c3, t4 = c4, t5 = c5, t6 = c6, t7 = c7 };
        if (!c.Enable) v.EnableCounter += 1;
        if (!c2.Enable) v.EnableCounter += 1;
        if (!c3.Enable) v.EnableCounter += 1;
        if (!c4.Enable) v.EnableCounter += 1;
        if (!c5.Enable) v.EnableCounter += 1;
        if (!c6.Enable) v.EnableCounter += 1;
        if (!c7.Enable) v.EnableCounter += 1;
        return v;
    }
    public override void Add(int key, Delegate d)
    {
        if (!sys.TryGetValue(key, out var lst))
            sys[key] = lst = new(1);
        lst.Add((Action<T, T2, T3, T4, T5, T6, T7>)d);
    }
    public override void _invoke_kvWatcher(int key, ComponentFilter cf)
    {
        if (!sys.TryGetValue(key, out var lst))
            return;
        var cs = (ComponentFilter<T, T2, T3, T4, T5, T6, T7>)cf;
        for (int i = 0; i < lst.Count; i++)
            lst[i].Invoke(cs.t, cs.t2, cs.t3, cs.t4, cs.t5, cs.t6, cs.t7);
    }
}
internal partial class KVWatcher<T, T2, T3, T4, T5, T6, T7, T8> : __KVWatcher where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent
{
    internal Dictionary<int, List<Action<T, T2, T3, T4, T5, T6, T7, T8>>> sys = new();

    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
    {
        if (!o.TryGetComponent<KVComponent>(out var kv)) return null;
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        if (!o.TryGetComponent<T4>(out var c4)) return null;
        if (!o.TryGetComponent<T5>(out var c5)) return null;
        if (!o.TryGetComponent<T6>(out var c6)) return null;
        if (!o.TryGetComponent<T7>(out var c7)) return null;
        if (!o.TryGetComponent<T8>(out var c8)) return null;
        var v = new ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8>() { system = this, kv = kv, t = c, t2 = c2, t3 = c3, t4 = c4, t5 = c5, t6 = c6, t7 = c7, t8 = c8 };
        if (!c.Enable) v.EnableCounter += 1;
        if (!c2.Enable) v.EnableCounter += 1;
        if (!c3.Enable) v.EnableCounter += 1;
        if (!c4.Enable) v.EnableCounter += 1;
        if (!c5.Enable) v.EnableCounter += 1;
        if (!c6.Enable) v.EnableCounter += 1;
        if (!c7.Enable) v.EnableCounter += 1;
        if (!c8.Enable) v.EnableCounter += 1;
        return v;
    }
    public override void Add(int key, Delegate d)
    {
        if (!sys.TryGetValue(key, out var lst))
            sys[key] = lst = new(1);
        lst.Add((Action<T, T2, T3, T4, T5, T6, T7, T8>)d);
    }
    public override void _invoke_kvWatcher(int key, ComponentFilter cf)
    {
        if (!sys.TryGetValue(key, out var lst))
            return;
        var cs = (ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8>)cf;
        for (int i = 0; i < lst.Count; i++)
            lst[i].Invoke(cs.t, cs.t2, cs.t3, cs.t4, cs.t5, cs.t6, cs.t7, cs.t8);
    }
}
internal partial class KVWatcher<T, T2, T3, T4, T5, T6, T7, T8, T9> : __KVWatcher where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent
{
    internal Dictionary<int, List<Action<T, T2, T3, T4, T5, T6, T7, T8, T9>>> sys = new();

    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
    {
        if (!o.TryGetComponent<KVComponent>(out var kv)) return null;
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        if (!o.TryGetComponent<T4>(out var c4)) return null;
        if (!o.TryGetComponent<T5>(out var c5)) return null;
        if (!o.TryGetComponent<T6>(out var c6)) return null;
        if (!o.TryGetComponent<T7>(out var c7)) return null;
        if (!o.TryGetComponent<T8>(out var c8)) return null;
        if (!o.TryGetComponent<T9>(out var c9)) return null;
        var v = new ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8, T9>() { system = this, kv = kv, t = c, t2 = c2, t3 = c3, t4 = c4, t5 = c5, t6 = c6, t7 = c7, t8 = c8, t9 = c9 };
        if (!c.Enable) v.EnableCounter += 1;
        if (!c2.Enable) v.EnableCounter += 1;
        if (!c3.Enable) v.EnableCounter += 1;
        if (!c4.Enable) v.EnableCounter += 1;
        if (!c5.Enable) v.EnableCounter += 1;
        if (!c6.Enable) v.EnableCounter += 1;
        if (!c7.Enable) v.EnableCounter += 1;
        if (!c8.Enable) v.EnableCounter += 1;
        if (!c9.Enable) v.EnableCounter += 1;
        return v;
    }
    public override void Add(int key, Delegate d)
    {
        if (!sys.TryGetValue(key, out var lst))
            sys[key] = lst = new(1);
        lst.Add((Action<T, T2, T3, T4, T5, T6, T7, T8, T9>)d);
    }
    public override void _invoke_kvWatcher(int key, ComponentFilter cf)
    {
        if (!sys.TryGetValue(key, out var lst))
            return;
        var cs = (ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8, T9>)cf;
        for (int i = 0; i < lst.Count; i++)
            lst[i].Invoke(cs.t, cs.t2, cs.t3, cs.t4, cs.t5, cs.t6, cs.t7, cs.t8, cs.t9);
    }
}
internal partial class KVWatcher<T, T2, T3, T4, T5, T6, T7, T8, T9, T10> : __KVWatcher where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent where T10 : SComponent
{
    internal Dictionary<int, List<Action<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>>> sys = new();

    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
    {
        if (!o.TryGetComponent<KVComponent>(out var kv)) return null;
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
        var v = new ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>() { system = this, kv = kv, t = c, t2 = c2, t3 = c3, t4 = c4, t5 = c5, t6 = c6, t7 = c7, t8 = c8, t9 = c9, t10 = c10 };
        if (!c.Enable) v.EnableCounter += 1;
        if (!c2.Enable) v.EnableCounter += 1;
        if (!c3.Enable) v.EnableCounter += 1;
        if (!c4.Enable) v.EnableCounter += 1;
        if (!c5.Enable) v.EnableCounter += 1;
        if (!c6.Enable) v.EnableCounter += 1;
        if (!c7.Enable) v.EnableCounter += 1;
        if (!c8.Enable) v.EnableCounter += 1;
        if (!c9.Enable) v.EnableCounter += 1;
        if (!c10.Enable) v.EnableCounter += 1;
        return v;
    }
    public override void Add(int key, Delegate d)
    {
        if (!sys.TryGetValue(key, out var lst))
            sys[key] = lst = new(1);
        lst.Add((Action<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>)d);
    }
    public override void _invoke_kvWatcher(int key, ComponentFilter cf)
    {
        if (!sys.TryGetValue(key, out var lst))
            return;
        var cs = (ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>)cf;
        for (int i = 0; i < lst.Count; i++)
            lst[i].Invoke(cs.t, cs.t2, cs.t3, cs.t4, cs.t5, cs.t6, cs.t7, cs.t8, cs.t9, cs.t10);
    }
}
