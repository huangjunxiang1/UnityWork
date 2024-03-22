using System.Collections.Generic;
using Core;
using Unity.Mathematics;

namespace game
{
    [Message(50 | 0 << 16, typeof(S2C_SyncTransform))]
    public partial class C2S_SyncTransform
    {
        public float2 dir { get; set; }
    }

    [Message(50 | 1 << 16)]
    public partial class S2C_SyncTransform
    {
        public float3 p { get; set; }
        public float4 r { get; set; }
    }

}
