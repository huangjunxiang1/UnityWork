using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Dispose<T> : GenericEvent where T : SComponent
{
    public Dispose(T t) { this.t = t; }
    public T t { get; }
}
