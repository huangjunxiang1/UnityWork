using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

static class GPUConstDefine
{
    public const float Define_World_Scale = 0.02f;

    public const int Tree_TypeCount = 3;
    public const int Tree_StyleCount = 2;
}
class GPUInstanceRender : SObject
{
    public GPUInstanceRender(IEnumerable<GameObject> target, bool viewEnable = true, int maxInstance = 2048)
    {
        this.RenderGroup = target.Count();
        this.MaxInstance = maxInstance;
        this.RenderParams = new RenderParams[this.RenderGroup];
        ArgsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, this.RenderGroup, GraphicsBuffer.IndirectDrawIndexedArgs.size);
        if (viewEnable)
            VisibleBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, (maxInstance * this.RenderGroup) / 32 + 1, sizeof(uint));

        List<GameObject> targets = new(target);
        var args = new GraphicsBuffer.IndirectDrawIndexedArgs[ArgsBuffer.count];
        for (int i = 0; i < this.RenderGroup; i++)
        {
            targets[i].SetActive(false);
            targets[i].transform.parent = Game.World.transform;

            var mesh = targets[i].GetComponent<MeshFilter>().sharedMesh;
            //不能用同一个材质球sharedMaterial   不然SetInt 和 setbuffer 会覆盖所有组
            var mat = targets[i].GetComponent<Renderer>().material;
            var arg = new GraphicsBuffer.IndirectDrawIndexedArgs();
            arg.indexCountPerInstance = (uint)mesh.GetIndexCount(0);
            arg.startIndex = (uint)mesh.GetIndexStart(0);
            arg.baseVertexIndex = (uint)mesh.GetBaseVertex(0);
            args[i] = arg;
            this.mesh.Add(mesh);
            mat.SetInt("offsetIndex", maxInstance * i);
            this.RenderParams[i] = new(mat);
            this.RenderParams[i].worldBounds = new Bounds(Vector3.zero, Vector3.one * 1000);
        }
        ArgsBuffer.SetData(args);
    }

    List<Mesh> mesh = new();
    Dictionary<string, GraphicsBuffer> bufferMap = new();

    public bool ViewEnable { get; set; } = true;
    public GraphicsBuffer ArgsBuffer { get; private set; }
    public GraphicsBuffer VisibleBuffer { get; private set; }
    public int MaxInstance { get; private set; }
    public int RenderGroup { get; private set; }
    public RenderParams[] RenderParams { get; private set; }


    [Timer(0,-1)]
    void Render()
    {
        if (!ViewEnable)
            return;
        for (int i = 0; i < this.RenderGroup; i++)
            Graphics.RenderMeshIndirect(RenderParams[i], mesh[i], ArgsBuffer, startCommand: i);
    }

    public unsafe GraphicsBuffer GetOrCreateBuffer<T>(string name) where T : unmanaged
    {
        if (!bufferMap.TryGetValue(name, out var buffer))
        {
            buffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, this.MaxInstance * RenderGroup, sizeof(T));
            bufferMap[name] = buffer;
            for (int i = 0; i < RenderGroup; i++)
                RenderParams[i].material.SetBuffer(name, buffer);
        }
        return buffer;
    }
    public override void Dispose()
    {
        base.Dispose();
        ArgsBuffer.Dispose();
        VisibleBuffer?.Dispose();
        foreach (var item in bufferMap.Values)
            item.Dispose();
        bufferMap.Clear();
    }
}
