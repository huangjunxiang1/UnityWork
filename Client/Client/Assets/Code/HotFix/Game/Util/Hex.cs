using System;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;

public static partial class Hex
{
    public static int2 Hex_QuadHalfSize = new int2(10, 10);
    public static int2 Hex_QuadSize => Hex_QuadHalfSize * 2 + 1;
    public static int Hex_GridCount => Hex_QuadSize.x * Hex_QuadSize.y;

    static float Hex_sqrt_3 = math.sqrt(3);

    public static int2 GetGridxy(float2 pos, float hexWidth = Define.HexWidth, PathGridHexParityType parity = PathGridHexParityType.Even, PathGridHexFacingType facing = PathGridHexFacingType.Up)
    {
        pos = math.lerp(pos.xy, pos.yx, (int)facing);

        float halfWidth = hexWidth * 0.5f;
        float2 quad = hexWidth * math.float2(1, Hex_sqrt_3);
        float2 fv = math.floor(pos / quad);
        float2 localxy = pos - fv * quad;

        float2 r1xy = math.abs(localxy - halfWidth * math.float2(1, Hex_sqrt_3));
        float threshold = -math.rcp(Hex_sqrt_3) * r1xy.x + hexWidth / Hex_sqrt_3;
        float sv = math.step(threshold, r1xy.y);
        float2 finalxy = fv * math.float2(1, 2) + math.float2((1 - sv) * (1 - (int)parity) + math.step(halfWidth, localxy.x) * sv, (1 - sv) + math.step(halfWidth * Hex_sqrt_3, localxy.y) * sv * 2);

        finalxy = math.lerp(finalxy.xy, finalxy.yx, (int)facing);
        return (int2)finalxy;
    }
    public static float3 GetPositon(int2 xy, float hexWidth = Define.HexWidth, PathGridHexParityType parity = PathGridHexParityType.Even, PathGridHexFacingType facing = PathGridHexFacingType.Up)
    {
        xy = facing == PathGridHexFacingType.Up ? xy : xy.yx;
        float3 v3 = math.float3((xy.x + (xy.y & 1) * ((int)parity - 0.5f)) * hexWidth, 0, xy.y * hexWidth * 0.5f * Hex_sqrt_3);
        v3.xz = math.lerp(v3.xz, v3.zx, (int)facing);
        return v3;
    }
    public static int2 GetQuadCenterGrid(int2 xy)
    {
        xy += Hex_QuadHalfSize;
        return xy - (xy % Hex_QuadSize + Hex_QuadSize) % Hex_QuadSize;
    }
    public static int2 GetQuadLocalxy(int2 xy)
    {
        xy += Hex.Hex_QuadHalfSize;
        xy %= Hex.Hex_QuadSize;
        return (xy + Hex.Hex_QuadSize) % Hex.Hex_QuadSize;
    }
}
