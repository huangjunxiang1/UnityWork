using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoCreateFormSceneTypeAttribute : SAttribute
    {
        public int type { get; }
        public AutoCreateFormSceneTypeAttribute(int type)
        {
            this.type = type;
        }
    }
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoCreateFormSceneIDAttribute : SAttribute
    {
        public int ID { get; }
        public AutoCreateFormSceneIDAttribute(int id)
        {
            this.ID = id;
        }
    }
}