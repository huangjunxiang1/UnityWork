using System;
using System.Runtime.CompilerServices;


abstract class TaskItem
{
    public TaskItem last;
    public abstract void Cancel();
    public abstract void SetResult();
}
class TaskItem<T> : TaskItem
{
    public uint version;
    public Action action;
    public T value;

    public override void Cancel()
    {
        ++version;
        action = null;
        value = default;
        var a = this.last;
        this.last = null;
        a?.Cancel();
        ObjectPool.Return(this);
    }

    public override void SetResult()
    {
        ++version;
        action = null;
        this.last = null;
        ObjectPool.Return(this);
    }
}

[AsyncMethodBuilder(typeof(SValueTaskBuilder))]
public struct SValueTask : ICriticalNotifyCompletionV2, IDispose
{
    public static Action<int, SValueTask> DelayHandle;

    public static SValueTask Create()
    {
        SValueTask task = new();
        task.taskItem = ObjectPool.Get<TaskItem<int>>();
        task._version = ++task.taskItem.version;
        return task;
    }

    uint _version;
    TaskItem<int> taskItem;
    TaskItem ICriticalNotifyCompletionV2.Current => taskItem;

    public bool Disposed => taskItem == null || taskItem.version != _version;
    /// <summary>
    /// 是否已完成
    /// </summary>
    public bool IsCompleted => taskItem == null || taskItem.version != _version;

    public SValueTask GetAwaiter()
    {
        return this;
    }
    public void GetResult()
    {

    }

    public void TryCancel()
    {
        if (this.Disposed) return;
        taskItem.Cancel();
    }
    public void TrySetResult()
    {
        if (this.Disposed) return;
        Action act = taskItem.action;
        taskItem.SetResult();
        act?.Invoke();
    }

    public void Dispose() => this.TryCancel();

    /// <summary>
    /// 错误
    /// </summary>
    /// <param name="e"></param>
    public void SetException(Exception e)
    {
        Loger.Error("SValueTask Error " + e);
        this.TrySetResult();
    }

    void INotifyCompletion.OnCompleted(Action continuation)
    {
        taskItem.action += continuation;
    }

    void ICriticalNotifyCompletion.UnsafeOnCompleted(Action continuation)
    {
        taskItem.action += continuation;
    }
    internal void Binding<K>(K task) where K : INotifyCompletionV2
    {
        if (task.Current != null)
            task.Current.last = this.taskItem;
    }
    internal void BindingCritical<K>(K task) where K : ICriticalNotifyCompletionV2
    {
        if (task.Current != null)
            task.Current.last = this.taskItem;
    }

    public static SValueTask Delay(int millisecondsDelay)
    {
        var task = SValueTask.Create();
#if DebugEnable
        if (DelayHandle == null)
            throw new Exception("muse be set DelayHandle before invoke Delay");
#endif
        DelayHandle.Invoke(millisecondsDelay, task);
        return task;
    }
}