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
        public bool test;
        public int test2;
        public int test3;
        public long test4;
        public long test5;
        public float test6;
        public string test7;
        public TestPB2 test8 = new TestPB2();
        /// <summary>
        ///  map<string, TestPB2> test9  = 9;
        /// </summary>
        public Dictionary<string, TestPB2> test9 = new Dictionary<string, TestPB2>();
        public Dictionary<long, bool> test10 = new Dictionary<long, bool>();
        public Dictionary<int, long> test11 = new Dictionary<int, long>();
        public byte[] test12;
        public int test14;
        public long test16;
        public COM_Client_Status test15;
        public double test18;
        public Dictionary<int, double> test19 = new Dictionary<int, double>();
        public List<bool> test21 = new List<bool>();
        public List<int> test22 = new List<int>();
        public List<int> test23 = new List<int>();
        public List<long> test24 = new List<long>();
        public List<long> test25 = new List<long>();
        public List<float> test26 = new List<float>();
        public List<string> test27 = new List<string>();
        public List<TestPB2> test28 = new List<TestPB2>();
        public List<int> test29 = new List<int>();
        public List<long> test31 = new List<long>();
        public List<double> test33 = new List<double>();
        public float2 test34;
        public float3 test35;
        public float4 test36;
    }

    [Message(1950426884)]
    public partial class TestPB2
    {
        public int test2;
        public List<int> test22 = new List<int>();
    }

    /// <summary>
    /// 测试空行 跳行不被认为是注释
    /// </summary>
    [Message(909401441, typeof(S2C_Login))]
    public partial class C2S_Login
    {
        public string acc;
        public string pw;
    }

    [Message(908352881)]
    public partial class S2C_Login
    {
        public string error;
        public long token;
        public string ip;
        public int port;
    }

    [Message(1532303876, typeof(S2C_LoginGame))]
    public partial class C2S_LoginGame
    {
        public long token;
    }

    [Message(1531255316)]
    public partial class S2C_LoginGame
    {
        public string error;
    }

    [Message(34144539)]
    public partial class RoomInfo
    {
        public long id;
        public string name;
        /// <summary>
        /// 玩家
        /// </summary>
        public List<UnitInfo> infos = new List<UnitInfo>();
        public Dictionary<int, RoomLinkItem> link = new Dictionary<int, RoomLinkItem>();
    }

    [Message(453967900)]
    public partial class UnitInfo
    {
        public long id;
        public string name;
    }

    [Message(453967918)]
    public partial class UnitInfo2
    {
        public long id;
        public game.S2C_SyncTransform t = new game.S2C_SyncTransform();
        public Dictionary<int, long> attribute = new Dictionary<int, long>();
    }

    [Message(1919449112)]
    public partial class UnitAttribute
    {
        public int id;
        public long v;
    }

    [Message(1801744983)]
    public partial class RoomLinkItem
    {
        /// <summary>
        /// 唯一下标
        /// </summary>
        public int index;
        /// <summary>
        /// room xy
        /// </summary>
        public int2 xy;
        /// <summary>
        /// 0=左 1=下 2=右  3=上
        /// </summary>
        public int dir;
        /// <summary>
        /// 链接对象
        /// </summary>
        public int link;
        public int colorIndex;
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
        public List<RoomInfo> lst = new List<RoomInfo>();
    }

    [Message(1365526555, typeof(S2C_CreateRoom))]
    public partial class C2S_CreateRoom
    {
        public string name;
    }

    [Message(1366575115)]
    public partial class S2C_CreateRoom
    {
        public RoomInfo info = new RoomInfo();
    }

    [Message(1549087323, typeof(S2C_JoinRoom))]
    public partial class C2S_JoinRoom
    {
        public long id;
    }

    [Message(1548038731)]
    public partial class S2C_JoinRoom
    {
        public RoomInfo info = new RoomInfo();
        public List<UnitInfo2> units = new List<UnitInfo2>();
        public long myid;
    }

    [Message(641347424)]
    public partial class S2C_PlayerJoinRoom
    {
        public UnitInfo2 info = new UnitInfo2();
    }

    [Message(223163496, typeof(S2C_DisRoom))]
    public partial class C2S_DisRoom
    {
        public long id;
    }

    [Message(224212088)]
    public partial class S2C_DisRoom
    {
        public long id;
    }

    [Message(1399019551, typeof(S2C_PlayerQuit))]
    public partial class C2S_PlayerQuit
    {
    }

    [Message(1400068111)]
    public partial class S2C_PlayerQuit
    {
        public long id;
    }

}
