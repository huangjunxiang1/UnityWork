using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;

class ManagerL<T> : EntityL where T : ManagerL<T>, new()
{
    public static T Inst { get; } = new T();

    public virtual void init()
    {

    }
}