using System;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;

static class Hex
{
    public const float HexWidth = 1.05f;
    public static int2 HexQuad = new int2(10, 10);
    static float sqrt_3 = math.sqrt(3);

    public static int2 PositionToHex(float3 pos)
    {
        float yf = pos.z / (HexWidth * sqrt_3 / 2f);
        int y = (int)math.round(yf);

        int parity = y & 1;
        float xf = (pos.x + parity * HexWidth / 2f) / HexWidth;
        int x = (int)math.round(xf);

        return new int2(x, y);
    }
    public static float3 HexToPosition(int2 xy)
    {
        return new float3(xy.x * HexWidth - (xy.y & 1) * HexWidth / 2, 0, xy.y * HexWidth * sqrt_3 / 2);
    }
}
