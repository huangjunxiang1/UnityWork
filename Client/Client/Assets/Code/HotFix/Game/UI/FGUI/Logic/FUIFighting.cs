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

partial class FUIFighting
{
    NativeArray<Entity> es = default;
    int entityCnt = 0;

    Mesh mesh;
    Material mat;
    protected override void OnEnter(params object[] data)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mesh = go.GetComponent<MeshFilter>().mesh;
        GameObject.Destroy(go);
        mat = AssetLoad.Load<Material>("3D/Model/ECS/ECSLit.mat");

        es = new NativeArray<Entity>((int)_slider.max, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

        _btnBack.onClick.Add(_clickBack);
        _slider.value = 0;
        _slider.onChanged.Add(_onValue);
    }

    protected override void OnExit()
    {
        if (es.IsCreated)
            es.Dispose();
    }

    [Event((int)EventIDM.QuitGame)]
    void quit()
    {
        es.Dispose();
    }

    void _clickBack()
    {
        _ = GameL.Scene.InLoginScene();
    }
    async void _onValue()
    {
        Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);
        int v = (int)_slider.value;
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        if (v < entityCnt)
        {
            em.DestroyEntity(new NativeSlice<Entity>(es, v, entityCnt - v));
        }
        else if (v > entityCnt)
        {
            if (entityCnt == 0)
            {
                var en = await AssetLoad.LoadEntityAsync(@"3D\Model\ECS\Cube.prefab", TaskCreater);
                es[0] = en;
                var p = random.NextFloat3(default, new float3(100, 0, 100));
                em.SetComponentData(en, new Translation() { Value = p });
                em.AddComponentData(en, new HDRPMaterialPropertyBaseColor() { Value = random.NextFloat4() });
                em.AddComponentData(en, new Target()
                {
                    value = p,
                    wait = 1,
                });
                entityCnt = 1;
            }
            for (int i = entityCnt; i < v; i++)
            {
                es[i] = em.Instantiate(es[0]);
                var p = random.NextFloat3(default, new float3(100, 0, 100));
                em.SetComponentData(es[i], new Translation() { Value = p });
                em.SetComponentData(es[i], new HDRPMaterialPropertyBaseColor() { Value = random.NextFloat4() });
                em.SetComponentData(es[i], new Target()
                {
                    value = p,
                    wait = 1,
                });
            }
        }
        entityCnt = v;
    }

    struct Target : IComponentData
    {
        public float3 last;
        public float3 value;

        public float cur;
        public float time;
        public float wait;
    }

    partial class MoveSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);
            var dt = Time.DeltaTime;
            Entities
                .WithAll<Target, HDRPMaterialPropertyBaseColor, Translation>()
                .ForEach((Entity e, ref Target t, ref HDRPMaterialPropertyBaseColor c, ref Translation p) =>
                {
                    if (t.wait > 0)
                    {
                        t.wait -= dt;
                        if (t.wait <= 0)
                        {
                            float3 pp = random.NextFloat3(default, new float3(100, 0, 100));
                            t.last = t.value;
                            t.value = pp;
                            t.cur = 0;
                            t.time = math.max(math.distance(t.last, t.value) / 2f, 0.1f);
                            c.Value = random.NextFloat4();
                        }
                    }
                    else
                    {
                        t.cur = math.clamp(t.cur + dt, 0, t.time);
                        p.Value = math.lerp(t.last, t.value, t.cur / t.time);
                        if (t.cur >= t.time)
                            t.wait = random.NextFloat(5, 10);
                    }
                }).ScheduleParallel();
        }
    }
}

