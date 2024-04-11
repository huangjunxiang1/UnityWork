using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//begin#2
//public class EventWatcher<?[T]+, ?> : __EventWatcher where T : class ?where [T] : SComponent+ #2?
//{
//    public EventWatcher(?[T] [t]+, ?) { ?this.[t] = [t];+ ? }
//?    public [T] [t] { get; }+\r?
//
//    internal static void Invoke(object e, SObject o)
//    {
//?        if (!o.TryGetComponent<[T]>(out var [c]) || ![c].Enable) return;+\r#2?
//        o.World.Event.RunEvent(new EventWatcher<?[T]+, ?>((T)e, ?[c]+, #2?));
//    }
//}
//end