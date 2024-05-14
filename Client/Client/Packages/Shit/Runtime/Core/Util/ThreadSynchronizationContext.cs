using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Core
{
    public class ThreadSynchronizationContext : SynchronizationContext
    {
        public readonly int threadId;

        // 线程同步队列,发送接收socket回调都放到该队列,由poll线程统一执行
        private readonly ConcurrentQueue<Action> queue = new();
        private Action a;

        static ConcurrentDictionary<int, ThreadSynchronizationContext> map = new();

        public static ThreadSynchronizationContext MainThread { get; private set; }

        public static ThreadSynchronizationContext GetOrCreate(int threadID)
        {
            if (!map.TryGetValue(threadID, out var v))
                v = map[threadID] = new(threadID);
            return v;
        }
        public static void SetMainThread(ThreadSynchronizationContext tsc) => MainThread = tsc;
        public ThreadSynchronizationContext(int threadId)
        {
            this.threadId = threadId;
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
            if (Environment.CurrentManagedThreadId == this.threadId)
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