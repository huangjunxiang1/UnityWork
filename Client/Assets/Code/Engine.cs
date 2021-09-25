using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Engine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        var a1 = typeof(MInit).Assembly.GetTypes();
        var a2 = typeof(LInit).Assembly.GetTypes();
        List<Type> lst = new List<Type>(a1.Length + a2.Length);
        lst.AddRange(a1);
        lst.AddRange(a2);
        TypesCache.InitTypes(lst.ToArray());
        MInit.Init();
        LInit.Init();
    }

    // Update is called once per frame
    void Update()
    {
        Main.ThreadSynchronizationContext.Instance.Update();
        Main.SysNet.Update();
        Timer.Update();
    }
}
