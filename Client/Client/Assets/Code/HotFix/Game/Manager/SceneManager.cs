using Game;
using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class SceneManager : ManagerL<SceneManager>
{
    public int CurScene { get; private set; }

    public override void init()
    {
        base.init();
        RecastNav.Init();
    }
    public override void Dispose()
    {
        base.Dispose();
        if (CurScene != 0)
        {
            RecastNav.FreeMap(CurScene);
            CurScene = 0;
        }
        RecastNav.Fini();
    }

    [Event((int)EventIDL.InScene)]
    void EnterScene(EventerContent e)
    {
        if (CurScene != 0)
            RecastNav.FreeMap(CurScene);
        CurScene = e.ValueInt;
        string p = Application.dataPath + "/../../MapObj/Main.bin";
        bool b = RecastNav.LoadMap(CurScene, p);
        if (b)
            Loger.Log("加载成功");
        else
            Loger.Error("地图数据加载失败");

        pathFind();
    }

    async void pathFind()
    {
        WRole role = new WRole(20, GameObject.CreatePrimitive(PrimitiveType.Cube));
        role.Speed = 6;
        WRoot.Inst.AddChild(role);

        while (true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                try
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit, 1000, -1))
                    {
                        if (RecastNav.FindPath(CurScene, role.Position, hit.point))
                        {
                            if (RecastNav.Smooth(CurScene, 2f, 0.5f))
                            {
                                float[] smooths = RecastNav.GetPathSmooth(CurScene, out int smoothCount);
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