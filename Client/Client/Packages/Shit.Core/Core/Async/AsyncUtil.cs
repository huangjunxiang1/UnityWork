using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

[DebuggerNonUserCode]
public static class AsyncUtil
{
    public static STask AsTask(this Eventer eventer)
    {
        STask task = new();

        //只触发一次 然后移除掉
        void trigger()
        {
            eventer.Remove(trigger);
            task.TrySetResult();
        }
        eventer.Add(trigger);

        return task;
    }
}