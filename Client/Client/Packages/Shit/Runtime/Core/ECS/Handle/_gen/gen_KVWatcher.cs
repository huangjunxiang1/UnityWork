using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KVWatcher<T> : __KVWatcher where T : SComponent
{
    public KVWatcher(KVComponent kv, T t) { this.kv = kv; this.t = t; }
    public KVComponent kv { get; }
    public T t { get; }

    internal static void TryCreateHandle(SObject o)
    {
        var kv = o.GetComponent<KVComponent>();
        if (!o.TryGetComponent<T>(out var c)) return;
        var v = new KVWatcher<T>(kv, c);
        if (kv._kvWatcherHandles == null) kv._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c._kvWatcherHandles == null) c._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        kv._kvWatcherHandles.Add(v);
        c._kvWatcherHandles.Add(v);
    }
    internal override void Dispose()
    {
        base.Dispose();
        kv.World.System.AddToKVWaitRemove(kv);
        t.World.System.AddToKVWaitRemove(t);
    }
    internal override void Invoke(int type)
    {
        if (!kv.Enable || !t.Enable) return;
        kv.World.Event.RunEventNoGCAndFaster(this, type);
    }
}
public class KVWatcher<T, T2> : __KVWatcher where T : SComponent where T2 : SComponent
{
    public KVWatcher(KVComponent kv, T t, T2 t2) { this.kv = kv; this.t = t; this.t2 = t2; }
    public KVComponent kv { get; }
    public T t { get; }
    public T2 t2 { get; }

    internal static void TryCreateHandle(SObject o)
    {
        var kv = o.GetComponent<KVComponent>();
        if (!o.TryGetComponent<T>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        var v = new KVWatcher<T, T2>(kv, c, c2);
        if (kv._kvWatcherHandles == null) kv._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c._kvWatcherHandles == null) c._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c2._kvWatcherHandles == null) c2._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        kv._kvWatcherHandles.Add(v);
        c._kvWatcherHandles.Add(v);
        c2._kvWatcherHandles.Add(v);
    }
    internal override void Dispose()
    {
        base.Dispose();
        kv.World.System.AddToKVWaitRemove(kv);
        t.World.System.AddToKVWaitRemove(t);
        t2.World.System.AddToKVWaitRemove(t2);
    }
    internal override void Invoke(int type)
    {
        if (!kv.Enable || !t.Enable || !t2.Enable) return;
        kv.World.Event.RunEventNoGCAndFaster(this, type);
    }
}
public class KVWatcher<T, T2, T3> : __KVWatcher where T : SComponent where T2 : SComponent where T3 : SComponent
{
    public KVWatcher(KVComponent kv, T t, T2 t2, T3 t3) { this.kv = kv; this.t = t; this.t2 = t2; this.t3 = t3; }
    public KVComponent kv { get; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }

    internal static void TryCreateHandle(SObject o)
    {
        var kv = o.GetComponent<KVComponent>();
        if (!o.TryGetComponent<T>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        var v = new KVWatcher<T, T2, T3>(kv, c, c2, c3);
        if (kv._kvWatcherHandles == null) kv._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c._kvWatcherHandles == null) c._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c2._kvWatcherHandles == null) c2._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c3._kvWatcherHandles == null) c3._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        kv._kvWatcherHandles.Add(v);
        c._kvWatcherHandles.Add(v);
        c2._kvWatcherHandles.Add(v);
        c3._kvWatcherHandles.Add(v);
    }
    internal override void Dispose()
    {
        base.Dispose();
        kv.World.System.AddToKVWaitRemove(kv);
        t.World.System.AddToKVWaitRemove(t);
        t2.World.System.AddToKVWaitRemove(t2);
        t3.World.System.AddToKVWaitRemove(t3);
    }
    internal override void Invoke(int type)
    {
        if (!kv.Enable || !t.Enable || !t2.Enable || !t3.Enable) return;
        kv.World.Event.RunEventNoGCAndFaster(this, type);
    }
}
public class KVWatcher<T, T2, T3, T4> : __KVWatcher where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent
{
    public KVWatcher(KVComponent kv, T t, T2 t2, T3 t3, T4 t4) { this.kv = kv; this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; }
    public KVComponent kv { get; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }

