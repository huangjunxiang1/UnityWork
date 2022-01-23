using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Main;
using System.Diagnostics;

[DebuggerNonUserCode]
[AsyncMethodBuilder(typeof(TaskAwaiterBuilder<>))]
public sealed class TaskAwaiter<T> : TaskAwaiter
{
    public TaskAwaiter() : base()
    {

    }
    public TaskAwaiter(object tag) : base(tag)
    {

    }

    T _result;

    public new TaskAwaiter<T> GetAwaiter()
    {
        return this;
    }
    public new T GetResult()
    {
        return _result;
    }

    /// <summary>
    /// 回传结果
    /// </summary>
    /// <param name="result"></param>
    public void TrySetResult(T result)
    {
        if (this.IsDisposed) return;

        this._result = result;
        base.TrySetResult();
    }
}
