using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class KVComponent : SComponent
{
    [Sirenix.OdinInspector.ShowInInspector]
    public Dictionary<int, long> Values { get; } = new();

    HashSet<int> changedIDs = ObjectPool.Get<HashSet<int>>();

    public bool Has(int id) => Values.ContainsKey(id);
    public bool TryGet(int id, out long v) => Values.TryGetValue(id, out v);
    public void Set(IDictionary<int,long> kvs)
    {
        foreach (var item in kvs)
        {
            if (!Values.TryGetValue(item.Key, out var v) || v != item.Value)
            {
                Values[item.Key] = item.Value;
                changedIDs.Add(item.Key);
                continue;
            }
        }
        foreach (var item in Values)
        {
            if (!kvs.ContainsKey(item.Key))
            {
                Values.Remove(item.Key);
                changedIDs.Add(item.Key);
            }
        }
        this.SetChange();
    }
    public void Set(int id, long v)
    {
        if (Values.TryGetValue(id, out var old) && old == v) return;

        changedIDs.Add(id);

        Values[id] = v;
        this.SetChange();
    }
    public void Add(int id, long v)
    {
        changedIDs.Add(id);

        Values.TryGetValue(id, out var old);
        Values[id] = old + v;
        this.SetChange();
    }
    public void Remove(int id)
    {
        if (!Values.ContainsKey(id)) return;

        changedIDs.Add(id);

        Values.Remove(id);
        this.SetChange();
    }
    public void Clear()
    {
        if (this.Values.Count == 0) return;

        foreach (var id in Values.Keys)
            changedIDs.Add(id);

        Values.Clear();
        this.SetChange();
    }
    public long Get(int id)
    {
        Values.TryGetValue(id, out long v);
        return v;
    }

    [Event]
    static void Change(Change<KVComponent> t)
    {
        if (t.t._kvWatcherHandles != null)
        {
            var tmp = t.t.changedIDs;
            t.t.changedIDs = ObjectPool.Get<HashSet<int>>();
            foreach (var id in tmp)
            {
                int len = t.t._kvWatcherHandles.Count;
                for (int i = 0; i < len; i++)
                {
                    if (t.t._kvWatcherHandles[i].Disposed) continue;
                    t.t._kvWatcherHandles[i].Invoke(id);
                }
            }
            tmp.Clear();
            ObjectPool.Return(tmp);
        }
    }
    [Event]
    static void Dispose(Dispose<KVComponent> t)
    {
        t.t.changedIDs.Clear();
        ObjectPool.Return(t.t.changedIDs);
    }
}
