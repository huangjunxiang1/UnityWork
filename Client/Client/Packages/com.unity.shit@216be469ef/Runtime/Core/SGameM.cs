using Game;
using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class SGameM
{
    public static EventSystem Event { get; private set; }
    public static NetSystem Net { get; private set; }
    public static SWorld World { get; private set; }
    public static SData Data { get; private set; }

    public static void Init()
    {
        Event = new EventSystem();
        Net = new NetSystem();
        World = new SWorld();
        Data = new SData();
        GameObject.DontDestroyOnLoad(new GameObject($"[{nameof(Engine)}]").AddComponent<Engine>());
    }
    public static void Close()
    {
        Net.Dispose();
    }
    static void Update()
    {
        ThreadSynchronizationContext.Instance.Update();
        Net.update();
        STimer.Update();
        SSystem.Update();
        STimer.AfterUpdate();
        SSystem.AfterUpdate();
    }

    class Engine : MonoBehaviour
    {
        private void Update()
        {
            SGameM.Update();
        }
    }
}
