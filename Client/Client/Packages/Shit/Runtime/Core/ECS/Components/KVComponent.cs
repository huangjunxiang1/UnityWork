using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class KVComponent : SComponent
{
    public static Func<int, (bool, int)> Parse = id => (id > 10000, id > 10000 ? id / 10000 : id);

    public const int SourceLength = 100;
    public const int Step = 10000;
    public const int PercentStart = Step / 2;

    int Max = 1;

#if UNITY_EDITOR
    [Sirenix.OdinInspector.ShowInInspector]
    SortedSet<int> Keys = ObjectPool.Get<SortedSet<int>>();

    [Sirenix.OdinInspector.ShowInInspector]
    SortedDictionary<int, long> Values = ObjectPool.Get<SortedDictionary<int, long>>();

    SortedDictionary<int, long> Changed = ObjectPool.Get<SortedDictionary<int, long>>();
#else
    [Sirenix.OdinInspector.ShowInInspector]
    HashSet<int> Keys = ObjectPool.Get<HashSet<int>>();

    [Sirenix.OdinInspector.ShowInInspector]
    Dictionary<int, long> Values = ObjectPool.Get<Dictionary<int, long>>();

    Dictionary<int, long> Changed = ObjectPool.Get<Dictionary<int, long>>();
#endif

    public void Set(IDictionary<int, long> kvs)
    {
        HashSet<int> ck = ObjectPool.Get<HashSet<int>>();
        Keys.Clear();
        Max = 1;
        foreach (var kv in kvs)
        {
            if (kv.Key < Step) continue;
            if (!Values.TryGetValue(kv.Key, out var old) || old != kv.Value)
            {
                int bk = GetBaseKey(kv.Key);
                ck.Add(bk);
                if (!Changed.ContainsKey(bk))
                    Changed.Add(bk, Get(bk));
                Values[kv.Key] = kv.Value;
                Keys.Add(bk);
                if (Max < kv.Key % PercentStart)
                    Max = kv.Key % PercentStart;
            }
        }
        foreach (var k in Values.Keys.ToList())
        {
            if (!kvs.ContainsKey(k) || k < Step)
            {
                int bk = GetBaseKey(k);
                ck.Add(bk);
                if (!Changed.ContainsKey(bk))
                    Changed.Add(bk, Get(bk));
                Values.Remove(k);
            }
        }
        foreach (var k in ck)
            ComputeTotalValue(k);
        ck.Clear();
        ObjectPool.Return(ck);
        this.SetChange();
    }
    public void Set(int id, long v, int Source = 1)
    {
#if DebugEnable
        if (Source < 1 || Source >= SourceLength)
            Loger.Error($"Source is in range 1-{SourceLength - 1}");
#endif
        var (p, bk) = Parse(id);
        int rk = GetRealKey(bk, Source + (p ? PercentStart : 0));
        Values.TryGetValue(rk, out var old);
        if (old == v) return;

        if (!Changed.ContainsKey(bk))
            Changed.Add(bk, Get(bk));
        if (Max < Source)
            Max = Source;

        Keys.Add(bk);
        Values[rk] = v;
        ComputeTotalValue(bk);
        this.SetChange();
    }
    public void Add(int id, long v, int Source = 1)
    {
        if (v == 0) return;
#if DebugEnable
        if (Source < 1 || Source >= SourceLength)
            Loger.Error($"Source is in range 1-{SourceLength - 1}");
#endif
        var (p, bk) = Parse(id);
        int rk = GetRealKey(bk, Source + (p ? PercentStart : 0));
        Values.TryGetValue(rk, out var old);
        if (!Changed.ContainsKey(bk))
            Changed.Add(bk, Get(bk));
        if (Max < Source)
            Max = Source;

        Keys.Add(bk);
        Values[rk] = old + v;
        ComputeTotalValue(bk);
        this.SetChange();
    }
    public void Remove(int id, int Source = 1)
    {
#if DebugEnable
        if (Source < 1 || Source >= SourceLength)
            Loger.Error($"Source is in range 1-{SourceLength - 1}");
#endif
        var (p, bk) = Parse(id);
        int rk = GetRealKey(bk, Source + (p ? PercentStart : 0));
        if (!Values.TryGetValue(rk, out var old) || old == 0) return;

        if (!Changed.ContainsKey(bk))
            Changed.Add(bk, Get(bk));

        Values.Remove(rk);
        ComputeTotalValue(bk);
        this.SetChange();
    }
    public void RemoveAllBySource(int Source)
    {
#if DebugEnable
        if (Source < 1 || Source >= SourceLength)
            Loger.Error($"Source is in range 1-{SourceLength - 1}");
#endif
        if (Source > Max) return;
        foreach (var bk in Keys)
        {
            Remove(bk, Source);
            Remove(bk, Source);
        }
    }
    public void RemoveAllByID(int id)
    {
        if (Values.TryGetValue(id, out var v))
        {
            if (!Changed.ContainsKey(id))
                Changed.Add(id, v);
        }
        RemoveKey(id);
    }
    public void Clear()
    {
        if (this.Values.Count == 0) return;

        HashSet<int> ck = ObjectPool.Get<HashSet<int>>();
        foreach (var k in Keys)
            ck.Add(k);
        foreach (var bk in ck)
        {
            if (!Changed.ContainsKey(bk))
                Changed.Add(bk, Get(bk));
        }
        Values.Clear();
        Max = 1;

        this.SetChange();
    }

    public bool Has(int id) => Values.TryGetValue(id, out var v) && v != 0;
    public bool TryGet(int id, out long v)
    {
        return Values.TryGetValue(id, out v) && v != 0;
    }
    public long Get(int id)
    {
        Values.TryGetValue(id, out long v);
        return v;
    }
    public long GetAddtion(int id)
    {
        var (p, bk) = Parse(id);
        Values.TryGetValue(GetRealKey(bk, p ? PercentStart : 0), out long v);
        return v;
    }
    public long Get(int id, int Source)
    {
#if DebugEnable
        if (Source < 1 || Source >= SourceLength)
            Loger.Error($"Source is in range 1-{SourceLength - 1}");
#endif
        Values.TryGetValue(GetRealKey(id, Source), out long v);
        return v;
    }
    public void CopyTo(Dictionary<int, long> target)
    {
        foreach (var item in Values)
        {
            if (item.Key % PercentStart < 1 || item.Key < Step || item.Value == 0) continue;
            target[item.Key] = item.Value;
        }
    }

    void ComputeTotalValue(int key)
    {
        long v = 0;
        for (int i = 1; i <= Max; i++)
        {
            var k = GetRealKey(key, i);
            if (Values.TryGetValue(k, out var value))
                v += value;
        }
        Values[GetRealKey(key)] = v;

        long pv = 0;
        for (int i = 1; i <= Max; i++)
        {
            var k = GetRealKey(key, PercentStart + i);
            if (Values.TryGetValue(k, out var value))
                pv += value;
        }
        Values[GetRealKey(key, PercentStart)] = pv;

        Values[key] = v + v * pv / 10000;
    }
    void RemoveKey(int key)
    {
        for (int i = 1; i <= Max; i++)
        {
            var k = GetRealKey(key, i);
            Values.Remove(k);
        }
        Values.Remove(GetRealKey(key));

        for (int i = 1; i <= Max; i++)
        {
            var k = GetRealKey(key, PercentStart + i);
            Values.Remove(k);
        }
        Values.Remove(GetRealKey(key, PercentStart));

        Values.Remove(key);
    }

    static int GetRealKey(int k, int source = 0)
    {
        return k * Step + source;
    }
    static int GetBaseKey(int k)
    {
        return k / Step;
    }

    [Event]
    static void Change(Change<KVComponent> t)
    {
        if (t.t._kvWatcherHandles != null)
        {
            var tmp = t.t.Changed;
#if UNITY_EDITOR
            t.t.Changed = ObjectPool.Get<SortedDictionary<int, long>>();
#else
            t.t.Changed = ObjectPool.Get<Dictionary<int, long>>();
#endif
            foreach (var kv in tmp)
            {
                int len = t.t._kvWatcherHandles.Count;
                for (int i = 0; i < len; i++)
                {
                    if (t.t._kvWatcherHandles[i].Disposed || t.t.Disposed) continue;
                    var v = t.t.Get(kv.Key);
                    if (kv.Value == v) continue;
                    t.t._kvWatcherHandles[i].Old = kv.Value;
                    t.t._kvWatcherHandles[i].New = v;
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
        t.t.Keys.Clear();
        t.t.Values.Clear();
        t.t.Changed.Clear();
        ObjectPool.Return(t.t.Keys);
        ObjectPool.Return(t.t.Changed);
        ObjectPool.Return(t.t.Values);
        t.t.Keys = null;
        t.t.Values = null;
        t.t.Changed = null;
    }
}
