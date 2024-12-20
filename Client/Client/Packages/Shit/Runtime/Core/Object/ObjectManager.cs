﻿using System;
using System.Collections.Generic;

namespace Core
{
    public class ObjectManager
    {
        Dictionary<long, List<SObject>> rpcMap = new();
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
            if (o.rpc != 0)
            {
                if (!rpcMap.TryGetValue(o.rpc, out var lst))
                    rpcMap[o.rpc] = lst = ObjectPool.Get<List<SObject>>();
                lst.Add(o);
            }
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
            if (o.rpc != 0)
                removed.Enqueue(o);
            foreach (var item in o._components.Keys)
            {
                if (eventWatcherFirstType.ContainsKey(item))
                    eventWatcherRemoved.Add(item);
            }
        }
        internal bool TryGetByRpc(long rpc, out List<SObject> lst) => rpcMap.TryGetValue(rpc, out lst);
        public bool TryGetByGid(long gid, out SObject o) => gidMap.TryGetValue(gid, out o);

        internal void LateUpdate()
        {
            while (removed.TryDequeue(out var o))
            {
                if (rpcMap.TryGetValue(o.rpc, out var lst))
                {
                    lst.RemoveAll(t => t.Disposed);
                    if (lst.Count == 0)
                    {
                        rpcMap.Remove(o.rpc);
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
