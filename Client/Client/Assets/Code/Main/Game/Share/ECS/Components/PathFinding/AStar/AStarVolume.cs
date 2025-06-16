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
    public virtual bool isNear(int2 self, int2 target, int near)
    {
        return math.abs(self.x - target.x) + math.abs(self.y - target.y) <= near;
    }
}

public class RectVolume : AStarVolume
{
    public RectVolume(int halfEdge) => this.halfEdge = math.max(halfEdge, 0);
    //半 边长
    int halfEdge;

    public override void Add(AStarData astar, int2 xy)
    {
        int mx = math.min(xy.x + halfEdge + 1, astar.width);
        int my = math.min(xy.y + halfEdge + 1, astar.height);
        for (int x = math.max(0, xy.x - halfEdge); x < mx; x++)
        {
            for (int y = math.max(0, xy.y - halfEdge); y < my; y++)
                astar.AddOccupation(new int2(x, y));
        }
    }
    public override void Remove(AStarData astar, int2 xy)
    {
        int mx = math.min(xy.x + halfEdge + 1, astar.width);
        int my = math.min(xy.y + halfEdge + 1, astar.height);
        for (int x = math.max(0, xy.x - halfEdge); x < mx; x++)
        {
            for (int y = math.max(0, xy.y - halfEdge); y < my; y++)
                astar.RemoveOccupation(new int2(x, y));
        }
    }
    public override bool isInScope(int2 self, int2 target)
    {
        int2 xy = math.abs(self - target);
        return xy.x <= halfEdge && xy.y <= halfEdge;
    }
    public override bool isNear(int2 self, int2 target, int near)
    {
        for (int x = -halfEdge; x <= halfEdge; x++)
        {
            for (int y = -halfEdge; y <= halfEdge; y++)
            {
                int2 v = target + new int2(x, y);
                if (math.abs(self.x - v.x) + math.abs(self.y - v.y) <= near) return true;
            }
        }
        return false;
    }
}