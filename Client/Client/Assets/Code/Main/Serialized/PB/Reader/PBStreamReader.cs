using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PB
{
    public unsafe class PBStreamReader : PBReader
    {
        public PBStreamReader(Stream s, int index, int length) : base(index, length)
        {
            stream = s;
        }
        public PBStreamReader(Stream s) : base(0, (int)s.Length)
        {
            stream = s;
        }

        Stream stream;
        byte[] buff;

        public override int Position { get => (int)stream.Position; protected set => this.Seek(value); }

        public override bool Readbool()
        {
            return stream.ReadByte() == 1;
        }

        public override byte[] Readbytes()
        {
            int len = Readint32();
            byte[] bs = new byte[len];
            stream.Read(bs, 0, len);
            return bs;
        }

        public override double Readdouble()
        {
            ulong v = Readfixed64();
            return *(double*)&v;
        }

        public override uint Readfixed32()
        {
            uint v = 0;
            for (int i = 0; i < sizeof(uint); i++)
                v |= (uint)stream.ReadByte() << (i * 8);
            return v;
        }

        public override ulong Readfixed64()
        {
            ulong v = 0;
            for (int i = 0; i < sizeof(ulong); i++)
                v |= (ulong)stream.ReadByte() << (i * 8);
            return v;
        }

        public override float Readfloat()
        {
            uint v = Readfixed32();
            return *(float*)&v;
        }

        public override int Readint32()
        {
            return (int)Readint64();
        }

        public override long Readint64()
        {
            ulong v = 0;
            for (int i = 0; i < sizeof(ulong) + 2; i++)
            {
                ulong bv = (ulong)stream.ReadByte();
                v |= ((bv & 0x7f) << 7 * i);
                if (bv < 128)
                    break;
            }
            return (long)v;
        }

        public override string Readstring()
        {
            int len = Readint32();
            if (len <= 0)
                return string.Empty;
            if (buff == null || buff.Length < len)
                buff = new byte[len];
            stream.Read(buff, 0, len);
            string s = Encoding.UTF8.GetString(buff, 0, len);
            return s;
        }

        public override void Seek(int index)
        {
            if (index < min)
                index = min;
            else if (index > max)
                index = max;
            stream.Seek(index, SeekOrigin.Begin);
        }
    }
}
