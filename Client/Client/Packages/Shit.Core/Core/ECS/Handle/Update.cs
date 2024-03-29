using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//begin#1
//public class Update<?[T]+, ?> : __UpdateHandle ?where [T] : SComponent+ ?
//{
//    public Update(?[T] [t]+, ?) { ?this.[t] = [t];+ ? }
//?    public [T] [t] { get; }+\r?
//
//    internal static __UpdateHandle TryCreateHandle(SObject o)
//    {
//?        if (!o.TryGetComponent<[T]>(out var [c])) return null;+\r?
//        return new Update<?[T]+, ?>(?[c]+, ?);
//    }
//    internal override bool IsValid() => ?![t].Disposed+ && ?;
//    internal override void Invoke()
//    {
//        if (?![t].Enable+ || ?) return;
//        t.World.Event.RunEventNoGCAndFaster(this);
//    }
//}
//end
public class Update
{
    private Update() { }
    static Update Default = new();
    internal static void Invoke(World world) => world.Event.RunEventNoGCAndFaster(Default);
}

