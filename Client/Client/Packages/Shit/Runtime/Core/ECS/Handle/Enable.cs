using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Enable<T> : __EnableHandle where T : SComponent
{
    public Enable(T o) => t = o;
    public T t { get; }

    internal static void Invoke(SComponent c)
    {
        c.World.Event.RunEvent(new Enable<T>((T)c));
    }
}