    internal static void TryCreateHandle(SObject o)
    {
        var kv = o.GetComponent<KVComponent>();
        if (!o.TryGetComponent<T>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        if (!o.TryGetComponent<T4>(out var c4)) return;
        var v = new KVWatcher<T, T2, T3, T4>(kv, c, c2, c3, c4);
        if (kv._kvWatcherHandles == null) kv._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c._kvWatcherHandles == null) c._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c2._kvWatcherHandles == null) c2._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c3._kvWatcherHandles == null) c3._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c4._kvWatcherHandles == null) c4._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        kv._kvWatcherHandles.Add(v);
        c._kvWatcherHandles.Add(v);
        c2._kvWatcherHandles.Add(v);
        c3._kvWatcherHandles.Add(v);
        c4._kvWatcherHandles.Add(v);
    }
    internal override void Dispose()
    {
        base.Dispose();
        kv.World.System.AddToKVWaitRemove(kv);
        t.World.System.AddToKVWaitRemove(t);
        t2.World.System.AddToKVWaitRemove(t2);
        t3.World.System.AddToKVWaitRemove(t3);
        t4.World.System.AddToKVWaitRemove(t4);
    }
    internal override void Invoke(int type)
    {
        if (!kv.Enable || !t.Enable || !t2.Enable || !t3.Enable || !t4.Enable) return;
        kv.World.Event.RunEventNoGCAndFaster(this, type);
    }
}
public class KVWatcher<T, T2, T3, T4, T5> : __KVWatcher where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent
{
    public KVWatcher(KVComponent kv, T t, T2 t2, T3 t3, T4 t4, T5 t5) { this.kv = kv; this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; }
    public KVComponent kv { get; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }

    internal static void TryCreateHandle(SObject o)
    {
        var kv = o.GetComponent<KVComponent>();
        if (!o.TryGetComponent<T>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        if (!o.TryGetComponent<T4>(out var c4)) return;
        if (!o.TryGetComponent<T5>(out var c5)) return;
        var v = new KVWatcher<T, T2, T3, T4, T5>(kv, c, c2, c3, c4, c5);
        if (kv._kvWatcherHandles == null) kv._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c._kvWatcherHandles == null) c._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c2._kvWatcherHandles == null) c2._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c3._kvWatcherHandles == null) c3._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c4._kvWatcherHandles == null) c4._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c5._kvWatcherHandles == null) c5._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        kv._kvWatcherHandles.Add(v);
        c._kvWatcherHandles.Add(v);
        c2._kvWatcherHandles.Add(v);
        c3._kvWatcherHandles.Add(v);
        c4._kvWatcherHandles.Add(v);
        c5._kvWatcherHandles.Add(v);
    }
    internal override void Dispose()
    {
        base.Dispose();
        kv.World.System.AddToKVWaitRemove(kv);
        t.World.System.AddToKVWaitRemove(t);
        t2.World.System.AddToKVWaitRemove(t2);
        t3.World.System.AddToKVWaitRemove(t3);
        t4.World.System.AddToKVWaitRemove(t4);
        t5.World.System.AddToKVWaitRemove(t5);
    }
    internal override void Invoke(int type)
    {
        if (!kv.Enable || !t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable) return;
        kv.World.Event.RunEventNoGCAndFaster(this, type);
    }
}
public class KVWatcher<T, T2, T3, T4, T5, T6> : __KVWatcher where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent
{
    public KVWatcher(KVComponent kv, T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) { this.kv = kv; this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; }
    public KVComponent kv { get; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }

