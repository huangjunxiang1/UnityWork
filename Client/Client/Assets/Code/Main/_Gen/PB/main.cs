using System.Collections.Generic;
using Game;
using Unity.Mathematics;

namespace main
{
    /// <summary>
    /// // 枚举类型定义
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        ///  必须包含0值
        /// </summary>
        ROLE_UNSPECIFIED = 0,
        ROLE_USER = 1,
        ROLE_ADMIN = 2,
        ROLE_MODERATOR = 3,
        ROLE_GUEST = 4,
    }
    /// <summary>
    /// // 嵌套消息定义
    /// </summary>
    [Message(945842497)]
    public partial class GeoLocation
    {
        public double latitude;
        public double longitude;
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude { get => _altitude; set { _altitude = value; Set_altitude(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude4 { get => _altitude4; set { _altitude4 = value; Set_altitude4(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude5 { get => _altitude5; set { _altitude5 = value; Set_altitude5(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude6 { get => _altitude6; set { _altitude6 = value; Set_altitude6(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude7 { get => _altitude7; set { _altitude7 = value; Set_altitude7(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude8 { get => _altitude8; set { _altitude8 = value; Set_altitude8(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude9 { get => _altitude9; set { _altitude9 = value; Set_altitude9(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude10 { get => _altitude10; set { _altitude10 = value; Set_altitude10(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude11 { get => _altitude11; set { _altitude11 = value; Set_altitude11(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude12 { get => _altitude12; set { _altitude12 = value; Set_altitude12(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude13 { get => _altitude13; set { _altitude13 = value; Set_altitude13(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude14 { get => _altitude14; set { _altitude14 = value; Set_altitude14(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude15 { get => _altitude15; set { _altitude15 = value; Set_altitude15(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude16 { get => _altitude16; set { _altitude16 = value; Set_altitude16(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude17 { get => _altitude17; set { _altitude17 = value; Set_altitude17(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude18 { get => _altitude18; set { _altitude18 = value; Set_altitude18(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude19 { get => _altitude19; set { _altitude19 = value; Set_altitude19(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude20 { get => _altitude20; set { _altitude20 = value; Set_altitude20(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude21 { get => _altitude21; set { _altitude21 = value; Set_altitude21(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude22 { get => _altitude22; set { _altitude22 = value; Set_altitude22(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude23 { get => _altitude23; set { _altitude23 = value; Set_altitude23(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude24 { get => _altitude24; set { _altitude24 = value; Set_altitude24(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude25 { get => _altitude25; set { _altitude25 = value; Set_altitude25(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude26 { get => _altitude26; set { _altitude26 = value; Set_altitude26(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude27 { get => _altitude27; set { _altitude27 = value; Set_altitude27(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude28 { get => _altitude28; set { _altitude28 = value; Set_altitude28(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude29 { get => _altitude29; set { _altitude29 = value; Set_altitude29(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude30 { get => _altitude30; set { _altitude30 = value; Set_altitude30(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude31 { get => _altitude31; set { _altitude31 = value; Set_altitude31(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude32 { get => _altitude32; set { _altitude32 = value; Set_altitude32(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude33 { get => _altitude33; set { _altitude33 = value; Set_altitude33(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude34 { get => _altitude34; set { _altitude34 = value; Set_altitude34(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude35 { get => _altitude35; set { _altitude35 = value; Set_altitude35(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude36 { get => _altitude36; set { _altitude36 = value; Set_altitude36(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude37 { get => _altitude37; set { _altitude37 = value; Set_altitude37(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude38 { get => _altitude38; set { _altitude38 = value; Set_altitude38(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude39 { get => _altitude39; set { _altitude39 = value; Set_altitude39(); } }
        /// <summary>
        ///  可选的海拔
        /// </summary>
        public float altitude40 { get => _altitude40; set { _altitude40 = value; Set_altitude40(); } }
    }
    [Message(1914115876)]
    public partial class Address
    {
        public string street;
        public string city;
        public string country;
        public string postal_code;
        /// <summary>
        ///  可选的坐标
        /// </summary>
        public GeoLocation location { get => _location; set { _location = value; Set_location(); } }
    }
    /// <summary>
    /// // 主消息定义 - 包含所有基础类型和特性
    /// </summary>
    [Message(1970165065)]
    public partial class ComprehensiveMessage
    {
        /// <summary>
        ///  双精度浮点数
        /// </summary>
        public double double_field;
        /// <summary>
        ///  可选的双精度浮点数
        /// </summary>
        public double optional_double_field { get => _optional_double_field; set { _optional_double_field = value; Set_optional_double_field(); } }
        /// <summary>
        ///  单精度浮点数
        /// </summary>
        public float float_field;
        /// <summary>
        ///  可选的单精度浮点数
        /// </summary>
        public float optional_float_field { get => _optional_float_field; set { _optional_float_field = value; Set_optional_float_field(); } }
        /// <summary>
        ///  32位有符号整数
        /// </summary>
        public int int32_field;
        /// <summary>
        ///  可选的32位有符号整数
        /// </summary>
        public int optional_int32_field { get => _optional_int32_field; set { _optional_int32_field = value; Set_optional_int32_field(); } }
        /// <summary>
        ///  64位有符号整数
        /// </summary>
        public long int64_field;
        /// <summary>
        ///  可选的64位有符号整数
        /// </summary>
        public long optional_int64_field { get => _optional_int64_field; set { _optional_int64_field = value; Set_optional_int64_field(); } }
        /// <summary>
        ///  32位无符号整数
        /// </summary>
        public uint uint32_field;
        /// <summary>
        ///  可选的32位无符号整数
        /// </summary>
        public uint optional_uint32_field { get => _optional_uint32_field; set { _optional_uint32_field = value; Set_optional_uint32_field(); } }
        /// <summary>
        ///  64位无符号整数
        /// </summary>
        public ulong uint64_field;
        /// <summary>
        ///  可选的64位无符号整数
        /// </summary>
        public ulong optional_uint64_field { get => _optional_uint64_field; set { _optional_uint64_field = value; Set_optional_uint64_field(); } }
        /// <summary>
        ///  有符号32位整数（对负数编码更高效）
        /// </summary>
        public int sint32_field;
        /// <summary>
        ///  可选的有符号32位整数
        /// </summary>
        public int optional_sint32_field { get => _optional_sint32_field; set { _optional_sint32_field = value; Set_optional_sint32_field(); } }
        /// <summary>
        ///  有符号64位整数（对负数编码更高效）
        /// </summary>
        public long sint64_field;
        /// <summary>
        ///  可选的有符号64位整数
        /// </summary>
        public long optional_sint64_field { get => _optional_sint64_field; set { _optional_sint64_field = value; Set_optional_sint64_field(); } }
        /// <summary>
        ///  固定32位无符号整数
        /// </summary>
        public int fixed32_field;
        /// <summary>
        ///  可选的固定32位无符号整数
        /// </summary>
        public int optional_fixed32_field { get => _optional_fixed32_field; set { _optional_fixed32_field = value; Set_optional_fixed32_field(); } }
        /// <summary>
        ///  固定64位无符号整数
        /// </summary>
        public long fixed64_field;
        /// <summary>
        ///  可选的固定64位无符号整数
        /// </summary>
        public long optional_fixed64_field { get => _optional_fixed64_field; set { _optional_fixed64_field = value; Set_optional_fixed64_field(); } }
        /// <summary>
        ///  布尔值
        /// </summary>
        public bool bool_field;
        /// <summary>
        ///  可选的布尔值
        /// </summary>
        public bool optional_bool_field { get => _optional_bool_field; set { _optional_bool_field = value; Set_optional_bool_field(); } }
        /// <summary>
        ///  字符串
        /// </summary>
        public string string_field;
        /// <summary>
        ///  可选的字符串
        /// </summary>
        public string optional_string_field { get => _optional_string_field; set { _optional_string_field = value; Set_optional_string_field(); } }
        /// <summary>
        ///  二进制数据
        /// </summary>
        public byte[] bytes_field;
        /// <summary>
        ///  可选的二进制数据
        /// </summary>
        public byte[] optional_bytes_field { get => _optional_bytes_field; set { _optional_bytes_field = value; Set_optional_bytes_field(); } }
        /// <summary>
        ///  枚举字段
        /// </summary>
        public UserRole enum_field;
        /// <summary>
        ///  可选的枚举字段
        /// </summary>
        public UserRole optional_enum_field { get => _optional_enum_field; set { _optional_enum_field = value; Set_optional_enum_field(); } }
        /// <summary>
        ///  嵌套消息
        /// </summary>
        public Address address_field = new();
        /// <summary>
        ///  可选的嵌套消息
        /// </summary>
        public Address optional_address_field { get => _optional_address_field; set { _optional_address_field = value; Set_optional_address_field(); } }
        /// <summary>
        ///  字符串数组
        /// </summary>
        public List<string> tags = new();
        /// <summary>
        ///  整数数组
        /// </summary>
        public List<int> scores = new();
        /// <summary>
        ///  枚举数组
        /// </summary>
        public List<UserRole> role_history = new();
        /// <summary>
        ///  消息对象数组
        /// </summary>
        public List<Address> addresses = new();
        /// <summary>
        ///  bytes数组
        /// </summary>
        public List<byte[]> binary_chunks = new();
        public List<double> optional_double_list = new();
        /// <summary>
        ///  string → string 映射
        /// </summary>
        public Dictionary<string, string> string_map = new();
        /// <summary>
        ///  int → string 映射
        /// </summary>
        public Dictionary<int, string> id_to_name_map = new();
        /// <summary>
        ///  string → 消息对象 映射
        /// </summary>
        public Dictionary<string, Address> location_map = new();
        /// <summary>
        ///  string → 枚举 映射
        /// </summary>
        public Dictionary<string, UserRole> role_assignments = new();
        /// <summary>
        ///  string → bytes 映射
        /// </summary>
        public Dictionary<string, byte[]> resource_map = new();
        /// <summary>
        ///  string → bytes 映射
        /// </summary>
        public Dictionary<UserRole, int> resource_map2 = new();
        public int2 xy;
        public List<int2> lstxy = new();
        public Dictionary<int, int2> mapxy = new();
        public int2 optional_xy { get => _optional_xy; set { _optional_xy = value; Set_optional_xy(); } }
        public float2 f2;
        public List<float2> lstf2 = new();
        public Dictionary<int, float2> mapf2 = new();
        public float2 optional_f2 { get => _optional_f2; set { _optional_f2 = value; Set_optional_f2(); } }
    }
    /// <summary>
    /// //测试空行 跳行不被认为是注释
    /// </summary>
    [Message(909401441, typeof(main.S2C_Login))]
    public partial class C2S_Login
    {
        public string acc;
        public string pw;
    }
    [Message(908352881)]
    public partial class S2C_Login
    {
        public long token;
        public string ip;
        public int port;
    }
    [Message(1532303876, typeof(main.S2C_LoginGame))]
    public partial class C2S_LoginGame
    {
        public long token;
    }
    [Message(1531255316)]
    public partial class S2C_LoginGame
    {
    }
    [Message(34144539)]
    public partial class RoomInfo
    {
        public long id;
        public string name;
        /// <summary>
        /// 玩家
        /// </summary>
        public List<UnitInfo> infos = new();
        public Dictionary<int, RoomLinkItem> link = new();
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
        public game.S2C_SyncTransform t = new();
        public Dictionary<int, long> attribute = new();
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
    /// //表明我是udp链接
    /// </summary>
    [Message(2037214234)]
    public partial class C2S_UDPConnect
    {
    }
    [Message(1179595869, typeof(main.S2C_RoomList))]
    public partial class C2S_RoomList
    {
    }
    [Message(1180644429)]
    public partial class S2C_RoomList
    {
        public List<RoomInfo> lst = new();
    }
    [Message(1365526555, typeof(main.S2C_CreateRoom))]
    public partial class C2S_CreateRoom
    {
        public string name;
    }
    [Message(1366575115)]
    public partial class S2C_CreateRoom
    {
        public RoomInfo info = new();
    }
    [Message(1549087323, typeof(main.S2C_JoinRoom))]
    public partial class C2S_JoinRoom
    {
        public long id;
    }
    [Message(1548038731)]
    public partial class S2C_JoinRoom
    {
        public RoomInfo info = new();
        public List<UnitInfo2> units = new();
        public long myid;
    }
    [Message(641347424)]
    public partial class S2C_PlayerJoinRoom
    {
        public UnitInfo2 info = new();
    }
    [Message(223163496, typeof(main.S2C_DisRoom))]
    public partial class C2S_DisRoom
    {
        public long id;
    }
    [Message(224212088)]
    public partial class S2C_DisRoom
    {
        public long id;
    }
    [Message(1399019551, typeof(main.S2C_PlayerQuit))]
    public partial class C2S_PlayerQuit
    {
    }
    [Message(1400068111)]
    public partial class S2C_PlayerQuit
    {
        public long id;
    }
}
