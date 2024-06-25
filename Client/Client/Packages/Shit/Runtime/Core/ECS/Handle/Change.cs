using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//begin#1
//public class Change<?[T]+, ?> : __ChangeHandle ?where [T] : SComponent+ ?
//{
//    internal override bool Disposed => ?[t].Disposed+ || ?;
//    public Change(?[T] [t]+, ?) { ?this.[t] = [t];+ ? }
//?    public [T] [t] { get; }+\r?
//
//    internal static void TryCreateHandle(SObject o)
//    {
//?        if (!o.TryGetComponent<[T]>(out var [c])) return;+\r?
//        var v = new Change<?[T]+, ?>(?[c]+, ?);
//        if (c._changeHandles == null) c._changeHandles = ObjectPool.Get<List<__ChangeHandle>>();
//        c._changeHandles.Add(v);
//        o.World.System.AddToChangeWaitInvoke(v);
//    }
//    internal override void Dispose()
//    {
//        t.World.System.AddToChangeWaitRemove(t);
//    }
//    internal override void Invoke()
//    {
//        ?[t]._setChanged+ = ? = false;
//        if (this.Disposed || ?![t].Enable+ || ?) return;
//        t.World.Event.RunEventNoGCAndFaster(this);
//    }
//}
//end