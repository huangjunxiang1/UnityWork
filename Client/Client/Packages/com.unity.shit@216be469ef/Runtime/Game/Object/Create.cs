using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Create : SAttribute
    {
        public int type { get; }
        public int ID { get; }
        public Create(int type = 0, int id = 0)
        {
            this.type = type;
            this.ID = id;
        }
    }
}