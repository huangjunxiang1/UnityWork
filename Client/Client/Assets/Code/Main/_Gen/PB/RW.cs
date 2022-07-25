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
