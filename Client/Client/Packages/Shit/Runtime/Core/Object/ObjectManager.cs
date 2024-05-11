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
        Dictionary<long, SObject> gidMap = new();
        Queue<SObject> removed = new();
        Dictionary<Type, List<SObject>> eventWatcherFirstType = new(100);
        HashSet<Type> eventWatcherRemoved = new();

        internal void AddEventWatcherFirstType(Type t)
        {
            if (eventWatcherFirstType.ContainsKey(t)) return;
            eventWatcherFirstType.Add(t, new List<SObject>());
        }
        internal void Add(SObject o)
        {
            gidMap.Add(o.gid, o);
            if (o.rpc == 0) return;
            if (!map.TryGetValue(o.rpc, out var lst))
                map[o.rpc] = lst = ObjectPool.Get<List<SObject>>();
            lst.Add(o);
        }
        internal void AddComponent(SComponent c, Type t)
        {
            if (eventWatcherFirstType.TryGetValue(t, out var value))
                value.Add(c.Entity);
        }
        internal List<SObject> GetObjectsByComponentType(Type t)
        {
            eventWatcherFirstType.TryGetValue(t, out var value);
            return value;
        }
        internal void Remove(SObject o)
        {
            gidMap.Remove(o.gid);
            removed.Enqueue(o);
            foreach (var item in o._components.Keys)
            {
                if (eventWatcherFirstType.ContainsKey(item))
                    eventWatcherRemoved.Add(item);
            }
        }
        internal bool TryGetByRpc(long rpc, out List<SObject> lst) => map.TryGetValue(rpc, out lst);
        internal bool TryGetByGid(long gid, out SObject o) => gidMap.TryGetValue(gid, out o);

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
            if (eventWatcherRemoved.Count > 0)
            {
                foreach (var item in eventWatcherRemoved)
                    eventWatcherFirstType[item].RemoveAll(t => t.Disposed);
                eventWatcherRemoved.Clear();
            }
        }
    }
}
