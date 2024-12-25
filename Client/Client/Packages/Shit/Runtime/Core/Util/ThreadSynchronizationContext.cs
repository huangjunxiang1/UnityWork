using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Core
{
    public class ThreadSynchronizationContext : SynchronizationContext
    {
        public readonly int threadId;

        // 线程同步队列,发送接收socket回调都放到该队列,由poll线程统一执行
        private readonly ConcurrentQueue<(SendOrPostCallback, object)> queue = new();

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
                if (!this.queue.TryDequeue(out var a))
                    return;

                try
                {
                    a.Item1(a.Item2);
                }
                catch (Exception e)
                {
                    Loger.Error(e);
                }
            }
        }

        public override void Post(SendOrPostCallback callback, object state = null)
        {
            if (Environment.CurrentManagedThreadId == this.threadId)
            {
                try
                {
                    callback(state);
                }
                catch (Exception e)
                {
                    Loger.Error(e);
                }

                return;
            }
            this.queue.Enqueue((callback, state));
        }
        public void PostNext(SendOrPostCallback callback, object state = null)
        {
            this.queue.Enqueue((callback, state));
        }
    }
}