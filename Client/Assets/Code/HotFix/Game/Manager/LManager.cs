using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;

class LManager<T> : LEntity where T : LManager<T>, new()
{
    public static T Inst { get; } = new T();

    public void init()
    {

    }
}