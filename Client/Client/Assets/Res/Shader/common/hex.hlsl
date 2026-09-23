
#include "math.hlsl"

#ifndef HEX
#define HEX

static const int2 Hex_QuadHalfSize = int2(10, 10);
static const int2 Hex_QuadSize = Hex_QuadHalfSize * 2 + 1;
static const int Hex_GridCount = Hex_QuadSize.x * Hex_QuadSize.y;
static const float Hex_sqrt_3 = sqrt(3.0);

int2 Hex_GetGridxy(float2 pos, float hexWidth, float parity, float facing)
{
    pos = lerp(pos.xy, pos.yx, facing);
    
    float halfWidth = hexWidth * 0.5;
    float2 quad = hexWidth * float2(1, Hex_sqrt_3);
    float2 fv = floor(pos / quad);
    float2 localxy = pos - fv * quad;
    
    float2 r1xy = abs(localxy - halfWidth * float2(1, Hex_sqrt_3));
    float threshold = -rcp(Hex_sqrt_3) * r1xy.x + hexWidth / Hex_sqrt_3;
    float sv = step(threshold, r1xy.y);
    float2 finalxy = fv * float2(1, 2) + float2((1 - sv) * (1 - parity) + step(halfWidth, localxy.x) * sv, (1 - sv) + step(halfWidth * Hex_sqrt_3, localxy.y) * sv * 2);
  
    finalxy = lerp(finalxy.xy, finalxy.yx, facing);
    return (int2) finalxy;
}
float3 Hex_GetPositon(int2 xy, float hexWidth, float parity, int facing)
{
    xy = facing == 0 ? xy : xy.yx;
    float3 v3 = float3((xy.x + (xy.y & 1) * (parity - 0.5)) * hexWidth, 0, xy.y * hexWidth * 0.5 * Hex_sqrt_3);
    v3.xz = lerp(v3.xz, v3.zx, facing);
    return v3;
}

int2 Hex_GetQuadCenterGrid(int2 xy)
{
    xy += Hex_QuadHalfSize;
    return xy - (xy % Hex_QuadSize + Hex_QuadSize) % Hex_QuadSize;
}
int2 Hex_GetQuadLocalxy(int2 xy)
{
    xy += Hex_QuadHalfSize;
    xy %= Hex_QuadSize;
    return (xy + Hex_QuadSize) % Hex_QuadSize;
}

#endif