using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Main;

public class TaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
{
    public TaskAwaiter()
    {

    }
    public TaskAwaiter(Action<TaskAwaiter> moveNextCallBack)
    {
        this._moveNextCallBack = moveNextCallBack;
    }

    Action _call;
    Action<TaskAwaiter> _moveNextCallBack;
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
    public void TryCancel()
    {
        this._isDisposed = true;
        this._call = null;
    }

    /// <summary>
    /// 执行下一步
    /// </summary>
    public void TryMoveNext()
    {
        if (this._isDisposed) return;

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
