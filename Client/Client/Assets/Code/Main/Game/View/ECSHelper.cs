using Game;
using Unity.Entities;
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
        var matId = egs.RegisterMaterial(r.sharedMaterial);
        var meshId = egs.RegisterMesh(mf.sharedMesh);
        RenderMeshUtility.AddComponents(e, mgr, new RenderMeshDescription(r), new MaterialMeshInfo(matId, meshId));

        mgr.AddComponentData(e, new LocalToWorld() { Value = float4x4.TRS(float3.zero, g.transform.rotation, g.transform.lossyScale) });
        SAsset.Release(g);
        return e;
    }
    public static async STask<(Entity, GameObject)> LoadEntityAndPrefab(string url)
    {
        GameObject g = await SAsset.LoadGameObjectAsync(url);
        var mgr = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entity e = mgr.CreateEntity();
        Renderer r = g.GetComponent<Renderer>();
        MeshFilter mf = g.GetComponent<MeshFilter>();
        var egs = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<EntitiesGraphicsSystem>();
        var matId = egs.RegisterMaterial(r.sharedMaterial);
        var meshId = egs.RegisterMesh(mf.sharedMesh);
        RenderMeshUtility.AddComponents(e, mgr, new RenderMeshDescription(r), new MaterialMeshInfo(matId, meshId));

        mgr.AddComponentData(e, new LocalToWorld() { Value = float4x4.TRS(float3.zero, g.transform.rotation, g.transform.lossyScale) });
        SAsset.Release(g);
        return (e, g);
    }
}
