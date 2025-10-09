using System.Collections.Generic;
using Game;
using Unity.Mathematics;

namespace game
{
    public partial class C2S_SyncTransform : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writefloat2(10, this.dir);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10: this.dir = reader.Readfloat2(); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (C2S_SyncTransform)message;
            this.dir = tmp.dir;
        }
    }
    public partial class S2C_SyncTransform : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writefloat3(10, this.p);
            writer.Writefloat4(18, this.r);
            writer.Writebool(24, this.isMoving);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10: this.p = reader.Readfloat3(); break;
                    case 18: this.r = reader.Readfloat4(); break;
                    case 24: this.isMoving = reader.Readbool(); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (S2C_SyncTransform)message;
            this.p = tmp.p;
            this.r = tmp.r;
            this.isMoving = tmp.isMoving;
        }
    }
    public partial class C2S_Ping : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (C2S_Ping)message;
        }
    }
    public partial class S2C_Ping : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (S2C_Ping)message;
        }
    }
}
namespace main
{
    public partial class GeoLocation : PB.PBMessage
    {
        int mask0;
        int mask1;
        float _altitude = new();
        public bool Has_altitude => (mask0 & (1 << 1)) != 0;
        void Set_altitude() => mask0 |= 1 << 1;
        public void Clear_altitude() => mask0 &= ~(1 << 1);
        float _altitude4 = new();
        public bool Has_altitude4 => (mask0 & (1 << 2)) != 0;
        void Set_altitude4() => mask0 |= 1 << 2;
        public void Clear_altitude4() => mask0 &= ~(1 << 2);
        float _altitude5 = new();
        public bool Has_altitude5 => (mask0 & (1 << 3)) != 0;
        void Set_altitude5() => mask0 |= 1 << 3;
        public void Clear_altitude5() => mask0 &= ~(1 << 3);
        float _altitude6 = new();
        public bool Has_altitude6 => (mask0 & (1 << 4)) != 0;
        void Set_altitude6() => mask0 |= 1 << 4;
        public void Clear_altitude6() => mask0 &= ~(1 << 4);
        float _altitude7 = new();
        public bool Has_altitude7 => (mask0 & (1 << 5)) != 0;
        void Set_altitude7() => mask0 |= 1 << 5;
        public void Clear_altitude7() => mask0 &= ~(1 << 5);
        float _altitude8 = new();
        public bool Has_altitude8 => (mask0 & (1 << 6)) != 0;
        void Set_altitude8() => mask0 |= 1 << 6;
        public void Clear_altitude8() => mask0 &= ~(1 << 6);
        float _altitude9 = new();
        public bool Has_altitude9 => (mask0 & (1 << 7)) != 0;
        void Set_altitude9() => mask0 |= 1 << 7;
        public void Clear_altitude9() => mask0 &= ~(1 << 7);
        float _altitude10 = new();
        public bool Has_altitude10 => (mask0 & (1 << 8)) != 0;
        void Set_altitude10() => mask0 |= 1 << 8;
        public void Clear_altitude10() => mask0 &= ~(1 << 8);
        float _altitude11 = new();
        public bool Has_altitude11 => (mask0 & (1 << 9)) != 0;
        void Set_altitude11() => mask0 |= 1 << 9;
        public void Clear_altitude11() => mask0 &= ~(1 << 9);
        float _altitude12 = new();
        public bool Has_altitude12 => (mask0 & (1 << 10)) != 0;
        void Set_altitude12() => mask0 |= 1 << 10;
        public void Clear_altitude12() => mask0 &= ~(1 << 10);
        float _altitude13 = new();
        public bool Has_altitude13 => (mask0 & (1 << 11)) != 0;
        void Set_altitude13() => mask0 |= 1 << 11;
        public void Clear_altitude13() => mask0 &= ~(1 << 11);
        float _altitude14 = new();
        public bool Has_altitude14 => (mask0 & (1 << 12)) != 0;
        void Set_altitude14() => mask0 |= 1 << 12;
        public void Clear_altitude14() => mask0 &= ~(1 << 12);
        float _altitude15 = new();
        public bool Has_altitude15 => (mask0 & (1 << 13)) != 0;
        void Set_altitude15() => mask0 |= 1 << 13;
        public void Clear_altitude15() => mask0 &= ~(1 << 13);
        float _altitude16 = new();
        public bool Has_altitude16 => (mask0 & (1 << 14)) != 0;
        void Set_altitude16() => mask0 |= 1 << 14;
        public void Clear_altitude16() => mask0 &= ~(1 << 14);
        float _altitude17 = new();
        public bool Has_altitude17 => (mask0 & (1 << 15)) != 0;
        void Set_altitude17() => mask0 |= 1 << 15;
        public void Clear_altitude17() => mask0 &= ~(1 << 15);
        float _altitude18 = new();
        public bool Has_altitude18 => (mask0 & (1 << 16)) != 0;
        void Set_altitude18() => mask0 |= 1 << 16;
        public void Clear_altitude18() => mask0 &= ~(1 << 16);
        float _altitude19 = new();
        public bool Has_altitude19 => (mask0 & (1 << 17)) != 0;
        void Set_altitude19() => mask0 |= 1 << 17;
        public void Clear_altitude19() => mask0 &= ~(1 << 17);
        float _altitude20 = new();
        public bool Has_altitude20 => (mask0 & (1 << 18)) != 0;
        void Set_altitude20() => mask0 |= 1 << 18;
        public void Clear_altitude20() => mask0 &= ~(1 << 18);
        float _altitude21 = new();
        public bool Has_altitude21 => (mask0 & (1 << 19)) != 0;
        void Set_altitude21() => mask0 |= 1 << 19;
        public void Clear_altitude21() => mask0 &= ~(1 << 19);
        float _altitude22 = new();
        public bool Has_altitude22 => (mask0 & (1 << 20)) != 0;
        void Set_altitude22() => mask0 |= 1 << 20;
        public void Clear_altitude22() => mask0 &= ~(1 << 20);
        float _altitude23 = new();
        public bool Has_altitude23 => (mask0 & (1 << 21)) != 0;
        void Set_altitude23() => mask0 |= 1 << 21;
        public void Clear_altitude23() => mask0 &= ~(1 << 21);
        float _altitude24 = new();
        public bool Has_altitude24 => (mask0 & (1 << 22)) != 0;
        void Set_altitude24() => mask0 |= 1 << 22;
        public void Clear_altitude24() => mask0 &= ~(1 << 22);
        float _altitude25 = new();
        public bool Has_altitude25 => (mask0 & (1 << 23)) != 0;
        void Set_altitude25() => mask0 |= 1 << 23;
        public void Clear_altitude25() => mask0 &= ~(1 << 23);
        float _altitude26 = new();
        public bool Has_altitude26 => (mask0 & (1 << 24)) != 0;
        void Set_altitude26() => mask0 |= 1 << 24;
        public void Clear_altitude26() => mask0 &= ~(1 << 24);
        float _altitude27 = new();
        public bool Has_altitude27 => (mask0 & (1 << 25)) != 0;
        void Set_altitude27() => mask0 |= 1 << 25;
        public void Clear_altitude27() => mask0 &= ~(1 << 25);
        float _altitude28 = new();
        public bool Has_altitude28 => (mask0 & (1 << 26)) != 0;
        void Set_altitude28() => mask0 |= 1 << 26;
        public void Clear_altitude28() => mask0 &= ~(1 << 26);
        float _altitude29 = new();
        public bool Has_altitude29 => (mask0 & (1 << 27)) != 0;
        void Set_altitude29() => mask0 |= 1 << 27;
        public void Clear_altitude29() => mask0 &= ~(1 << 27);
        float _altitude30 = new();
        public bool Has_altitude30 => (mask0 & (1 << 28)) != 0;
        void Set_altitude30() => mask0 |= 1 << 28;
        public void Clear_altitude30() => mask0 &= ~(1 << 28);
        float _altitude31 = new();
        public bool Has_altitude31 => (mask0 & (1 << 29)) != 0;
        void Set_altitude31() => mask0 |= 1 << 29;
        public void Clear_altitude31() => mask0 &= ~(1 << 29);
        float _altitude32 = new();
        public bool Has_altitude32 => (mask0 & (1 << 30)) != 0;
        void Set_altitude32() => mask0 |= 1 << 30;
        public void Clear_altitude32() => mask0 &= ~(1 << 30);
        float _altitude33 = new();
        public bool Has_altitude33 => (mask0 & (1 << 31)) != 0;
        void Set_altitude33() => mask0 |= 1 << 31;
        public void Clear_altitude33() => mask0 &= ~(1 << 31);
        float _altitude34 = new();
        public bool Has_altitude34 => (mask1 & (1 << 0)) != 0;
        void Set_altitude34() => mask1 |= 1 << 0;
        public void Clear_altitude34() => mask1 &= ~(1 << 0);
        float _altitude35 = new();
        public bool Has_altitude35 => (mask1 & (1 << 1)) != 0;
        void Set_altitude35() => mask1 |= 1 << 1;
        public void Clear_altitude35() => mask1 &= ~(1 << 1);
        float _altitude36 = new();
        public bool Has_altitude36 => (mask1 & (1 << 2)) != 0;
        void Set_altitude36() => mask1 |= 1 << 2;
        public void Clear_altitude36() => mask1 &= ~(1 << 2);
        float _altitude37 = new();
        public bool Has_altitude37 => (mask1 & (1 << 3)) != 0;
        void Set_altitude37() => mask1 |= 1 << 3;
        public void Clear_altitude37() => mask1 &= ~(1 << 3);
        float _altitude38 = new();
        public bool Has_altitude38 => (mask1 & (1 << 4)) != 0;
        void Set_altitude38() => mask1 |= 1 << 4;
        public void Clear_altitude38() => mask1 &= ~(1 << 4);
        float _altitude39 = new();
        public bool Has_altitude39 => (mask1 & (1 << 5)) != 0;
        void Set_altitude39() => mask1 |= 1 << 5;
        public void Clear_altitude39() => mask1 &= ~(1 << 5);
        float _altitude40 = new();
        public bool Has_altitude40 => (mask1 & (1 << 6)) != 0;
        void Set_altitude40() => mask1 |= 1 << 6;
        public void Clear_altitude40() => mask1 &= ~(1 << 6);
        public override void Write(PB.PBWriter writer)
        {
            writer.Writedouble(9, this.latitude);
            writer.Writedouble(17, this.longitude);
            if (Has_altitude) writer.Writefloat(29, this._altitude);
            if (Has_altitude4) writer.Writefloat(37, this._altitude4);
            if (Has_altitude5) writer.Writefloat(45, this._altitude5);
            if (Has_altitude6) writer.Writefloat(53, this._altitude6);
            if (Has_altitude7) writer.Writefloat(61, this._altitude7);
            if (Has_altitude8) writer.Writefloat(69, this._altitude8);
            if (Has_altitude9) writer.Writefloat(77, this._altitude9);
            if (Has_altitude10) writer.Writefloat(85, this._altitude10);
            if (Has_altitude11) writer.Writefloat(93, this._altitude11);
            if (Has_altitude12) writer.Writefloat(101, this._altitude12);
            if (Has_altitude13) writer.Writefloat(109, this._altitude13);
            if (Has_altitude14) writer.Writefloat(117, this._altitude14);
            if (Has_altitude15) writer.Writefloat(125, this._altitude15);
            if (Has_altitude16) writer.Writefloat(133, this._altitude16);
            if (Has_altitude17) writer.Writefloat(141, this._altitude17);
            if (Has_altitude18) writer.Writefloat(149, this._altitude18);
            if (Has_altitude19) writer.Writefloat(157, this._altitude19);
            if (Has_altitude20) writer.Writefloat(165, this._altitude20);
            if (Has_altitude21) writer.Writefloat(173, this._altitude21);
            if (Has_altitude22) writer.Writefloat(181, this._altitude22);
            if (Has_altitude23) writer.Writefloat(189, this._altitude23);
            if (Has_altitude24) writer.Writefloat(197, this._altitude24);
            if (Has_altitude25) writer.Writefloat(205, this._altitude25);
            if (Has_altitude26) writer.Writefloat(213, this._altitude26);
            if (Has_altitude27) writer.Writefloat(221, this._altitude27);
            if (Has_altitude28) writer.Writefloat(229, this._altitude28);
            if (Has_altitude29) writer.Writefloat(237, this._altitude29);
            if (Has_altitude30) writer.Writefloat(245, this._altitude30);
            if (Has_altitude31) writer.Writefloat(253, this._altitude31);
            if (Has_altitude32) writer.Writefloat(261, this._altitude32);
            if (Has_altitude33) writer.Writefloat(269, this._altitude33);
            if (Has_altitude34) writer.Writefloat(277, this._altitude34);
            if (Has_altitude35) writer.Writefloat(285, this._altitude35);
            if (Has_altitude36) writer.Writefloat(293, this._altitude36);
            if (Has_altitude37) writer.Writefloat(301, this._altitude37);
            if (Has_altitude38) writer.Writefloat(309, this._altitude38);
            if (Has_altitude39) writer.Writefloat(317, this._altitude39);
            if (Has_altitude40) writer.Writefloat(325, this._altitude40);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 9: this.latitude = reader.Readdouble(); break;
                    case 17: this.longitude = reader.Readdouble(); break;
                    case 29: this.altitude = reader.Readfloat(); break;
                    case 37: this.altitude4 = reader.Readfloat(); break;
                    case 45: this.altitude5 = reader.Readfloat(); break;
                    case 53: this.altitude6 = reader.Readfloat(); break;
                    case 61: this.altitude7 = reader.Readfloat(); break;
                    case 69: this.altitude8 = reader.Readfloat(); break;
                    case 77: this.altitude9 = reader.Readfloat(); break;
                    case 85: this.altitude10 = reader.Readfloat(); break;
                    case 93: this.altitude11 = reader.Readfloat(); break;
                    case 101: this.altitude12 = reader.Readfloat(); break;
                    case 109: this.altitude13 = reader.Readfloat(); break;
                    case 117: this.altitude14 = reader.Readfloat(); break;
                    case 125: this.altitude15 = reader.Readfloat(); break;
                    case 133: this.altitude16 = reader.Readfloat(); break;
                    case 141: this.altitude17 = reader.Readfloat(); break;
                    case 149: this.altitude18 = reader.Readfloat(); break;
                    case 157: this.altitude19 = reader.Readfloat(); break;
                    case 165: this.altitude20 = reader.Readfloat(); break;
                    case 173: this.altitude21 = reader.Readfloat(); break;
                    case 181: this.altitude22 = reader.Readfloat(); break;
                    case 189: this.altitude23 = reader.Readfloat(); break;
                    case 197: this.altitude24 = reader.Readfloat(); break;
                    case 205: this.altitude25 = reader.Readfloat(); break;
                    case 213: this.altitude26 = reader.Readfloat(); break;
                    case 221: this.altitude27 = reader.Readfloat(); break;
                    case 229: this.altitude28 = reader.Readfloat(); break;
                    case 237: this.altitude29 = reader.Readfloat(); break;
                    case 245: this.altitude30 = reader.Readfloat(); break;
                    case 253: this.altitude31 = reader.Readfloat(); break;
                    case 261: this.altitude32 = reader.Readfloat(); break;
                    case 269: this.altitude33 = reader.Readfloat(); break;
                    case 277: this.altitude34 = reader.Readfloat(); break;
                    case 285: this.altitude35 = reader.Readfloat(); break;
                    case 293: this.altitude36 = reader.Readfloat(); break;
                    case 301: this.altitude37 = reader.Readfloat(); break;
                    case 309: this.altitude38 = reader.Readfloat(); break;
                    case 317: this.altitude39 = reader.Readfloat(); break;
                    case 325: this.altitude40 = reader.Readfloat(); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (GeoLocation)message;
            this.latitude = tmp.latitude;
            this.longitude = tmp.longitude;
            if (tmp.Has_altitude) this.altitude = tmp.altitude;
            if (tmp.Has_altitude4) this.altitude4 = tmp.altitude4;
            if (tmp.Has_altitude5) this.altitude5 = tmp.altitude5;
            if (tmp.Has_altitude6) this.altitude6 = tmp.altitude6;
            if (tmp.Has_altitude7) this.altitude7 = tmp.altitude7;
            if (tmp.Has_altitude8) this.altitude8 = tmp.altitude8;
            if (tmp.Has_altitude9) this.altitude9 = tmp.altitude9;
            if (tmp.Has_altitude10) this.altitude10 = tmp.altitude10;
            if (tmp.Has_altitude11) this.altitude11 = tmp.altitude11;
            if (tmp.Has_altitude12) this.altitude12 = tmp.altitude12;
            if (tmp.Has_altitude13) this.altitude13 = tmp.altitude13;
            if (tmp.Has_altitude14) this.altitude14 = tmp.altitude14;
            if (tmp.Has_altitude15) this.altitude15 = tmp.altitude15;
            if (tmp.Has_altitude16) this.altitude16 = tmp.altitude16;
            if (tmp.Has_altitude17) this.altitude17 = tmp.altitude17;
            if (tmp.Has_altitude18) this.altitude18 = tmp.altitude18;
            if (tmp.Has_altitude19) this.altitude19 = tmp.altitude19;
            if (tmp.Has_altitude20) this.altitude20 = tmp.altitude20;
            if (tmp.Has_altitude21) this.altitude21 = tmp.altitude21;
            if (tmp.Has_altitude22) this.altitude22 = tmp.altitude22;
            if (tmp.Has_altitude23) this.altitude23 = tmp.altitude23;
            if (tmp.Has_altitude24) this.altitude24 = tmp.altitude24;
            if (tmp.Has_altitude25) this.altitude25 = tmp.altitude25;
            if (tmp.Has_altitude26) this.altitude26 = tmp.altitude26;
            if (tmp.Has_altitude27) this.altitude27 = tmp.altitude27;
            if (tmp.Has_altitude28) this.altitude28 = tmp.altitude28;
            if (tmp.Has_altitude29) this.altitude29 = tmp.altitude29;
            if (tmp.Has_altitude30) this.altitude30 = tmp.altitude30;
            if (tmp.Has_altitude31) this.altitude31 = tmp.altitude31;
            if (tmp.Has_altitude32) this.altitude32 = tmp.altitude32;
            if (tmp.Has_altitude33) this.altitude33 = tmp.altitude33;
            if (tmp.Has_altitude34) this.altitude34 = tmp.altitude34;
            if (tmp.Has_altitude35) this.altitude35 = tmp.altitude35;
            if (tmp.Has_altitude36) this.altitude36 = tmp.altitude36;
            if (tmp.Has_altitude37) this.altitude37 = tmp.altitude37;
            if (tmp.Has_altitude38) this.altitude38 = tmp.altitude38;
            if (tmp.Has_altitude39) this.altitude39 = tmp.altitude39;
            if (tmp.Has_altitude40) this.altitude40 = tmp.altitude40;
        }
    }
    public partial class Address : PB.PBMessage
    {
        int mask0;
        GeoLocation _location = new();
        public bool Has_location => (mask0 & (1 << 1)) != 0;
        void Set_location() => mask0 |= 1 << 1;
        public void Clear_location() => mask0 &= ~(1 << 1);
        public override void Write(PB.PBWriter writer)
        {
            writer.Writestring(10, this.street);
            writer.Writestring(18, this.city);
            writer.Writestring(26, this.country);
            writer.Writestring(34, this.postal_code);
            if(Has_location)
                writer.Writemessage(42, this._location);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10: this.street = reader.Readstring(); break;
                    case 18: this.city = reader.Readstring(); break;
                    case 26: this.country = reader.Readstring(); break;
                    case 34: this.postal_code = reader.Readstring(); break;
                    case 42: this.location = reader.Readmessage(this.location); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (Address)message;
            this.street = tmp.street;
            this.city = tmp.city;
            this.country = tmp.country;
            this.postal_code = tmp.postal_code;
            if (tmp.Has_location) this.location = tmp.location;
        }
    }
    public partial class ComprehensiveMessage : PB.PBMessage
    {
        int mask0;
        double _optional_double_field = new();
        public bool Has_optional_double_field => (mask0 & (1 << 1)) != 0;
        void Set_optional_double_field() => mask0 |= 1 << 1;
        public void Clear_optional_double_field() => mask0 &= ~(1 << 1);
        float _optional_float_field = new();
        public bool Has_optional_float_field => (mask0 & (1 << 2)) != 0;
        void Set_optional_float_field() => mask0 |= 1 << 2;
        public void Clear_optional_float_field() => mask0 &= ~(1 << 2);
        int _optional_int32_field = new();
        public bool Has_optional_int32_field => (mask0 & (1 << 3)) != 0;
        void Set_optional_int32_field() => mask0 |= 1 << 3;
        public void Clear_optional_int32_field() => mask0 &= ~(1 << 3);
        long _optional_int64_field = new();
        public bool Has_optional_int64_field => (mask0 & (1 << 4)) != 0;
        void Set_optional_int64_field() => mask0 |= 1 << 4;
        public void Clear_optional_int64_field() => mask0 &= ~(1 << 4);
        uint _optional_uint32_field = new();
        public bool Has_optional_uint32_field => (mask0 & (1 << 5)) != 0;
        void Set_optional_uint32_field() => mask0 |= 1 << 5;
        public void Clear_optional_uint32_field() => mask0 &= ~(1 << 5);
        ulong _optional_uint64_field = new();
        public bool Has_optional_uint64_field => (mask0 & (1 << 6)) != 0;
        void Set_optional_uint64_field() => mask0 |= 1 << 6;
        public void Clear_optional_uint64_field() => mask0 &= ~(1 << 6);
        int _optional_sint32_field = new();
        public bool Has_optional_sint32_field => (mask0 & (1 << 7)) != 0;
        void Set_optional_sint32_field() => mask0 |= 1 << 7;
        public void Clear_optional_sint32_field() => mask0 &= ~(1 << 7);
        long _optional_sint64_field = new();
        public bool Has_optional_sint64_field => (mask0 & (1 << 8)) != 0;
        void Set_optional_sint64_field() => mask0 |= 1 << 8;
        public void Clear_optional_sint64_field() => mask0 &= ~(1 << 8);
        int _optional_fixed32_field = new();
        public bool Has_optional_fixed32_field => (mask0 & (1 << 9)) != 0;
        void Set_optional_fixed32_field() => mask0 |= 1 << 9;
        public void Clear_optional_fixed32_field() => mask0 &= ~(1 << 9);
        long _optional_fixed64_field = new();
        public bool Has_optional_fixed64_field => (mask0 & (1 << 10)) != 0;
        void Set_optional_fixed64_field() => mask0 |= 1 << 10;
        public void Clear_optional_fixed64_field() => mask0 &= ~(1 << 10);
        bool _optional_bool_field = new();
        public bool Has_optional_bool_field => (mask0 & (1 << 11)) != 0;
        void Set_optional_bool_field() => mask0 |= 1 << 11;
        public void Clear_optional_bool_field() => mask0 &= ~(1 << 11);
        string _optional_string_field = string.Empty;
        public bool Has_optional_string_field => (mask0 & (1 << 12)) != 0;
        void Set_optional_string_field() => mask0 |= 1 << 12;
        public void Clear_optional_string_field() => mask0 &= ~(1 << 12);
        byte[] _optional_bytes_field;
        public bool Has_optional_bytes_field => (mask0 & (1 << 13)) != 0;
        void Set_optional_bytes_field() => mask0 |= 1 << 13;
        public void Clear_optional_bytes_field() => mask0 &= ~(1 << 13);
        UserRole _optional_enum_field = new();
        public bool Has_optional_enum_field => (mask0 & (1 << 14)) != 0;
        void Set_optional_enum_field() => mask0 |= 1 << 14;
        public void Clear_optional_enum_field() => mask0 &= ~(1 << 14);
        Address _optional_address_field = new();
        public bool Has_optional_address_field => (mask0 & (1 << 15)) != 0;
        void Set_optional_address_field() => mask0 |= 1 << 15;
        public void Clear_optional_address_field() => mask0 &= ~(1 << 15);
        int2 _optional_xy = new();
        public bool Has_optional_xy => (mask0 & (1 << 16)) != 0;
        void Set_optional_xy() => mask0 |= 1 << 16;
        public void Clear_optional_xy() => mask0 &= ~(1 << 16);
        float2 _optional_f2 = new();
        public bool Has_optional_f2 => (mask0 & (1 << 17)) != 0;
        void Set_optional_f2() => mask0 |= 1 << 17;
        public void Clear_optional_f2() => mask0 &= ~(1 << 17);
        public override void Write(PB.PBWriter writer)
        {
            writer.Writedouble(9, this.double_field);
            if (Has_optional_double_field) writer.Writedouble(17, this._optional_double_field);
            writer.Writefloat(29, this.float_field);
            if (Has_optional_float_field) writer.Writefloat(37, this._optional_float_field);
            writer.Writeint32(40, this.int32_field);
            if (Has_optional_int32_field) writer.Writeint32(48, this._optional_int32_field);
            writer.Writeint64(56, this.int64_field);
            if (Has_optional_int64_field) writer.Writeint64(64, this._optional_int64_field);
            writer.Writeuint32(72, this.uint32_field);
            if (Has_optional_uint32_field) writer.Writeuint32(80, this._optional_uint32_field);
            writer.Writeuint64(88, this.uint64_field);
            if (Has_optional_uint64_field) writer.Writeuint64(96, this._optional_uint64_field);
            writer.Writesint32(104, this.sint32_field);
            if (Has_optional_sint32_field) writer.Writesint32(112, this._optional_sint32_field);
            writer.Writesint64(120, this.sint64_field);
            if (Has_optional_sint64_field) writer.Writesint64(128, this._optional_sint64_field);
            writer.Writefixed32(141, this.fixed32_field);
            if (Has_optional_fixed32_field) writer.Writefixed32(149, this._optional_fixed32_field);
            writer.Writefixed64(153, this.fixed64_field);
            if (Has_optional_fixed64_field) writer.Writefixed64(161, this._optional_fixed64_field);
            writer.Writebool(200, this.bool_field);
            if (Has_optional_bool_field) writer.Writebool(208, this._optional_bool_field);
            writer.Writestring(218, this.string_field);
            if (Has_optional_string_field) writer.Writestring(226, this._optional_string_field);
            writer.Writebytes(234, this.bytes_field);
            if (Has_optional_bytes_field) writer.Writebytes(242, this._optional_bytes_field);
            writer.Writeenum(248, this.enum_field);
            if (Has_optional_enum_field) writer.Writeenum(256, this._optional_enum_field);
            writer.Writemessage(266, this.address_field);
            if(Has_optional_address_field)
                writer.Writemessage(274, this._optional_address_field);
            writer.Writestrings(282, this.tags);
            writer.Writeint32s(290, this.scores);
            writer.Writeenums(298, this.role_history);
            writer.Writemessages(306, this.addresses);
            writer.Writebytess(314, this.binary_chunks);
            writer.Writedoubles(322, this.optional_double_list);
            if (this.string_map != null)
            {
                PB.PBWriter tmp = PB.PBBuffPool.Get();
                foreach (var item in this.string_map)
                {
                    tmp.Seek(0);
                    tmp.Writestring(10, item.Key);
                    tmp.Writestring(18, item.Value);
                    writer.WriteBuff(330, tmp);
                }
                PB.PBBuffPool.Return(tmp);
            }
            if (this.id_to_name_map != null)
            {
                PB.PBWriter tmp = PB.PBBuffPool.Get();
                foreach (var item in this.id_to_name_map)
                {
                    tmp.Seek(0);
                    tmp.Writeint32(8, item.Key);
                    tmp.Writestring(18, item.Value);
                    writer.WriteBuff(338, tmp);
                }
                PB.PBBuffPool.Return(tmp);
            }
            if (this.location_map != null)
            {
                PB.PBWriter tmp = PB.PBBuffPool.Get();
                foreach (var item in this.location_map)
                {
                    tmp.Seek(0);
                    tmp.Writestring(10, item.Key);
                    tmp.Writemessage(18, item.Value);
                    writer.WriteBuff(346, tmp);
                }
                PB.PBBuffPool.Return(tmp);
            }
            if (this.role_assignments != null)
            {
                PB.PBWriter tmp = PB.PBBuffPool.Get();
                foreach (var item in this.role_assignments)
                {
                    tmp.Seek(0);
                    tmp.Writestring(10, item.Key);
                    tmp.Writeenum(16, item.Value);
                    writer.WriteBuff(354, tmp);
                }
                PB.PBBuffPool.Return(tmp);
            }
            if (this.resource_map != null)
            {
                PB.PBWriter tmp = PB.PBBuffPool.Get();
                foreach (var item in this.resource_map)
                {
                    tmp.Seek(0);
                    tmp.Writestring(10, item.Key);
                    tmp.Writebytes(18, item.Value);
                    writer.WriteBuff(362, tmp);
                }
                PB.PBBuffPool.Return(tmp);
            }
            if (this.resource_map2 != null)
            {
                PB.PBWriter tmp = PB.PBBuffPool.Get();
                foreach (var item in this.resource_map2)
                {
                    tmp.Seek(0);
                    tmp.Writeenum(8, item.Key);
                    tmp.Writefixed32(21, item.Value);
                    writer.WriteBuff(370, tmp);
                }
                PB.PBBuffPool.Return(tmp);
            }
            writer.Writeint2(378, this.xy);
            writer.Writeint2s(386, this.lstxy);
            if (this.mapxy != null)
            {
                PB.PBWriter tmp = PB.PBBuffPool.Get();
                foreach (var item in this.mapxy)
                {
                    tmp.Seek(0);
                    tmp.Writeint32(8, item.Key);
                    tmp.Writeint2(18, item.Value);
                    writer.WriteBuff(394, tmp);
                }
                PB.PBBuffPool.Return(tmp);
            }
            if (Has_optional_xy) writer.Writeint2(402, this._optional_xy);
            writer.Writefloat2(410, this.f2);
            writer.Writefloat2s(418, this.lstf2);
            if (this.mapf2 != null)
            {
                PB.PBWriter tmp = PB.PBBuffPool.Get();
                foreach (var item in this.mapf2)
                {
                    tmp.Seek(0);
                    tmp.Writeint32(8, item.Key);
                    tmp.Writefloat2(18, item.Value);
                    writer.WriteBuff(426, tmp);
                }
                PB.PBBuffPool.Return(tmp);
            }
            if (Has_optional_f2) writer.Writefloat2(434, this._optional_f2);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 9: this.double_field = reader.Readdouble(); break;
                    case 17: this.optional_double_field = reader.Readdouble(); break;
                    case 29: this.float_field = reader.Readfloat(); break;
                    case 37: this.optional_float_field = reader.Readfloat(); break;
                    case 40: this.int32_field = reader.Readint32(); break;
                    case 48: this.optional_int32_field = reader.Readint32(); break;
                    case 56: this.int64_field = reader.Readint64(); break;
                    case 64: this.optional_int64_field = reader.Readint64(); break;
                    case 72: this.uint32_field = reader.Readuint32(); break;
                    case 80: this.optional_uint32_field = reader.Readuint32(); break;
                    case 88: this.uint64_field = reader.Readuint64(); break;
                    case 96: this.optional_uint64_field = reader.Readuint64(); break;
                    case 104: this.sint32_field = reader.Readsint32(); break;
                    case 112: this.optional_sint32_field = reader.Readsint32(); break;
                    case 120: this.sint64_field = reader.Readsint64(); break;
                    case 128: this.optional_sint64_field = reader.Readsint64(); break;
                    case 141: this.fixed32_field = reader.Readfixed32(); break;
                    case 149: this.optional_fixed32_field = reader.Readfixed32(); break;
                    case 153: this.fixed64_field = reader.Readfixed64(); break;
                    case 161: this.optional_fixed64_field = reader.Readfixed64(); break;
                    case 200: this.bool_field = reader.Readbool(); break;
                    case 208: this.optional_bool_field = reader.Readbool(); break;
                    case 218: this.string_field = reader.Readstring(); break;
                    case 226: this.optional_string_field = reader.Readstring(); break;
                    case 234: this.bytes_field = reader.Readbytes(); break;
                    case 242: this.optional_bytes_field = reader.Readbytes(); break;
                    case 248: this.enum_field = (UserRole)reader.Readint32(); break;
                    case 256: this.optional_enum_field = (UserRole)reader.Readint32(); break;
                    case 266: this.address_field = reader.Readmessage(this.address_field); break;
                    case 274: this.optional_address_field = reader.Readmessage(this.optional_address_field); break;
                    case 282: tags.Add(reader.Readstring()); break;
                    case 290: reader.Readint32s(this.scores); break;
                    case 298: reader.Readenums(this.role_history); break;
                    case 306: addresses.Add(reader.Readmessage(new Address())); break;
                    case 314: binary_chunks.Add(reader.Readbytes()); break;
                    case 322: reader.Readdoubles(this.optional_double_list); break;
                    case 330:
                        {
                            int size = reader.Readint32();
                            int max = reader.max;
                            int tag2;
                            string k = string.Empty;
                            string v = string.Empty;
                            reader.SetMax(reader.Position + size);
                            while ((tag2 = reader.ReadTag()) != 0)
                            {
                                if (tag2 == 10) k = reader.Readstring();
                                else if (tag2 == 18) v = reader.Readstring();
                            }
                            this.string_map[k] = v;
                            reader.SeekLast();
                            reader.SetMax(max);
                        }
                        break;
                    case 338:
                        {
                            int size = reader.Readint32();
                            int max = reader.max;
                            int tag2;
                            int k = new();
                            string v = string.Empty;
                            reader.SetMax(reader.Position + size);
                            while ((tag2 = reader.ReadTag()) != 0)
                            {
                                if (tag2 == 8) k = reader.Readint32();
                                else if (tag2 == 18) v = reader.Readstring();
                            }
                            this.id_to_name_map[k] = v;
                            reader.SeekLast();
                            reader.SetMax(max);
                        }
                        break;
                    case 346:
                        {
                            int size = reader.Readint32();
                            int max = reader.max;
                            int tag2;
                            string k = string.Empty;
                            Address v = new();
                            reader.SetMax(reader.Position + size);
                            while ((tag2 = reader.ReadTag()) != 0)
                            {
                                if (tag2 == 10) k = reader.Readstring();
                                else if (tag2 == 18) v = reader.Readmessage(v);
                            }
                            this.location_map[k] = v;
                            reader.SeekLast();
                            reader.SetMax(max);
                        }
                        break;
                    case 354:
                        {
                            int size = reader.Readint32();
                            int max = reader.max;
                            int tag2;
                            string k = string.Empty;
                            UserRole v = new();
                            reader.SetMax(reader.Position + size);
                            while ((tag2 = reader.ReadTag()) != 0)
                            {
                                if (tag2 == 10) k = reader.Readstring();
                                else if (tag2 == 16) v = (UserRole)reader.Readint32();
                            }
                            this.role_assignments[k] = v;
                            reader.SeekLast();
                            reader.SetMax(max);
                        }
                        break;
                    case 362:
                        {
                            int size = reader.Readint32();
                            int max = reader.max;
                            int tag2;
                            string k = string.Empty;
                            byte[] v = default;
                            reader.SetMax(reader.Position + size);
                            while ((tag2 = reader.ReadTag()) != 0)
                            {
                                if (tag2 == 10) k = reader.Readstring();
                                else if (tag2 == 18) v = reader.Readbytes();
                            }
                            this.resource_map[k] = v;
                            reader.SeekLast();
                            reader.SetMax(max);
                        }
                        break;
                    case 370:
                        {
                            int size = reader.Readint32();
                            int max = reader.max;
                            int tag2;
                            UserRole k = new();
                            int v = new();
                            reader.SetMax(reader.Position + size);
                            while ((tag2 = reader.ReadTag()) != 0)
                            {
                                if (tag2 == 8) k = (UserRole)reader.Readint32();
                                else if (tag2 == 21) v = reader.Readfixed32();
                            }
                            this.resource_map2[k] = v;
                            reader.SeekLast();
                            reader.SetMax(max);
                        }
                        break;
                    case 378: this.xy = reader.Readint2(); break;
                    case 386: lstxy.Add(reader.Readint2()); break;
                    case 394:
                        {
                            int size = reader.Readint32();
                            int max = reader.max;
                            int tag2;
                            int k = new();
                            int2 v = new();
                            reader.SetMax(reader.Position + size);
                            while ((tag2 = reader.ReadTag()) != 0)
                            {
                                if (tag2 == 8) k = reader.Readint32();
                                else if (tag2 == 18) v = reader.Readint2();
                            }
                            this.mapxy[k] = v;
                            reader.SeekLast();
                            reader.SetMax(max);
                        }
                        break;
                    case 402: this.optional_xy = reader.Readint2(); break;
                    case 410: this.f2 = reader.Readfloat2(); break;
                    case 418: lstf2.Add(reader.Readfloat2()); break;
                    case 426:
                        {
                            int size = reader.Readint32();
                            int max = reader.max;
                            int tag2;
                            int k = new();
                            float2 v = new();
                            reader.SetMax(reader.Position + size);
                            while ((tag2 = reader.ReadTag()) != 0)
                            {
                                if (tag2 == 8) k = reader.Readint32();
                                else if (tag2 == 18) v = reader.Readfloat2();
                            }
                            this.mapf2[k] = v;
                            reader.SeekLast();
                            reader.SetMax(max);
                        }
                        break;
                    case 434: this.optional_f2 = reader.Readfloat2(); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (ComprehensiveMessage)message;
            this.double_field = tmp.double_field;
            if (tmp.Has_optional_double_field) this.optional_double_field = tmp.optional_double_field;
            this.float_field = tmp.float_field;
            if (tmp.Has_optional_float_field) this.optional_float_field = tmp.optional_float_field;
            this.int32_field = tmp.int32_field;
            if (tmp.Has_optional_int32_field) this.optional_int32_field = tmp.optional_int32_field;
            this.int64_field = tmp.int64_field;
            if (tmp.Has_optional_int64_field) this.optional_int64_field = tmp.optional_int64_field;
            this.uint32_field = tmp.uint32_field;
            if (tmp.Has_optional_uint32_field) this.optional_uint32_field = tmp.optional_uint32_field;
            this.uint64_field = tmp.uint64_field;
            if (tmp.Has_optional_uint64_field) this.optional_uint64_field = tmp.optional_uint64_field;
            this.sint32_field = tmp.sint32_field;
            if (tmp.Has_optional_sint32_field) this.optional_sint32_field = tmp.optional_sint32_field;
            this.sint64_field = tmp.sint64_field;
            if (tmp.Has_optional_sint64_field) this.optional_sint64_field = tmp.optional_sint64_field;
            this.fixed32_field = tmp.fixed32_field;
            if (tmp.Has_optional_fixed32_field) this.optional_fixed32_field = tmp.optional_fixed32_field;
            this.fixed64_field = tmp.fixed64_field;
            if (tmp.Has_optional_fixed64_field) this.optional_fixed64_field = tmp.optional_fixed64_field;
            this.bool_field = tmp.bool_field;
            if (tmp.Has_optional_bool_field) this.optional_bool_field = tmp.optional_bool_field;
            this.string_field = tmp.string_field;
            if (tmp.Has_optional_string_field) this.optional_string_field = tmp.optional_string_field;
            this.bytes_field = tmp.bytes_field;
            if (tmp.Has_optional_bytes_field) this.optional_bytes_field = tmp.optional_bytes_field;
            this.enum_field = tmp.enum_field;
            if (tmp.Has_optional_enum_field) this.optional_enum_field = tmp.optional_enum_field;
            this.address_field = tmp.address_field;
            if (tmp.Has_optional_address_field) this.optional_address_field = tmp.optional_address_field;
            this.tags = tmp.tags;
            this.scores = tmp.scores;
            this.role_history = tmp.role_history;
            this.addresses = tmp.addresses;
            this.binary_chunks = tmp.binary_chunks;
            this.optional_double_list = tmp.optional_double_list;
            this.string_map = tmp.string_map;
            this.id_to_name_map = tmp.id_to_name_map;
            this.location_map = tmp.location_map;
            this.role_assignments = tmp.role_assignments;
            this.resource_map = tmp.resource_map;
            this.resource_map2 = tmp.resource_map2;
            this.xy = tmp.xy;
            this.lstxy = tmp.lstxy;
            this.mapxy = tmp.mapxy;
            if (tmp.Has_optional_xy) this.optional_xy = tmp.optional_xy;
            this.f2 = tmp.f2;
            this.lstf2 = tmp.lstf2;
            this.mapf2 = tmp.mapf2;
            if (tmp.Has_optional_f2) this.optional_f2 = tmp.optional_f2;
        }
    }
    public partial class C2S_Login : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writestring(10, this.acc);
            writer.Writestring(18, this.pw);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10: this.acc = reader.Readstring(); break;
                    case 18: this.pw = reader.Readstring(); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (C2S_Login)message;
            this.acc = tmp.acc;
            this.pw = tmp.pw;
        }
    }
    public partial class S2C_Login : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writefixed64(9, this.token);
            writer.Writestring(18, this.ip);
            writer.Writeint32(24, this.port);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 9: this.token = reader.Readfixed64(); break;
                    case 18: this.ip = reader.Readstring(); break;
                    case 24: this.port = reader.Readint32(); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (S2C_Login)message;
            this.token = tmp.token;
            this.ip = tmp.ip;
            this.port = tmp.port;
        }
    }
    public partial class C2S_LoginGame : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writefixed64(9, this.token);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 9: this.token = reader.Readfixed64(); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (C2S_LoginGame)message;
            this.token = tmp.token;
        }
    }
    public partial class S2C_LoginGame : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (S2C_LoginGame)message;
        }
    }
    public partial class RoomInfo : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writeint64(8, this.id);
            writer.Writestring(18, this.name);
            writer.Writemessages(26, this.infos);
            if (this.link != null)
            {
                PB.PBWriter tmp = PB.PBBuffPool.Get();
                foreach (var item in this.link)
                {
                    tmp.Seek(0);
                    tmp.Writeint32(8, item.Key);
                    tmp.Writemessage(18, item.Value);
                    writer.WriteBuff(34, tmp);
                }
                PB.PBBuffPool.Return(tmp);
            }
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 8: this.id = reader.Readint64(); break;
                    case 18: this.name = reader.Readstring(); break;
                    case 26: infos.Add(reader.Readmessage(new UnitInfo())); break;
                    case 34:
                        {
                            int size = reader.Readint32();
                            int max = reader.max;
                            int tag2;
                            int k = new();
                            RoomLinkItem v = new();
                            reader.SetMax(reader.Position + size);
                            while ((tag2 = reader.ReadTag()) != 0)
                            {
                                if (tag2 == 8) k = reader.Readint32();
                                else if (tag2 == 18) v = reader.Readmessage(v);
                            }
                            this.link[k] = v;
                            reader.SeekLast();
                            reader.SetMax(max);
                        }
                        break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (RoomInfo)message;
            this.id = tmp.id;
            this.name = tmp.name;
            this.infos = tmp.infos;
            this.link = tmp.link;
        }
    }
    public partial class UnitInfo : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writeint64(8, this.id);
            writer.Writestring(18, this.name);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 8: this.id = reader.Readint64(); break;
                    case 18: this.name = reader.Readstring(); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (UnitInfo)message;
            this.id = tmp.id;
            this.name = tmp.name;
        }
    }
    public partial class UnitInfo2 : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writeint64(8, this.id);
            writer.Writemessage(18, this.t);
            if (this.attribute != null)
            {
                PB.PBWriter tmp = PB.PBBuffPool.Get();
                foreach (var item in this.attribute)
                {
                    tmp.Seek(0);
                    tmp.Writeint32(8, item.Key);
                    tmp.Writeint64(16, item.Value);
                    writer.WriteBuff(26, tmp);
                }
                PB.PBBuffPool.Return(tmp);
            }
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 8: this.id = reader.Readint64(); break;
                    case 18: this.t = reader.Readmessage(this.t); break;
                    case 26:
                        {
                            int size = reader.Readint32();
                            int max = reader.max;
                            int tag2;
                            int k = new();
                            long v = new();
                            reader.SetMax(reader.Position + size);
                            while ((tag2 = reader.ReadTag()) != 0)
                            {
                                if (tag2 == 8) k = reader.Readint32();
                                else if (tag2 == 16) v = reader.Readint64();
                            }
                            this.attribute[k] = v;
                            reader.SeekLast();
                            reader.SetMax(max);
                        }
                        break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (UnitInfo2)message;
            this.id = tmp.id;
            this.t = tmp.t;
            this.attribute = tmp.attribute;
        }
    }
    public partial class UnitAttribute : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writeint32(8, this.id);
            writer.Writeint64(16, this.v);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 8: this.id = reader.Readint32(); break;
                    case 16: this.v = reader.Readint64(); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (UnitAttribute)message;
            this.id = tmp.id;
            this.v = tmp.v;
        }
    }
    public partial class RoomLinkItem : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writeint32(8, this.index);
            writer.Writeint2(18, this.xy);
            writer.Writeint32(24, this.dir);
            writer.Writeint32(32, this.link);
            writer.Writeint32(40, this.colorIndex);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 8: this.index = reader.Readint32(); break;
                    case 18: this.xy = reader.Readint2(); break;
                    case 24: this.dir = reader.Readint32(); break;
                    case 32: this.link = reader.Readint32(); break;
                    case 40: this.colorIndex = reader.Readint32(); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (RoomLinkItem)message;
            this.index = tmp.index;
            this.xy = tmp.xy;
            this.dir = tmp.dir;
            this.link = tmp.link;
            this.colorIndex = tmp.colorIndex;
        }
    }
    public partial class C2S_UDPConnect : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (C2S_UDPConnect)message;
        }
    }
    public partial class C2S_RoomList : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (C2S_RoomList)message;
        }
    }
    public partial class S2C_RoomList : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writemessages(10, this.lst);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10: lst.Add(reader.Readmessage(new RoomInfo())); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (S2C_RoomList)message;
            this.lst = tmp.lst;
        }
    }
    public partial class C2S_CreateRoom : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writestring(10, this.name);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10: this.name = reader.Readstring(); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (C2S_CreateRoom)message;
            this.name = tmp.name;
        }
    }
    public partial class S2C_CreateRoom : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writemessage(10, this.info);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10: this.info = reader.Readmessage(this.info); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (S2C_CreateRoom)message;
            this.info = tmp.info;
        }
    }
    public partial class C2S_JoinRoom : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writeint64(8, this.id);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 8: this.id = reader.Readint64(); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (C2S_JoinRoom)message;
            this.id = tmp.id;
        }
    }
    public partial class S2C_JoinRoom : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writemessage(10, this.info);
            writer.Writemessages(18, this.units);
            writer.Writeint64(24, this.myid);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10: this.info = reader.Readmessage(this.info); break;
                    case 18: units.Add(reader.Readmessage(new UnitInfo2())); break;
                    case 24: this.myid = reader.Readint64(); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (S2C_JoinRoom)message;
            this.info = tmp.info;
            this.units = tmp.units;
            this.myid = tmp.myid;
        }
    }
    public partial class S2C_PlayerJoinRoom : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writemessage(10, this.info);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10: this.info = reader.Readmessage(this.info); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (S2C_PlayerJoinRoom)message;
            this.info = tmp.info;
        }
    }
    public partial class C2S_DisRoom : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writeint64(8, this.id);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 8: this.id = reader.Readint64(); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (C2S_DisRoom)message;
            this.id = tmp.id;
        }
    }
    public partial class S2C_DisRoom : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writeint64(8, this.id);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 8: this.id = reader.Readint64(); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (S2C_DisRoom)message;
            this.id = tmp.id;
        }
    }
    public partial class C2S_PlayerQuit : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (C2S_PlayerQuit)message;
        }
    }
    public partial class S2C_PlayerQuit : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writeint64(8, this.id);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 8: this.id = reader.Readint64(); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
        public override void Merge(PB.PBMessage message)
        {
            var tmp = (S2C_PlayerQuit)message;
            this.id = tmp.id;
        }
    }
}
