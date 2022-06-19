using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 事件监听类
/// </summary>
public class Eventer
{
    public Eventer() { }
    public Eventer(object creater)
    {
        this.Creater = creater;
    }

    readonly List<Temp> _evtLst = new();
    bool _isExcuting = false;

    /// <summary>
    /// 创建者
    /// </summary>
    public object Creater { get; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enable { get; set; } = true;

    /// <summary>
    /// add事件
    /// </summary>
    /// <param name="call"></param>
    public void Add(Action call)
    {
        Temp t = new();
        t.isP0 = true;
        t.action0 = call;
        _evtLst.Add(t);
    }
    public void Add(Action<EventerContent> call)
    {
        Temp t = new();
        t.isP0 = false;
        t.action1 = call;
        _evtLst.Add(t);
    }

    /// <summary>
    /// 重置事件
    /// </summary>
    /// <param name="call"></param>
    public void Set(Action call)
    {
        this.Clear();
        Add(call);
    }
    public void Set(Action<EventerContent> call)
    {
        this.Clear();
        Add(call);
    }

    /// <summary>
    /// 是否已经包含了
    /// </summary>
    /// <param name="call"></param>
    /// <returns></returns>
    public bool Contains(Action call)
    {
        for (int i = 0; i < _evtLst.Count; i++)
        {
            if (_evtLst[i].isP0 && !_evtLst[i].isDisposed && _evtLst[i].action0 == call)
                return true;
        }
        return false;
    }
    public bool Contains(Action<EventerContent> call)
    {
        for (int i = 0; i < _evtLst.Count; i++)
        {
            if (!_evtLst[i].isP0 && !_evtLst[i].isDisposed && _evtLst[i].action1 == call)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 移除事件
    /// </summary>
    /// <param name="call"></param>
    public void Remove(Action call)
    {
        if (_isExcuting)
        {
            for (int i = 0; i < _evtLst.Count; i++)
            {
                if (_evtLst[i].isP0 && _evtLst[i].action0 == call)
                    _evtLst[i].isDisposed = true;
            }
        }
        else
            _evtLst.RemoveAll(t => t.isP0 && t.action0 == call);
    }
    public void Remove(Action<EventerContent> call)
    {
        if (_isExcuting)
        {
            for (int i = 0; i < _evtLst.Count; i++)
            {
                if (!_evtLst[i].isP0 && _evtLst[i].action1 == call)
                    _evtLst[i].isDisposed = true;
            }
        }
        else
            _evtLst.RemoveAll(t => !t.isP0 && t.action1 == call);
    }

    /// <summary>
    /// 清除所有事件
    /// </summary>
    public void Clear()
    {
        if (!_isExcuting) _evtLst.Clear();
        else
        {
            for (int i = 0; i < _evtLst.Count; i++)
                _evtLst[i].isDisposed = true;
        }
    }

    /// <summary>
    /// call叫事件
    /// </summary>
    public void Call()
    {
        this.Call(0, null);
    }
    public void Call(int value)
    {
        this.Call(value, null);
    }
    public void Call(object data)
    {
        this.Call(0, data);
    }
    public void Call(int value, object data)
    {
        if (!Enable) return;

        if (_isExcuting)
        {
            Loger.Error("事件循环 target=" + this.Creater);
            return;
        }
        _isExcuting = true;

        //缓存长度  只执行当前个数  在执行过程中如果有添加新事件则不执行
        int cnt = _evtLst.Count;
        for (int i = 0; i < cnt; i++)
        {
            Temp t = _evtLst[i];
            if (t.isDisposed) continue;

            try
            {
                if (t.isP0)
                    t.action0();
                else
                    t.action1(new EventerContent(this.Creater, value, data));
            }
            catch (Exception ex)
            { Loger.Error("Eventer Error:" + ex); }
        }

        _isExcuting = false;
    }


    /// <summary>
    /// 事件信息
    /// </summary>
    class Temp
    {
        public bool isP0;
        public Action action0;
        public Action<EventerContent> action1;
        public bool isDisposed;
    }
}
