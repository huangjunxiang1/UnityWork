using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;


public class MulNode
{
    public Dictionary<long, MulNodeInfo> Nodes = new();

    internal bool isFinding = false;
    internal int vs;

    public MulNodeInfo CreateNode(long id, float3 pos)
    {
        if (!Nodes.TryGetValue(id, out var node))
            Nodes[id] = node = new MulNodeInfo() { id = id, position = pos, Node = this };
        return node;
    }
    public MulNodeInfo GetNode(long id)
    {
        Nodes.TryGetValue(id, out var node);
        return node;
    }
}
