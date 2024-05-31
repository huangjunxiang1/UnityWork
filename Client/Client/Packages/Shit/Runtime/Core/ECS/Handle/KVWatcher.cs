using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//begin#1
//public class KVWatcher<?[T]+, ?> : __KVWatcher ?where [T] : SComponent+ ?
//{
//    public KVWatcher(KVComponent kv, ?[T] [t]+, ?) { this.kv = kv; ?this.[t] = [t];+ ? }
//    public KVComponent kv { get; }
//?    public [T] [t] { get; }+\r?
//
//    internal static void TryCreateHandle(SObject o)
//    {
//        var kv = o.GetComponent<KVComponent>();
//?        if (!o.TryGetComponent<[T]>(out var [c])) return;+\r?
//        var v = new KVWatcher<?[T]+, ?>(kv, ?[c]+, ?);
//        if (kv._kvWatcherHandles == null) kv._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();
//?        if ([c]._kvWatcherHandles == null) [c]._kvWatcherHandles = ObjectPool.Get<List<__KVWatcher>>();+\r?
//        kv._kvWatcherHandles.Add(v);
//?        [c]._kvWatcherHandles.Add(v);+\r?
//    }
//    internal override void Dispose()
//    {
//        base.Dispose();
//        kv.World.System.AddToKVWaitRemove(kv);
//?        [t].World.System.AddToKVWaitRemove([t]);+\r?
//    }
//    internal override void Invoke(int type)
//    {
//        if (!kv.Enable || ?![t].Enable+ || ?) return;
//        kv.World.Event.RunEventNoGCAndFaster(this, type);
//    }
//}
//end
public class KVWatcher : __KVWatcher
{
    public KVWatcher(KVComponent kv) { this.kv = kv; }
    public KVComponent kv { get; }

    internal static void TryCreateHandle(SObject target)
    {
        var kv = target.GetComponent<KVComponent>();
        var v = new KVWatcher(kv);
        (kv._kvWatcherHandles ??= ObjectPool.Get<List<__KVWatcher>>()).Add(v);
    }
    internal override void Dispose()
    {
        base.Dispose();
        kv.World.System.AddToKVWaitRemove(kv);
    }
    internal override void Invoke(int type)
    {
        if (!kv.Enable) return;
        kv.World.Event.RunEventNoGCAndFaster(this, type);
    }
}