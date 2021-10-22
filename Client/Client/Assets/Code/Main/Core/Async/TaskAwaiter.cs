using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Main;
using System.Diagnostics;
using System.Security;

[AsyncMethodBuilder(typeof(TaskAwaiterBuilder))]
public sealed class TaskAwaiter : ICriticalNotifyCompletion
{
    public TaskAwaiter()
    {

    }
    public TaskAwaiter(Action<TaskAwaiter> onComplete)
    {
        this._onComplete = onComplete;
    }

    public int id = 0;
    Action _call;
    Action<TaskAwaiter> _onComplete;
    bool _isDisposed;
    
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
    public void TryCancel(bool completed = false)
    {
        this._isDisposed = true;
        this._call = null;

        if (completed)
        {
            try { this._onComplete?.Invoke(this); }
            catch (Exception ex) { Loger.Error("TryCancel Error:" + ex); }
            this._onComplete = null;
        }
    }

    /// <summary>
    /// 执行下一步
    /// </summary>
    public void TrySetResult()
    {
        if (this._isDisposed) return;

        this._isDisposed = true;
        this.IsCompleted = true;

        //先回调Complete  再继续走异步的下一步
        try { this._onComplete?.Invoke(this); }
        catch (Exception ex) { Loger.Error("TrySetResult Error:" + ex); }
        this._onComplete = null;

        try { this._call.Invoke(); }
        catch (Exception ex) { Loger.Error("TrySetResult Error:" + ex); }
        this._call = null;
    }

    /// <summary>
    /// 错误
    /// </summary>
    /// <param name="e"></param>
    public void SetException(Exception e)
    {
        if (this._isDisposed) return;

        Loger.Error("TaskAwaiter Error " + e);

        //先回调Complete  再继续走异步的下一步
        try { this._onComplete?.Invoke(this); }
        catch (Exception ex) { Loger.Error("SetException Error:" + ex); }
        this._onComplete = null;

        try { this._call.Invoke(); }
        catch (Exception ex) { Loger.Error("SetException Error:" + ex); }
        this._call = null;
    }

    void INotifyCompletion.OnCompleted(Action continuation)
    {
        this._call = continuation;
    }
    void ICriticalNotifyCompletion.UnsafeOnCompleted(Action continuation)
    {
        this._call = continuation;
    }
}
