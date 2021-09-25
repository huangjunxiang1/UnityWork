using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main;

namespace Game
{
    public class Manager<T> : Entity where T : Manager<T>, new()
    {
        public static T Inst { get; } = new T();

        public void init()
        {

        }
    }
}
