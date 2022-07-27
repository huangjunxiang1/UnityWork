using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB
{
    public unsafe class PBBytesWriter : PBWriter
    {
        public PBBytesWriter(byte[] bytes)
        {
            this.bytes = bytes;
        }

        byte[] bytes;
        int point;

        public override int Position { get => point; protected set => point = value; }

        public override void Writebool(bool v)
        {
            checkLength(sizeof(byte));
            bytes[point++] = v ? (byte)1 : (byte)0;
        }

        public override void Writeint32(int v)
        {
            Writeint64(v);
        }

        public override void Writeint64(long v)
        {
            checkLength(sizeof(long) + 2);
            ulong uv = (ulong)v;
            while (uv > 127)
            {
                bytes[point++] = (byte)(uv | 0x80);
                uv >>= 7;
            }
            bytes[point++] = (byte)uv;
        }

        public override void Writefixed32(uint v)
        {
            checkLength(sizeof(uint));
            fixed (byte* ptr = &bytes[point])
                *(uint*)ptr = v;
            point += sizeof(uint);
        }

        public override void Writefixed64(ulong v)
        {
            checkLength(sizeof(ulong));
            fixed (byte* ptr = &bytes[point])
                *(ulong*)ptr = v;
            point += sizeof(ulong);
        }

        public override void Writedouble(double v)
        {
            checkLength(sizeof(double));
            fixed (byte* ptr = &bytes[point])
                *(double*)ptr = v;
            point += sizeof(double);
        }
        public override void Writefloat(float v)
        {
            checkLength(sizeof(int));
            fixed (byte* ptr = &bytes[point])
                *(int*)ptr = *(int*)(&v);
            point += sizeof(int);
        }

        public override void Writestring(int tag, string v)
        {
            if (string.IsNullOrEmpty(v))
                return;

            int len = Encoding.UTF8.GetByteCount(v);
            checkLength((sizeof(int) + 1) * 2 + len);
            WriteTag(tag);
            Writeint32(len);
            Encoding.UTF8.GetBytes(v, 0, v.Length, bytes, point);
            point += len;
        }

        public override void Writebytes(int tag, byte[] v, int index, int length)
        {
            if (index < 0 || length < 0 || index + length > v.Length)
                throw new ArgumentOutOfRangeException();
            
            if (length == 0)
            {
                checkLength((sizeof(int) + 1) * 2);
                WriteTag(tag);
                Writeint32(0);
                return;
            }
            checkLength((sizeof(int) + 1) * 2 + length);
            WriteTag(tag);
            Writeint32(length);
            Array.Copy(v, index, bytes, point, length);
            point += length;
        }

        public override void Seek(int index)
        {
            this.point = index;
        }
        public byte[] GetNativeBytes()
        {
            return bytes;
        }

        public void ReSize(int newSize)
        {
            if (newSize <= bytes.Length)
                throw new ArgumentOutOfRangeException("长度过短");
            
            byte[] bs = new byte[newSize];
            Array.Copy(bytes, 0, bs, 0, point);
        }

        void checkLength(int size)
        {
            if (point + size > bytes.Length)
                ReSize(Math.Max(point + size, bytes.Length * 2));
        }
    }
}
