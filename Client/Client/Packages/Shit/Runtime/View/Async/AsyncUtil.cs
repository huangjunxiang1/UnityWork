using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

#if Dotween
using DG.Tweening;
using Core;

#endif

#if FairyGUI
using FairyGUI;
#endif

namespace Game
{
    public static class AsyncUtil
    {
        public static STask AsTask(this AsyncOperation op)
        {
            if (op.isDone)
                return STask.Completed;

            STask task = new();
            op.completed += e => task.TrySetResult();
            return task;
        }
#if UNITY_2023_1_OR_NEWER
        public static STask<T> AsTask<T>(this AsyncInstantiateOperation<T> op)
        {
            STask<T> task = new();
            if (op.isDone)
                task.TrySetResult(op.Result[0]);
            else
                op.completed += e => task.TrySetResult(op.Result[0]);
            return task;
        }
#endif
        public static STask AsTask(this UnityEvent unityEvent)
        {
            STask task = new();

            //只触发一次 然后移除掉
            void trigger()
            {
                unityEvent.RemoveListener(trigger);
                task.TrySetResult();
            }
            unityEvent.AddListener(trigger);

            return task;
        }
        public static STask AsTask(this IEnumerator ie)
        {
            if (ie.MoveNext())
                return STask.Completed;

            STask task = new();
            void update()
            {
                if (ie.MoveNext())
                {
                    Client.World.Timer.Remove(update);
                    task.TrySetResult();
                }
            }
            Client.World.Timer.Add(0.1f, -1, update);
            return task;
        }
#if FairyGUI
        public static STask AsTask(this GTweener tween)
        {
            if (tween.completed)
                return STask.Completed;

            STask task = new();
            tween.OnComplete((GTweenCallback)task.TrySetResult);
            return task;
        }
        public static STask AsTask(this Transition t)
        {
            STask task = new();
            t.Play(task.TrySetResult);
            return task;
        }
        public static STask AsReverseTask(this Transition t)
        {
            STask task = new();
            t.PlayReverse(task.TrySetResult);
            return task;
        }
        public static STask AsTask(this EventListener eventListener)
        {
            STask task = new();

            //只触发一次 然后移除掉
            void trigger()
            {
                eventListener.Remove(trigger);
                task.TrySetResult();
            }
            eventListener.Add(trigger);

            return task;
        }
#endif
#if Dotween
        public static STask AsTask(this Tween tween)
        {
            if (tween.IsComplete())
                return STask.Completed;

            STask task = new();
            tween.OnComplete(task.TrySetResult);
            return task;
        }
#endif
#if Addressables
        public static STask<T> AsTask<T>(this UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<T> op)
        {
            STask<T> task = new();
            if (op.IsDone)
                task.TrySetResult(op.Result);
            else
                op.Completed += e => task.TrySetResult(e.Result);
            return task;
        }
#endif
    }
}
