using System.Collections.Generic;
using Game;
using Unity.Mathematics;

namespace hot
{
    public partial class TestPBhot : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            if (this.test != 0)
            {
                writer.WriteTag(8);
                writer.Writesint32(this.test);
            }
            if (this.test2 != 0)
            {
                writer.WriteTag(16);
                writer.Writeint32(this.test2);
            }
            writer.Writestring(26, this.test3);
            if (this.test4)
            {
                writer.WriteTag(32);
                writer.Writebool(this.test4);
            }
            if (this.test5 != 0F)
            {
                writer.WriteTag(45);
                writer.Writefloat(this.test5);
            }
            if (this.test7 != 0)
            {
                writer.WriteTag(56);
                writer.Writeint64(this.test7);
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
                        this.test = reader.Readsint32();
                        break;
                    case 16:
                        this.test2 = reader.Readint32();
                        break;
                    case 26:
                        this.test3 = reader.Readstring();
                        break;
                    case 32:
                        this.test4 = reader.Readbool();
                        break;
                    case 45:
                        this.test5 = reader.Readfloat();
                        break;
                    case 56:
                        this.test7 = reader.Readint64();
                        break;
                    default:
                        reader.SeekNext(tag);
                        break;
                }
            }
        }
    }
}
