using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//begin#2
//public class Dispose<?[T]+, ?> : __DisposeHandle ?where [T] : SComponent+ ?
//{
//    public Dispose(?[T] [t]+, ?) { ?this.[t] = [t];+ ? }
//?    public [T] [t] { get; }+\r?
//
//    static void Invoke(object oo)
//    {
//        var c = (T)oo;
//        var o = c.Entity;
//        if (!o.World.Event.HasEvent(typeof(Dispose<?[T]+, ?>))) return;
//?        if (!o.TryGetComponent<[T]>(out var [c])) return;+\r#2?
//        o.World.Event.RunEvent(new Dispose<?[T]+, ?>(?[c]+, ?));
//    }
//}
//end
public class Dispose<T> : __DisposeHandle
{
    public Dispose(T o) => t = o;
    public T t { get; }

    internal static void Invoke(object oo)
    {
        if (typeof(SComponent).IsAssignableFrom(typeof(T)))
        {
            var c = (SComponent)oo;
            c.World.Event.RunEvent(new Dispose<T>((T)(object)c));
        }
        else
        {
            var o = (SObject)oo;
            o.World.Event.RunEvent(new Dispose<T>((T)oo));
        }
    }
}
