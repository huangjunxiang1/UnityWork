using Core;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

static class GPUConstDefine
{
    public const int Define_Args_Size = 5;

    public const int Tree_TypeCount = 3;
    public const int Tree_StyleCount = 2;
}
class GPUInstanceRender : SObject
{
    public GPUInstanceRender(IEnumerable<GameObject> target, int maxInstance = 2048)
    {
        this.Batch = target.Count();
        this.MaxInstance = maxInstance;
        //args+visible  +1 是多预留一个 以防maxInstance传参不是32的倍数
        ArgsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, GPUConstDefine.Define_Args_Size * this.Batch + (maxInstance * this.Batch) / 32 + 1, sizeof(uint));

        this.targets.AddRange(target);
        uint[] args = new uint[ArgsBuffer.count];
        for (int i = 0; i < this.targets.Count; i++)
        {
            this.targets[i].SetActive(false);
            this.targets[i].transform.parent = Client.gameObject.transform;

            var mesh = this.targets[i].GetComponent<MeshFilter>().sharedMesh;
            var mat = this.targets[i].GetComponent<Renderer>().material;
            args[0 + i * GPUConstDefine.Define_Args_Size] = (uint)mesh.GetIndexCount(0);
            args[1 + i * GPUConstDefine.Define_Args_Size] = (uint)0;
            args[2 + i * GPUConstDefine.Define_Args_Size] = (uint)mesh.GetIndexStart(0);
            args[3 + i * GPUConstDefine.Define_Args_Size] = (uint)mesh.GetBaseVertex(0);
            mms.Add((mesh, mat));
            mat.SetInt("offsetIndex", maxInstance * i);
        }
        ArgsBuffer.SetData(args);
    }

    List<GameObject> targets = new();
    List<(Mesh,Material)> mms = new();
    Dictionary<string, GraphicsBuffer> bufferMap = new();

    public bool ViewEnable { get; set; } = true;
    public GraphicsBuffer ArgsBuffer { get; private set; }
    public int MaxInstance { get; private set; }
    public int Batch { get; private set; }


    [Timer(0,-1)]
    void Render()
    {
        if (!ViewEnable)
            return;
        for (int i = 0; i < Batch; i++)
            Graphics.DrawMeshInstancedIndirect(mms[i].Item1, 0, mms[i].Item2, new Bounds((float3)0, (float3)1000), ArgsBuffer, argsOffset: GPUConstDefine.Define_Args_Size * sizeof(uint) * i);
    }

    public unsafe GraphicsBuffer GetOrCreateBuffer<T>(string name) where T : unmanaged
    {
        if (!bufferMap.TryGetValue(name, out var buffer))
        {
            buffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, this.MaxInstance * Batch, sizeof(T));
            bufferMap[name] = buffer;
            for (int i = 0; i < targets.Count; i++)
                mms[i].Item2.SetBuffer(name, buffer);
        }
        return buffer;
    }
    public override void Dispose()
    {
        base.Dispose();
        ArgsBuffer.Dispose();
        foreach (var item in bufferMap.Values)
            item.Dispose();
        bufferMap.Clear();

        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].SetActive(true);
            SAsset.Release(targets[i]);
        }
    }
}
