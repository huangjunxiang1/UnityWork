using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

internal partial class SystemHandler<T> : __SystemHandle where T : SComponent
{
    internal List<Action<T>> sys = new(1);
    internal Queue<ComponentFilter<T>> cfq = ObjectPool.Get<Queue<ComponentFilter<T>>>();

    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
    {
        if (!o.TryGetComponent<T>(out var c)) return null;
        var v = new ComponentFilter<T>() { system = this, world = o.World, t = c };
        if (!c.Enable) v.EnableCounter += 1;
        if (addToQueue) cfq.Enqueue(v);
        return v;
    }

    public override void Add(Delegate d) => sys.Add((Action<T>)d);
    public override void _invoke_update()
    {
        var tmp = cfq;
        cfq = ObjectPool.Get<Queue<ComponentFilter<T>>>();
        while (tmp.TryDequeue(out var cf))
        {
            if (cf.EnableCounter == 0)
            {
                for (int i = 0; i < sys.Count; i++)
                {
                    if (cf.Disposed) continue;
                    try { sys[i].Invoke(cf.t); }
                    catch (Exception e) { Loger.Error(e); }
                }
            }
            cfq.Enqueue(cf);
        }
        ObjectPool.Return(tmp);
    }
    public override void Invoke(ComponentFilter filter)
    {
        var cf = (ComponentFilter<T>)filter;
        for (int i = 0; i < sys.Count; i++)
        {
            if (cf.Disposed) continue;
            try { sys[i].Invoke(cf.t); }
            catch (Exception e) { Loger.Error(e); }
        }
    }
    public override Type _get_firstType() => typeof(T);
    internal override object GetActions() => sys;
}
internal partial class SystemHandler<T, T2> : __SystemHandle where T : SComponent where T2 : SComponent
{
    internal List<Action<T, T2>> sys = new(1);
    internal Queue<ComponentFilter<T, T2>> cfq = ObjectPool.Get<Queue<ComponentFilter<T, T2>>>();

    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
    {
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        var v = new ComponentFilter<T, T2>() { system = this, world = o.World, t = c, t2 = c2 };
        if (!c.Enable) v.EnableCounter += 1;
        if (!c2.Enable) v.EnableCounter += 1;
        if (addToQueue) cfq.Enqueue(v);
        return v;
    }

    public override void Add(Delegate d) => sys.Add((Action<T, T2>)d);
    public override void _invoke_update()
    {
        var tmp = cfq;
        cfq = ObjectPool.Get<Queue<ComponentFilter<T, T2>>>();
        while (tmp.TryDequeue(out var cf))
        {
            if (cf.EnableCounter == 0)
            {
                for (int i = 0; i < sys.Count; i++)
                {
                    if (cf.Disposed) continue;
                    try { sys[i].Invoke(cf.t, cf.t2); }
                    catch (Exception e) { Loger.Error(e); }
                }
            }
            cfq.Enqueue(cf);
        }
        ObjectPool.Return(tmp);
    }
    public override void Invoke(ComponentFilter filter)
    {
        var cf = (ComponentFilter<T, T2>)filter;
        for (int i = 0; i < sys.Count; i++)
        {
            if (cf.Disposed) continue;
            try { sys[i].Invoke(cf.t, cf.t2); }
            catch (Exception e) { Loger.Error(e); }
        }
    }
    public override Type _get_firstType() => typeof(T);
    internal override object GetActions() => sys;
}
internal partial class SystemHandler<T, T2, T3> : __SystemHandle where T : SComponent where T2 : SComponent where T3 : SComponent
{
    internal List<Action<T, T2, T3>> sys = new(1);
    internal Queue<ComponentFilter<T, T2, T3>> cfq = ObjectPool.Get<Queue<ComponentFilter<T, T2, T3>>>();

    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
    {
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        var v = new ComponentFilter<T, T2, T3>() { system = this, world = o.World, t = c, t2 = c2, t3 = c3 };
        if (!c.Enable) v.EnableCounter += 1;
        if (!c2.Enable) v.EnableCounter += 1;
        if (!c3.Enable) v.EnableCounter += 1;
        if (addToQueue) cfq.Enqueue(v);
        return v;
    }