    internal static void TryCreateHandle(SObject o)
    {
        var kv = o.GetComponent<KVComponent>();
        if (!o.TryGetComponent<T>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        if (!o.TryGetComponent<T4>(out var c4)) return;
        if (!o.TryGetComponent<T5>(out var c5)) return;
        if (!o.TryGetComponent<T6>(out var c6)) return;
        var v = new KVWatcher<T, T2, T3, T4, T5, T6>(kv, c, c2, c3, c4, c5, c6);
        if (kv._kvWatcherHandles == null) kv._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c._kvWatcherHandles == null) c._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c2._kvWatcherHandles == null) c2._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c3._kvWatcherHandles == null) c3._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c4._kvWatcherHandles == null) c4._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c5._kvWatcherHandles == null) c5._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c6._kvWatcherHandles == null) c6._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        kv._kvWatcherHandles.Add(v);
        c._kvWatcherHandles.Add(v);
        c2._kvWatcherHandles.Add(v);
        c3._kvWatcherHandles.Add(v);
        c4._kvWatcherHandles.Add(v);
        c5._kvWatcherHandles.Add(v);
        c6._kvWatcherHandles.Add(v);
    }
    internal override void Dispose()
    {
        base.Dispose();
        kv.World.System.AddToKVWaitRemove(kv);
        t.World.System.AddToKVWaitRemove(t);
        t2.World.System.AddToKVWaitRemove(t2);
        t3.World.System.AddToKVWaitRemove(t3);
        t4.World.System.AddToKVWaitRemove(t4);
        t5.World.System.AddToKVWaitRemove(t5);
        t6.World.System.AddToKVWaitRemove(t6);
    }
    internal override void Invoke(int type)
    {
        if (!kv.Enable || !t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable || !t6.Enable) return;
        kv.World.Event.RunEventNoGCAndFaster(this, type);
    }
}
public class KVWatcher<T, T2, T3, T4, T5, T6, T7> : __KVWatcher where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent
{
    public KVWatcher(KVComponent kv, T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) { this.kv = kv; this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; }
    public KVComponent kv { get; }
    public T t { get; }
    public T2 t2 { get; }
    public T3 t3 { get; }
    public T4 t4 { get; }
    public T5 t5 { get; }
    public T6 t6 { get; }
    public T7 t7 { get; }

