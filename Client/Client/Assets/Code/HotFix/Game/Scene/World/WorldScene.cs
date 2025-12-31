using Game;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Scene("World")]
class WorldScene : Scene
{
    ComputeShader_GridCulling gridCulling;
    GPUInstanceRender tree_render;
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

        var grid_render = new GPUInstanceRender(new List<GameObject>() { SAsset.LoadGameObject($"3D_hex") });
        this.AddChild(grid_render);
        gridCulling = new();
        gridCulling.Culling_grid_args = grid_render.ArgsBuffer;
        gridCulling.Culling_grid_datas = grid_render.GetOrCreateBuffer<float2>("grid_datas");

        List<GameObject> lst = new();
        for (int i = 0; i < GPUConstDefine.Tree_TypeCount; i++)
        {
            for (int j = 0; j < GPUConstDefine.Tree_StyleCount; j++)
                lst.Add(SAsset.LoadGameObject($"3D_Tree_{i + 1}_{j + 1}"));
        }
        tree_render = new GPUInstanceRender(lst);
        gridCulling.Culling_tree_args = tree_render.ArgsBuffer;
        gridCulling.Culling_tree_datas = tree_render.GetOrCreateBuffer<float2>("tree_datas");
        this.AddChild(tree_render);

        gridCulling.maxBatchInstance = tree_render.MaxInstance;

        await Client.UI.OpenAsync<FUIWorld>(go);
    }
    public override void Dispose()
    {
        base.Dispose();
        gridCulling.Dispose();
    }

    public void CreateRoundGrid(int2 center, int2 xy)
    {
        WorldData.Inst.CopyVisibleToGraphicsBuffer(tree_render.ArgsBuffer, GPUConstDefine.Define_Args_Size * tree_render.Batch, xy);
        gridCulling.Culling_Dispatch();
    }
    public void GridChange(int2 xy)
    {
        gridCulling.playerPos_xy = xy;
        gridCulling.Culling_Dispatch();
    }
    public void Logging(int2 xy)
    {
        if (!WorldData.Inst.Logging(xy))
            return;
        WorldData.Inst.CopyVisibleToGraphicsBuffer(tree_render.ArgsBuffer, GPUConstDefine.Define_Args_Size * tree_render.Batch, xy);
        gridCulling.Culling_Dispatch();
    }
}