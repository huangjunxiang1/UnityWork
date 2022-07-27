using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PB
{
    public unsafe class PBStreamWriter : PBWriter
    {
        public PBStreamWriter(Stream s)
        {
            stream = s;
        }

        Stream stream;
        byte[] buff;

        public override int Position { get => (int)stream.Position; protected set => this.Seek(value); }

        public override void Seek(int index)
        {
            stream.Seek(index, SeekOrigin.Begin);
        }

        public override void Writebool(bool v)
        {
            stream.WriteByte(v ? (byte)1 : (byte)0);
        }

        public override void Writebytes(int tag, byte[] v, int index, int length)
        {
            if (index < 0 || length < 0 || index + length > v.Length)
                throw new ArgumentOutOfRangeException();

            if (length == 0)
            {
                WriteTag(tag);
                Writeint32(0);
                return;
            }
            WriteTag(tag);
            Writeint32(length);
            stream.Write(v, index, length);
        }

        public override void Writedouble(double v)
        {
            ulong uv = *(ulong*)&v;
            Writefixed64(uv);
        }

        public override void Writefixed32(uint v)
        {
            for (int i = 0; i < sizeof(uint); i++)
                stream.WriteByte((byte)(v >> (8 * i)));
        }

        public override void Writefixed64(ulong v)
        {
            for (int i = 0; i < sizeof(ulong); i++)
                stream.WriteByte((byte)(v >> (8 * i)));
        }

        public override void Writefloat(float v)
        {
            uint uv = *(uint*)&v;
            Writefixed32(uv);
        }

        public override void Writeint32(int v)
        {
            Writeint64(v);
        }

        public override void Writeint64(long v)
        {
            ulong uv = (ulong)v;
            while (uv > 127)
            {
                stream.WriteByte((byte)(uv | 0x80));
                uv >>= 7;
            }
            stream.WriteByte((byte)uv);
        }

        public override void Writestring(int tag, string v)
        {
            if (string.IsNullOrEmpty(v))
                return;

            int len = Encoding.UTF8.GetByteCount(v);
            WriteTag(tag);
            Writeint32(len);
            if (buff == null || buff.Length < len)
                buff = new byte[len];
            Encoding.UTF8.GetBytes(v, 0, v.Length, buff, 0);
            stream.Write(buff, 0, len);
        }
    }
}
