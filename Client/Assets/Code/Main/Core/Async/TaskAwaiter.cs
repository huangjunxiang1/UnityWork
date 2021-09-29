using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

public class TaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
{
    public TaskAwaiter()
    {

    }

    Action _call;
    
    public bool IsCompleted { get; private set; }
    
    public TaskAwaiter GetAwaiter()
    {
        return this;
    }
    public void GetResult()
    {

    }

    public void TryCancel()
    {
        this.TryCancel(false);
    }
    public void TryCancel(bool isCompleted)
    {
        this.IsCompleted = isCompleted;
    }
    public void TryMoveNext()
    {
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
