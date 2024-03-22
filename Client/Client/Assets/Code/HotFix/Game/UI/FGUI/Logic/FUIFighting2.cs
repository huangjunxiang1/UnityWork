using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;
using Game;
using Unity.Jobs;
using Event;

partial class FUIFighting2
{
    public int2 esSize = new int2(100, 100);
    public NativeArray<Entity> es = default;
    public int2 roadSize = new int2(200, 200);
    public NativeArray<int> road;
    public bool start = false;
    protected override async void OnEnter(params object[] data)
    {
        base.OnEnter(data);
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        Entity one = await ECSHelper.LoadEntity(@"3D\Model\ECS\Cube.prefab");

        es = new NativeArray<Entity>(esSize.x * esSize.y, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        es[0] = one;
        road = new NativeArray<int>(roadSize.x * roadSize.y, Allocator.Persistent, NativeArrayOptions.ClearMemory);

        em.SetComponentData(one, new LocalToWorld() { Value = float4x4.Translate(float3.zero) });
        em.AddComponentData(one, new HDRPMaterialPropertyEmissiveColor() { Value = new float3(1, 0, 0) });
        em.AddComponentData(one, new AI_XunLuo() { last = default, target = new float3(1, 0, 0) });
        ++road[0];

        var random = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);
        for (int j = 1; j < es.Length; j++)
        {
            es[j] = em.Instantiate(es[0]);
            int2 p = random.NextInt2(0, roadSize.x);
            float3 cur = new float3(p.x, 0, p.y);
            em.SetComponentData(es[j], new LocalToWorld() { Value = float4x4.Translate(cur) });
            em.SetComponentData(es[j], new HDRPMaterialPropertyEmissiveColor() { Value = new float3(1, 0, 0) });
            int2 r = math.clamp(random.NextInt2(-1, 1), 0, roadSize.x - 1);
            em.SetComponentData(es[j], new AI_XunLuo() { last = cur, target = new float3(p.x + r.x, 0, p.y + r.y) });
            ++road[p.y * roadSize.x + p.x];
        }

        _btnBack.onClick.Add(_clickBack);
        _play.onClick.Add(_onPlay);

        AI_XunLuoSys.start = true;
        AI_XunLuoSys.road = road;
        AI_XunLuoSys.roadSize = roadSize;
    }
    protected override void OnExit()
    {
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        if (start)
            start = false;
        
        if (es.IsCreated)
        {
            em.DestroyEntity(es);
            es.Dispose();
        }
        if (road.IsCreated)
            road.Dispose();
        AI_XunLuoSys.start = false;
    }

    [Event]
    void quit(EC_QuitGame e)
    {
        if (es.IsCreated)
            es.Dispose();
        if (road.IsCreated)
            road.Dispose();
    }

    void _clickBack()
    {
        _ = GameWorld.World.Scene.InLoginScene();
    }
    void _onPlay()
    {
        start = !start;
    }
}

