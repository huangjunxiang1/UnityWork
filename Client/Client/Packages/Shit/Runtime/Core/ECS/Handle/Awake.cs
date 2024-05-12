using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Awake<T> : GenericEvent where T : SComponent
{
    public Awake(T t) { this.t = t; }
    public T t { get; }
}