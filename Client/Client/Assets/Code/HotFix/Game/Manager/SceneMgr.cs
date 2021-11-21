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
        SysEvent.ExcuteEvent((int)EIDL.OutScene);
        SysNet.DisConnect();
    }

    public async TaskAwaiter InLoginScene()
    {
        if (CurScene == 1) return;

        UIS.CloseAll();
        var ui = UIS.Open<FUILoading>();

        CM.Exit();

        SysEvent.ExcuteEvent((int)EIDL.OutScene, CurScene);
        CurScene = 1;

        await SceneManager.LoadSceneAsync(TabL.GetScene(CurScene).name);

        ui.max = 1;
        UIS.Open<FUILogin>();

        SysEvent.ExcuteEvent((int)EIDL.InScene, CurScene);
    }
    public async TaskAwaiter InScene(int SceneID)
    {
        if (SceneID <= 1) return;

        UIS.CloseAll();
        var ui = UIS.Open<FUILoading>();

        SysEvent.ExcuteEvent((int)EIDL.OutScene, CurScene);
        CurScene = SceneID;
        
        await SceneManager.LoadSceneAsync(TabL.GetScene(CurScene).name);

        ui.max = 1;
        UIS.Open<FUIFighting>();

        SysEvent.ExcuteEvent((int)EIDL.InScene, CurScene);
    }
}