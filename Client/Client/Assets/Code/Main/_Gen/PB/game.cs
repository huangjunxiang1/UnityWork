using System.Collections.Generic;
using Game;
using Unity.Mathematics;

namespace game
{
    /// <summary>
    /// MsgID=50@0
    /// </summary>
    [Message(540237658, typeof(S2C_SyncTransform))]
    public partial class C2S_SyncTransform
    {
        public float2 dir { get; set; }
    }

    /// <summary>
    /// MsgID=50@1
    /// </summary>
    [Message(539189066)]
    public partial class S2C_SyncTransform
    {
        public float3 p { get; set; }
        public float4 r { get; set; }
        public bool isMoving { get; set; }
    }

    [Message(943545107, typeof(S2C_Ping))]
    public partial class C2S_Ping
    {
    }

    [Message(942496515)]
    public partial class S2C_Ping
    {
    }

}
