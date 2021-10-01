using Game;
using Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class MInit
{
    public static void Init()
    {
        test();
        return;
        SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);
        SysEvent.RigisterAllStaticListener();
        WObjectManager.Inst.init();
        SysNet.Connect(NetType.KCP, MUtil.ToIPEndPoint(MGameSetting.LoginAddress));
    }
    static async void test()
    {
        Loger.Error("test");
        TaskAwaiter task = new TaskAwaiter();
        Timer.Add(5,1,()=>
        {
            Loger.Error("TryMoveNext");
            task.TryMoveNext();
        });
        await task; 
        Loger.Error("task1");
        TaskAwaiter<int> task1 = new TaskAwaiter<int>();
        Timer.Add(5, 1, () =>
        {
            Loger.Error("TrySetResult");
            task1.TrySetResult(77);
            Loger.Error("TrySetResult  end");
        });
        int a = await task1;
        Loger.Error("get "+a); 
        int b = await task1;
        Loger.Error("get " + b);
    }
}
