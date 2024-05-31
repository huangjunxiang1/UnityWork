using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class KVComponent : SComponent
{
    [Sirenix.OdinInspector.ShowInInspector]
    Dictionary<int, long> Values = ObjectPool.Get<Dictionary<int, long>>();

    Dictionary<int, long> changeKV = ObjectPool.Get<Dictionary<int, long>>();

    public bool Has(int id) => Values.ContainsKey(id);
    public bool TryGet(int id, out long v) => Values.TryGetValue(id, out v);
    public void Set(IDictionary<int,long> kvs)
    {
        foreach (var item in kvs)
        {
            if (!Values.TryGetValue(item.Key, out var old) || old != item.Value)
            {
                if (!changeKV.ContainsKey(item.Key))
                    changeKV.Add(item.Key, old);
                Values[item.Key] = item.Value;
            }
        }
        foreach (var item in Values)
        {
            if (!kvs.ContainsKey(item.Key))
            {
                if (!changeKV.ContainsKey(item.Key))
                    changeKV.Add(item.Key, item.Value);
                Values.Remove(item.Key);
            }
        }
        this.SetChange();
    }
    public void Set(int id, long v)
    {
        if (Values.TryGetValue(id, out var old) && old == v) return;

        if (!changeKV.ContainsKey(id))
            changeKV.Add(id, old);

        Values[id] = v;
        this.SetChange();
    }
    public void Add(int id, long v)
    {
        if (Values.TryGetValue(id, out var old) && v == 0) return;

        if (!changeKV.ContainsKey(id))
            changeKV.Add(id, old);

        Values[id] = old + v;
        this.SetChange();
    }
    public void Remove(int id)
    {
        if (!Values.TryGetValue(id, out var old)) return;

        if (!changeKV.ContainsKey(id))
            changeKV.Add(id, old);

        Values.Remove(id);
        this.SetChange();
    }
    public void Clear()
    {
        if (this.Values.Count == 0) return;

        foreach (var kv in Values)
        {
            if (!changeKV.ContainsKey(kv.Key))
                changeKV.Add(kv.Key, kv.Value);
        }

        Values.Clear();
        this.SetChange();
    }
    public long Get(int id)
    {
        Values.TryGetValue(id, out long v);
        return v;
    }
    public void CopyTo(Dictionary<int, long> target)
    {
        foreach (var item in Values)
            target[item.Key] = item.Value;
    }

    [Event]
    static void Change(Change<KVComponent> t)
    {
        if (t.t._kvWatcherHandles != null)
        {
            var tmp = t.t.changeKV;
            t.t.changeKV = ObjectPool.Get<Dictionary<int, long>>();
            foreach (var kv in tmp)
            {
                int len = t.t._kvWatcherHandles.Count;
                for (int i = 0; i < len; i++)
                {
                    if (t.t._kvWatcherHandles[i].Disposed) continue;
                    t.t._kvWatcherHandles[i].Old = kv.Value;
                    t.t._kvWatcherHandles[i].New = t.t.Get(kv.Key);
                    t.t._kvWatcherHandles[i].Invoke(kv.Key);
                }
            }
            tmp.Clear();
            ObjectPool.Return(tmp);
        }
    }
    [Event]
    static void Dispose(Dispose<KVComponent> t)
    {
        t.t.Values.Clear();
        t.t.changeKV.Clear();
        ObjectPool.Return(t.t.changeKV);
        ObjectPool.Return(t.t.Values);
        t.t.Values = null;
        t.t.changeKV = null;
    }
}
