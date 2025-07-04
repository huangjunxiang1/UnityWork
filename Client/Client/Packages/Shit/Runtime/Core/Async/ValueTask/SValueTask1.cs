using System;
using System.Runtime.CompilerServices;

[AsyncMethodBuilder(typeof(SValueTaskBuilder<>))]
public struct SValueTask<T> : ICriticalNotifyCompletionV2, IDispose
{
    public static SValueTask<T> Create()
    {
        SValueTask<T> task = new();
        task.taskItem = ObjectPool.Get<TaskItem<T>>();
        task._version = ++task.taskItem.version;
        return task;
    }

    uint _version;
    TaskItem<T> taskItem;
    TaskItem ICriticalNotifyCompletionV2.Current => taskItem;

    public bool Disposed => taskItem == null || taskItem.version != _version;
    /// <summary>
    /// 是否已完成
    /// </summary>
    public bool IsCompleted => taskItem == null || taskItem.version != _version;


    public SValueTask<T> GetAwaiter()
    {
        return this;
    }
    public T GetResult()
    {
        if (taskItem.version != this._version + 1) return default;
        return taskItem.value;
    }

    public void TryCancel()
    {
        if (this.Disposed) return;
        taskItem.Cancel();
    }
    public void TrySetResult(T value)
    {
        if (this.Disposed) return;
        taskItem.value = value;
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
        this.TrySetResult(default);
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
}