    public override void Add(Delegate d) => sys.Add((Action<T, T2, T3>)d);
    public override void _invoke_update()
    {
        var tmp = cfq;
        cfq = ObjectPool.Get<Queue<ComponentFilter<T, T2, T3>>>();
        while (tmp.TryDequeue(out var cf))
        {
            if (cf.EnableCounter == 0)
            {
                for (int i = 0; i < sys.Count; i++)
                {
                    if (cf.Disposed) continue;
                    try { sys[i].Invoke(cf.t, cf.t2, cf.t3); }
                    catch (Exception e) { Loger.Error(e); }
                }
            }
            cfq.Enqueue(cf);
        }
        ObjectPool.Return(tmp);
    }
    public override void Invoke(ComponentFilter filter)
    {
        var cf = (ComponentFilter<T, T2, T3>)filter;
        for (int i = 0; i < sys.Count; i++)
        {
            if (cf.Disposed) continue;
            try { sys[i].Invoke(cf.t, cf.t2, cf.t3); }
            catch (Exception e) { Loger.Error(e); }
        }
    }
    public override Type _get_firstType() => typeof(T);
    internal override object GetActions() => sys;
}
internal partial class SystemHandler<T, T2, T3, T4> : __SystemHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent
{
    internal List<Action<T, T2, T3, T4>> sys = new(1);
    internal Queue<ComponentFilter<T, T2, T3, T4>> cfq = ObjectPool.Get<Queue<ComponentFilter<T, T2, T3, T4>>>();

    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
    {
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        if (!o.TryGetComponent<T4>(out var c4)) return null;
        var v = new ComponentFilter<T, T2, T3, T4>() { system = this, world = o.World, t = c, t2 = c2, t3 = c3, t4 = c4 };
        if (!c.Enable) v.EnableCounter += 1;
        if (!c2.Enable) v.EnableCounter += 1;
        if (!c3.Enable) v.EnableCounter += 1;
        if (!c4.Enable) v.EnableCounter += 1;
        if (addToQueue) cfq.Enqueue(v);
        return v;
    }

    public override void Add(Delegate d) => sys.Add((Action<T, T2, T3, T4>)d);
    public override void _invoke_update()
    {
        var tmp = cfq;
        cfq = ObjectPool.Get<Queue<ComponentFilter<T, T2, T3, T4>>>();
        while (tmp.TryDequeue(out var cf))
        {
            if (cf.EnableCounter == 0)
            {
                for (int i = 0; i < sys.Count; i++)
                {
                    if (cf.Disposed) continue;
                    try { sys[i].Invoke(cf.t, cf.t2, cf.t3, cf.t4); }
                    catch (Exception e) { Loger.Error(e); }
                }
            }
            cfq.Enqueue(cf);
        }
        ObjectPool.Return(tmp);
    }
    public override void Invoke(ComponentFilter filter)
    {
        var cf = (ComponentFilter<T, T2, T3, T4>)filter;
        for (int i = 0; i < sys.Count; i++)
        {
            if (cf.Disposed) continue;
            try { sys[i].Invoke(cf.t, cf.t2, cf.t3, cf.t4); }
            catch (Exception e) { Loger.Error(e); }
        }
    }
    public override Type _get_firstType() => typeof(T);
    internal override object GetActions() => sys;
}
internal partial class SystemHandler<T, T2, T3, T4, T5> : __SystemHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent
{
    internal List<Action<T, T2, T3, T4, T5>> sys = new(1);
    internal Queue<ComponentFilter<T, T2, T3, T4, T5>> cfq = ObjectPool.Get<Queue<ComponentFilter<T, T2, T3, T4, T5>>>();

    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
    {
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        if (!o.TryGetComponent<T4>(out var c4)) return null;
        if (!o.TryGetComponent<T5>(out var c5)) return null;
        var v = new ComponentFilter<T, T2, T3, T4, T5>() { system = this, world = o.World, t = c, t2 = c2, t3 = c3, t4 = c4, t5 = c5 };
        if (!c.Enable) v.EnableCounter += 1;
        if (!c2.Enable) v.EnableCounter += 1;
        if (!c3.Enable) v.EnableCounter += 1;
        if (!c4.Enable) v.EnableCounter += 1;
        if (!c5.Enable) v.EnableCounter += 1;
        if (addToQueue) cfq.Enqueue(v);
        return v;
    }

