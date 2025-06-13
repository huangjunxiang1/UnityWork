using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;


public class AStarVolume
{
    public static AStarVolume Empty { get; } = new AStarVolume();
    public static AStarVolume One { get; } = new RectVolume(1);

    public virtual void Add(AStarData astar, int2 xy) { }
    public virtual void Remove(AStarData astar, int2 xy) { }
    public virtual bool isInScope(int2 self, int2 target) => self.Equals(target);
}

public class RectVolume : AStarVolume
{
    public RectVolume(int halfEdge) => this.halfEdge = halfEdge;
    //半 边长
    int halfEdge;

    public override void Add(AStarData astar, int2 xy)
    {
        for (int x = -halfEdge; x <= halfEdge; x++)
        {
            for (int y = -halfEdge; y <= halfEdge; y++)
            {
                int2 v = xy + new int2(x, y);
                if (astar.isInScope(v))
                    astar.AddOccupation(v);
            }
        }
    }
    public override void Remove(AStarData astar, int2 xy)
    {
        for (int x = -halfEdge; x <= halfEdge; x++)
        {
            for (int y = -halfEdge; y <= halfEdge; y++)
            {
                int2 v = xy + new int2(x, y);
                if (astar.isInScope(v))
                    astar.RemoveOccupation(v);
            }
        }
    }
    public override bool isInScope(int2 self, int2 target)
    {
        int2 xy = math.abs(self - target);
        return xy.x <= halfEdge && xy.y <= halfEdge;
    }
}