﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

public struct SValueTaskBuilder
{
    SValueTask _task;
    public static SValueTaskBuilder Create()
    {
        return new();
    }
    public SValueTask Task => _task;

    public void SetException(Exception ex)
    {
        this._task.SetException(ex);
    }
    public void SetResult()
    {
        this._task.TrySetResult();
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
        this._task = new(0);
        //this._task.MakeAutoCancel(CoreTypes.AsyncInvokeIsNeedAutoCancel(stateMachine.GetType()));
        stateMachine.MoveNext();
    }
    public void SetStateMachine(IAsyncStateMachine stateMachine)
    {

    }
}