    public override void Add(Delegate d) => sys.Add((Action<T, T2, T3, T4, T5>)d);
    public override void _invoke_update()
    {
        var tmp = cfq;
        cfq = ObjectPool.Get<Queue<ComponentFilter<T, T2, T3, T4, T5>>>();
        while (tmp.TryDequeue(out var cf))
        {
            if (cf.EnableCounter == 0)
            {
                for (int i = 0; i < sys.Count; i++)
                {
                    if (cf.Disposed) continue;
                    try { sys[i].Invoke(cf.t, cf.t2, cf.t3, cf.t4, cf.t5); }
                    catch (Exception e) { Loger.Error(e); }
                }
            }
            cfq.Enqueue(cf);
        }
        ObjectPool.Return(tmp);
    }
    public override void Invoke(ComponentFilter filter)
    {
        var cf = (ComponentFilter<T, T2, T3, T4, T5>)filter;
        for (int i = 0; i < sys.Count; i++)
        {
            if (cf.Disposed) continue;
            try { sys[i].Invoke(cf.t, cf.t2, cf.t3, cf.t4, cf.t5); }
            catch (Exception e) { Loger.Error(e); }
        }
    }
    public override Type _get_firstType() => typeof(T);
    internal override object GetActions() => sys;
}
internal partial class SystemHandler<T, T2, T3, T4, T5, T6> : __SystemHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent
{
    internal List<Action<T, T2, T3, T4, T5, T6>> sys = new(1);
    internal Queue<ComponentFilter<T, T2, T3, T4, T5, T6>> cfq = ObjectPool.Get<Queue<ComponentFilter<T, T2, T3, T4, T5, T6>>>();

    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
    {
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        if (!o.TryGetComponent<T4>(out var c4)) return null;
        if (!o.TryGetComponent<T5>(out var c5)) return null;
        if (!o.TryGetComponent<T6>(out var c6)) return null;
        var v = new ComponentFilter<T, T2, T3, T4, T5, T6>() { system = this, world = o.World, t = c, t2 = c2, t3 = c3, t4 = c4, t5 = c5, t6 = c6 };
        if (!c.Enable) v.EnableCounter += 1;
        if (!c2.Enable) v.EnableCounter += 1;
        if (!c3.Enable) v.EnableCounter += 1;
        if (!c4.Enable) v.EnableCounter += 1;
        if (!c5.Enable) v.EnableCounter += 1;
        if (!c6.Enable) v.EnableCounter += 1;
        if (addToQueue) cfq.Enqueue(v);
        return v;
    }

    public override void Add(Delegate d) => sys.Add((Action<T, T2, T3, T4, T5, T6>)d);
    public override void _invoke_update()
    {
        var tmp = cfq;
        cfq = ObjectPool.Get<Queue<ComponentFilter<T, T2, T3, T4, T5, T6>>>();
        while (tmp.TryDequeue(out var cf))
        {
            if (cf.EnableCounter == 0)
            {
                for (int i = 0; i < sys.Count; i++)
                {
                    if (cf.Disposed) continue;
                    try { sys[i].Invoke(cf.t, cf.t2, cf.t3, cf.t4, cf.t5, cf.t6); }
                    catch (Exception e) { Loger.Error(e); }
                }
            }
            cfq.Enqueue(cf);
        }
        ObjectPool.Return(tmp);
    }
    public override void Invoke(ComponentFilter filter)
    {
        var cf = (ComponentFilter<T, T2, T3, T4, T5, T6>)filter;
        for (int i = 0; i < sys.Count; i++)
        {
            if (cf.Disposed) continue;
            try { sys[i].Invoke(cf.t, cf.t2, cf.t3, cf.t4, cf.t5, cf.t6); }
            catch (Exception e) { Loger.Error(e); }
        }
    }
    public override Type _get_firstType() => typeof(T);
    internal override object GetActions() => sys;
}
internal partial class SystemHandler<T, T2, T3, T4, T5, T6, T7> : __SystemHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent
{
    internal List<Action<T, T2, T3, T4, T5, T6, T7>> sys = new(1);
    internal Queue<ComponentFilter<T, T2, T3, T4, T5, T6, T7>> cfq = ObjectPool.Get<Queue<ComponentFilter<T, T2, T3, T4, T5, T6, T7>>>();

    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
    {
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        if (!o.TryGetComponent<T4>(out var c4)) return null;
        if (!o.TryGetComponent<T5>(out var c5)) return null;
        if (!o.TryGetComponent<T6>(out var c6)) return null;
        if (!o.TryGetComponent<T7>(out var c7)) return null;
        var v = new ComponentFilter<T, T2, T3, T4, T5, T6, T7>() { system = this, world = o.World, t = c, t2 = c2, t3 = c3, t4 = c4, t5 = c5, t6 = c6, t7 = c7 };
        if (!c.Enable) v.EnableCounter += 1;
        if (!c2.Enable) v.EnableCounter += 1;
        if (!c3.Enable) v.EnableCounter += 1;
        if (!c4.Enable) v.EnableCounter += 1;
        if (!c5.Enable) v.EnableCounter += 1;
        if (!c6.Enable) v.EnableCounter += 1;
        if (!c7.Enable) v.EnableCounter += 1;
        if (addToQueue) cfq.Enqueue(v);
        return v;
    }

