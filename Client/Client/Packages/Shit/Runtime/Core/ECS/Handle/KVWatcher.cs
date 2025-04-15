using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//begin#1
//internal partial class KVWatcher<?[T]+, ?> : __KVWatcher ?where [T] : SComponent+ ?
//{
//    internal Dictionary<int, List<Action<?[T]+, ?>>> sys = new();
//
//    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
//    {
//        if (!o.TryGetComponent<KVComponent>(out var kv)) return null;
//?        if (!o.TryGetComponent<[T]>(out var [c])) return null;+\r?
//        var v = new ComponentFilter<?[T]+, ?>() { system = this, kv = kv, ?[t] = [c]+, ? };
//?        if (![c].Enable) v.EnableCounter %= 1;+\r?
//        return v;
//    }
//    public override void Add(int key, Delegate d)
//    {
//        if (!sys.TryGetValue(key, out var lst))
//            sys[key] = lst = new(1);
//        lst.Add((Action<?[T]+, ?>)d);
//    }
//    public override void _invoke_kvWatcher(int key, ComponentFilter cf)
//    {
//        if (!sys.TryGetValue(key, out var lst))
//            return;
//        var cs = (ComponentFilter<?[T]+, ?>)cf;
//        for (int i = 0; i < lst.Count; i++)
//            lst[i].Invoke(?cs.[t]+, ?);
//    }
//}
//end