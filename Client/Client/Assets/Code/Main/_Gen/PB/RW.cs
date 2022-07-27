using System.Collections.Generic;

namespace PB.main
{
    public partial class TestPBmain : IPBMessage
    {
        public void Write(PBWriter writer)
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
                PBBytesWriter tmp = PBBuffPool.Get();
                foreach (var item in this.test9)
                {
                    tmp.Seek(0);
                    tmp.Writestring(10, item.Key);
                    tmp.Writemessage(18, item.Value);
                    writer.Writebytes(74, tmp.GetNativeBytes(), 0, tmp.Position);
                }
                PBBuffPool.Return(tmp);
            }
            if (this.test10 != null)
            {
                PBBytesWriter tmp = PBBuffPool.Get();
                foreach (var item in this.test10)
                {
                    tmp.Seek(0);
                    tmp.WriteTag(8);
                    tmp.Writeint64(item.Key);
                    tmp.WriteTag(16);
                    tmp.Writebool(item.Value);
                    writer.Writebytes(82, tmp.GetNativeBytes(), 0, tmp.Position);
                }
                PBBuffPool.Return(tmp);
            }
            if (this.test11 != null)
            {
                PBBytesWriter tmp = PBBuffPool.Get();
                foreach (var item in this.test11)
                {
                    tmp.Seek(0);
                    tmp.WriteTag(8);
                    tmp.Writesint32(item.Key);
                    tmp.WriteTag(16);
                    tmp.Writesint64(item.Value);
                    writer.Writebytes(90, tmp.GetNativeBytes(), 0, tmp.Position);
                }
                PBBuffPool.Return(tmp);
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
                PBBytesWriter tmp = PBBuffPool.Get();
                foreach (var item in this.test19)
                {
                    tmp.Seek(0);
                    tmp.WriteTag(13);
                    tmp.Writefixed32(item.Key);
                    tmp.WriteTag(17);
                    tmp.Writedouble(item.Value);
                    writer.Writebytes(154, tmp.GetNativeBytes(), 0, tmp.Position);
                }
                PBBuffPool.Return(tmp);
            }
            if (this.test20 != null)
            {
                PBBytesWriter tmp = PBBuffPool.Get();
                foreach (var item in this.test20)
                {
                    tmp.Seek(0);
                    tmp.WriteTag(9);
                    tmp.Writesfixed64(item.Key);
                    tmp.Writestring(18, item.Value);
                    writer.Writebytes(162, tmp.GetNativeBytes(), 0, tmp.Position);
                }
                PBBuffPool.Return(tmp);
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
        }
        public void Read(PBReader reader)
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
                            int point;
                            int max = reader.max;
                            do
                            {
                                int tag2;
                                string k = string.Empty;
                                TestPB2 v = new TestPB2();
                                int size = reader.Readint32();
                                point = reader.Position;
                                reader.SetLimit(point, point + size);
                                while ((tag2 = reader.ReadTag()) != 0)
                                {
                                    if (tag2 == 10)
                                        k = reader.Readstring();
                                    else if (tag2 == 18)
                                        reader.Readmessage(v);
                                    else
                                        break;
                                }
                                point += size;
                                reader.SetLimit(point, max);
                                reader.Seek(point);
                                this.test9[k] = v;
                            } while (reader.ReadTag() == 74);
                            reader.Seek(point);
                        }
                        break;
                    case 82:
                        {
                            int point;
                            int max = reader.max;
                            do
                            {
                                int tag2;
                                long k = 0;
                                bool v = false;
                                int size = reader.Readint32();
                                point = reader.Position;
                                reader.SetLimit(point, point + size);
                                while ((tag2 = reader.ReadTag()) != 0)
                                {
                                    if (tag2 == 8)
                                        k = reader.Readint64();
                                    else if (tag2 == 16)
                                        v = reader.Readbool();
                                    else
                                        break;
                                }
                                point += size;
                                reader.SetLimit(point, max);
                                reader.Seek(point);
                                this.test10[k] = v;
                            } while (reader.ReadTag() == 82);
                            reader.Seek(point);
                        }
                        break;
                    case 90:
                        {
                            int point;
                            int max = reader.max;
                            do
                            {
                                int tag2;
                                int k = 0;
                                long v = 0;
                                int size = reader.Readint32();
                                point = reader.Position;
                                reader.SetLimit(point, point + size);
                                while ((tag2 = reader.ReadTag()) != 0)
                                {
                                    if (tag2 == 8)
                                        k = reader.Readsint32();
                                    else if (tag2 == 16)
                                        v = reader.Readsint64();
                                    else
                                        break;
                                }
                                point += size;
                                reader.SetLimit(point, max);
                                reader.Seek(point);
                                this.test11[k] = v;
                            } while (reader.ReadTag() == 90);
                            reader.Seek(point);
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
                            int point;
                            int max = reader.max;
                            do
                            {
                                int tag2;
                                uint k = 0;
                                double v = 0;
                                int size = reader.Readint32();
                                point = reader.Position;
                                reader.SetLimit(point, point + size);
                                while ((tag2 = reader.ReadTag()) != 0)
                                {
                                    if (tag2 == 13)
                                        k = reader.Readfixed32();
                                    else if (tag2 == 21)
                                        v = reader.Readdouble();
                                    else
                                        break;
                                }
                                point += size;
                                reader.SetLimit(point, max);
                                reader.Seek(point);
                                this.test19[k] = v;
                            } while (reader.ReadTag() == 154);
                            reader.Seek(point);
                        }
                        break;
                    case 162:
                        {
                            int point;
                            int max = reader.max;
                            do
                            {
                                int tag2;
                                long k = 0;
                                string v = string.Empty;
                                int size = reader.Readint32();
                                point = reader.Position;
                                reader.SetLimit(point, point + size);
                                while ((tag2 = reader.ReadTag()) != 0)
                                {
                                    if (tag2 == 9)
                                        k = reader.Readsfixed64();
                                    else if (tag2 == 17)
                                        v = reader.Readstring();
                                    else
                                        break;
                                }
                                point += size;
                                reader.SetLimit(point, max);
                                reader.Seek(point);
                                this.test20[k] = v;
                            } while (reader.ReadTag() == 162);
                            reader.Seek(point);
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
                            int point;
                            int max = reader.max;
                            do
                            {
                                int size = reader.Readint32();
                                point = reader.Position;
                                reader.SetLimit(point, point + size);
                                TestPB2 message = new TestPB2();
                                message.Read(reader);
                                this.test28.Add(message);
                                reader.SetLimit(point += size, max);
                                reader.Seek(point);
                            } while (reader.ReadTag() == 226);
                            reader.Seek(point);
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
                    default:
                        reader.SeekNext(tag);
                        break;
                }
            }
        }
    }
    public partial class TestPB2 : IPBMessage
    {
        public void Write(PBWriter writer)
        {
            if (this.test2 != 0)
            {
                writer.WriteTag(16);
                writer.Writeint32(this.test2);
            }
            writer.Writeint32s(178, this.test22);
        }
        public void Read(PBReader reader)
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
}
