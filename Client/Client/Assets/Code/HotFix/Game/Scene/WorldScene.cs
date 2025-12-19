using Game;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

[Scene("World")]
class WorldScene : Scene
{
    Dictionary<int2, List<GameObject>> grids = new();
    public override async void OnEnter()
    {
        CMInput input = new CMInput();
        input.CMEditorMouseClick.performed += e =>
        {
            if (e.ReadValueAsButton())
            {
                var point = UnityEngine.InputSystem.Pointer.current.position.value;
                var ray = Camera.main.ScreenPointToRay(point);
                if (Physics.Raycast(ray, out var hit, 1000, 1 << LayerMask.NameToLayer("ground")))
                {
                    var p = hit.point;
                }
            }
        };

        SGameObject go = new() { ActorId = 1 };
        Client.Scene.Current.AddChild(go);
        go.GameObject.SetGameObject("3D_chan");
        go.AddComponent<PlayerComponent>().scene = this;
        go.AddComponent<MoveComponent>();
        var kv = go.AddComponent<KVComponent>();
        kv.Set((int)KType.MoveSpeed, 5);
        kv.Set((int)KType.RotateSpeed, 20);

        await Client.UI.OpenAsync<FUIWorld>(go);
    }
    public override void Dispose()
    {
        base.Dispose();
        foreach (var item in grids.Values)
        {
            for (int i = 0; i < item.Count; i++)
                SAsset.Release(item[i]);
        }
    }

    public void CreateRoundGrid(int2 xy)
    {
        int2 dis = Hex.HexQuad * 2 + 1;

        Dictionary<int2, List<GameObject>> tmp = new();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int2 n = xy + new int2(dis.x * i, dis.y * j);
                if (grids.TryGetValue(n,out var v))
                {
                    tmp[n] = v;
                    grids.Remove(n);
                    continue;
                }
                tmp[n] = CreateGrid(n);
            }
        }
        foreach (var item in grids.Values)
        {
            for (int i = 0; i < item.Count; i++)
                SAsset.Release(item[i]);
        }
        grids = tmp;
    }

    List<GameObject> CreateGrid(int2 xy)
    {
        List<GameObject> gos = new();
        for (int dx = xy.x - Hex.HexQuad.x; dx <= xy.x + Hex.HexQuad.x; dx++)
        {
            for (int dy = xy.y - Hex.HexQuad.y; dy <= xy.y + Hex.HexQuad.y; dy++)
            {
                var go = SAsset.LoadGameObject("3D_hex");
                go.transform.position = Hex.HexToPosition(new int2(dx, dy));
                go.transform.parent = this.root.transform;
                go.name = $"{dx},{dy}";
                gos.Add(go);
            }
        }
        return gos;
    }
}
