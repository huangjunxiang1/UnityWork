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
using UnityEngine.Rendering;
using Unity.Jobs;
using Unity.Collections.LowLevel.Unsafe;

[UIConfig(20)]
partial class FUIFighting
{
    NativeArray<Entity> es = default;

    protected override async void OnEnter(params object[] data)
    {
        /*var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        Entity one = await ECSHelper.LoadEntity(@"3D\Model\ECS\Cube.prefab");

        if (this.Disposed)
        {
            em.DestroyEntity(one);
            return;
        }

        em.SetComponentData(one, new LocalToWorld() { Value = float4x4.Translate(float3.zero) });
        em.AddComponentData(one, new HDRPMaterialPropertyEmissiveColor() { Value = new float3(1, 0, 0) });

        es = new NativeArray<Entity>(10000, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        es[0] = one;

        for (int j = 1; j < es.Length; j++)
        {
            es[j] = em.Instantiate(es[0]);
            em.SetComponentData(es[j], new LocalToWorld() { Value = float4x4.Translate(float3.zero) });
            em.SetComponentData(es[j], new HDRPMaterialPropertyEmissiveColor() { Value = new float3(1, 0, 0) });
        }*/

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
    BatchMaterialID[] mat = new BatchMaterialID[2];
    BatchMeshID[] mesh = new BatchMeshID[2];
    BatchID[] bid = new BatchID[2];
    float3x4[] arr2;
    unsafe void _onPlay()
    {
        BatchRendererGroup brg = new(job, default);
        brg.SetEnabledViewTypes(new BatchCullingViewType[]
            {
                BatchCullingViewType.Camera,
                BatchCullingViewType.Light,
            });
        var g = SAsset.LoadGameObject(@"3D\Model\ECS\Cube.prefab");
        GameObject g2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        mat[0] = brg.RegisterMaterial(g.GetComponent<Renderer>().sharedMaterial);
        mesh[0] = brg.RegisterMesh(g.GetComponent<MeshFilter>().sharedMesh);

        mat[1] = brg.RegisterMaterial(g2.GetComponent<Renderer>().sharedMaterial);
        mesh[1] = brg.RegisterMesh(g2.GetComponent<MeshFilter>().sharedMesh);
        GameObject.DestroyImmediate(g2);

        var arr = new NativeArray<MetadataValue>(2, Allocator.Temp);
        const uint kIsPerInstanceBit = 0x80000000;
        arr[0] = new MetadataValue()
        {
            NameID = Shader.PropertyToID("unity_ObjectToWorld"),
            Value = kIsPerInstanceBit | (uint)sizeof(float3x4)
        };
        arr[1] = new MetadataValue()
        {
            NameID = Shader.PropertyToID("_EmissiveColor"),
            Value = /*kIsPerInstanceBit |*/ (uint)(5 * sizeof(float3x4))
        };

        {
            GraphicsBuffer gb = new GraphicsBuffer(GraphicsBuffer.Target.Raw, 5 * (12 + 4) + 12, 4);
            arr2 = new float3x4[5];
            for (int i = 0; i < arr2.Length; i++)
            {
                var v44 = float4x4.TRS(new float3(i * 2, 0, 0), quaternion.identity, 1);
                arr2[i] = new float3x4(v44.c0.xyz, v44.c1.xyz, v44.c2.xyz, v44.c3.xyz);
            }
            gb.SetData(arr2, 0, 1, arr2.Length);

            {
                var tmp = new float4[5];
                tmp[0] = new float4(0, 0, 900, 1);
                tmp[1] = new float4(900, 0, 0, 1);
                tmp[2] = new float4(0, 900, 0, 1);
                tmp[4] = 1;
                gb.SetData(tmp, 0, 6 * 3, tmp.Length);
            }
            bid[0] = brg.AddBatch(arr, gb.bufferHandle);
        }
        /*{
            GraphicsBuffer gb = new GraphicsBuffer(GraphicsBuffer.Target.Raw, 5, sizeof(float3x4));
            arr2 = new float3x4[5];
            for (int i = 0; i < arr2.Length; i++)
            {
                var v44 = float4x4.TRS(new float3(i * 5, 0, 10), quaternion.identity, i + 1);
                arr2[i] = new float3x4(v44.c0.xyz, v44.c1.xyz, v44.c2.xyz, v44.c3.xyz);
            }
            gb.SetData(arr2);
            bid[1] = brg.AddBatch(arr, gb.bufferHandle);
        }*/

        UnityEngine.Bounds bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(1048576.0f, 1048576.0f, 1048576.0f));
        brg.SetGlobalBounds(bounds);

        unsafe JobHandle job(BatchRendererGroup rendererGroup, BatchCullingContext cullingContext, BatchCullingOutput cullingOutput, IntPtr userContext)
        {
            for (int i = 0; i < cullingOutput.drawCommands.Length; i++)
            {
                BatchCullingOutputDrawCommands d = new();
                d.drawCommands = Alloc<BatchDrawCommand>(2);
                d.drawCommands[0] = new BatchDrawCommand
                {
                    visibleOffset = 0,
                    visibleCount = (uint)3,
                    batchID = bid[i],
                    materialID = mat[0],
                    meshID = mesh[0],
                    splitVisibilityMask = 0xff,
                }; 
                d.drawCommands[1] = new BatchDrawCommand
                {
                    visibleOffset = 3,
                    visibleCount = (uint)2,
                    batchID = bid[i],
                    materialID = mat[1],
                    meshID = mesh[1],
                    splitVisibilityMask = 0xff,
                };
                d.drawCommandCount = 2;

                d.drawRangeCount = 1;
                d.drawRanges = Alloc<BatchDrawRange>(1);
                d.drawRanges[0] = new BatchDrawRange
                {
                    drawCommandsBegin = 0,
                    drawCommandsCount = (uint)2,
                    filterSettings = new BatchFilterSettings
                    {
                        renderingLayerMask = 1,
                        layer = 0,
                        motionMode = MotionVectorGenerationMode.Camera,
                        shadowCastingMode = ShadowCastingMode.On,
                        receiveShadows = true,
                        staticShadowCaster = false,
                        allDepthSorted = false
                    }
                };

                d.visibleInstanceCount = 5;
                d.visibleInstances = Alloc<int>(d.visibleInstanceCount);
                for (int j = 0; j < d.visibleInstanceCount; j++)
                    d.visibleInstances[j] = j;

                cullingOutput.drawCommands[i] = d;
            }
            return new JobHandle();
        }
        unsafe static T* Alloc<T>(int count) where T:unmanaged
        {
            return (T*)UnsafeUtility.Malloc(
            UnsafeUtility.SizeOf<T>() * count,
            UnsafeUtility.AlignOf<T>(),
            Allocator.TempJob);
        }

        SAsset.Release(g);
        return;

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
