using Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[DebuggerNonUserCode]
public sealed class TaskAwaiterBuilder<T> : AsyncBaseBuilder
{
    public static TaskAwaiterBuilder<T> Create()
    {
        return new();
    }
    public TaskAwaiter<T> Task => (TaskAwaiter<T>)Awaiter;

    public void SetException(Exception ex)
    {
        this.Task.SetException(ex);
    }
    public void SetResult(T result)
    {
        this.Task.TrySetResult(result);
    }
    public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
    {
        awaiter.OnCompleted(stateMachine.MoveNext);
    }
    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
    {
        awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
    }
    public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
    {
        this.Awaiter = new();
        this.Awaiter.MakeAutoCancel(Types.AsyncInvokeIsNeedAutoCancel(stateMachine.GetType()));
        this.Target = Types.GetStateMachineThisField(stateMachine.GetType())?.GetValue(stateMachine) as IAsyncDisposed;
        stateMachine.MoveNext();
    }
    public void SetStateMachine(IAsyncStateMachine stateMachine)
    {

    }
}
