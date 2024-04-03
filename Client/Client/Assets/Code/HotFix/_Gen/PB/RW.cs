using System.Collections.Generic;
using Game;
using Unity.Mathematics;

namespace hot
{
    public partial class TestPBhot : PB.PBMessage
    {
        public override void Write(PB.PBWriter writer)
        {
            writer.Writesint32(8, this.test);
            writer.Writeint32(16, this.test2);
            writer.Writestring(26, this.test3);
            writer.Writebool(32, this.test4);
            writer.Writefloat(45, this.test5);
            writer.Writeint64(56, this.test7);
        }
        public override void Read(PB.PBReader reader)
        {
            int tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 8: this.test = reader.Readsint32(); break;
                    case 16: this.test2 = reader.Readint32(); break;
                    case 26: this.test3 = reader.Readstring(); break;
                    case 32: this.test4 = reader.Readbool(); break;
                    case 45: this.test5 = reader.Readfloat(); break;
                    case 56: this.test7 = reader.Readint64(); break;
                    default: reader.SeekNext(tag); break;
                }
            }
        }
    }
}
