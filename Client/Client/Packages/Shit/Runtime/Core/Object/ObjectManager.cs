using System;
using System.Collections.Generic;

namespace Core
{
    public class ObjectManager
    {
        Dictionary<long, List<SObject>> actorMap = new();
        Dictionary<long, SObject> gidMap = new();
        Queue<SObject> removed = new();

        internal void Add(SObject o)
        {
            gidMap.Add(o.gid, o);
            if (o.ActorId != 0)
            {
                if (!actorMap.TryGetValue(o.ActorId, out var lst))
                    actorMap[o.ActorId] = lst = ObjectPool.Get<List<SObject>>();
                lst.Add(o);
            }
        }
        internal void AddToComponentMapLst(Type t,SObject obj)
        {

        }
        internal void Remove(SObject o)
        {
            gidMap.Remove(o.gid);
            if (o.ActorId != 0)
                removed.Enqueue(o);
        }
        internal bool TryGetByActorId(long actorId, out List<SObject> lst) => actorMap.TryGetValue(actorId, out lst);
        public bool TryGetByGid(long gid, out SObject o) => gidMap.TryGetValue(gid, out o);

        internal void LateUpdate()
        {
            while (removed.TryDequeue(out var o))
            {
                if (actorMap.TryGetValue(o.ActorId, out var lst))
                {
                    lst.RemoveAll(t => t.Disposed);
                    if (lst.Count == 0)
                    {
                        actorMap.Remove(o.ActorId);
                        ObjectPool.Return(lst);
                    }
                }
            }
        }
    }
}
