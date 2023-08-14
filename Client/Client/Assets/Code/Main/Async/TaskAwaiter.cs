using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Unity.Entities.UniversalDelegates;

[AsyncMethodBuilder(typeof(TaskAwaiterBuilder))]
public class TaskAwaiter : ICriticalNotifyCompletion
{
    public TaskAwaiter()
    {
        this.Builders = ObjectPool.Get<List<AsyncBaseBuilder>>();
    }
    public TaskAwaiter(object tag)
    {
        this.Tag = tag;
        this.Builders = ObjectPool.Get<List<AsyncBaseBuilder>>();
    }
    public TaskAwaiter(Task warpTask)
    {
        this.WarpTask = warpTask;
        this.Builders = ObjectPool.Get<List<AsyncBaseBuilder>>();
        waitTask(this, warpTask);
    }

    Action _event;
    bool _isDisposed = false;

    public object Tag { get; }
    public Task WarpTask { get; }
    public bool AutoCancel { get; private set; }
    public List<AsyncBaseBuilder> Builders { get; }
    internal object Target { get; set; }

    public bool IsDisposed
    {
        get
        {
            if (_isDisposed)
                return true;
            if (this.AutoCancel)
            {
                for (int i = 0; i < Builders.Count; i++)
                {
                    if (Builders[i].Target == null || !Builders[i].Target.Disposed)
                        return false;
                }
                return true;
            }
            return false;
        }
    }
    /// <summary>
    /// 是否已完成
    /// </summary>
    public bool IsCompleted { get; private set; }

    public static TaskAwaiter Completed { get; } = new TaskAwaiter() { IsCompleted = true };


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
        if (this.IsCompleted || this.IsDisposed) return;

        this._isDisposed = true;
        Builders.Clear();
        ObjectPool.Return(Builders);

        this._event = null;
    }

    /// <summary>
    /// 执行下一步
    /// </summary>
    public bool TrySetResult()
    {
        if (this.IsCompleted || this.IsDisposed) return false;
       
        this._isDisposed = true;
        this.IsCompleted = true;
        Builders.Clear();
        ObjectPool.Return(Builders);

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
        if (this.IsCompleted || this.IsDisposed) return;

        this._isDisposed = true;
        Builders.Clear();
        ObjectPool.Return(Builders);

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
        Builders.Clear();
    }
    public TaskAwaiter MakeAutoCancel(bool autoCancel = true)
    {
        this.AutoCancel = autoCancel;
        return this;
    }

    void INotifyCompletion.OnCompleted(Action callBack)
    {
        if (this.AutoCancel && callBack.Target is IAsyncStateMachine)
        {
            var builder = (AsyncBaseBuilder)Types.GetStateMachineBuilderField(callBack.Target.GetType()).GetValue(callBack.Target);
            Builders.Add(builder);
        }

        this._event += callBack;
    }
    void ICriticalNotifyCompletion.UnsafeOnCompleted(Action callBack)
    {
        if (this.AutoCancel && callBack.Target is IAsyncStateMachine)
        {
            var builder = (AsyncBaseBuilder)Types.GetStateMachineBuilderField(callBack.Target.GetType()).GetValue(callBack.Target);
            Builders.Add(builder);
        }

        this._event += callBack;
    }

    static async void waitTask(TaskAwaiter taskAwaiter, Task task)
    {
        await task;
        taskAwaiter.TrySetResult();
    }

    public static TaskAwaiter Delay(int millisecondsDelay)
    {
        return new TaskAwaiter(Task.Delay(millisecondsDelay));
    }
    public static async TaskAwaiter All(IEnumerable<TaskAwaiter> itor, bool copy = true)
    {
        if (itor == null)
            await TaskAwaiter.Completed;
        var ie = itor.GetEnumerator();
        while (ie.MoveNext())
            await ie.Current;
    }
    public static async TaskAwaiter All(params TaskAwaiter[] tasks)
    {
        for (int i = 0; i < tasks.Length; i++)
            await tasks[i];
    }
    public static async TaskAwaiter<K[]> All<K>(IEnumerable<TaskAwaiter<K>> itor)
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
    public static async TaskAwaiter<K[]> All<K>(params TaskAwaiter<K>[] tasks)
    {
        K[] rs = new K[tasks.Length];
        for (int i = 0; i < tasks.Length; i++)
            rs[i] = await tasks[i];
        return rs;
    }

    public static TaskAwaiter Any(IEnumerable<TaskAwaiter> itor)
    {
        if (itor == null)
            return TaskAwaiter.Completed;

        TaskAwaiter waiter = new();
        async void wait(TaskAwaiter task)
        {
            await task;
            waiter.TrySetResult();
        }
        var ie = itor.GetEnumerator();
        while (ie.MoveNext())
            wait(ie.Current);
        return waiter;
    }
    public static TaskAwaiter Any(params TaskAwaiter[] tasks)
    {
        TaskAwaiter waiter = new();
        async void wait(TaskAwaiter task)
        {
            await task;
            waiter.TrySetResult();
        }

        for (int i = 0; i < tasks.Length; i++)
            wait(tasks[i]);
        return waiter;
    }
    public static TaskAwaiter<K> Any<K>(IEnumerable<TaskAwaiter<K>> itor)
    {
        TaskAwaiter<K> waiter = new();
        if (itor == null) waiter.TrySetResult(default);
        else
        {
            async void wait(TaskAwaiter<K> task)
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
    public static TaskAwaiter<K> Any<K>(params TaskAwaiter<K>[] tasks)
    {
        TaskAwaiter<K> waiter = new();

        async void wait(TaskAwaiter<K> task)
        {
            await task;
            waiter.TrySetResult(task.GetResult());
        }

        for (int i = 0; i < tasks.Length; i++)
            wait(tasks[i]);

        return waiter;
    }

    public static TaskAwaiter AnyAfterCancel(IEnumerable<TaskAwaiter> itor)
    {
        if (itor == null)
            return TaskAwaiter.Completed;

        TaskAwaiter waiter = new();

        async void wait(TaskAwaiter task)
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
    public static TaskAwaiter AnyAfterCancel(params TaskAwaiter[] tasks)
    {
        TaskAwaiter waiter = new();
        async void wait(TaskAwaiter task)
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
    public static TaskAwaiter<K> AnyAfterCancel<K>(IEnumerable<TaskAwaiter<K>> itor)
    {
        TaskAwaiter<K> waiter = new();
        if (itor == null) waiter.TrySetResult(default);
        else
        {
            async void wait(TaskAwaiter<K> task)
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
    public static TaskAwaiter<K> AnyAfterCancel<K>(params TaskAwaiter<K>[] tasks)
    {
        TaskAwaiter<K> waiter = new();

        async void wait(TaskAwaiter<K> task)
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
