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
using Unity.Burst;
using Event;

partial class FUIFighting3
{
    public NativeArray<int> road;
    NativeArray<Entity> block;
    int blockCount = 3000;
    NativeArray<Entity> Player;
    public const int playerCount = 2000;
    SystemHandle sys;

    protected override async void OnEnter()
    {
        Entity one = await ECSHelper.LoadEntity(@"3D_Cube");
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
        _findStyle.selectedIndex = 0;
 
        _rangeRoad.onClick.Add(_click_rangeRoad);
        _play.onClick.Add(_click_play);
        _btnBack.onClick.Add(_clickBack);

        Demo3Sys.playerCount = playerCount;
    }
    protected override void OnExit()
    {
        base.OnExit();
        if (sys != SystemHandle.Null)
            Unity.Entities.World.DefaultGameObjectInjectionWorld.DestroySystem(sys);
        if (road.IsCreated)
            road.Dispose();
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        if (block.IsCreated)
        {
            em.DestroyEntity(block);
            block.Dispose();
        }
        if (Player.IsCreated)
        {
            em.DestroyEntity(Player);
            Player.Dispose();
        }
    }

    [Event]
    void quit(EC_QuitGame e)
    {
        if (road.IsCreated)
            road.Dispose();
        if (block.IsCreated)
            block.Dispose();
        if (Player.IsCreated)
            Player.Dispose();
        if (sys != SystemHandle.Null)
            Unity.Entities.World.DefaultGameObjectInjectionWorld.DestroySystem(sys);
    }
    async void _clickBack()
    {
        await Client.Scene.InScene<LoginScene>();
    }
    void _click_rangeRoad()
    {
        if (!road.IsCreated)
        {
            road = new NativeArray<int>(Demo3DF.size * Demo3DF.size, Allocator.Persistent, NativeArrayOptions.ClearMemory);
            Demo3Sys.road = road;
        }
        for (int i = 0; i < Demo3DF.size * Demo3DF.size; i++)
            road[i] = 0;
        Unity.Mathematics.Random random = new((uint)DateTime.Now.Ticks);
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        for (int i = 0; i < block.Length; i++)
        {
            int2 xy = random.NextInt2(Demo3DF.size);
            road[xy.y * Demo3DF.size + xy.x] = 1;
            em.SetComponentData(block[i], new LocalToWorld() { Value = float4x4.Translate(new float3(xy.x, 0, xy.y) + 0.5f) });
        }
    }
    void _click_play()
    {
        if (!road.IsCreated)
        {
            Box.Tips("还未生成地图");
            return;
        }
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        if (!Player.IsCreated)
        {
             Player = new NativeArray<Entity>(playerCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            for (int i = 0; i < Player.Length; i++)
            {
                var e = em.Instantiate(block[0]);
                em.AddComponentData(e, new HDRPMaterialPropertyEmissiveColor() { Value = new float3(1, 0, 0) });
                em.AddComponentData(e, new Demo3Com());
                Player[i] = e;
            }
        }

        Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);
        for (int i = 0; i < Player.Length; i++)
        {
            int2 r = random.NextInt2(0, Demo3DF.size);
            bool find = false;
            for (int j = r.y * Demo3DF.size + r.x; j < road.Length; j++)
            {
                if (road[j] > 0)
                    continue;
                int x = j % Demo3DF.size;
                int y = j / Demo3DF.size;
                r.x = x;
                r.y = y;
                find = true;
                break;
            }
            if (!find)
            {
                for (int j = 0; j < r.y * Demo3DF.size + r.x; j++)
                {
                    if (road[j] > 0)
                        continue;
                    int x = j % Demo3DF.size;
                    int y = j / Demo3DF.size;
                    r.x = x;
                    r.y = y;
                    break;
                }
            }
            //road[r.y * size + r.x]++;
            em.SetComponentData(Player[i], new LocalToWorld() { Value = float4x4.Translate(new float3(r.x, 0, r.y) + 0.5f) });
        }

        if (sys == SystemHandle.Null)
        {
            sys = Unity.Entities.World.DefaultGameObjectInjectionWorld.CreateSystem<Demo3Sys>();
            Unity.Entities.World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>().AddSystemToUpdateList(sys);
        }
    }
}



