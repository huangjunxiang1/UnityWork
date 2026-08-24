#include "math.hlsl"
#include "Define.hlsl"
#include "hex.hlsl"

int offsetIndex;

static float vertex_sqrt_3 = sqrt(3.0);

void Vertex_Grid_float(float3 worldPos,float stepv, out float sv)
{
    int2 gridxy = Hex_GetGridxy(worldPos.xz, HexWidth, 0, 0);
    float2 abs_xy = abs(worldPos.xz - Hex_GetPositon(gridxy, HexWidth, 0, 0).xz);
    float rv = rcp(vertex_sqrt_3);
    float2 p1 = HexWidth * float2(0, rv);
    float2 p2 = HexWidth * 0.5 * float2(1, rv);
    float m = distancePAB(abs_xy, p1, p2);
    m = min(HexWidth / 2 - abs_xy.x, m);
    sv = 1-step(stepv, m / (HexWidth / 2));
}

StructuredBuffer<float2> wall_datas;
void Vertex_Wall_float(int instanceid, float3 objPos, out float3 pos)
{
    float2 dt = wall_datas[offsetIndex + instanceid];
    pos = objPos + float3(dt.x, 0.0, dt.y);
}

StructuredBuffer<float2> tree_datas;
void Vertex_Tree_float(int instanceid, float3 objPos,float3 normal, out float3 pos,out float3 out_normal)
{
    float2 dt = tree_datas[offsetIndex + instanceid];
    objPos *= 0.12;
    float angle = random_uint2Tofloat(asuint(dt)) * Define_2PI;
    objPos.xz = rotate(objPos.xz, angle);
    float2 n = rotate(normal.xz, angle);
    out_normal = float3(n.x, normal.y,n.y);
    pos = objPos + float3(dt.x, 0.0, dt.y);
}
