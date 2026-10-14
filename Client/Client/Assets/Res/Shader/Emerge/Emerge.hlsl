
static const int2 Emerge_SceneSize = int2(2400, 1080);

#ifdef EMERGE_COMPUTE
RWStructuredBuffer<int> kingLand;
RWStructuredBuffer<int> unitLand;
RWStructuredBuffer<int> unitCenter;
RWStructuredBuffer<int> unitDir;        // 单位当前朝向 0~7
RWStructuredBuffer<int> pheromoneLand;   // 新增：信息素
#else
StructuredBuffer<int> kingLand;
StructuredBuffer<int> unitLand; //0 无  1小兵  2.将军
#endif

static const float4 kingLandColors[4] =
{
    float4(0, 0, 0, 0),
    float4(1, 0, 0, 0.2),
    float4(0, 1, 0, 0.2),
    float4(0, 0, 1, 0.2)
};

void GetPixel_float(float2 uv,out float4 color)
{
    int2 xy = (int2) (uv * Emerge_SceneSize);
    xy = clamp(xy, 0, Emerge_SceneSize - 1);
    int index = xy.y * Emerge_SceneSize.x + xy.x;
    color = kingLandColors[kingLand[index]]; 
    
    int u = unitLand[index];
    if (u == 1)
        color = float4(1, 1, 1, 1); // 小兵：纯白，不透明
    else if (u == 2)
        color = float4(1, 1, 0, 1); // 将军：纯黄，不透明
    else
        color = float4(color.rgb, 0.2); // 无单位：地图颜色
}