    public override void Add(Delegate d) => sys.Add((Action<T, T2, T3, T4, T5, T6, T7>)d);
    public override void _invoke_update()
    {
        var tmp = cfq;
        cfq = ObjectPool.Get<Queue<ComponentFilter<T, T2, T3, T4, T5, T6, T7>>>();
        while (tmp.TryDequeue(out var cf))
        {
            if (cf.EnableCounter == 0)
            {
                for (int i = 0; i < sys.Count; i++)
                {
                    if (cf.Disposed) continue;
                    try { sys[i].Invoke(cf.t, cf.t2, cf.t3, cf.t4, cf.t5, cf.t6, cf.t7); }
                    catch (Exception e) { Loger.Error(e); }
                }
            }
            cfq.Enqueue(cf);
        }
        ObjectPool.Return(tmp);
    }
    public override void Invoke(ComponentFilter filter)
    {
        var cf = (ComponentFilter<T, T2, T3, T4, T5, T6, T7>)filter;
        for (int i = 0; i < sys.Count; i++)
        {
            if (cf.Disposed) continue;
            try { sys[i].Invoke(cf.t, cf.t2, cf.t3, cf.t4, cf.t5, cf.t6, cf.t7); }
            catch (Exception e) { Loger.Error(e); }
        }
    }
    public override Type _get_firstType() => typeof(T);
    internal override object GetActions() => sys;
}
internal partial class SystemHandler<T, T2, T3, T4, T5, T6, T7, T8> : __SystemHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent
{
    internal List<Action<T, T2, T3, T4, T5, T6, T7, T8>> sys = new(1);
    internal Queue<ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8>> cfq = ObjectPool.Get<Queue<ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8>>>();

    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
    {
        if (!o.TryGetComponent<T>(out var c)) return null;
        if (!o.TryGetComponent<T2>(out var c2)) return null;
        if (!o.TryGetComponent<T3>(out var c3)) return null;
        if (!o.TryGetComponent<T4>(out var c4)) return null;
        if (!o.TryGetComponent<T5>(out var c5)) return null;
        if (!o.TryGetComponent<T6>(out var c6)) return null;
        if (!o.TryGetComponent<T7>(out var c7)) return null;
        if (!o.TryGetComponent<T8>(out var c8)) return null;
        var v = new ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8>() { system = this, world = o.World, t = c, t2 = c2, t3 = c3, t4 = c4, t5 = c5, t6 = c6, t7 = c7, t8 = c8 };
        if (!c.Enable) v.EnableCounter += 1;
        if (!c2.Enable) v.EnableCounter += 1;
        if (!c3.Enable) v.EnableCounter += 1;
        if (!c4.Enable) v.EnableCounter += 1;
        if (!c5.Enable) v.EnableCounter += 1;
        if (!c6.Enable) v.EnableCounter += 1;
        if (!c7.Enable) v.EnableCounter += 1;
        if (!c8.Enable) v.EnableCounter += 1;
        if (addToQueue) cfq.Enqueue(v);
        return v;
    }

