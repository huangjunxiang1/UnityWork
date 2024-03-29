using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//begin#2
//public class Awake<?[T]+, ?> : __AwakeHandle ?where [T] : SComponent+ ?
//{
//    public Awake(?[T] [t]+, ?) { ?this.[t] = [t];+ ? }
//?    public [T] [t] { get; }+\r?
//
//    static void Invoke(SObject o)
//    {
//        if (!o.World.Event.HasEvent(typeof(Awake<?[T]+, ?>))) return;
//
//?        if (!o.TryGetComponent<[T]>(out var [c])) return;+\r?
//        o.World.Event.RunEvent(new Awake<?[T]+, ?>(?[c]+, ?));
//    }
//}
//end
public class Awake<T> : __AwakeHandle
{
    public Awake(T o) => t = o;
    public T t { get; }

    internal static void Invoke(SObject o)
    {
        if (typeof(SComponent).IsAssignableFrom(typeof(T)))
        {
            if (!o.TryGetComponent(typeof(T), out var c)) return;
            o.World.Event.RunEvent(new Awake<T>((T)(object)c));
        }
        else
            o.World.Event.RunEvent(new Awake<T>((T)(object)o));
    }
}
