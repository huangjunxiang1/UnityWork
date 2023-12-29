using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Main;
using Game;
using System.Reflection;

public static class STimer
{
    static readonly DateTime _dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    static long _timeOffset;

    static readonly List<Temp> _timerLst = new List<Temp>();

    static long minUtc;
    static readonly List<TempUtc> _utcTimerLst = new List<TempUtc>();

    static bool _isExcutingUTCTimer;
    static bool _isRemovedTimer;
    static bool _isRemovedUTCTimer;

    public static long ServerTime
    {
        get { return ClientTime - _timeOffset; }
        set { _timeOffset = ClientTime - value; }
    }
    public static long ClientTime
    {
        get { return (DateTime.Now.Ticks - _dt1970.Ticks) / 10000; }
    }


    public static void Add(float time, int count, Action call, object target = null)
    {
        if (SAppSetting.Debug)
        {
            if (Contains(call))
                Loger.Error("已经包含timer calss:" + call.Method.ReflectedType + " method:" + call.Method.Name);
        }

        if (count == 0) return;

        Temp t = new Temp();
        t.time = time;
        t.count = count;
        t.isEveryFrame = time <= 0;
        t.action = call;
        t.target = target;
        _timerLst.Add(t);
    }
    public static void Remove(Action call)
    {
        for (int i = 0; i < _timerLst.Count; i++)
        {
            if (_timerLst[i].action == call)
            {
                _timerLst[i].isDisposed = true;
                _isRemovedTimer = true;
            }
        }
    }
    public static bool Contains(Action call)
    {
        for (int i = 0; i < _timerLst.Count; i++)
        {
            if (_timerLst[i].isDisposed) continue;
            if (_timerLst[i].action == call) return true;
        }
        return false;
    }

    public static void AddUTC(long utc, Action call)
    {
        if (SAppSetting.Debug)
        {
            if (ContainsUTC(call))
                Loger.Error("已经包含utcTimer calss:" + call.Method.ReflectedType + " method:" + call.Method.Name);
        }

        TempUtc t = new TempUtc();
        t.utc = utc;
        t.action = call;
        _utcTimerLst.Add(t);

        if (!_isExcutingUTCTimer)
        {
            if (minUtc == 0) minUtc = utc;
            else minUtc = Math.Min(minUtc, utc);
        }
    }
    public static void RemoveUTC(Action call)
    {
        for (int i = 0; i < _utcTimerLst.Count; i++)
        {
            if (_utcTimerLst[i].action == call)
            {
                _utcTimerLst[i].isDisposed = true;
                _isRemovedUTCTimer = true;
            }
        }
    }
    public static bool ContainsUTC(Action call)
    {
        for (int i = 0; i < _utcTimerLst.Count; i++)
        {
            if (_utcTimerLst[i].isDisposed) continue;
            if (_utcTimerLst[i].action == call) return true;
        }
        return false;
    }

    internal static void Update()
    {
        int cnt = _timerLst.Count;
        for (int i = 0; i < cnt; i++)
        {
            Temp t = _timerLst[i];
            if (t.isDisposed) continue;

            if (!t.isEveryFrame)
                t.curTime += Time.deltaTime;

            // t.time <=0 则表示每帧执行
            if (t.isEveryFrame || t.curTime >= t.time)
            {
                if (!t.isEveryFrame) t.curTime -= t.time;
                if (t.count > 0)
                {
                    t.curCount++;
                    if (t.curCount >= t.count)
                    {
                        t.isDisposed = true;
                        _isRemovedTimer = true;
                    }
                }
                try
                {
                    t.action();
                }
                catch (Exception e)
                { Loger.Error("timer error:" + e); }
            }
        }

        _isExcutingUTCTimer = true;
        if (minUtc <= ServerTime)
        {
            cnt = _utcTimerLst.Count;
            for (int i = 0; i < cnt; i++)
            {
                TempUtc t = _utcTimerLst[i];
                if (t.isDisposed) continue;
                if (t.utc > minUtc) continue;

                t.isDisposed = true;
                _isRemovedUTCTimer = true;
                try
                {
                    t.action();
                }
                catch (Exception e)
                { Loger.Error("utcTimer error:" + e); }
            }
        }
        _isExcutingUTCTimer = false;
    }
    internal static void AfterUpdate()
    {
        if (_isRemovedTimer)
        {
            _timerLst.RemoveAll(t => t.isDisposed);
            _isRemovedTimer = false;
        }
        if (_isRemovedUTCTimer)
        {
            _utcTimerLst.RemoveAll(t => t.isDisposed);
            minUtc = 0;
            int cnt = _utcTimerLst.Count;
            for (int i = 0; i < cnt; i++)
            {
                if (minUtc == 0) minUtc = _utcTimerLst[i].utc;
                else minUtc = Math.Min(_utcTimerLst[i].utc, minUtc);
            }
            _isRemovedUTCTimer = false;
        }
    }

    [Event]
    static void RigisterStaticTimer(EC_GameInit e)
    {
        var lst = Types.GetStaticMethods();
        for (int i = 0; i < lst.Count; i++)
        {
            MethodAndAttribute ma = lst[i];
            if (ma.attribute is STimerAttribute ea)
            {
#if DebugEnable
                if (ma.method.GetParameters().Length != 0)
                {
                    Loger.Error($"{ma.method.ReflectedType.FullName}  {ma.method.Name}  timer必须没有参数");
                    continue;
                }
#endif
                Add(ea.Time, ea.Count, (Action)ma.method.CreateDelegate(typeof(Action)));
            }
        }
    }
    public static bool AutoRigisterTimer(object target)
    {
        Type t;
#if ILRuntime
            if (target is ILRuntime.Runtime.Intepreter.ILTypeInstance ilInstance)
                t = ilInstance.Type.ReflectionType;
            else if (target is ILRuntime.Runtime.Enviorment.CrossBindingAdaptorType ilWarp)
                t = ilWarp.ILInstance.Type.ReflectionType;
            else
#endif
        t = target.GetType();
        var ts = Types.GetInstanceMethodsWithAttribute<STimerAttribute>(t);
        for (int i = 0; i < ts.Length; i++)
        {
            var method = ts[i].method;
#if DebugEnable
            if (method.GetParameters().Length != 0)
            {
                Loger.Error($"{method.ReflectedType.FullName}  {method.Name}  timer必须没有参数");
                continue;
            }
#endif
            var timer = ts[i].attribute.As<STimerAttribute>();
#if ILRuntime
            Add(timer.Time, timer.Count, () =>
            {
                method.Invoke(o, default, default, Array.Empty<object>(), default);
            }, target);
#else
            Add(timer.Time, timer.Count, (Action)method.CreateDelegate(typeof(Action), target), target);
#endif
        }
        return ts.Length > 0;
    }
    public static void AutoRemoveTimer(object target)
    {
        if (target == null) return;
        for (int i = 0; i < _timerLst.Count; i++)
        {
            if (_timerLst[i].target == target)
            {
                _timerLst[i].isDisposed = true;
                _isRemovedTimer = true;
            }
        }
    }

    class Temp
    {
        public float time;
        public int count;
        public bool isEveryFrame;//每帧
        public Action action;
        public object target;

        public float curTime;
        public int curCount;

        public bool isDisposed;
    }
    class TempUtc
    {
        public long utc;
        public Action action;
        public bool isDisposed;
    }
}
