#include "math.hlsl"
#include "Define.hlsl"

int offsetIndex;

StructuredBuffer<float2> grid_datas;
void Vertex_Grid_float(int instanceid, float3 objPos, out float3 pos)
{
    float2 dt = grid_datas[offsetIndex + instanceid];
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
