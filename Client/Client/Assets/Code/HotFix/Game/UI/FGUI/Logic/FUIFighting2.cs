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
using Main;
using Game;
using Unity.Physics;
using Unity.Jobs;

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
        Entity one = await AssetLoad.LoadEntityAsync(@"3D\Model\ECS\Cube.prefab", TaskCreater);

        if (this.Disposed)
        {
            em.DestroyEntity(one);
            return;
        }

        es = new NativeArray<Entity>(esSize.x * esSize.y, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        es[0] = one;
        road = new NativeArray<int>(roadSize.x * roadSize.y, Allocator.Persistent, NativeArrayOptions.ClearMemory);

        em.SetComponentData(one, new LocalToWorld() { Value = float4x4.Translate(float3.zero) });
        em.AddComponentData(one, new HDRPMaterialPropertyEmissiveColor1() { Value = new float4(1, 0, 0, 1) });
        em.AddComponentData(one, new AI_XunLuo() { last = default, target = new float3(1, 0, 0) });
        ++road[0];

        var random = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);
        for (int j = 1; j < es.Length; j++)
        {
            es[j] = em.Instantiate(es[0]);
            int2 p = random.NextInt2(0, roadSize.x);
            float3 cur = new float3(p.x, 0, p.y);
            em.SetComponentData(es[j], new LocalToWorld() { Value = float4x4.Translate(cur) });
            em.SetComponentData(es[j], new HDRPMaterialPropertyEmissiveColor1() { Value = new float4(1, 0, 0, 1) });
            int2 r = math.clamp(random.NextInt2(-1, 1), 0, roadSize.x - 1);
            em.SetComponentData(es[j], new AI_XunLuo() { last = cur, target = new float3(p.x + r.x, 0, p.y + r.y) });
            ++road[p.y * roadSize.x + p.x];
        }

        _btnBack.onClick.Add(_clickBack);
        _play.onClick.Add(_onPlay);
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
    }
    [Event((int)EventIDM.QuitGame)]
    void quit()
    {
        if (es.IsCreated)
            es.Dispose();
        if (road.IsCreated)
            road.Dispose();
    }

    void _clickBack()
    {
        _ = GameL.Scene.InLoginScene();
    }
    void _onPlay()
    {
        start = !start;
    }
}
[GenerateTestsForBurstCompatibility]
partial struct AI_XunLuoSys : ISystem
{
    public void OnCreate(ref SystemState state)
    {

    }

    public void OnDestroy(ref SystemState state)
    {

    }

    [GenerateTestsForBurstCompatibility]
    public void OnUpdate(ref SystemState state)
    {
        FUIFighting2 ui = GameL.UI.Get<FUIFighting2>();
        if (ui == null || !ui.start)
            return;
        var dt = SystemAPI.Time.DeltaTime;
        var random = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);
        foreach (var t in SystemAPI.Query<AI_XunLuoAsp>())
            t.Move(dt, ui.road, ui.roadSize, ref random);
    }
}
[GenerateTestsForBurstCompatibility]
struct AI_XunLuo : IComponentData
{
    public float3 last;
    public float3 target;
    public float dTime;
}
[GenerateTestsForBurstCompatibility]
readonly partial struct AI_XunLuoAsp : IAspect
{
    readonly RefRW<AI_XunLuo> target;
    readonly RefRW<LocalToWorld> pos;

    [GenerateTestsForBurstCompatibility]
    public void Move(float dt, NativeArray<int> roads, int2 size, ref Unity.Mathematics.Random random)
    {
        float4x4 f44 = pos.ValueRO.Value;
        int2 now = math.clamp((int2)f44.c3.xz, 0, size.x);
        int2 t = (int2)target.ValueRO.target.xz;
        int num = roads[t.y * size.x + t.x];
        if (num == 1)
        {
            int2 r = random.NextInt2(-1, 2);
            target.ValueRW.last = f44.c3.xyz;
            target.ValueRW.target = math.clamp(f44.c3.xyz + new float3(r.x, 0, r.y), 0, size.x - 1);
            target.ValueRW.dTime = 0;
        }
        if (math.all(now == t) || num == 0)
        {
            f44.c3.xyz = math.lerp(target.ValueRO.last, target.ValueRO.target, math.clamp(target.ValueRW.dTime += dt, 0, 1));
            pos.ValueRW.Value = f44;
            int2 next = (int2)f44.c3.xz;
            if (math.any(now != next) || math.all(now == t))
            {
                --roads[now.y * size.x + now.x];
                ++roads[next.y * size.x + next.x];
                int2 r = random.NextInt2(-1, 2);
                target.ValueRW.last = f44.c3.xyz;
                target.ValueRW.target = math.clamp(f44.c3.xyz + new float3(r.x, 0, r.y), 0, size.x - 1);
                target.ValueRW.dTime = 0;
            }
        }
    }
}