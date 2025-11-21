using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;


public class AStarVolume
{
    public static AStarVolume Empty { get; } = new AStarVolume();//not Occupation
    public static AStarVolume Cube { get; } = new CubeVolume();//1x1

    class CubeVolume : AStarVolume
    {
        public override void Add(AStarData astar, int2 xy)
        {
            if (xy.x > -1 && xy.y > -1 && xy.x < astar.width && xy.y < astar.height)
            {
                astar.AddOccupation(xy);
                astar.GridChangeHandle(xy);
            }
        }
        public override void Remove(AStarData astar, int2 xy)
        {
            if (xy.x > -1 && xy.y > -1 && xy.x < astar.width && xy.y < astar.height)
            {
                astar.RemoveOccupation(xy);
                astar.GridChangeHandle(xy);
            }
        }
    }


    public virtual void Add(AStarData astar, int2 xy) { }
    public virtual void Remove(AStarData astar, int2 xy) { }
    public virtual bool isInScope(int2 point, int2 center) => point.Equals(center);
    public virtual bool isNear(int2 point, int2 center, int near) => maths.ManhattanDistance(point, center) <= near;
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
            {
                astar.AddOccupation(new int2(x, y));
                astar.GridChangeHandle(new int2(x, y));
            }
        }
    }
    public override void Remove(AStarData astar, int2 xy)
    {
        int mx = math.min(xy.x + halfEdge + 1, astar.width);
        int my = math.min(xy.y + halfEdge + 1, astar.height);
        for (int x = math.max(0, xy.x - halfEdge); x < mx; x++)
        {
            for (int y = math.max(0, xy.y - halfEdge); y < my; y++)
            {
                astar.RemoveOccupation(new int2(x, y));
                astar.GridChangeHandle(new int2(x, y));
            }
        }
    }
    public override bool isInScope(int2 point, int2 center)
    {
        int2 xy = math.abs(point - center);
        return xy.x <= halfEdge && xy.y <= halfEdge;
    }
    public override bool isNear(int2 point, int2 center, int near)
    {
        for (int x = -halfEdge; x <= halfEdge; x++)
        {
            for (int y = -halfEdge; y <= halfEdge; y++)
            {
                int2 v = center + new int2(x, y);
                if (maths.ManhattanDistance(point, v) <= near) return true;
            }
        }
        return false;
    }
}