using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Main;
using System.Diagnostics;

[AsyncMethodBuilder(typeof(TaskAwaiterBuilder<>))]
public sealed class TaskAwaiter<T> : TaskAwaiter
{
    public TaskAwaiter() : base()
    {

    }
    public TaskAwaiter(object tag) : base(tag)
    {

    }
    public TaskAwaiter(Task<T> warpTask) : base(warpTask)
    {

    }

    T _result;

    public new Task<T> WarpTask { get; }

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
    public bool TrySetResult(T result)
    {
        this._result = result;
        return base.TrySetResult();
    }
    public new TaskAwaiter<T> MakeAutoCancel(bool autoCancel = true)
    {
        base.MakeAutoCancel(autoCancel);
        return this;
    }
}
