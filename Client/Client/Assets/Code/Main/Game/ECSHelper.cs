﻿using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public static class ECSHelper
{
    public static async TaskAwaiter<Entity> LoadEntity(string url)
    {
        GameObject g = await AssetLoad.LoadGameObjectAsync(url);
        var mgr = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entity e = mgr.CreateEntity();
        Renderer r = g.GetComponent<Renderer>();
        MeshFilter mf = g.GetComponent<MeshFilter>();
        RenderMeshUtility.AddComponents(e, mgr, new RenderMeshDescription(r), new RenderMeshArray(new[] { r.sharedMaterial }, new[] { mf.sharedMesh }), MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));

        mgr.AddComponentData(e, new LocalToWorld() { Value = float4x4.TRS(float3.zero, g.transform.rotation, g.transform.lossyScale) });
        AssetLoad.Release(g);
        return e;
    }
}