using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

partial class FUIFighting4
{
    public const int size = 100;
    public NativeArray<int> road;
    NativeArray<Entity> block;
    const int blockCount = 3000;
    public const int playerCount = 2000;

    Material mat;
    Mesh mesh;
    ComputeShader cs;
    ComputeBuffer roadCb = new ComputeBuffer(size * size, sizeof(int) * 2);
    ComputeBuffer pCb = new ComputeBuffer(playerCount, sizeof(float) * 2);
    ComputeBuffer mark = new ComputeBuffer(size * size * playerCount, sizeof(int));
    ComputeBuffer mvs = new ComputeBuffer(playerCount, sizeof(int));
    ComputeBuffer temp = new ComputeBuffer(size * size * playerCount, 4 * 4);
    ComputeBuffer targetP = new ComputeBuffer(playerCount, sizeof(int) * 2);
    protected override async STask OnTask(params object[] data)
    {
        mat = await SAsset.LoadAsync<Material>(@"3D\Model\ECS\ECSLit2.mat");
        var go = await SAsset.LoadGameObjectAsync(@"3D\Model\ECS\Cube.prefab");
        mesh = go.GetComponent<MeshFilter>().mesh;
        SAsset.Release(go);
        cs = await SAsset.LoadAsync<ComputeShader>(@"Shader/PathFinding.compute");

        Entity one = await ECSHelper.LoadEntity(@"3D\Model\ECS\Cube.prefab");
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        em.SetComponentData(one, new Unity.Transforms.LocalToWorld() { Value = float4x4.Translate(float3.zero) });
        em.AddComponentData(one, new HDRPMaterialPropertyEmissiveColor() { Value = new float3(0, 0, 1) });
        block = new NativeArray<Entity>(blockCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        block[0] = one;
        for (int i = 1; i < blockCount; i++)
        {
            Entity e = em.Instantiate(one);
            block[i] = e;
        }

        _rangeRoad.onClick.Add(_click_rangeRoad);
        _play.onClick.Add(click_play);
        _btnBack.onClick.Add(_clickBack);
    }
    protected override void OnExit()
    {
        base.OnExit();
        World.Timer.Remove(draw);
        pCb.Dispose();
        roadCb.Dispose();
        mark.Dispose();
        mvs.Dispose();
        temp.Dispose();
        targetP.Dispose();

        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        if (block.IsCreated)
        {
            em.DestroyEntity(block);
            block.Dispose();
        }
    }
    void _clickBack()
    {
        _ = Client.Scene.InLoginScene();
    }
    void _click_rangeRoad()
    {
        if (!road.IsCreated)
            road = new NativeArray<int>(size * size, Allocator.Persistent, NativeArrayOptions.ClearMemory);
        for (int i = 0; i < size * size; i++)
            road[i] = 0;
        Unity.Mathematics.Random random = new((uint)DateTime.Now.Ticks);
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        for (int i = 0; i < block.Length; i++)
        {
            int2 xy = random.NextInt2(size);
            road[xy.y * size + xy.x] = 1;
            em.SetComponentData(block[i], new LocalToWorld() { Value = float4x4.Translate(new float3(xy.x, 0, xy.y) + 0.5f) });
        }
    }
    void click_play()
    {
        if(!road.IsCreated)
        {
            Box.Tips("未初始化障碍");
            return;
        }
        World.Timer.Add(0, -1, draw);

        mat.SetBuffer(Shader.PropertyToID("_pCb"), pCb);

        kernel = cs.FindKernel("CSMain");

        cs.SetInts("size", size, size);
        roadCb.SetData(road);
        cs.SetBuffer(kernel, "road", roadCb);
        cs.SetBuffer(kernel, "mark", mark);
        cs.SetBuffer(kernel, "mvs", mvs);
        cs.SetBuffer(kernel, "temp", temp);

        Bounds bs = new Bounds(Vector3.zero, Vector3.one * 100);
        rp = new RenderParams(mat);
        rp.worldBounds = bs;

        Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);

        {
            NativeArray<float2> ps = new NativeArray<float2>(playerCount, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            for (int i = 0; i < playerCount; i++)
            {
                int2 r = random.NextInt2(0, size);
                bool find = false;
                for (int j = r.y * size + r.x; j < road.Length; j++)
                {
                    if (road[j] > 0)
                        continue;
                    int x = j % size;
                    int y = j / size;
                    r.x = x;
                    r.y = y;
                    find = true;
                    break;
                }
                if (!find)
                {
                    for (int j = 0; j < r.y * size + r.x; j++)
                    {
                        if (road[j] > 0)
                            continue;
                        int x = j % size;
                        int y = j / size;
                        r.x = x;
                        r.y = y;
                        break;
                    }
                }
                //road[r.y * size + r.x]++;
                ps[i] = new float2(r.x, r.y) + 0.5f;
            }

            //
            ps[0] = new float2(0, 0);
            //

            pCb.SetData(ps);
            ps.Dispose();
            cs.SetBuffer(kernel, "ps", pCb);
        }

        {
            NativeArray<float2> targets = new NativeArray<float2>(playerCount, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            for (int i = 0; i < playerCount; i++)
            {
                int2 r = random.NextInt2(0, size);
                bool find = false;
                for (int j = r.y * size + r.x; j < road.Length; j++)
                {
                    if (road[j] > 0)
                        continue;
                    int x = j % size;
                    int y = j / size;
                    r.x = x;
                    r.y = y;
                    find = true;
                    break;
                }
                if (!find)
                {
                    for (int j = 0; j < r.y * size + r.x; j++)
                    {
                        if (road[j] > 0)
                            continue;
                        int x = j % size;
                        int y = j / size;
                        r.x = x;
                        r.y = y;
                        break;
                    }
                }
                targets[i] = r;
            }

            //
            targets[0] = new float2(2, 2) + 0.5f;
            //

            targetP.SetData(targets);
            targets.Dispose();
            cs.SetBuffer(kernel, "targetP", targetP);
        }
    }

    int kernel;
    RenderParams rp;
    void draw()
    {
        cs.Dispatch(kernel, 8, 8, 1);

        if (_showCube.selected)
            Graphics.RenderMeshPrimitives(rp, mesh, 0, playerCount);
    }
}
