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
using Cinemachine;

class SceneMgr : ManagerL<SceneMgr>
{
    public override bool DisposeOnChangeScene => false;
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
        if (Application.isEditor)
            SysEvent.ExecuteEvent((int)EIDM.OutScene);
        SysNet.DisConnect();
    }

    public async TaskAwaiter InLoginScene()
    {
        if (CurScene == 1) return;

        BaseCamera.Current?.DisableCamera();
        UIS.CloseAll();
        WRoot.Inst.RemoveAllChildren();
        var ui = await UIS.OpenAsync<FUILoading>();

        SysEvent.ExecuteEvent((int)EIDM.OutScene, CurScene);
        CurScene = 1;

        await SceneManager.LoadSceneAsync(TabL.GetScene(CurScene).name);
        await Task.Delay(100);//场景重复加载时 会有一帧延迟才能find场景的GameObject

        await UIS.OpenAsync<FUILogin>();
        ui.max = 1;

        SysEvent.ExecuteEvent((int)EIDM.InScene, CurScene);
    }
    public async TaskAwaiter InScene(int SceneID)
    {
        if (SceneID <= 1 || CurScene == SceneID) return;

        UIS.CloseAll();
        WRoot.Inst.RemoveAllChildren();
        var ui = await UIS.OpenAsync<FUILoading>();

        SysEvent.ExecuteEvent((int)EIDM.OutScene, CurScene);
        CurScene = SceneID;

        await SceneManager.LoadSceneAsync(TabL.GetScene(CurScene).name);
        await Task.Delay(100);

        ui.max = 1;

        SysEvent.ExecuteEvent((int)EIDM.InScene, CurScene);
        BaseCamera.SetCamera(new FreedomCamera(GameObject.FindObjectOfType<CinemachineBrain>()));
        GameObject cm = new GameObject("CMTarget");
        cm.transform.position = new Vector3(0, 0, 0);
        BaseCamera.Current.Init(cm);
        BaseCamera.Current.EnableCamera();
    }
}