using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KVCalculator
{
    public virtual bool ConvertKey(int k, out int fk)
    {
        if (k > 10000)
        {
            fk = k - 10000;
            return true;
        }
        fk = k;
        return false;
    }
    public virtual long Calculating(int k, KVComponent kv)
    {
        var v = kv.GetValue(KVComponent.GetRealKey(k));
        if (k > 10000)
            return v;
        var pv = kv.GetValue(KVComponent.GetRealKey(k + 10000));
        return v * (10000 + pv) / 10000;
    }
}
public class KVComponent : SComponent
{
    public const int SourceMax = 100;

    public static KVCalculator DefaultCalculator = new();
    /// <summary>
    /// 数值计算器
    /// </summary>
    public KVCalculator Calculator = DefaultCalculator;

    int Max = 1;

#if UNITY_EDITOR
    [ShowInInspector]
    SortedDictionary<int, long> Values = ObjectPool.Get<SortedDictionary<int, long>>();
    [ShowInInspector]
    SortedDictionary<int, long> FinalValue = ObjectPool.Get<SortedDictionary<int, long>>();
#else
    Dictionary<int, long> Values = ObjectPool.Get<Dictionary<int, long>>();
    Dictionary<int, long> FinalValue = ObjectPool.Get<Dictionary<int, long>>();
#endif
    Dictionary<int, long> Changed = ObjectPool.Get<Dictionary<int, long>>();

    public void Set(IDictionary<int, long> kvs)
    {
        HashSet<int> ck = ObjectPool.Get<HashSet<int>>();
        Max = 1;
        foreach (var kv in kvs)
        {
            if (!Values.TryGetValue(kv.Key, out var old) || old != kv.Value)
            {
                var k = GetBaseKey(kv.Key);
                if (!Changed.ContainsKey(k))
                    Changed.Add(k, Get(k));
                Values[kv.Key] = kv.Value;
                ck.Add(k);
                Max = Math.Max(kv.Key % SourceMax, Max);
            }
        }
        foreach (var rk in Values.Keys.ToList())
        {
            var k = GetBaseKey(rk);
            if (!kvs.ContainsKey(rk))
            {
                if (!Changed.ContainsKey(k))
                    Changed.Add(k, Get(k));
                Values.Remove(rk);
                ck.Add(k);
            }
        }
        ComputeTotalValue(ck);
        ck.Clear();
        ObjectPool.Return(ck);
        this.SetChangeFlag();
    }
    public void Set(int k, long v, int Source = 1)
    {
#if DebugEnable
        if (Source < 1 || Source >= SourceMax)
            Loger.Error($"Source is in range 1-{SourceMax - 1}");
#endif
        var rk = GetRealKey(k, Source);
        var old = GetValue(rk);
        if (old == v) return;

        if (!Changed.ContainsKey(k))
            Changed.Add(k, Get(k));
        Max = Math.Max(Source, Max);

        Values[rk] = v;
        ComputeTotalValue(k);
        this.SetChangeFlag();
    }
    public void Add(int k, long v, int Source = 1)
    {
        if (v == 0) return;
#if DebugEnable
        if (Source < 1 || Source >= SourceMax)
            Loger.Error($"Source is in range 1-{SourceMax - 1}");
#endif
        var rk = GetRealKey(k, Source);
        var old = GetValue(rk);

        if (!Changed.ContainsKey(k))
            Changed.Add(k, Get(k));
        Max = Math.Max(Source, Max);

        Values[rk] = old + v;
        ComputeTotalValue(k);
        this.SetChangeFlag();
    }
    public void Remove(int k, int Source = 1)
    {
#if DebugEnable
        if (Source < 1 || Source >= SourceMax)
            Loger.Error($"Source is in range 1-{SourceMax - 1}");
#endif
        var rk = GetRealKey(k, Source);

        if (!Changed.ContainsKey(k))
            Changed.Add(k, Get(k));

        Values.Remove(rk);
        ComputeTotalValue(k);
        this.SetChangeFlag();
    }
    public void RemoveAllBySource(int Source)
    {
#if DebugEnable
        if (Source < 1 || Source >= SourceMax)
            Loger.Error($"Source is in range 1-{SourceMax - 1}");
#endif
        if (Source > Max) return;
        HashSet<int> ck = ObjectPool.Get<HashSet<int>>();
        foreach (var rk in Values.Keys.ToList())
        {
            if (rk % SourceMax == Source)
            {
                Values.Remove(rk);
                var k = GetBaseKey(rk);
                if (!Changed.ContainsKey(k))
                    Changed.Add(k, Get(k));
                ck.Add(k);
            }
        }
        if (ck.Count > 0)
        {
            ComputeTotalValue(ck);
            this.SetChangeFlag();
        }
        ck.Clear();
        ObjectPool.Return(ck);
    }
    public void RemoveAllByID(int k)
    {
        for (int i = 1; i < Max; i++)
        {
            var rk = GetRealKey(k, i);
            if (!Changed.ContainsKey(k))
            {
                Changed.Add(k, Get(k));
                this.SetChangeFlag();
            }
            Values.Remove(rk);
        }
        ComputeTotalValue(k);
    }
    public void Clear()
    {
        if (this.Values.Count == 0) return;

        foreach (var rk in Values.Keys)
        {
            var k = GetBaseKey(rk);
            if (!Changed.ContainsKey(k))
                Changed.Add(k, Get(k));
        }
        Values.Clear();
        FinalValue.Clear();
        Max = 1;

        this.SetChangeFlag();
    }

