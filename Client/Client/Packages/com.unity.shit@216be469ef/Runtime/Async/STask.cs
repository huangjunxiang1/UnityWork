using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Reflection;

[AsyncMethodBuilder(typeof(STaskBuilder))]
public class STask : ICriticalNotifyCompletion
{
    static STask()
    {
        runner = typeof(AsyncVoidMethodBuilder).Assembly.GetTypes().First(t => t.FullName == "System.Runtime.CompilerServices.AsyncMethodBuilderCore+MoveNextRunner");
        runnerField = runner.GetField("m_stateMachine", BindingFlags.NonPublic | BindingFlags.Instance);
    }
    public STask()
    {
    }
    public STask(object tag)
    {
        this.Tag = tag;
    }
    public STask(Task warpTask)
    {
        this.WarpTask = warpTask;
        waitTask(this, warpTask);
    }

    static Type runner;
    static FieldInfo runnerField;
    Action _event;
    bool _Disposed = false;

    public object Tag { get; }
    public Task WarpTask { get; }
    public bool AutoCancel { get; private set; }
    public IAsyncCancel Target { get; private set; }

    public bool Disposed
    {
        get
        {
            if (_Disposed)
                return true;
            if (this.AutoCancel)
            {
                if (Target != null)
                    return Target.Disposed;
                return false;
            }
            return false;
        }
    }
    /// <summary>
    /// 是否已完成
    /// </summary>
    public bool IsCompleted { get; private set; }

    public static STask Completed { get; } = new STask() { IsCompleted = true };


    public STask GetAwaiter()
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
        if (this.IsCompleted || this.Disposed) return;

        this._Disposed = true;

