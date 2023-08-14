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

[Main.UIConfig(20)]
partial class FUIFighting
{
    NativeArray<Entity> es = default;

    protected override async void OnEnter(params object[] data)
    {
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        Entity one = await ECSHelper.LoadEntity(@"3D\Model\ECS\Cube.prefab");
      
        if (this.Disposed)
        {
            em.DestroyEntity(one);
            return;
        }
        
        em.SetComponentData(one, new LocalToWorld() { Value = float4x4.Translate(float3.zero) });
        em.AddComponentData(one, new HDRPMaterialPropertyEmissiveColor1() { Value = new float4(1, 0, 0, 1) });

        es = new NativeArray<Entity>(10000, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        es[0] = one;

        for (int j = 1; j < es.Length; j++)
        {
            es[j] = em.Instantiate(es[0]);
            em.SetComponentData(es[j], new LocalToWorld() { Value = float4x4.Translate(float3.zero) });
            em.SetComponentData(es[j], new HDRPMaterialPropertyEmissiveColor1() { Value = new float4(1, 0, 0, 1) });
        }

        _btnBack.onClick.Add(_clickBack);
        _play.onClick.Add(_onPlay);
    }

    protected override void OnExit()
    {
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        if (es.IsCreated)
        {
            em.DestroyEntity(es);
            es.Dispose();
        }
    }

    [Event]
    void quit(EC_QuitGame e)
    {
        if (es.IsCreated)
            es.Dispose();
    }

    void _clickBack()
    {
        _ = GameL.Scene.InLoginScene();
    }
    int lastRange = -1;
    void _onPlay()
    {
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
     
        if (lastRange == -1)
            lastRange = UnityEngine.Random.Range(0, 6);
        else
        {
            int r = UnityEngine.Random.Range(1, 6);
            var rv = (lastRange + r) % 6;
            lastRange = rv;
        }
        
        int idx = lastRange / 2;
        int v = lastRange % 2;
        for (int i = 0; i < es.Length; i++)
        {
            var n = noise.cnoise(new float2(i % 100, i / 100) / 3f);
            var dTime = math.remap(-1, 1, 0, 1, n);
            float3 f3 = default;
            f3[idx % 3] = i % 100;
            f3[(idx + 1) % 3] = i / 100;
            f3[(idx + 2) % 3] = v * 100;
            em.AddComponentData(es[i], new Demo1Delay { dTime = dTime, lTime = 1, v3 = f3 });
        }
    }
}
