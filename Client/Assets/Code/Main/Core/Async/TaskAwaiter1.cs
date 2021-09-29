using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

public class TaskAwaiter<T> : ICriticalNotifyCompletion, INotifyCompletion
{
    public TaskAwaiter()
    {

    }

    Action _call;
    T _result;

    public bool IsCompleted { get; private set; }

    public TaskAwaiter<T> GetAwaiter()
    {
        return this;
    }
    public T GetResult()
    {
        return _result;
    }

    public void TryCancel()
    {
        this.TryCancel(false);
    }
    public void TryCancel(bool isCompleted)
    {
        this.IsCompleted = isCompleted;
        this._call = null;
    }
    public void TrySetResult(T result)
    {
        this.IsCompleted = true;
        this._result = result;
        this._call?.Invoke();
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
