using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Main;
using Game;
using System.Reflection;
using System.Diagnostics;

public static class STimer
{
    static readonly DateTime _dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    static long _timeOffset;

    static readonly List<TimerItem> _timerLst = new List<TimerItem>();

    static long minUtc;
    static readonly List<UTCTimerItem> _utcTimerLst = new List<UTCTimerItem>();

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

#if UNITY_EDITOR
    internal static void EditorClear()
    {
        _timerLst.Clear();
        minUtc = 0;
        _utcTimerLst.Clear();
    }
#endif

    public static void Add(float time, int count, Action call, ITimer target = null)
    {
#if DebugEnable
        if (Contains(call))
            Loger.Error("已经包含timer calss:" + call.Method.ReflectedType + " method:" + call.Method.Name);
#endif

        if (count == 0) return;

        TimerItem t = ObjectPool<TimerItem>.Get();
        t.time = time;
        t.count = count;
        t.isEveryFrame = time <= 0;
        t.action = call;
        t.target = target;

        t.curTime = 0;
        t.curCount = 0;
        t.isDisposed = false;

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
#if DebugEnable
        if (ContainsUTC(call))
            Loger.Error("已经包含utcTimer calss:" + call.Method.ReflectedType + " method:" + call.Method.Name);
#endif

        UTCTimerItem t = new UTCTimerItem();
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
            TimerItem t = _timerLst[i];
            if (t.isDisposed) continue;
            if (t.target != null && (!t.target.TimerEnable || t.target.Disposed)) continue;

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
                UTCTimerItem t = _utcTimerLst[i];
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
            _timerLst.RemoveAll(t =>
            {
                if (t.isDisposed || (t.target != null && t.target.Disposed))
                {
                    t.action = null;
                    t.target = null;
                    ObjectPool<TimerItem>.Return(t);
                    return true;
                }
                return false;
            });
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

    [Conditional("DebugEnable")]
    internal static void Check(List<Type> types)
    {
        for (int i = 0; i < types.Count; i++)
        {
            var type = types[i];
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            for (int j = 0; j < methods.Length; j++)
            {
                var method = methods[j];
                if (method.IsDefined(typeof(STimerAttribute)))
                {
                    if(method.IsGenericMethod)
                        Loger.Error($"{method.ReflectedType.FullName}  {method.Name}  timer函数不能是泛型函数");
                    if (method.GetParameters().Length != 0)
                        Loger.Error($"{method.ReflectedType.FullName}  {method.Name}  timer函数必须没有参数");
                    if (method.ReturnType != typeof(void))
                        Loger.Error($"{method.ReflectedType.FullName}  {method.Name}  timer函数必须没有返回值");
                }
            }
        }
    }
    internal static void Init(List<MethodParseData> methods)
    {
        for (int i = 0; i < methods.Count; i++)
        {
            MethodParseData ma = methods[i];
            if (ma.attribute is STimerAttribute ea && ma.method.IsStatic)
                Add(ea.Time, ea.Count, (Action)ma.method.CreateDelegate(typeof(Action)));
        }
    }
    public static bool RigisterTimer(ITimer target)
    {
        bool ret = false;
        var methods = Types.                                                                                                                                                                                                                                                                                       GetInstanceMethodsAttribute(target.GetType());
        for (int i = 0; i < methods.Length; i++)
        {
            if (methods[i].attribute is STimerAttribute ta)
            {
                Add(ta.Time, ta.Count, (Action)methods[i].method.CreateDelegate(typeof(Action), target), target);
                ret = true;
            }
        }
        return ret;
    }
    public static void RemoveTimer() => _isRemovedTimer = true;

    class TimerItem
    {
        public float time;
        public int count;
        public bool isEveryFrame;//每帧
        public Action action;
        public ITimer target;

        public float curTime;
        public int curCount;

        public bool isDisposed;
    }
    class UTCTimerItem
    {
        public long utc;
        public Action action;
        public bool isDisposed;
    }
}
