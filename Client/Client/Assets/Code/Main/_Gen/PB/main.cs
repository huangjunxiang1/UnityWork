using System.Collections.Generic;
using Game;
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
    [Message(354306413)]
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
        public int test14 { get; set; }
        public long test16 { get; set; }
        public COM_Client_Status test15 { get; set; }
        public double test18 { get; set; }
        public Dictionary<int, double> test19 { get; set; } = new Dictionary<int, double>();
        public List<bool> test21 { get; set; } = new List<bool>();
        public List<int> test22 { get; set; } = new List<int>();
        public List<int> test23 { get; set; } = new List<int>();
        public List<long> test24 { get; set; } = new List<long>();
        public List<long> test25 { get; set; } = new List<long>();
        public List<float> test26 { get; set; } = new List<float>();
        public List<string> test27 { get; set; } = new List<string>();
        public List<TestPB2> test28 { get; set; } = new List<TestPB2>();
        public List<int> test29 { get; set; } = new List<int>();
        public List<long> test31 { get; set; } = new List<long>();
        public List<double> test33 { get; set; } = new List<double>();
        public float2 test34 { get; set; }
        public float3 test35 { get; set; }
        public float4 test36 { get; set; }
    }

    [Message(1950426884)]
    public partial class TestPB2
    {
        public int test2 { get; set; }
        public List<int> test22 { get; set; } = new List<int>();
    }

    /// <summary>
    /// 测试空行 跳行不被认为是注释
    /// </summary>
    [Message(909401441, typeof(S2C_Login))]
    public partial class C2S_Login
    {
        public string acc { get; set; }
        public string pw { get; set; }
    }

    [Message(908352881)]
    public partial class S2C_Login
    {
        public string error { get; set; }
        public long token { get; set; }
        public string ip { get; set; }
        public int port { get; set; }
    }

    [Message(1532303876, typeof(S2C_LoginGame))]
    public partial class C2S_LoginGame
    {
        public long token { get; set; }
    }

    [Message(1531255316)]
    public partial class S2C_LoginGame
    {
        public string error { get; set; }
    }

    [Message(34144539)]
    public partial class RoomInfo
    {
        public long id { get; set; }
        public string name { get; set; }
        /// <summary>
        /// 玩家
        /// </summary>
        public List<UnitInfo> infos { get; set; } = new List<UnitInfo>();
        public Dictionary<int, RoomLinkItem> link { get; set; } = new Dictionary<int, RoomLinkItem>();
    }

    [Message(453967900)]
    public partial class UnitInfo
    {
        public long id { get; set; }
        public string name { get; set; }
    }

    [Message(453967918)]
    public partial class UnitInfo2
    {
        public long id { get; set; }
        public game.S2C_SyncTransform t { get; set; } = new game.S2C_SyncTransform();
        public Dictionary<int, long> attribute { get; set; } = new Dictionary<int, long>();
    }

    [Message(1919449112)]
    public partial class UnitAttribute
    {
        public int id { get; set; }
        public long v { get; set; }
    }

    [Message(1801744983)]
    public partial class RoomLinkItem
    {
        /// <summary>
        /// 唯一下标
        /// </summary>
        public int index { get; set; }
        /// <summary>
        /// room xy
        /// </summary>
        public int2 xy { get; set; }
        /// <summary>
        /// 0=左 1=下 2=右  3=上
        /// </summary>
        public int dir { get; set; }
        /// <summary>
        /// 链接对象
        /// </summary>
        public int link { get; set; }
        public int colorIndex { get; set; }
    }

    /// <summary>
    /// 表明我是udp链接
    /// </summary>
    [Message(2037214234)]
    public partial class C2S_UDPConnect
    {
    }

    [Message(1179595869, typeof(S2C_RoomList))]
    public partial class C2S_RoomList
    {
    }

    [Message(1180644429)]
    public partial class S2C_RoomList
    {
        public List<RoomInfo> lst { get; set; } = new List<RoomInfo>();
    }

    [Message(1365526555, typeof(S2C_CreateRoom))]
    public partial class C2S_CreateRoom
    {
        public string name { get; set; }
    }

    [Message(1366575115)]
    public partial class S2C_CreateRoom
    {
        public RoomInfo info { get; set; } = new RoomInfo();
    }

    [Message(1549087323, typeof(S2C_JoinRoom))]
    public partial class C2S_JoinRoom
    {
        public long id { get; set; }
    }

    [Message(1548038731)]
    public partial class S2C_JoinRoom
    {
        public RoomInfo info { get; set; } = new RoomInfo();
        public List<UnitInfo2> units { get; set; } = new List<UnitInfo2>();
        public long myid { get; set; }
    }

    [Message(641347424)]
    public partial class S2C_PlayerJoinRoom
    {
        public UnitInfo2 info { get; set; } = new UnitInfo2();
    }

    [Message(223163496, typeof(S2C_DisRoom))]
    public partial class C2S_DisRoom
    {
        public long id { get; set; }
    }

    [Message(224212088)]
    public partial class S2C_DisRoom
    {
        public long id { get; set; }
    }

    [Message(1399019551, typeof(S2C_PlayerQuit))]
    public partial class C2S_PlayerQuit
    {
    }

    [Message(1400068111)]
    public partial class S2C_PlayerQuit
    {
        public long id { get; set; }
    }

}
