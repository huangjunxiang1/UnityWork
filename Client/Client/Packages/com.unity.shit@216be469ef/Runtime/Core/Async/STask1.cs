using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Main;
using System.Diagnostics;

[AsyncMethodBuilder(typeof(STaskBuilder1<>))]
public sealed class STask<T> : STask
{
    public STask() : base()
    {

    }
    public STask(object tag) : base(tag)
    {

    }
    public STask(Task<T> warpTask) : base(warpTask)
    {
        this.WarpTask = warpTask;
    }

    T _result;

    public new Task<T> WarpTask { get; }

    public new STask<T> GetAwaiter()
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
        if (this.IsCompleted || this.Disposed) return;
        this._result = result;
        base.TrySetResult();
    }
    public override void TrySetResult(object result)
    {
        if (this.IsCompleted || this.Disposed) return;
        if (result is T t)
            this._result = t;
        base.TrySetResult(result);
    }
    public new STask<T> MakeAutoCancel(bool autoCancel = true)
    {
        base.MakeAutoCancel(autoCancel);
        return this;
    }
}
