using FairyGUI;
using Game;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[Scene("World")]
class WorldScene : Scene
{
    public GameObject hex;
    ComputeShader_GridCulling gridCulling;
    GPUInstanceRender tree_render;
    GPUInstanceRender wall_render;
    SGameObject player;
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

        player = new() { ActorId = 1 };
        Client.Scene.Current.AddChild(player);
        player.GameObject.SetGameObject("model_chan");
        player.AddComponent<PlayerComponent>().scene = this;
        player.AddComponent<MoveComponent>();
        var kv = player.AddComponent<KVComponent>();
        kv.Set((int)KType.MoveSpeed, 5);
        kv.Set((int)KType.RotateSpeed, 20);

        hex = this.Loader.LoadGameObject("model_hex");

        wall_render = new GPUInstanceRender(new List<GameObject>() { Client.Scene.Current.Loader.LoadGameObject("model_hex2") }, viewEnable: true, maxInstance: ((Hex.Hex_GridCount - 1) / 32 + 1) * 32 * 9);
        this.AddChild(wall_render);
        gridCulling = new();
        gridCulling.Culling_wall_args = wall_render.ArgsBuffer;
        gridCulling.Culling_wall_Visible = wall_render.VisibleBuffer;
        gridCulling.Culling_wall_datas = wall_render.GetOrCreateBuffer<float2>("wall_datas");

        List<GameObject> lst = new();
        for (int i = 0; i < GPUConstDefine.Tree_TypeCount; i++)
        {
            for (int j = 0; j < GPUConstDefine.Tree_StyleCount; j++)
                lst.Add(Client.Scene.Current.Loader.LoadGameObject($"model_Tree_{i + 1}_{j + 1}"));
        }
        //单位格子最多3个树
        tree_render = new GPUInstanceRender(lst, viewEnable: true, maxInstance: ((Hex.Hex_GridCount * 3 - 1) / 32 + 1) * 32 * 9);
        gridCulling.Culling_tree_args = tree_render.ArgsBuffer;
        gridCulling.Culling_tree_Visible = tree_render.VisibleBuffer;
        gridCulling.Culling_tree_datas = tree_render.GetOrCreateBuffer<float2>("tree_datas");
        this.AddChild(tree_render);

        gridCulling.maxBatchInstance = tree_render.MaxInstance;

        await Client.UI.OpenAsync<FUIWorld>(player);
    }
    public override void Dispose()
    {
        base.Dispose();
        gridCulling.Dispose();
    }
    [Timer(0, -1)]
    void update()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            int2 xy = Hex.GetGridxy(player.Transform.position.xz);
            this.SetWall(xy, !Keyboard.current.leftAltKey.isPressed);
        }
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            int2 xy = Hex.GetGridxy(player.Transform.position.xz);
            this.Logging(xy);
        }
    }

    public void CreateRoundGrid(int2 center, int2 xy)
    {
        WorldData.Inst.CopyTreeVisibleToGraphicsBuffer(tree_render.VisibleBuffer, xy);
        WorldData.Inst.CopyWallVisibleToGraphicsBuffer(wall_render.VisibleBuffer, xy);
        gridCulling.Culling_Dispatch();
    }
    public void GridChange(int2 xy)
    {
        gridCulling.playerPos_xy = xy;
        gridCulling.Culling_Dispatch();
        hex.transform.position = Hex.GetPositon(xy);
    }
    public void Logging(int2 xy)
    {
        if (!Hex.HasTree(xy))
            return;
        if (!WorldData.Inst.Logging(xy))
            return;
        WorldData.Inst.CopyTreeVisibleToGraphicsBuffer(tree_render.VisibleBuffer, xy);
        gridCulling.Culling_Dispatch();
    }
    public void SetWall(int2 xy, bool visible)
    {
        if (!WorldData.Inst.SetWall(xy, visible))
            return;
        WorldData.Inst.CopyWallVisibleToGraphicsBuffer(wall_render.VisibleBuffer, xy);
        gridCulling.Culling_Dispatch();


        //var array3 = new int[gridCulling.Culling_wall_Visible.count];
        //gridCulling.Culling_wall_Visible.GetData(array3);
        //for (int i = 0; i < array3.Length; i++)
        //{
        //    if (array3[i]!=0)
        //    {
        //        Loger.Error($"has {i} {array3[i]}");
        //    }
        //}

        //var array = new int[5];
        //gridCulling.Culling_wall_args.GetData(array);
        //Loger.Error($" count {array[1]}");

        //var array2 = new float2[gridCulling.Culling_wall_datas.count];
        //gridCulling.Culling_wall_datas.GetData(array2);
        //Loger.Error($" pos {array2[array[1] - 1]}");
    }
}