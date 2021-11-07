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

    Action _call;

    public bool IsDisposed { get; protected set; }
    /// <summary>
    /// 是否已完成
    /// </summary>
    public bool IsCompleted { get; protected set; }

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

    void INotifyCompletion.OnCompleted(Action continuation)
    {
        this._call += continuation;
    }
    void ICriticalNotifyCompletion.UnsafeOnCompleted(Action continuation)
    {
        this._call += continuation;
    }
}
