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

class SceneMgr : ManagerL<SceneMgr>
{
    public int CurScene { get; private set; }

    public override void Init()
    {
        base.Init();
        Recast.Init();
    }
    public override void Dispose()
    {
        base.Dispose();
        if (CurScene != 0)
        {
            Recast.FreeMap(CurScene);
            CurScene = 0;
        }
        Recast.Fini();
    }

    [Event((int)EIDM.QuitGame)]
    static void QuitGame()
    {
        SysEvent.ExcuteEvent((int)EIDL.OutScene);
    }

    public async TaskAwaiter InLoginScene()
    {
        if (CurScene == 1) return;

        UIS.CloseAll();
        var ui = UIS.Open<FUILoading>();

        CM.Follow(null);
        CM.LookAt(null);
        isBreak = true;
        SysEvent.ExcuteEvent((int)EIDL.OutScene, CurScene);
        if (CurScene != 0)
            Recast.FreeMap(CurScene);
        CurScene = 1;
        WRoot.Inst.RemoveAllChildren();

        TaskAwaiter task = new TaskAwaiter();
        void loadRet(UnityEngine.SceneManagement.Scene s, LoadSceneMode m)
        {
            SceneManager.sceneLoaded -= loadRet;
            task.TrySetResult();
        }
        SceneManager.sceneLoaded += loadRet;
        SceneManager.LoadSceneAsync(TabL.GetScene(CurScene).name);

        await task;
        ui.max = 1;
        UIS.Open<FUILogin>();

        SysEvent.ExcuteEvent((int)EIDL.InScene, CurScene);
    }
    public async TaskAwaiter InScene(int SceneID)
    {
        if (SceneID <= 1) return;

        UIS.CloseAll();
        var ui = UIS.Open<FUILoading>();

        isBreak = true;
        SysEvent.ExcuteEvent((int)EIDL.OutScene, CurScene);
        if (CurScene != 0)
            Recast.FreeMap(CurScene);
        CurScene = SceneID;
        WRoot.Inst.RemoveAllChildren();
        
        TaskAwaiter task = new TaskAwaiter();
        void loadRet(UnityEngine.SceneManagement.Scene s, LoadSceneMode m)
        {
            SceneManager.sceneLoaded -= loadRet;
            task.TrySetResult();
        }
        SceneManager.sceneLoaded += loadRet;
        SceneManager.LoadSceneAsync(TabL.GetScene(CurScene).name);

        await task;
        ui.max = 1;
        UIS.Open<FUIFighting>();

        string p = Application.dataPath + "/../../MapObj/Main.bin";
        bool b = Recast.LoadMap(CurScene, p);
        if (b)
            Loger.Log("地图数据加载成功");
        else
            Loger.Error("地图数据加载失败");

        pathFind();

        SysEvent.ExcuteEvent((int)EIDL.InScene, CurScene);
    }

    bool isBreak = false;
    async void pathFind()
    {
        WRole role = new WRole(20, GameObject.CreatePrimitive(PrimitiveType.Cube));
        role.Speed = 6;
        WRoot.Inst.AddChild(role);
        CM.Follow(role.GameObject.transform);
        CM.LookAt(role.GameObject.transform);

        float wheel = 1f;

        isBreak = false;
        while (!isBreak)
        {
            float _wheel = Input.GetAxis("Mouse ScrollWheel");
            wheel -= _wheel;
            wheel = Mathf.Clamp(wheel, 0.1f, 2);
            CM.Wheel(wheel);
            if (Input.GetMouseButtonDown(1))
            {
                try
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit, 1000, -1))
                    {
                        if (Recast.FindPath(CurScene, role.Position, hit.point))
                        {
                            if (Recast.Smooth(CurScene, 2f, 0.5f))
                            {
                                float[] smooths = Recast.GetPathSmooth(CurScene, out int smoothCount);
                                List<Vector3> result = new List<Vector3>(20);
                                for (int i = 0; i < smoothCount; ++i)
                                {
                                    Vector3 node = new Vector3(smooths[i * 3], smooths[i * 3 + 1], smooths[i * 3 + 2]);
                                    result.Add(node);
                                }
                                role.MovePath(result);
                            }
                            else
                                Loger.Error("平滑失败");
                        }
                        else
                            Loger.Error("寻路失败");
                    }
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                }
            }
            await Task.Delay(1);
        }
    }
}