    public override void Add(Delegate d) => sys.Add((Action<T, T2, T3, T4, T5, T6, T7, T8>)d);
    public override void _invoke_update()
    {
        var tmp = cfq;
        cfq = ObjectPool.Get<Queue<ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8>>>();
        while (tmp.TryDequeue(out var cf))
        {
            if (cf.EnableCounter == 0)
            {
                for (int i = 0; i < sys.Count; i++)
                {
                    if (cf.Disposed) continue;
                    try { sys[i].Invoke(cf.t, cf.t2, cf.t3, cf.t4, cf.t5, cf.t6, cf.t7, cf.t8); }
                    catch (Exception e) { Loger.Error(e); }
                }
            }
            cfq.Enqueue(cf);
        }
        ObjectPool.Return(tmp);
    }
    public override void Invoke(ComponentFilter filter)
    {
        var cf = (ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8>)filter;
        for (int i = 0; i < sys.Count; i++)
        {
            if (cf.Disposed) continue;
            try { sys[i].Invoke(cf.t, cf.t2, cf.t3, cf.t4, cf.t5, cf.t6, cf.t7, cf.t8); }
            catch (Exception e) { Loger.Error(e); }
        }
    }
    public override Type _get_firstType() => typeof(T);
    internal override object GetActions() => sys;
}
internal partial class SystemHandler<T, T2, T3, T4, T5, T6, T7, T8, T9> : __SystemHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent
{
    internal List<Action<T, T2, T3, T4, T5, T6, T7, T8, T9>> sys = new(1);
    internal Queue<ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8, T9>> cfq = ObjectPool.Get<Queue<ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8, T9>>>();

    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
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
        var v = new ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8, T9>() { system = this, world = o.World, t = c, t2 = c2, t3 = c3, t4 = c4, t5 = c5, t6 = c6, t7 = c7, t8 = c8, t9 = c9 };
        if (!c.Enable) v.EnableCounter += 1;
        if (!c2.Enable) v.EnableCounter += 1;
        if (!c3.Enable) v.EnableCounter += 1;
        if (!c4.Enable) v.EnableCounter += 1;
        if (!c5.Enable) v.EnableCounter += 1;
        if (!c6.Enable) v.EnableCounter += 1;
        if (!c7.Enable) v.EnableCounter += 1;
        if (!c8.Enable) v.EnableCounter += 1;
        if (!c9.Enable) v.EnableCounter += 1;
        if (addToQueue) cfq.Enqueue(v);
        return v;
    }

    public override void Add(Delegate d) => sys.Add((Action<T, T2, T3, T4, T5, T6, T7, T8, T9>)d);
    public override void _invoke_update()
    {
        var tmp = cfq;
        cfq = ObjectPool.Get<Queue<ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8, T9>>>();
        while (tmp.TryDequeue(out var cf))
        {
            if (cf.EnableCounter == 0)
            {
                for (int i = 0; i < sys.Count; i++)
                {
                    if (cf.Disposed) continue;
                    try { sys[i].Invoke(cf.t, cf.t2, cf.t3, cf.t4, cf.t5, cf.t6, cf.t7, cf.t8, cf.t9); }
                    catch (Exception e) { Loger.Error(e); }
                }
            }
            cfq.Enqueue(cf);
        }
        ObjectPool.Return(tmp);
    }
    public override void Invoke(ComponentFilter filter)
    {
        var cf = (ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8, T9>)filter;
        for (int i = 0; i < sys.Count; i++)
        {
            if (cf.Disposed) continue;
            try { sys[i].Invoke(cf.t, cf.t2, cf.t3, cf.t4, cf.t5, cf.t6, cf.t7, cf.t8, cf.t9); }
            catch (Exception e) { Loger.Error(e); }
        }
    }
    public override Type _get_firstType() => typeof(T);
    internal override object GetActions() => sys;
}
internal partial class SystemHandler<T, T2, T3, T4, T5, T6, T7, T8, T9, T10> : __SystemHandle where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent where T10 : SComponent
{
    internal List<Action<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>> sys = new(1);
    internal Queue<ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>> cfq = ObjectPool.Get<Queue<ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>>>();

    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
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
        var v = new ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>() { system = this, world = o.World, t = c, t2 = c2, t3 = c3, t4 = c4, t5 = c5, t6 = c6, t7 = c7, t8 = c8, t9 = c9, t10 = c10 };
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
        if (addToQueue) cfq.Enqueue(v);
        return v;
    }

    public override void Add(Delegate d) => sys.Add((Action<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>)d);
    public override void _invoke_update()
    {
        var tmp = cfq;
        cfq = ObjectPool.Get<Queue<ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>>>();
        while (tmp.TryDequeue(out var cf))
        {
            if (cf.EnableCounter == 0)
            {
                for (int i = 0; i < sys.Count; i++)
                {
                    if (cf.Disposed) continue;
                    try { sys[i].Invoke(cf.t, cf.t2, cf.t3, cf.t4, cf.t5, cf.t6, cf.t7, cf.t8, cf.t9, cf.t10); }
                    catch (Exception e) { Loger.Error(e); }
                }
            }
            cfq.Enqueue(cf);
        }
        ObjectPool.Return(tmp);
    }
    public override void Invoke(ComponentFilter filter)
    {
        var cf = (ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>)filter;
        for (int i = 0; i < sys.Count; i++)
        {
            if (cf.Disposed) continue;
            try { sys[i].Invoke(cf.t, cf.t2, cf.t3, cf.t4, cf.t5, cf.t6, cf.t7, cf.t8, cf.t9, cf.t10); }
            catch (Exception e) { Loger.Error(e); }
        }
    }
    public override Type _get_firstType() => typeof(T);
    internal override object GetActions() => sys;
}
