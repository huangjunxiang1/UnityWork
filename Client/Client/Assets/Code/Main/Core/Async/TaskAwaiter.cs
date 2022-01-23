using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Main;
using System.Diagnostics;
using System.Security;

[DebuggerNonUserCode]
[AsyncMethodBuilder(typeof(TaskAwaiterBuilder))]
public class TaskAwaiter : ICriticalNotifyCompletion
{
    public TaskAwaiter()
    {

    }
    public TaskAwaiter(object tag)
    {
        this.Tag = tag;
    }

    Action _call;

    public object Tag { get; }

    public bool IsDisposed { get; private set; }
    /// <summary>
    /// 是否已完成
    /// </summary>
    public bool IsCompleted { get; private set; }

    public TaskAwaiter GetAwaiter()
    {
        return this;
    }
    public void GetResult()
    {

    }

    /// <summary>
    /// 取消
    /// </summary>
    public void TryCancel()
    {
        if (this.IsDisposed) return;

        this.IsDisposed = true;
        this._call = null;
    }

    /// <summary>
    /// 执行下一步
    /// </summary>
    public void TrySetResult()
    {
        if (this.IsDisposed) return;

        this.IsDisposed = true;
        this.IsCompleted = true;

        //如果异步使用弃元 则不会有下一步的回调
        try { this._call?.Invoke(); }
        catch (Exception ex) { Loger.Error("TrySetResult Error:" + ex); }
        this._call = null;
    }

    /// <summary>
    /// 错误
    /// </summary>
    /// <param name="e"></param>
    public void SetException(Exception e)
    {
        if (this.IsDisposed) return;

        this.IsDisposed = true;

        Loger.Error("TaskAwaiter Error " + e);

        //如果异步使用弃元 则不会有下一步的回调
        try { this._call?.Invoke(); }
        catch (Exception ex) { Loger.Error("SetException Error:" + ex); }
        this._call = null;
    }

    public void Reset()
    {
        this._call = null;
    }

    public void AddWaitCall(Action callBack)
    {
        if (this.IsDisposed) return;

        if (!this.IsCompleted)
            this._call += callBack;
        else
        {
            try { callBack?.Invoke(); }
            catch (Exception ex) { Loger.Error("AddCall Error:" + ex); }
        }
    }

    void INotifyCompletion.OnCompleted(Action callBack)
    {
        this._call += callBack;
    }
    void ICriticalNotifyCompletion.UnsafeOnCompleted(Action callBack)
    {
        this._call += callBack;
    }


    public static async TaskAwaiter WaitAll(IEnumerable<TaskAwaiter> itor)
    {
        if (itor == null) return;

        TaskAwaiter[] tasks = itor.ToArray();
        for (int i = 0; i < tasks.Length; i++)
            await tasks[i];
    }

    public static async TaskAwaiter<K[]> WaitAll<K>(IEnumerable<TaskAwaiter<K>> itor)
    {
        if (itor == null) return new K[0];

        TaskAwaiter<K>[] tasks = itor.ToArray();
        K[] ret = new K[tasks.Length];
        for (int i = 0; i < tasks.Length; i++)
            ret[i] = await tasks[i];
        return ret;
    }
}
