using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Core
{
    public class ThreadSynchronizationContext : SynchronizationContext
    {
        public static ThreadSynchronizationContext Instance { get; } = new ThreadSynchronizationContext(Thread.CurrentThread.ManagedThreadId);

        private readonly int threadId;

        // 线程同步队列,发送接收socket回调都放到该队列,由poll线程统一执行
        private readonly ConcurrentQueue<Action> queue = new();

        private Action a;

        public ThreadSynchronizationContext(int threadId)
        {
            SetSynchronizationContext(this);
            this.threadId = threadId;

            Loger.__get__log += o => Post(() => UnityEngine.Debug.Log(o));
            Loger.__get__warning += o => Post(() => UnityEngine.Debug.LogWarning(o));
            Loger.__get__error += o => Post(() => UnityEngine.Debug.LogError(o));
        }

        public void Update()
        {
            while (true)
            {
                if (!this.queue.TryDequeue(out a))
                    return;

                try
                {
                    a();
                }
                catch (Exception e)
                {
                    Loger.Error(e);
                }
            }
        }

        public override void Post(SendOrPostCallback callback, object state)
        {
            this.Post(() => callback(state));
        }
		
        public void Post(Action action)
        {
            if (Thread.CurrentThread.ManagedThreadId == this.threadId)
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    Loger.Error(e);
                }

                return;
            }

            this.queue.Enqueue(action);
        }
		
        public void PostNext(Action action)
        {
            this.queue.Enqueue(action);
        }
    }
}