    public long GetValue(int id)
    {
        Values.TryGetValue(id, out var v);
        return v;
    }
    public long Get(int id)
    {
        FinalValue.TryGetValue(id, out long v);
        return v;
    }

    public long Get(int id, int Source)
    {
#if DebugEnable
        if (Source < 1 || Source >= SourceMax)
            Loger.Error($"Source is in range 1-{SourceMax - 1}");
#endif
        Values.TryGetValue(GetRealKey(id, Source), out long v);
        return v;
    }
    public void CopyTo(Dictionary<int, long> target)
    {
        foreach (var item in Values)
            target[item.Key] = item.Value;
    }

    void ComputeTotalValue(int k)
    {
        long v = 0;
        for (int i = 1; i <= Max; i++)
        {
            var rk = GetRealKey(k, i);
            v += GetValue(rk);
        }
        Values[k * SourceMax] = v;
        if (Calculator.ConvertKey(k, out var fk))
            FinalValue[k] = Calculator.Calculating(k, this);
        FinalValue[fk] = Calculator.Calculating(fk, this);
    }
    void ComputeTotalValue(HashSet<int> ks)
    {
        foreach (var k in ks)
        {
            long v = 0;
            for (int i = 1; i <= Max; i++)
            {
                var rk = GetRealKey(k, i);
                v += GetValue(rk);
            }
            Values[k * SourceMax] = v;
        }
        foreach (var k in ks)
        {
            if (Calculator.ConvertKey(k, out var fk))
                FinalValue[k] = Calculator.Calculating(k, this);
            FinalValue[fk] = Calculator.Calculating(fk, this);
        }
    }

    public static int GetRealKey(int k, int source = 0)
    {
        return k * 100 + source;
    }
    public static int GetBaseKey(int k)
    {
        return k / 100;
    }

    [ChangeSystem]
    static void Change(KVComponent t)
    {
        var tmp = t.Changed;
        t.Changed = ObjectPool.Get<Dictionary<int, long>>();
        foreach (var kv in tmp)
        {
            int len = t._Handles.Count;
            for (int i = 0; i < len; i++)
            {
                if (t._Handles[i].type != SystemType.KvWatcher || t._Handles[i].Disposed) continue;
                t._Handles[i].KvInvoke(kv.Key);
                if (t.Disposed) break;
            }
            if (t.Disposed) break;
        }
        tmp.Clear();
        ObjectPool.Return(tmp);
    }
    [OutSystem]
    static void Dispose(KVComponent t)
    {
        if (t.Disposed)
        {
            t.Values.Clear();
            t.Changed.Clear();
            ObjectPool.Return(t.Changed);
            ObjectPool.Return(t.Values);
            t.Values = null;
            t.Changed = null;
        }
    }
}
