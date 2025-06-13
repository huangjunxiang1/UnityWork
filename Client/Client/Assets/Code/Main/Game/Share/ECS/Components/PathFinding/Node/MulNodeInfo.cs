using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;


public class MulNodeInfo
{
    [Sirenix.OdinInspector.ShowInInspector]
    public long id { get; internal set; }
    [Sirenix.OdinInspector.ShowInInspector]
    public float3 position { get; internal set; }
    public MulNode Node { get; internal set; }

    [Sirenix.OdinInspector.ShowInInspector]
    public List<MulNodeLinkInfo> Next { get; } = new();

    internal int vs;

    public void AddNext(long id, float distance)
    {
        if (Next.FindIndex(t => t.Node.id == id) != -1)
            return;
        var n = Node.GetNode(id);
        var link = new MulNodeLinkInfo() { Node = n, Distance = distance };
        Next.Add(link);
    }
}
