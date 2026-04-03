using System;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;

public static partial class Hex
{
    public static bool HasTree(int2 gxy)
    {
        float2 pos = Hex.GetPositon(gxy).xz;
        float height = noise.snoise((pos + 0) * GPUConstDefine.Define_World_Scale);
        const float step = 0;
        return height > step && maths.random_uint2Touint((uint2)gxy) % 100 < 80;
    }
}
