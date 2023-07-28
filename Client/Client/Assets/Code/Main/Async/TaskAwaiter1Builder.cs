using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[DebuggerNonUserCode]
public sealed class TaskAwaiterBuilder<T>
{
    TaskAwaiter<T> _awaiter;
    public static TaskAwaiterBuilder<T> Create()
    {
        TaskAwaiterBuilder<T> builder = new() { _awaiter = new TaskAwaiter<T>() };
        return builder;
    }
    public TaskAwaiter<T> Task => _awaiter;

    public void SetException(Exception ex)
    {
        this._awaiter.SetException(ex);
    }
    public void SetResult(T result)
    {
        this._awaiter.TrySetResult(result);
    }
    public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
    {
        if (awaiter is TaskAwaiter task)
            task.AddAsyncRoute(_awaiter);
        awaiter.OnCompleted(stateMachine.MoveNext);
    }
    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
    {
        if (awaiter is TaskAwaiter task)
            task.AddAsyncRoute(_awaiter);
        awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
    }
    public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
    {
        stateMachine.MoveNext();
    }
    public void SetStateMachine(IAsyncStateMachine stateMachine)
    {

    }
}
