using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//begin#1
//public class Out<?[T]+, ?> : __OutHandle ?where [T] : SComponent+ ?
//{
//    public Out(?[T] [t]+, ?) { ?this.[t] = [t];+ ? }
//?    public [T] [t] { get; }+\r?
//
//    internal static void Invoke(Type type, SComponent c0)
//    {
//        ?[T] [c]+;?;
//?        if (typeof([T]) == type) [c] = c0 as [T]; else { if (!c0.Entity.TryGetComponent(out [c]) || ![c].Enable) return; }+\r?
//        c0.Entity.World.Event.RunEvent(new Out<?[T]+, ?>(?[c]+, ?));
//    }
//    internal static void Invoke2(SObject o, Dictionary<Type, __OutHandle> record)
//    {
//        if (record.ContainsKey(typeof(Out<?[T]+, ?>))) return;
//?        if (!o.Entity.TryGetComponent<[T]>(out var [c]) || ![c].Enable) return;+\r?
//        record.Add(typeof(Out<?[T]+, ?>), new Out<?[T]+, ?>(?[c]+, ?));
//    }
//    internal override void Invoke(SObject o) => o.World.Event.RunEventNoGCAndFaster(this);
//}
//end
