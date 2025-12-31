using System;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;

public static class Hex
{
    public const float HexWidth = 1.5f;
    public static int2 QuadHalfSize = new int2(10, 10);
    public static int2 QuadSize => QuadHalfSize * 2 + 1;
    public static int GridCount => QuadSize.x * QuadSize.y;

    static float sqrt_3 = math.sqrt(3);

    public static int2 GetGridxy(float3 pos)
    {
        float yf = pos.z / (HexWidth * sqrt_3 / 2f);
        int y = (int)math.round(yf);

        int parity = y & 1;
        float xf = (pos.x + parity * HexWidth / 2f) / HexWidth;
        int x = (int)math.round(xf);

        return new int2(x, y);
    }
    public static float3 GetPositon(int2 xy)
    {
        return new float3(xy.x * HexWidth - (xy.y & 1) * HexWidth / 2, 0, xy.y * HexWidth * sqrt_3 / 2);
    }
    public static int2 GetCenterGrid(int2 xy)
    {
        xy += Hex.QuadHalfSize;
        int2 n = xy / Hex.QuadSize;
        return new int2(xy.x < 0 ? n.x - 1 : n.x, xy.y < 0 ? n.y - 1 : n.y) * Hex.QuadSize;
    }
    public static int2 GetQuadLocalxy(int2 xy)
    {
        xy += Hex.QuadHalfSize;
        xy %= Hex.QuadSize;
        if (xy.x < 0)
            xy.x += Hex.QuadSize.x;
        if (xy.y < 0)
            xy.y += Hex.QuadSize.y;
        return xy;
    }
}
