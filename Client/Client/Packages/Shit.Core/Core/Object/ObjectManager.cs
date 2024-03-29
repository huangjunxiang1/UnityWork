using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ObjectManager
    {
        Dictionary<long, List<SObject>> map = new();
        Queue<SObject> removed = new();

        public void Add(SObject o)
        {
            if (o.rpc == 0) return;
            if (!map.TryGetValue(o.rpc, out var lst))
                map[o.rpc] = lst = ObjectPool.Get<List<SObject>>();
            lst.Add(o);
        }
        public void Remove(SObject o) => removed.Enqueue(o);
        public bool TryGetByRpc(long rpc, out List<SObject> lst) => map.TryGetValue(rpc, out lst);

        internal void AfterUpdate()
        {
            while (removed.TryDequeue(out var o))
            {
                if (o.rpc == 0) continue;
                if (map.TryGetValue(o.rpc, out var lst))
                {
                    lst.RemoveAll(t => t.Disposed);
                    if (lst.Count == 0)
                    {
                        map.Remove(o.rpc);
                        ObjectPool.Return(lst);
                    }
                }
            }
        }
    }
}