        this._event = null;
    }

    /// <summary>
    /// 执行下一步
    /// </summary>
    public bool TrySetResult()
    {
        if (this.IsCompleted || this.Disposed) return false;
       
        this._Disposed = true;
        this.IsCompleted = true;

        Action act = this._event;
        this._event = null;
        act?.Invoke();
        return true;
    }

    /// <summary>
    /// 错误
    /// </summary>
    /// <param name="e"></param>
    public void SetException(Exception e)
    {
        Loger.Error("TaskAwaiter Error " + e);
        if (this.IsCompleted || this.Disposed) return;

        this._Disposed = true;

        Action act = this._event;
        this._event = null;
        act?.Invoke();
    }

    public void AddEvent(Action evt)
    {
        if (evt == null)
            return;
        if (IsCompleted)
        {
            evt.Invoke();
            return;
        }
        this._event += evt;
        this.AutoCancel = false;
    }
    public STask MakeAutoCancel(bool autoCancel = true)
    {
        this.AutoCancel = autoCancel;
        return this;
    }

    void INotifyCompletion.OnCompleted(Action callBack)
    {
        if (this.AutoCancel)
        {
            if (Target == null)
            {
                if (callBack.Target is IAsyncStateMachine)
                {
                    if (Types.GetStateMachineThisField(callBack.Target.GetType())?.GetValue(callBack.Target) is IAsyncCancel o)
                        Target = o;
                    else
                        this.AutoCancel = false;
                }
                else if (callBack.Target.GetType() == runner)
                {
                    var v = runnerField.GetValue(callBack.Target);
                    if (Types.GetStateMachineThisField(v.GetType())?.GetValue(v) is IAsyncCancel o)
                        Target = o;
                    else
                        this.AutoCancel = false;
                }
                else
                    this.AutoCancel = false;
            }
            else
                this.AutoCancel = false;
        }

        this._event += callBack;
    }
    void ICriticalNotifyCompletion.UnsafeOnCompleted(Action callBack)
    {
        if (this.AutoCancel)
        {
            if (Target == null)
            {
                if (callBack.Target is IAsyncStateMachine)
                {
                    if (Types.GetStateMachineThisField(callBack.Target.GetType())?.GetValue(callBack.Target) is IAsyncCancel o)
                        Target = o;
                    else
                        this.AutoCancel = false;
                }
                else if (callBack.Target.GetType() == runner)
                {
                    var v = runnerField.GetValue(callBack.Target);
                    if (Types.GetStateMachineThisField(v.GetType())?.GetValue(v) is IAsyncCancel o)
                        Target = o;
                    else
                        this.AutoCancel = false;
                }
                else
                    this.AutoCancel = false;
            }
            else
                this.AutoCancel = false;
        }

        this._event += callBack;
    }

    static async void waitTask(STask taskAwaiter, Task task)
    {
        await task;
        taskAwaiter.TrySetResult();
    }

    public static STask Delay(int millisecondsDelay)
    {
        return new STask(Task.Delay(millisecondsDelay));
    }
    public static async STask All(IEnumerable<STask> itor)
    {
        if (itor == null)
            await STask.Completed;
        var ie = itor.GetEnumerator();
        while (ie.MoveNext())
            await ie.Current;
    }
    public static async STask All(params STask[] tasks)
    {
        for (int i = 0; i < tasks.Length; i++)
            await tasks[i];
    }
    public static async STask<K[]> All<K>(IEnumerable<STask<K>> itor)
    {
        if (itor == null)
            return new K[0];
        else
        {
            K[] rs = new K[itor.Count()];
            var ie = itor.GetEnumerator();
            int i = 0;
            while (ie.MoveNext())
                rs[i++] = await ie.Current;
            return rs;
        }
    }
    public static async STask<K[]> All<K>(params STask<K>[] tasks)
    {
        K[] rs = new K[tasks.Length];
        for (int i = 0; i < tasks.Length; i++)
            rs[i] = await tasks[i];
        return rs;
    }

    public static STask Any(IEnumerable<STask> itor)
    {
        if (itor == null)
            return STask.Completed;

        STask waiter = new();
        async void wait(STask task)
        {
            await task;
            waiter.TrySetResult();
        }
        var ie = itor.GetEnumerator();
        while (ie.MoveNext())
            wait(ie.Current);
        return waiter;
    }
    public static STask Any(params STask[] tasks)
    {
        STask waiter = new();
        async void wait(STask task)
        {
            await task;
            waiter.TrySetResult();
        }

        for (int i = 0; i < tasks.Length; i++)
            wait(tasks[i]);
        return waiter;
    }
    public static STask<K> Any<K>(IEnumerable<STask<K>> itor)
    {
        STask<K> waiter = new();
        if (itor == null) waiter.TrySetResult(default);
        else
        {
            async void wait(STask<K> task)
            {
                await task;
                waiter.TrySetResult(task.GetResult());
            }

            var ie = itor.GetEnumerator();
            while (ie.MoveNext())
                wait(ie.Current);
        }
        return waiter;
    }
    public static STask<K> Any<K>(params STask<K>[] tasks)
    {
        STask<K> waiter = new();

        async void wait(STask<K> task)
        {
            await task;
            waiter.TrySetResult(task.GetResult());
        }

        for (int i = 0; i < tasks.Length; i++)
            wait(tasks[i]);

        return waiter;
    }

    public static STask AnyAfterCancel(IEnumerable<STask> itor)
    {
        if (itor == null)
            return STask.Completed;

        STask waiter = new();

        async void wait(STask task)
        {
            await task;
            waiter.TrySetResult();

            var ie = itor.GetEnumerator();
            while (ie.MoveNext())
                ie.Current.TryCancel();
        }
        var ie = itor.GetEnumerator();
        while (ie.MoveNext())
            wait(ie.Current);
        return waiter;
    }
    public static STask AnyAfterCancel(params STask[] tasks)
    {
        STask waiter = new();
        async void wait(STask task)
        {
            await task;
            waiter.TrySetResult();
            for (int i = 0; i < tasks.Length; i++)
                tasks[i].TryCancel();
        }

        for (int i = 0; i < tasks.Length; i++)
            wait(tasks[i]);
        return waiter;
    }
    public static STask<K> AnyAfterCancel<K>(IEnumerable<STask<K>> itor)
    {
        STask<K> waiter = new();
        if (itor == null) waiter.TrySetResult(default);
        else
        {
            async void wait(STask<K> task)
            {
                await task;
                waiter.TrySetResult(task.GetResult());

                var ie = itor.GetEnumerator();
                while (ie.MoveNext())
                    ie.Current.TryCancel();
            }
            var ie = itor.GetEnumerator();
            while (ie.MoveNext())
                wait(ie.Current);
        }
        return waiter;
    }
    public static STask<K> AnyAfterCancel<K>(params STask<K>[] tasks)
    {
        STask<K> waiter = new();

        async void wait(STask<K> task)
        {
            await task;
            waiter.TrySetResult(task.GetResult());
            for (int i = 0; i < tasks.Length; i++)
                tasks[i].TryCancel();
        }

        for (int i = 0; i < tasks.Length; i++)
            wait(tasks[i]);

        return waiter;
    }
}
