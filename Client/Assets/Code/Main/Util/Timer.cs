using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Main;

public static class Timer
{
    static readonly DateTime _dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    static long _timeOffset;

    static readonly List<Temp> _timerLst = new List<Temp>();

    static long minUtc;
    static readonly List<TempUtc> _utcTimerLst = new List<TempUtc>();

    static bool _isExcutingTimer;
    static bool _isRemoved;

    public static long ServerTime
    {
        get { return ClientTime - _timeOffset; }
        set { _timeOffset = ClientTime - value; }
    }
    public static long ClientTime
    {
        get { return (DateTime.Now.Ticks - _dt1970.Ticks) / 10000; }
    }


    public static void Add(float time, int count, Action call)
    {
        if (MGameSetting.Debug)
        {
            if (Contains(call))
                Loger.Error("已经包含timer calss:" + call.Method.ReflectedType + " method" + call.Method.Name);
        }

        if (count == 0) return;
        Temp t = new Temp();
        t.time = time;
        t.count = count;
        t.action = call;
        _timerLst.Add(t);
    }
    public static void Remove(Action call)
    {
        if (!_isExcutingTimer)
            _timerLst.RemoveAll(t => t.action == call);
        else
            for (int i = 0; i < _timerLst.Count; i++)
            {
                if (_timerLst[i].action == call)
                {
                    _timerLst[i].isDisposed = true;
                    _isRemoved = true;
                }
            }
    }
    public static bool Contains(Action call)
    {
        for (int i = 0; i < _timerLst.Count; i++)
        {
            if (_timerLst[i].action == call) return true;
        }
        return false;
    }

    public static void AddUTC(long utc, Action call)
    {
        if (MGameSetting.Debug)
        {
            if (ContainsUTC(call))
                Loger.Error("已经包含utcTimer calss:" + call.Method.ReflectedType + " method" + call.Method.Name);
        }

        TempUtc t = new TempUtc();
        t.utc = utc;
        t.action = call;
        _utcTimerLst.Add(t);

        if (!_isExcutingTimer)
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
                _isRemoved = true;
            }
        }
    }
    public static bool ContainsUTC(Action call)
    {
        for (int i = 0; i < _utcTimerLst.Count; i++)
        {
            if (_utcTimerLst[i].action == call) return true;
        }
        return false;
    }

    public static void Update()
    {
        _isExcutingTimer = true;

        {
            int cnt = _timerLst.Count;
            for (int i = 0; i < cnt; i++)
            {
                Temp t = _timerLst[i];
                if (t.isDisposed) continue;
                t.curTime += Time.deltaTime;

                bool ex = t.curTime >= t.time;
                // t.time <=0 则表示每帧执行
                if (ex || t.time <= 0)
                {
                    if (ex) t.curTime -= t.time;
                    if (t.count > 0)
                    {
                        t.curCount++;
                        if (t.curCount >= t.count)
                        {
                            t.isDisposed = true;
                            _isRemoved = true;
                        }
                    }
                    try { t.action(); }
                    catch (Exception e)
                    { Loger.Error("timer error:" + e); }
                }
            }
        }

        if (minUtc <= ServerTime)
        {
            int cnt = _utcTimerLst.Count;
            for (int i = 0; i < cnt; i++)
            {
                TempUtc t = _utcTimerLst[i];
                if (t.isDisposed) continue;

                try { t.action(); }
                catch (Exception e)
                { Loger.Error("utcTimer error:" + e); }
            }
        }
      
        if (_isRemoved)
        {
            _timerLst.RemoveAll(t => t.isDisposed);

            _utcTimerLst.RemoveAll(t => t.isDisposed);
            minUtc = 0;
            for (int i = 0; i < _utcTimerLst.Count; i++)
            {
                if (minUtc == 0) minUtc = _utcTimerLst[i].utc;
                else minUtc = Math.Min(_utcTimerLst[i].utc, minUtc);
            }

            _isRemoved = false;
        }

        _isExcutingTimer = false;
    }

    class Temp
    {
        public float time;
        public int count;
        public Action action;

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
