using System.Collections.Generic;
using Core;
using Unity.Mathematics;

namespace main
{
    /// <summary>
    /// 枚举定义注释
    /// </summary>
    public enum COM_Client_Status
    {
        S_Disconnected = 0,
        /// <summary>
        /// 枚举注释
        /// </summary>
        S_Connected = 1,
        S_Logined = 2,
        S_Loading = 3,
        S_Playing = 5,
        S_Relocating = 6,
    }

    /// <summary>
    /// 类定义注释
    /// </summary>
    [Message(1 | 2 << 16)]
    public partial class TestPBmain
    {
        /// <summary>
        /// 字段注释
        /// </summary>
        public bool test { get; set; }
        public int test2 { get; set; }
        public int test3 { get; set; }
        public long test4 { get; set; }
        public long test5 { get; set; }
        public float test6 { get; set; }
        public string test7 { get; set; }
        public TestPB2 test8 { get; set; } = new TestPB2();
        /// <summary>
        ///  map<string, TestPB2> test9  = 9;
        /// </summary>
        public Dictionary<string, TestPB2> test9 { get; set; } = new Dictionary<string, TestPB2>();
        public Dictionary<long, bool> test10 { get; set; } = new Dictionary<long, bool>();
        public Dictionary<int, long> test11 { get; set; } = new Dictionary<int, long>();
        public byte[] test12 { get; set; }
        public uint test14 { get; set; }
        public int test15 { get; set; }
        public ulong test16 { get; set; }
        public long test17 { get; set; }
        public double test18 { get; set; }
        public Dictionary<uint, double> test19 { get; set; } = new Dictionary<uint, double>();
        public Dictionary<long, string> test20 { get; set; } = new Dictionary<long, string>();
        public List<bool> test21 { get; set; } = new List<bool>();
        public List<int> test22 { get; set; } = new List<int>();
        public List<int> test23 { get; set; } = new List<int>();
        public List<long> test24 { get; set; } = new List<long>();
        public List<long> test25 { get; set; } = new List<long>();
        public List<float> test26 { get; set; } = new List<float>();
        public List<string> test27 { get; set; } = new List<string>();
        public List<TestPB2> test28 { get; set; } = new List<TestPB2>();
        public List<uint> test29 { get; set; } = new List<uint>();
        public List<int> test30 { get; set; } = new List<int>();
        public List<ulong> test31 { get; set; } = new List<ulong>();
        public List<long> test32 { get; set; } = new List<long>();
        public List<double> test33 { get; set; } = new List<double>();
        public float2 test34 { get; set; }
        public float3 test35 { get; set; }
        public float4 test36 { get; set; }
    }

    public partial class TestPB2
    {
        public int test2 { get; set; }
        public List<int> test22 { get; set; } = new List<int>();
    }

    public partial class RoomInfo
    {
        public long id { get; set; }
        public string name { get; set; }
        /// <summary>
        /// 玩家
        /// </summary>
        public List<UnitInfo> infos { get; set; } = new List<UnitInfo>();
    }

    public partial class UnitInfo
    {
        public long id { get; set; }
        public string name { get; set; }
    }

    public partial class UnitInfo2
    {
        public long id { get; set; }
        public game.S2C_SyncTransform t { get; set; } = new game.S2C_SyncTransform();
        public Dictionary<int, long> attribute { get; set; } = new Dictionary<int, long>();
    }

    [Message(100 | 0 << 16)]
    public partial class UnitAttribute
    {
        public int id { get; set; }
        public long v { get; set; }
    }

    [Message(100 | 1 << 16, typeof(S2C_RoomList))]
    public partial class C2S_RoomList
    {
    }

    [Message(100 | 2 << 16)]
    public partial class S2C_RoomList
    {
        public List<RoomInfo> lst { get; set; } = new List<RoomInfo>();
    }

    [Message(100 | 3 << 16, typeof(S2C_CreateRoom))]
    public partial class C2S_CreateRoom
    {
        public string name { get; set; }
    }

    [Message(100 | 4 << 16)]
    public partial class S2C_CreateRoom
    {
        public RoomInfo info { get; set; } = new RoomInfo();
    }

    [Message(100 | 5 << 16, typeof(S2C_JoinRoom))]
    public partial class C2S_JoinRoom
    {
        public long id { get; set; }
    }

    [Message(100 | 6 << 16)]
    public partial class S2C_JoinRoom
    {
        public RoomInfo info { get; set; } = new RoomInfo();
        public List<UnitInfo2> units { get; set; } = new List<UnitInfo2>();
        public long myid { get; set; }
    }

    [Message(100 | 7 << 16)]
    public partial class S2C_PlayerJoinRoom
    {
        public UnitInfo2 info { get; set; } = new UnitInfo2();
    }

    [Message(100 | 8 << 16, typeof(S2C_DisRoom))]
    public partial class C2S_DisRoom
    {
        public long id { get; set; }
    }

    [Message(100 | 9 << 16)]
    public partial class S2C_DisRoom
    {
        public long id { get; set; }
    }

}