    internal static void TryCreateHandle(SObject o)
    {
        var kv = o.GetComponent<KVComponent>();
        if (!o.TryGetComponent<T>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        if (!o.TryGetComponent<T4>(out var c4)) return;
        if (!o.TryGetComponent<T5>(out var c5)) return;
        if (!o.TryGetComponent<T6>(out var c6)) return;
        if (!o.TryGetComponent<T7>(out var c7)) return;
        var v = new KVWatcher<T, T2, T3, T4, T5, T6, T7>(kv, c, c2, c3, c4, c5, c6, c7);
        if (kv._kvWatcherHandles == null) kv._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c._kvWatcherHandles == null) c._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c2._kvWatcherHandles == null) c2._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c3._kvWatcherHandles == null) c3._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c4._kvWatcherHandles == null) c4._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c5._kvWatcherHandles == null) c5._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c6._kvWatcherHandles == null) c6._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c7._kvWatcherHandles == null) c7._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        kv._kvWatcherHandles.Add(v);
        c._kvWatcherHandles.Add(v);
        c2._kvWatcherHandles.Add(v);
        c3._kvWatcherHandles.Add(v);
        c4._kvWatcherHandles.Add(v);
        c5._kvWatcherHandles.Add(v);
        c6._kvWatcherHandles.Add(v);
        c7._kvWatcherHandles.Add(v);
    }
    internal override void Dispose()
    {
        base.Dispose();
        kv.World.System.AddToKVWaitRemove(kv);
        t.World.System.AddToKVWaitRemove(t);
        t2.World.System.AddToKVWaitRemove(t2);
        t3.World.System.AddToKVWaitRemove(t3);
        t4.World.System.AddToKVWaitRemove(t4);
        t5.World.System.AddToKVWaitRemove(t5);
        t6.World.System.AddToKVWaitRemove(t6);
        t7.World.System.AddToKVWaitRemove(t7);
    }
    internal override void Invoke(int type)
    {
        if (!kv.Enable || !t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable || !t6.Enable || !t7.Enable) return;
        kv.World.Event.RunEventNoGCAndFaster(this, type);
    }
}
public class KVWatcher<T, T2, T3, T4, T5, T6, T7, T8> : __KVWatcher where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent
{
    public KVWatcher(KVComponent kv, T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) { this.kv = kv; this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; }
    public KVComponent kv { get; }
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
        var kv = o.GetComponent<KVComponent>();
        if (!o.TryGetComponent<T>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        if (!o.TryGetComponent<T4>(out var c4)) return;
        if (!o.TryGetComponent<T5>(out var c5)) return;
        if (!o.TryGetComponent<T6>(out var c6)) return;
        if (!o.TryGetComponent<T7>(out var c7)) return;
        if (!o.TryGetComponent<T8>(out var c8)) return;
        var v = new KVWatcher<T, T2, T3, T4, T5, T6, T7, T8>(kv, c, c2, c3, c4, c5, c6, c7, c8);
        if (kv._kvWatcherHandles == null) kv._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c._kvWatcherHandles == null) c._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c2._kvWatcherHandles == null) c2._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c3._kvWatcherHandles == null) c3._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c4._kvWatcherHandles == null) c4._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c5._kvWatcherHandles == null) c5._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c6._kvWatcherHandles == null) c6._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c7._kvWatcherHandles == null) c7._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c8._kvWatcherHandles == null) c8._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        kv._kvWatcherHandles.Add(v);
        c._kvWatcherHandles.Add(v);
        c2._kvWatcherHandles.Add(v);
        c3._kvWatcherHandles.Add(v);
        c4._kvWatcherHandles.Add(v);
        c5._kvWatcherHandles.Add(v);
        c6._kvWatcherHandles.Add(v);
        c7._kvWatcherHandles.Add(v);
        c8._kvWatcherHandles.Add(v);
    }
    internal override void Dispose()
    {
        base.Dispose();
        kv.World.System.AddToKVWaitRemove(kv);
        t.World.System.AddToKVWaitRemove(t);
        t2.World.System.AddToKVWaitRemove(t2);
        t3.World.System.AddToKVWaitRemove(t3);
        t4.World.System.AddToKVWaitRemove(t4);
        t5.World.System.AddToKVWaitRemove(t5);
        t6.World.System.AddToKVWaitRemove(t6);
        t7.World.System.AddToKVWaitRemove(t7);
        t8.World.System.AddToKVWaitRemove(t8);
    }
    internal override void Invoke(int type)
    {
        if (!kv.Enable || !t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable || !t6.Enable || !t7.Enable || !t8.Enable) return;
        kv.World.Event.RunEventNoGCAndFaster(this, type);
    }
}
public class KVWatcher<T, T2, T3, T4, T5, T6, T7, T8, T9> : __KVWatcher where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent
{
    public KVWatcher(KVComponent kv, T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9) { this.kv = kv; this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; this.t9 = t9; }
    public KVComponent kv { get; }
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
        var kv = o.GetComponent<KVComponent>();
        if (!o.TryGetComponent<T>(out var c)) return;
        if (!o.TryGetComponent<T2>(out var c2)) return;
        if (!o.TryGetComponent<T3>(out var c3)) return;
        if (!o.TryGetComponent<T4>(out var c4)) return;
        if (!o.TryGetComponent<T5>(out var c5)) return;
        if (!o.TryGetComponent<T6>(out var c6)) return;
        if (!o.TryGetComponent<T7>(out var c7)) return;
        if (!o.TryGetComponent<T8>(out var c8)) return;
        if (!o.TryGetComponent<T9>(out var c9)) return;
        var v = new KVWatcher<T, T2, T3, T4, T5, T6, T7, T8, T9>(kv, c, c2, c3, c4, c5, c6, c7, c8, c9);
        if (kv._kvWatcherHandles == null) kv._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c._kvWatcherHandles == null) c._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c2._kvWatcherHandles == null) c2._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c3._kvWatcherHandles == null) c3._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c4._kvWatcherHandles == null) c4._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c5._kvWatcherHandles == null) c5._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c6._kvWatcherHandles == null) c6._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c7._kvWatcherHandles == null) c7._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c8._kvWatcherHandles == null) c8._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c9._kvWatcherHandles == null) c9._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        kv._kvWatcherHandles.Add(v);
        c._kvWatcherHandles.Add(v);
        c2._kvWatcherHandles.Add(v);
        c3._kvWatcherHandles.Add(v);
        c4._kvWatcherHandles.Add(v);
        c5._kvWatcherHandles.Add(v);
        c6._kvWatcherHandles.Add(v);
        c7._kvWatcherHandles.Add(v);
        c8._kvWatcherHandles.Add(v);
        c9._kvWatcherHandles.Add(v);
    }
    internal override void Dispose()
    {
        base.Dispose();
        kv.World.System.AddToKVWaitRemove(kv);
        t.World.System.AddToKVWaitRemove(t);
        t2.World.System.AddToKVWaitRemove(t2);
        t3.World.System.AddToKVWaitRemove(t3);
        t4.World.System.AddToKVWaitRemove(t4);
        t5.World.System.AddToKVWaitRemove(t5);
        t6.World.System.AddToKVWaitRemove(t6);
        t7.World.System.AddToKVWaitRemove(t7);
        t8.World.System.AddToKVWaitRemove(t8);
        t9.World.System.AddToKVWaitRemove(t9);
    }
    internal override void Invoke(int type)
    {
        if (!kv.Enable || !t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable || !t6.Enable || !t7.Enable || !t8.Enable || !t9.Enable) return;
        kv.World.Event.RunEventNoGCAndFaster(this, type);
    }
}
public class KVWatcher<T, T2, T3, T4, T5, T6, T7, T8, T9, T10> : __KVWatcher where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent where T10 : SComponent
{
    public KVWatcher(KVComponent kv, T t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10) { this.kv = kv; this.t = t; this.t2 = t2; this.t3 = t3; this.t4 = t4; this.t5 = t5; this.t6 = t6; this.t7 = t7; this.t8 = t8; this.t9 = t9; this.t10 = t10; }
    public KVComponent kv { get; }
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
        var kv = o.GetComponent<KVComponent>();
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
        var v = new KVWatcher<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>(kv, c, c2, c3, c4, c5, c6, c7, c8, c9, c10);
        if (kv._kvWatcherHandles == null) kv._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c._kvWatcherHandles == null) c._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c2._kvWatcherHandles == null) c2._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c3._kvWatcherHandles == null) c3._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c4._kvWatcherHandles == null) c4._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c5._kvWatcherHandles == null) c5._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c6._kvWatcherHandles == null) c6._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c7._kvWatcherHandles == null) c7._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c8._kvWatcherHandles == null) c8._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c9._kvWatcherHandles == null) c9._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        if (c10._kvWatcherHandles == null) c10._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
        kv._kvWatcherHandles.Add(v);
        c._kvWatcherHandles.Add(v);
        c2._kvWatcherHandles.Add(v);
        c3._kvWatcherHandles.Add(v);
        c4._kvWatcherHandles.Add(v);
        c5._kvWatcherHandles.Add(v);
        c6._kvWatcherHandles.Add(v);
        c7._kvWatcherHandles.Add(v);
        c8._kvWatcherHandles.Add(v);
        c9._kvWatcherHandles.Add(v);
        c10._kvWatcherHandles.Add(v);
    }
    internal override void Dispose()
    {
        base.Dispose();
        kv.World.System.AddToKVWaitRemove(kv);
        t.World.System.AddToKVWaitRemove(t);
        t2.World.System.AddToKVWaitRemove(t2);
        t3.World.System.AddToKVWaitRemove(t3);
        t4.World.System.AddToKVWaitRemove(t4);
        t5.World.System.AddToKVWaitRemove(t5);
        t6.World.System.AddToKVWaitRemove(t6);
        t7.World.System.AddToKVWaitRemove(t7);
        t8.World.System.AddToKVWaitRemove(t8);
        t9.World.System.AddToKVWaitRemove(t9);
        t10.World.System.AddToKVWaitRemove(t10);
    }
    internal override void Invoke(int type)
    {
        if (!kv.Enable || !t.Enable || !t2.Enable || !t3.Enable || !t4.Enable || !t5.Enable || !t6.Enable || !t7.Enable || !t8.Enable || !t9.Enable || !t10.Enable) return;
        kv.World.Event.RunEventNoGCAndFaster(this, type);
    }
}
