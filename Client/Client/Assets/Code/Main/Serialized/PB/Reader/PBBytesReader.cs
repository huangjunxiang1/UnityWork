using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB
{
    public unsafe class PBBytesReader : PBReader
    {
        public PBBytesReader(byte[] bytes, int index, int length) : base(index, length)
        {
            this.bytes = bytes;
        }
        public PBBytesReader(byte[] bytes) : base(0, bytes.Length)
        {
            this.bytes = bytes;
        }

        byte[] bytes;
        int point;

        public override int Position { get => point; protected set => point = value; }

        public override bool Readbool()
        {
            return bytes[point++] == 1;
        }

        public override byte[] Readbytes()
        {
            int len = Readint32();
            byte[] bs = new byte[len];
            Array.Copy(bytes, point, bs, 0, len);
            point += len;
            return bs;
        }

        public override float Readfloat()
        {
            fixed (byte* ptr = &bytes[point])
            {
                point += sizeof(int);
                return *(float*)ptr;
            }
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
                ulong bv = bytes[point++];
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
            string s = Encoding.UTF8.GetString(bytes, point, len);
            point += len;
            return s;
        }

        public override void Seek(int index)
        {
            if (index < min)
                index = min;
            else if (index > max)
                index = max;
            point = index;
        }
    }
}
