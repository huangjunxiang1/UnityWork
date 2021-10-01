using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Main;

public class TaskAwaiter<T> : ICriticalNotifyCompletion, INotifyCompletion
{
    public TaskAwaiter()
    {
        
    }
    public TaskAwaiter(Action<TaskAwaiter<T>> moveNextCallBack)
    {
        this._moveNextCallBack = moveNextCallBack;
    }

    Action _call;
    Action<TaskAwaiter<T>> _moveNextCallBack;
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
    public void TryCancel()
    {
        this._isDisposed = true;
        this._call = null;
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
        this._call?.Invoke();
        this._call = null;
        this._moveNextCallBack?.Invoke(this);
        this._moveNextCallBack = null;
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
