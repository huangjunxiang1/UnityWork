using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//begin#1
//public class Awake<?[T]+, ?> : __AwakeHandle ?where [T] : SComponent+ ?
//{
//    public Awake(?[T] [t]+, ?) { ?this.[t] = [t];+ ? }
//?    public [T] [t] { get; }+\r?
//
//    internal static void Invoke(SObject o)
//    {
//        if (!o.World.Event.HasEvent(typeof(Awake<?[T]+, ?>))) return;
//
//?        if (!o.TryGetComponent<[T]>(out var [c]) || ![c].Enable) return;+\r?
//        o.World.Event.RunEvent(new Awake<?[T]+, ?>(?[c]+, ?));
//    }
//}
//end
