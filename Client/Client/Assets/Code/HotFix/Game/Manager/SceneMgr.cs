using Game;
using Main;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

class SceneMgr : ManagerL<SceneMgr>
{
    public int CurScene { get; private set; }

    public override void Init()
    {
        base.Init();
    }
    public override void Dispose()
    {
        base.Dispose();
        if (CurScene != 0)
            CurScene = 0;
    }

    [Event((int)EIDM.QuitGame)]
    static void QuitGame()
    {
        SysEvent.ExecuteEvent((int)EIDL.OutScene);
        SysNet.DisConnect();
    }

    public async TaskAwaiter InLoginScene()
    {
        if (CurScene == 1) return;

        UIS.CloseAll();
        var ui = UIS.Open<FUILoading>();

        CM.Exit();

        SysEvent.ExecuteEvent((int)EIDL.OutScene, CurScene);
        CurScene = 1;

        await SceneManager.LoadSceneAsync(TabL.GetScene(CurScene).name);
        await Task.Delay(100);//场景重复加载时 会有一帧延迟才能find场景的GameObject

        ui.max = 1;
        UIS.Open<FUILogin>();

        SysEvent.ExecuteEvent((int)EIDL.InScene, CurScene);
    }
    public async TaskAwaiter InScene(int SceneID)
    {
        if (SceneID <= 1 || CurScene == SceneID) return;

        UIS.CloseAll();
        var ui = UIS.Open<FUILoading>();

        SysEvent.ExecuteEvent((int)EIDL.OutScene, CurScene);
        CurScene = SceneID;
        
        await SceneManager.LoadSceneAsync(TabL.GetScene(CurScene).name);
        await Task.Delay(100);//场景重复加载时 会有一帧延迟才能find场景的GameObject

        ui.max = 1;
        UIS.Open<FUIFighting>();

        SysEvent.ExecuteEvent((int)EIDL.InScene, CurScene);
        WObject cm = new WObject(2, new GameObject("CMTarget"));
        WRoot.Inst.AddChild(cm);
        cm.Position = default;
        CM.Init(cm);
    }
}