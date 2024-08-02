using System;
using System.Collections.Generic;

namespace Core
{
    public class STimer
    {
        public STimer(float wheelInterval = 1f, int utc_wheelInterval = 1000)
        {
            this.wheelInterval = Math.Max(wheelInterval, 0.1f);
            this.utc_wheelInterval = Math.Max(utc_wheelInterval, 1);
        }

        bool isSetUtc = false;
        readonly DateTime _dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        long _timeOffset;
        public long utc
        {
            get { return getNow() - _timeOffset; }
            set
            {
#if DebugEnable
                long old = _timeOffset;
#endif
                _timeOffset = getNow() - value;
                if (!isSetUtc)
                {
                    isSetUtc = true;
                    this.utc_wheel = utc / utc_wheelInterval;
                }
                else
                {
                    this.utc_wheel = Math.Min(this.utc_wheel, utc / utc_wheelInterval);
#if DebugEnable
                    if (Math.Abs(old - _timeOffset) > 1000 * 60)
                        Loger.Error($"utc 连续两次偏移 超出预期 old={old} new={_timeOffset} abs={old - _timeOffset}");
#endif
                }
            }
        }
        long getNow() => (DateTime.UtcNow.Ticks - _dt1970.Ticks) / 10000;

        float wheelInterval;//轮间隔
        long wheel;//时间轮的当前轮
        float time;
        Dictionary<long, Queue<TimerItem>> timers = new();
        Dictionary<Action, TimerItem> atMap = new();
        Dictionary<ITimer, List<TimerItem>> targetMap = new();

        int utc_wheelInterval;//轮间隔
        long utc_wheel;//时间轮的当前轮
        Dictionary<long, Queue<utcTimerItem>> utc_timers = new();
        Dictionary<Action, utcTimerItem> utc_atMap = new();

        public void Add(float time, int count, Action call, ITimer target = null)
        {
            Remove(call);
            atMap[call] = createTimer(time, count, call, target);
        }
        TimerItem createTimer(float time, int count, Action call, ITimer target = null)
        {
            if (count <= 0 && count != -1)
            {
                Loger.Error("error count=" + count);
                return null;
            }
            var ti = ObjectPool.Get<TimerItem>();
            ti.deltaTime = Math.Max(time, 0);
            ti.cnt = count;
            ti.action = call;
            ti.target = target;

            ti.nextTime = this.time + time;

            ti.disposed = false;
            ti.wheel = (long)(ti.nextTime / wheelInterval);
            ti.wheel = Math.Max(ti.wheel, this.wheel);

            addToQueue(ti);

            return ti;
        }
        public void Remove(Action call)
        {
            if (atMap.TryGetValue(call, out var ti))
            {
                ti.disposed = true;
                ti.target = null;
                ti.action = null;
                atMap.Remove(call);
                if (ti.wheel > this.wheel)
                {
                    var u = timers[ti.wheel];
                    timers.Remove(ti.wheel);
                    var q = ObjectPool.Get<Queue<TimerItem>>();
                    while (u.TryDequeue(out var t))
                    {
                        if (t.disposed)
                        {
                            ObjectPool.Return(t);
                            continue;
                        }
                        q.Enqueue(t);
                    }
                    ObjectPool.Return(u);
                    if (q.Count == 0)
                        ObjectPool.Return(q);
                    else
                        timers[ti.wheel] = q;
                }
            }
        }
        public void AddUTC(long utc, Action call)
        {
            if (!isSetUtc)
            {
                Loger.Error("还未设置当前 utc 不可添加timer");
                return;
            }
            RemoveUTC(call);
            if (utc < this.utc)
                Loger.Error($"{call.Method.ReflectedType} {call.Method.Name} utc时间小于当前");

            utcTimerItem ti = ObjectPool.Get<utcTimerItem>();
            ti.utc = utc;
            ti.action = call;
            ti.disposed = false;
            ti.wheel = ti.utc / utc_wheelInterval;
            ti.wheel = Math.Max(ti.wheel, this.utc_wheel);

            addToUtcQueue(ti);
            utc_atMap[call] = ti;
        }
        public void NextUTC(long delay, Action call) => AddUTC(this.utc + delay, call);
        public void RemoveUTC(Action call)
        {
            if (utc_atMap.TryGetValue(call, out var ti))
            {
                ti.disposed = true;
                ti.action = null;
                utc_atMap.Remove(call);
                if (ti.wheel > this.utc_wheel)
                {
                    var u = utc_timers[ti.wheel];
                    utc_timers.Remove(ti.wheel);
                    var q = ObjectPool.Get<Queue<utcTimerItem>>();
                    while (u.TryDequeue(out var t))
                    {
                        if (t.disposed)
                        {
                            ObjectPool.Return(t);
                            continue;
                        }
                        q.Enqueue(t);
                    }
                    ObjectPool.Return(u);
                    if (q.Count == 0)
                        ObjectPool.Return(q);
                    else
                        utc_timers[ti.wheel] = q;
                }
            }
        }

