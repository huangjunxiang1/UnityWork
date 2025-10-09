using System.Collections.Generic;
using Game;
using Unity.Mathematics;

namespace game
{
    /// <summary>
    /// //MsgID=50@0
    /// </summary>
    [Message(540237658, typeof(game.S2C_SyncTransform))]
    public partial class C2S_SyncTransform
    {
        public float2 dir;
    }
    /// <summary>
    /// //MsgID=50@1
    /// </summary>
    [Message(539189066)]
    public partial class S2C_SyncTransform
    {
        public float3 p;
        public float4 r;
        public bool isMoving;
    }
    [Message(943545107, typeof(game.S2C_Ping))]
    public partial class C2S_Ping
    {
    }
    [Message(942496515)]
    public partial class S2C_Ping
    {
    }
}
