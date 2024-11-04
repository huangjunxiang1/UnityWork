using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//begin#1
//public class LateUpdate<?[T]+, ?> : __LateUpdateHandle ?where [T] : SComponent+ ?
//{
//    public LateUpdate(?[T] [t]+, ?) { ?this.[t] = [t];+ ? }
//?    public [T] [t] { get; }+\r?
//
//    internal static __UpdateHandle TryCreateHandle(SObject o)
//    {
//?        if (!o.TryGetComponent<[T]>(out var [c])) return null;+\r?
//        return new LateUpdate<?[T]+, ?>(?[c]+, ?);
//    }
//    internal override bool Disposed => ?[t].Disposed+ || ?;
//    internal override void Invoke()
//    {
//        if (?![t].Enable+ || ?) return;
//        t.World.Event.RunEventNoGCAndFaster(this);
//    }
//}
//end
public class LateUpdate
{
    private LateUpdate() { }
    static LateUpdate Default = new();
    internal static void Invoke(World world) => world.Event.RunEventNoGCAndFaster(Default);
}

