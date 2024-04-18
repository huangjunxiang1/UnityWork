using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//begin#1
//public class Dispose<?[T]+, ?> : __DisposeHandle ?where [T] : SComponent+ ?
//{
//    public Dispose(?[T] [t]+, ?) { ?this.[t] = [t];+ ? }
//?    public [T] [t] { get; }+\r?
//
//    internal static void Invoke(object oo)
//    {
//        var c = (T)oo;
//        var o = c.Entity;
//        if (!o.World.Event.HasEvent(typeof(Dispose<?[T]+, ?>))) return;
//?        if (!o.TryGetComponent<[T]>(out var [c]) || ![c].Enable) return;+\r#2?
//        o.World.Event.RunEvent(new Dispose<?[T]+, ?>(?[c]+, ?));
//    }
//}
//end
