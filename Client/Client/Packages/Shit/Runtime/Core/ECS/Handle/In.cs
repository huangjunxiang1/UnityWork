using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//begin#1
//public class In<?[T]+, ?> : __InHandle ?where [T] : SComponent+ ?
//{
//    public In(?[T] [t]+, ?) { ?this.[t] = [t];+ ? }
//?    public [T] [t] { get; }+\r?
//
//    internal static void Invoke(SObject o)
//    {
//        if (!o.World.Event.HasEvent(typeof(In<?[T]+, ?>))) return;
//
//?        if (!o.TryGetComponent<[T]>(out var [c]) || ![c].Enable) return;+\r?
//        o.World.Event.RunEvent(new In<?[T]+, ?>(?[c]+, ?));
// #if UNITY_EDITOR
//        o.World.Event.GetEventQueue(typeof(In<?[T]+, ?>), out var v);
//        o._In.AddRange(v);
// #endif
//    }
//}
//end
