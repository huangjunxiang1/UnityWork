using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class AttributeComponent : SComponent
    {
        Dictionary<int, long> Values = new();

        public void Set(int id, long v)
        {
            Values[id] = v;
            this.SetChange();
        }
        public void Remove(int id)
        {
            Values.Remove(id);
            this.SetChange();
        }
        public void Clear()
        {
            Values.Clear();
            this.SetChange();
        }
        public long Get(int id)
        {
            Values.TryGetValue(id, out long v);
            return v;
        }
    }
}
