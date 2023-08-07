using Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

[DebuggerNonUserCode]
public sealed class TaskAwaiterBuilder : AsyncBaseBuilder
{
    public static TaskAwaiterBuilder Create()
    {
        return new();
    }
    public TaskAwaiter Task => Awaiter;

    public void SetException(Exception ex)
    {
        this.Task.SetException(ex);
    }
    public void SetResult()
    {
        this.Task.TrySetResult();
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
        this.Awaiter.Target = Types.GetStateMachineThisField(stateMachine.GetType())?.GetValue(stateMachine);
        stateMachine.MoveNext();
    }
    public void SetStateMachine(IAsyncStateMachine stateMachine)
    {

    }
}
