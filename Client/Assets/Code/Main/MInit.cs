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
        SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);
        SysEvent.RigisterAllStaticListener();
        WObjectManager.Inst.init();
        SysNet.Connect(NetType.KCP, MUtil.ToIPEndPoint(MGameSetting.LoginAddress));
    }
}
