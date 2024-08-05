using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//begin#2
//public class AnyChange<?[T]+, ?> : __ChangeHandle ?where [T] : SComponent+ ?
//{
//    internal override bool Disposed => ?[t].Disposed+ || ?;
//    public AnyChange(?[T] [t]+, ?) { ?this.[t] = [t];+ ? }
//?    public [T] [t] { get; }+\r?
//
//    internal static void TryCreateHandle(SObject target)
//    {
//?        if (!target.TryGetComponent<[T]>(out var [c])) return;+\r?
//        var v = new AnyChange<?[T]+, ?>(?[c]+, ?);
//?        [c].AddChangeHandler(v);+\r?
//    }
//    internal override void Dispose()
//    {
//?        [t].World.System.AddToChangeWaitRemove([t]);+\r?
//    }
//    internal override void Invoke()
//    {
//        if (?![t].Enable+ || ?) return;
//        t.World.Event.RunEventNoGCAndFaster(this);
//    }
//}
//end

