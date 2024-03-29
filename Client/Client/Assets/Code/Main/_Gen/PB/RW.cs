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
                    case 10:
                        this.dir = reader.Readfloat2();
                        break;
                    default:
                        reader.SeekNext(tag);
                        break;
                }
            }
        }
    }
    public partial class S2C_SyncTransform : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writefloat3(10, this.p);
            writer.Writefloat4(18, this.r);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10:
                        this.p = reader.Readfloat3();
                        break;
                    case 18:
                        this.r = reader.Readfloat4();
                        break;
                    default:
                        reader.SeekNext(tag);
                        break;
                }
            }
        }
    }
}
namespace main
{
    public partial class TestPBmain : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            if (this.test)
            {
                writer.WriteTag(8);
                writer.Writebool(this.test);
            }
            if (this.test2 != 0)
            {
                writer.WriteTag(16);
                writer.Writeint32(this.test2);
            }
            if (this.test3 != 0)
            {
                writer.WriteTag(24);
                writer.Writesint32(this.test3);
            }
            if (this.test4 != 0)
            {
                writer.WriteTag(32);
                writer.Writeint64(this.test4);
            }
            if (this.test5 != 0)
            {
                writer.WriteTag(40);
                writer.Writesint64(this.test5);
            }
            if (this.test6 != 0F)
            {
                writer.WriteTag(53);
                writer.Writefloat(this.test6);
            }
            writer.Writestring(58, this.test7);
            writer.Writemessage(66, this.test8);
            if (this.test9 != null)
            {
                PB.PBWriter tmp = PB.PBBuffPool.Get();
                foreach (var item in this.test9)
                {
                    tmp.Seek(0);
                    tmp.Writestring(10, item.Key);
                    tmp.Writemessage(18, item.Value);
                    writer.WriteBuff(74, tmp);
                }
                PB.PBBuffPool.Return(tmp);
            }
            if (this.test10 != null)
            {
                PB.PBWriter tmp = PB.PBBuffPool.Get();
                foreach (var item in this.test10)
                {
                    tmp.Seek(0);
                    tmp.WriteTag(8);
                    tmp.Writeint64(item.Key);
                    tmp.WriteTag(16);
                    tmp.Writebool(item.Value);
                    writer.WriteBuff(82, tmp);
                }
                PB.PBBuffPool.Return(tmp);
            }
            if (this.test11 != null)
            {
                PB.PBWriter tmp = PB.PBBuffPool.Get();
                foreach (var item in this.test11)
                {
                    tmp.Seek(0);
                    tmp.WriteTag(8);
                    tmp.Writesint32(item.Key);
                    tmp.WriteTag(16);
                    tmp.Writesint64(item.Value);
                    writer.WriteBuff(90, tmp);
                }
                PB.PBBuffPool.Return(tmp);
            }
            writer.Writebytes(98, this.test12);
            if (this.test14 != 0)
            {
                writer.WriteTag(117);
                writer.Writefixed32(this.test14);
            }
            if (this.test15 != 0)
            {
                writer.WriteTag(125);
                writer.Writesfixed32(this.test15);
            }
            if (this.test16 != 0)
            {
                writer.WriteTag(129);
                writer.Writefixed64(this.test16);
            }
            if (this.test17 != 0)
            {
                writer.WriteTag(137);
                writer.Writesfixed64(this.test17);
            }
            if (this.test18 != 0D)
            {
                writer.WriteTag(145);
                writer.Writedouble(this.test18);
            }
            if (this.test19 != null)
            {
                PB.PBWriter tmp = PB.PBBuffPool.Get();
                foreach (var item in this.test19)
                {
                    tmp.Seek(0);
                    tmp.WriteTag(13);
                    tmp.Writefixed32(item.Key);
                    tmp.WriteTag(17);
                    tmp.Writedouble(item.Value);
                    writer.WriteBuff(154, tmp);
                }
                PB.PBBuffPool.Return(tmp);
            }
            if (this.test20 != null)
            {
                PB.PBWriter tmp = PB.PBBuffPool.Get();
                foreach (var item in this.test20)
                {
                    tmp.Seek(0);
                    tmp.WriteTag(9);
                    tmp.Writesfixed64(item.Key);
                    tmp.Writestring(18, item.Value);
                    writer.WriteBuff(162, tmp);
                }
                PB.PBBuffPool.Return(tmp);
            }
            writer.Writebools(170, this.test21);
            writer.Writeint32s(178, this.test22);
            writer.Writesint32s(186, this.test23);
            writer.Writeint64s(194, this.test24);
            writer.Writesint64s(202, this.test25);
            writer.Writefloats(210, this.test26);
            writer.Writestrings(218, this.test27);
            if (this.test28 != null)
            {
                int len = this.test28.Count;
                for (int i = 0; i < len; i++)
                    writer.Writemessage(226, this.test28[i]);
            }
            writer.Writefixed32s(234, this.test29);
            writer.Writesfixed32s(242, this.test30);
            writer.Writefixed64s(250, this.test31);
            writer.Writesfixed64s(258, this.test32);
            writer.Writedoubles(266, this.test33);
            writer.Writefloat2(274, this.test34);
            writer.Writefloat3(282, this.test35);
            writer.Writefloat4(290, this.test36);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 8:
                        this.test = reader.Readbool();
                        break;
                    case 16:
                        this.test2 = reader.Readint32();
                        break;
                    case 24:
                        this.test3 = reader.Readsint32();
                        break;
                    case 32:
                        this.test4 = reader.Readint64();
                        break;
                    case 40:
                        this.test5 = reader.Readsint64();
                        break;
                    case 53:
                        this.test6 = reader.Readfloat();
                        break;
                    case 58:
                        this.test7 = reader.Readstring();
                        break;
                    case 66:
                        reader.Readmessage(this.test8);
                        break;
                    case 74:
                        {
                            int size = reader.Readint32();
                            int max = reader.max;
                            int tag2;
                            string k = string.Empty;
                            TestPB2 v = new TestPB2();
                            reader.SetMax(reader.Position + size);
                            while ((tag2 = reader.ReadTag()) != 0)
                            {
                                if (tag2 == 10) k = reader.Readstring();
                                else if (tag2 == 18) reader.Readmessage(v);
                                else break;
                            }
                            this.test9[k] = v;
                            reader.SeekLast();
                            reader.SetMax(max);
                        }
                        break;
                    case 82:
                        {
                            int size = reader.Readint32();
                            int max = reader.max;
                            int tag2;
                            long k = default;
                            bool v = default;
                            reader.SetMax(reader.Position + size);
                            while ((tag2 = reader.ReadTag()) != 0)
                            {
                                if (tag2 == 8) k = reader.Readint64();
                                else if (tag2 == 16) v = reader.Readbool();
                                else break;
                            }
                            this.test10[k] = v;
                            reader.SeekLast();
                            reader.SetMax(max);
                        }
                        break;
                    case 90:
                        {
                            int size = reader.Readint32();
                            int max = reader.max;
                            int tag2;
                            int k = default;
                            long v = default;
                            reader.SetMax(reader.Position + size);
                            while ((tag2 = reader.ReadTag()) != 0)
                            {
                                if (tag2 == 8) k = reader.Readsint32();
                                else if (tag2 == 16) v = reader.Readsint64();
                                else break;
                            }
                            this.test11[k] = v;
                            reader.SeekLast();
                            reader.SetMax(max);
                        }
                        break;
                    case 98:
                        this.test12 = reader.Readbytes();
                        break;
                    case 117:
                        this.test14 = reader.Readfixed32();
                        break;
                    case 125:
                        this.test15 = reader.Readsfixed32();
                        break;
                    case 129:
                        this.test16 = reader.Readfixed64();
                        break;
                    case 137:
                        this.test17 = reader.Readsfixed64();
                        break;
                    case 145:
                        this.test18 = reader.Readdouble();
                        break;
                    case 154:
                        {
                            int size = reader.Readint32();
                            int max = reader.max;
                            int tag2;
                            uint k = default;
                            double v = default;
                            reader.SetMax(reader.Position + size);
                            while ((tag2 = reader.ReadTag()) != 0)
                            {
                                if (tag2 == 13) k = reader.Readfixed32();
                                else if (tag2 == 21) v = reader.Readdouble();
                                else break;
                            }
                            this.test19[k] = v;
                            reader.SeekLast();
                            reader.SetMax(max);
                        }
                        break;
                    case 162:
                        {
                            int size = reader.Readint32();
                            int max = reader.max;
                            int tag2;
                            long k = default;
                            string v = string.Empty;
                            reader.SetMax(reader.Position + size);
                            while ((tag2 = reader.ReadTag()) != 0)
                            {
                                if (tag2 == 9) k = reader.Readsfixed64();
                                else if (tag2 == 17) v = reader.Readstring();
                                else break;
                            }
                            this.test20[k] = v;
                            reader.SeekLast();
                            reader.SetMax(max);
                        }
                        break;
                    case 170:
                        reader.Readbools(this.test21);
                        break;
                    case 178:
                        reader.Readint32s(this.test22);
                        break;
                    case 186:
                        reader.Readsint32s(this.test23);
                        break;
                    case 194:
                        reader.Readint64s(this.test24);
                        break;
                    case 202:
                        reader.Readsint64s(this.test25);
                        break;
                    case 210:
                        reader.Readfloats(this.test26);
                        break;
                    case 218:
                        reader.Readstrings(tag, this.test27);
                        break;
                    case 226:
                        {
                            int size = reader.Readint32();
                            int max = reader.max;
                            reader.SetMax(reader.Position + size);
                            TestPB2 message = new TestPB2();
                            message.Read(reader);
                            this.test28.Add(message);
                            reader.SeekLast();
                            reader.SetMax(max);
                        }
                        break;
                    case 234:
                        reader.Readfixed32s(this.test29);
                        break;
                    case 242:
                        reader.Readsfixed32s(this.test30);
                        break;
                    case 250:
                        reader.Readfixed64s(this.test31);
                        break;
                    case 258:
                        reader.Readsfixed64s(this.test32);
                        break;
                    case 266:
                        reader.Readdoubles(this.test33);
                        break;
                    case 274:
                        this.test34 = reader.Readfloat2();
                        break;
                    case 282:
                        this.test35 = reader.Readfloat3();
                        break;
                    case 290:
                        this.test36 = reader.Readfloat4();
                        break;
                    default:
                        reader.SeekNext(tag);
                        break;
                }
            }
        }
    }
    public partial class TestPB2 : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            if (this.test2 != 0)
            {
                writer.WriteTag(16);
                writer.Writeint32(this.test2);
            }
            writer.Writeint32s(178, this.test22);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 16:
                        this.test2 = reader.Readint32();
                        break;
                    case 178:
                        reader.Readint32s(this.test22);
                        break;
                    default:
                        reader.SeekNext(tag);
                        break;
                }
            }
        }
    }
    public partial class RoomInfo : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            if (this.id != 0)
            {
                writer.WriteTag(8);
                writer.Writeint64(this.id);
            }
            writer.Writestring(18, this.name);
            if (this.infos != null)
            {
                int len = this.infos.Count;
                for (int i = 0; i < len; i++)
                    writer.Writemessage(26, this.infos[i]);
            }
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 8:
                        this.id = reader.Readint64();
                        break;
                    case 18:
                        this.name = reader.Readstring();
                        break;
                    case 26:
                        {
                            int size = reader.Readint32();
                            int max = reader.max;
                            reader.SetMax(reader.Position + size);
                            UnitInfo message = new UnitInfo();
                            message.Read(reader);
                            this.infos.Add(message);
                            reader.SeekLast();
                            reader.SetMax(max);
                        }
                        break;
                    default:
                        reader.SeekNext(tag);
                        break;
                }
            }
        }
    }
    public partial class UnitInfo : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            if (this.id != 0)
            {
                writer.WriteTag(8);
                writer.Writeint64(this.id);
            }
            writer.Writestring(18, this.name);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 8:
                        this.id = reader.Readint64();
                        break;
                    case 18:
                        this.name = reader.Readstring();
                        break;
                    default:
                        reader.SeekNext(tag);
                        break;
                }
            }
        }
    }
    public partial class UnitInfo2 : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            if (this.id != 0)
            {
                writer.WriteTag(8);
                writer.Writeint64(this.id);
            }
            writer.Writemessage(18, this.t);
            if (this.attribute != null)
            {
                PB.PBWriter tmp = PB.PBBuffPool.Get();
                foreach (var item in this.attribute)
                {
                    tmp.Seek(0);
                    tmp.WriteTag(8);
                    tmp.Writeint32(item.Key);
                    tmp.WriteTag(16);
                    tmp.Writeint64(item.Value);
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
                    case 8:
                        this.id = reader.Readint64();
                        break;
                    case 18:
                        reader.Readmessage(this.t);
                        break;
                    case 26:
                        {
                            int size = reader.Readint32();
                            int max = reader.max;
                            int tag2;
                            int k = default;
                            long v = default;
                            reader.SetMax(reader.Position + size);
                            while ((tag2 = reader.ReadTag()) != 0)
                            {
                                if (tag2 == 8) k = reader.Readint32();
                                else if (tag2 == 16) v = reader.Readint64();
                                else break;
                            }
                            this.attribute[k] = v;
                            reader.SeekLast();
                            reader.SetMax(max);
                        }
                        break;
                    default:
                        reader.SeekNext(tag);
                        break;
                }
            }
        }
    }
    public partial class UnitAttribute : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            if (this.id != 0)
            {
                writer.WriteTag(8);
                writer.Writeint32(this.id);
            }
            if (this.v != 0)
            {
                writer.WriteTag(16);
                writer.Writeint64(this.v);
            }
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 8:
                        this.id = reader.Readint32();
                        break;
                    case 16:
                        this.v = reader.Readint64();
                        break;
                    default:
                        reader.SeekNext(tag);
                        break;
                }
            }
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
                    default:
                        reader.SeekNext(tag);
                        break;
                }
            }
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
                    default:
                        reader.SeekNext(tag);
                        break;
                }
            }
        }
    }
    public partial class S2C_RoomList : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            if (this.lst != null)
            {
                int len = this.lst.Count;
                for (int i = 0; i < len; i++)
                    writer.Writemessage(10, this.lst[i]);
            }
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10:
                        {
                            int size = reader.Readint32();
                            int max = reader.max;
                            reader.SetMax(reader.Position + size);
                            RoomInfo message = new RoomInfo();
                            message.Read(reader);
                            this.lst.Add(message);
                            reader.SeekLast();
                            reader.SetMax(max);
                        }
                        break;
                    default:
                        reader.SeekNext(tag);
                        break;
                }
            }
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
                    case 10:
                        this.name = reader.Readstring();
                        break;
                    default:
                        reader.SeekNext(tag);
                        break;
                }
            }
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
                    case 10:
                        reader.Readmessage(this.info);
                        break;
                    default:
                        reader.SeekNext(tag);
                        break;
                }
            }
        }
    }
    public partial class C2S_JoinRoom : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            if (this.id != 0)
            {
                writer.WriteTag(8);
                writer.Writeint64(this.id);
            }
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 8:
                        this.id = reader.Readint64();
                        break;
                    default:
                        reader.SeekNext(tag);
                        break;
                }
            }
        }
    }
    public partial class S2C_JoinRoom : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writemessage(10, this.info);
            if (this.units != null)
            {
                int len = this.units.Count;
                for (int i = 0; i < len; i++)
                    writer.Writemessage(18, this.units[i]);
            }
            if (this.myid != 0)
            {
                writer.WriteTag(24);
                writer.Writeint64(this.myid);
            }
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10:
                        reader.Readmessage(this.info);
                        break;
                    case 18:
                        {
                            int size = reader.Readint32();
                            int max = reader.max;
                            reader.SetMax(reader.Position + size);
                            UnitInfo2 message = new UnitInfo2();
                            message.Read(reader);
                            this.units.Add(message);
                            reader.SeekLast();
                            reader.SetMax(max);
                        }
                        break;
                    case 24:
                        this.myid = reader.Readint64();
                        break;
                    default:
                        reader.SeekNext(tag);
                        break;
                }
            }
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
                    case 10:
                        reader.Readmessage(this.info);
                        break;
                    default:
                        reader.SeekNext(tag);
                        break;
                }
            }
        }
    }
    public partial class C2S_DisRoom : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            if (this.id != 0)
            {
                writer.WriteTag(8);
                writer.Writeint64(this.id);
            }
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 8:
                        this.id = reader.Readint64();
                        break;
                    default:
                        reader.SeekNext(tag);
                        break;
                }
            }
        }
    }
    public partial class S2C_DisRoom : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            if (this.id != 0)
            {
                writer.WriteTag(8);
                writer.Writeint64(this.id);
            }
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 8:
                        this.id = reader.Readint64();
                        break;
                    default:
                        reader.SeekNext(tag);
                        break;
                }
            }
        }
    }
}
