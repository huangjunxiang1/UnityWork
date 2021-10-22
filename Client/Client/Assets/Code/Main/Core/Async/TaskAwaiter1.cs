using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Main;

[AsyncMethodBuilder(typeof(TaskAwaiterBuilder<>))]
public sealed class TaskAwaiter<T> : ICriticalNotifyCompletion
{
    public TaskAwaiter()
    {
        
    }
    public TaskAwaiter(Action<TaskAwaiter<T>> onComplete)
    {
        this._onComplete = onComplete;
    }

    Action _call;
    Action<TaskAwaiter<T>> _onComplete;
    bool _isDisposed;
    T _result;

    /// <summary>
    /// 是否已完成
    /// </summary>
    public bool IsCompleted { get; private set; }

    public TaskAwaiter<T> GetAwaiter()
    {
        return this;
    }
    public T GetResult()
    {
        return _result;
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
    /// 回传结果
    /// </summary>
    /// <param name="result"></param>
    public void TrySetResult(T result)
    {
        if (this._isDisposed) return;

        this._result = result;
        this._isDisposed = true;
        this.IsCompleted = true;

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

        Loger.Error($"TaskAwaiter<{typeof(T)}> Error " + e);

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
