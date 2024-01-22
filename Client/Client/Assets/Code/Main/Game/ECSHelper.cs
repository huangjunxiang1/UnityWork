using Game;
using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public static class ECSHelper
{
    public static async STask<Entity> LoadEntity(string url)
    {
        GameObject g = await SAsset.LoadGameObjectAsync(url);
        var mgr = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entity e = mgr.CreateEntity();
        Renderer r = g.GetComponent<Renderer>();
        MeshFilter mf = g.GetComponent<MeshFilter>();
        var egs = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<EntitiesGraphicsSystem>();
        var matId= egs.RegisterMaterial(r.sharedMaterial);
        var meshId = egs.RegisterMesh(mf.sharedMesh);
        RenderMeshUtility.AddComponents(e, mgr, new RenderMeshDescription(r), new MaterialMeshInfo(matId, meshId));

        mgr.AddComponentData(e, new LocalToWorld() { Value = float4x4.TRS(float3.zero, g.transform.rotation, g.transform.lossyScale) });
        SAsset.Release(g);
        return e;
    }
}