        internal void Update(float deltatime)
        {
            time += deltatime;
            long i = wheel;
            long max = wheel = (long)(time / wheelInterval);
            for (; i <= max; ++i)
            {
                if (timers.TryGetValue(i, out var queue))
                {
                    timers.Remove(i);

                    while (queue.TryDequeue(out var ti))
                    {
                        if (ti.disposed)
                        {
                            ObjectPool.Return(ti);
                            continue;
                        }
                        if (ti.nextTime > time)
                        {
                            addToQueue(ti);
                            continue;
                        }

                        if (ti.cnt == 1)
                            atMap.Remove(ti.action);
                        if (ti.target == null || ti.target.TimerEnable)
                        {
                            atMap.Remove(ti.action);
                            try { ti.action(); }
                            catch (Exception e)
                            { Loger.Error("timer error:" + e); }
                        }

                        if (ti.cnt == -1)
                        {
                            ti.nextTime = Math.Max(ti.nextTime + ti.deltaTime, time);
                            ti.wheel = (long)(ti.nextTime / wheelInterval);
                            ti.wheel = Math.Max(ti.wheel, this.wheel);
                            addToQueue(ti);
                        }
                        else if (ti.cnt > 1)
                        {
                            ti.cnt--;
                            ti.nextTime = Math.Max(ti.nextTime + ti.deltaTime, time);
                            ti.wheel = (long)(ti.nextTime / wheelInterval);
                            ti.wheel = Math.Max(ti.wheel, this.wheel);
                            addToQueue(ti);
                        }
                        else
                        {
                            ti.action = null;
                            ti.target = null;
                            ObjectPool.Return(ti);
                        }
                    }

                    ObjectPool.Return(queue);
                }
            }

            if (utc_wheel == 0) return;
            long utc = this.utc;
            i = utc_wheel;
            utc_wheel = utc / utc_wheelInterval;
            max = utc_wheel = Math.Min(utc_wheel, i + 10000);//单次最多循环10000 以免循环量太大 导致卡死
            for (; i <= max; ++i)
            {
                if (utc_timers.TryGetValue(i, out var queue))
                {
                    utc_timers.Remove(i);

                    while (queue.TryDequeue(out var ti))
                    {
                        if (ti.disposed)
                        {
                            ObjectPool.Return(ti);
                            continue;
                        }
                        if (ti.utc > utc)
                        {
                            addToUtcQueue(ti);
                            continue;
                        }
                        utc_atMap.Remove(ti.action);
                        try { ti.action(); }
                        catch (Exception e)
                        { Loger.Error("utc timer error:" + e); }
                        ObjectPool.Return(ti);
                    }

                    ObjectPool.Return(queue);
                }
            }
        }
        void addToQueue(TimerItem ti)
        {
            if (!timers.TryGetValue(ti.wheel, out var queue))
                timers[ti.wheel] = queue = ObjectPool.Get<Queue<TimerItem>>();
            queue.Enqueue(ti);
        }
        void addToUtcQueue(utcTimerItem ti)
        {
            if (!utc_timers.TryGetValue(ti.wheel, out var queue))
                utc_timers[ti.wheel] = queue = ObjectPool.Get<Queue<utcTimerItem>>();
            queue.Enqueue(ti);
        }

        internal void Load(List<MethodParseData> methods)
        {
            for (int i = 0; i < methods.Count; i++)
            {
                MethodParseData ma = methods[i];
                if (ma.attribute is TimerAttribute ea && ma.method.IsStatic && ma.parameters.Length == 0)
                    Add(ea.delay, ea.count, (Action)ma.method.CreateDelegate(typeof(Action)));
            }
        }
        public bool RigisterTimer(ITimer target)
        {
            if (targetMap.ContainsKey(target))
            {
                Loger.Error($"{target} 已经注册timer");
                return false;
            }
            bool ret = false;
            var methods = Types.GetInstanceMethodsAttribute(target.GetType());
            for (int i = 0; i < methods.Length; i++)
            {
                if (methods[i].attribute is TimerAttribute ta)
                {
                    var ti = createTimer(ta.delay, ta.count, (Action)methods[i].method.CreateDelegate(typeof(Action), target), target);
                    if (!targetMap.TryGetValue(target, out var tis))
                        targetMap[target] = tis = ObjectPool.Get<List<TimerItem>>();
                    tis.Add(ti);
                    ret = true;
                }
            }
            return ret;
        }
        public void RemoveTimer(ITimer target)
        {
            if (targetMap.TryGetValue(target, out var tis))
            {
                targetMap.Remove(target);
                for (int i = 0; i < tis.Count; i++)
                {
                    tis[i].disposed = true;
                    tis[i].target = null;
                    tis[i].action = null;
                }
                tis.Clear();
                ObjectPool.Return(tis);
            }
        }

        class TimerItem
        {
            public long wheel;

            public float deltaTime;
            public int cnt;
            public Action action;
            public ITimer target;

            public float nextTime;//下一次调用时间
            public bool disposed;
        }
        class utcTimerItem
        {
            public long wheel;

            public long utc;
            public Action action;
            public bool disposed;
        }
    }
}