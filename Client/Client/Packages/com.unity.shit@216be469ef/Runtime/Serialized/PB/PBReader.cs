using System;
using System.Buffers;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB
{
    public unsafe class PBReader : IDisposable
    {
        public PBReader(Stream s, int index, int length)
        {
            min = index;
            max = index + length;
            stream = s;
        }
        public PBReader(Stream s) : this(s, 0, (int)s.Length) { }

        Stream stream;

        public int min { get; private set; }
        public int max { get; private set; }

        public int Position { get => (int)stream.Position; private set => this.Seek(value); }

        public int ReadTag()
        {
            if (Position >= max)
                return 0;
            return Readint32();
        }
        public bool Readbool()
        {
            return stream.ReadByte() == 1;
        }
        public int Readint32()
        {
            return (int)Readuint64();
        }
        public uint Readuint32()
        {
            return (uint)Readuint64();
        }
        public int Readsint32()
        {
            int v = Readint32();
            return (v >> 1) ^ -(v & 1);
        }
        public long Readint64()
        {
            return (long)Readuint64();
        }
        public ulong Readuint64()
        {
            ulong v = 0;
            for (int i = 0; i < sizeof(ulong) + 2; i++)
            {
                ulong bv = (ulong)stream.ReadByte();
                v |= ((bv & 0x7f) << 7 * i);
                if (bv < 128)
                    break;
            }
            return v;
        }
        public long Readsint64()
        {
            long v = Readint64();
            return (v >> 1) ^ -(v & 1);
        }
        public uint Readfixed32()
        {
            uint v = 0;
            for (int i = 0; i < sizeof(uint); i++)
                v |= (uint)stream.ReadByte() << (i * 8);
            return v;
        }
        public int Readsfixed32()
        {
            return (int)Readfixed32();
        }
        public ulong Readfixed64()
        {
            ulong v = 0;
            for (int i = 0; i < sizeof(ulong); i++)
                v |= (ulong)stream.ReadByte() << (i * 8);
            return v;
        }
        public long Readsfixed64()
        {
            return (long)Readfixed64();
        }
        public double Readdouble()
        {
            ulong v = Readfixed64();
            return *(double*)&v;
        }
        public float Readfloat()
        {
            uint v = Readfixed32();
            return *(float*)&v;
        }
        public string Readstring()
        {
            int len = Readint32();
            if (len <= 0)
                return string.Empty;

            byte[] buffer = ArrayPool<byte>.Shared.Rent(len);
            try
            {
                stream.Read(buffer, 0, len);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
            return Encoding.UTF8.GetString(buffer, 0, len);
        }
        public void Readmessage(PBMessage message)
        {
            int max = this.max;
            int len = this.Readint32();
            this.SetMax(Position + len);
            message.Read(this);
            this.SeekLast();
            this.SetMax(max);
        }
        public byte[] Readbytes()
        {
            int len = Readint32();
            byte[] bs = new byte[len];
            stream.Read(bs, 0, len);
            return bs;
        }
        public void Readbools(List<bool> lst)
        {
            int len = Readint32();
            lst.Capacity = lst.Count + len;
            int next = Position + len;
            int max = this.max;
            this.SetMax(next);
            for (int i = 0; i < len; i++)
                lst.Add(Readbool());
            this.SeekLast();
            this.SetMax(max);
        }
        public void Readint32s(List<int> lst)
        {
            int len = Readint32();
            int next = Position + len;
            while (Position < next)
                lst.Add(Readint32());
            this.Seek(next);
        }
        public void Readuint32s(List<uint> lst)
        {
            int len = Readint32();
            int next = Position + len;
            while (Position < next)
                lst.Add(Readuint32());
            this.Seek(next);
        }
        public void Readsint32s(List<int> lst)
        {
            int len = Readint32();
            int next = Position + len;
            while (Position < next)
                lst.Add(Readsint32());
            this.Seek(next);
        }
        public void Readint64s(List<long> lst)
        {
            int len = Readint32();
            int next = Position + len;
            while (Position < next)
                lst.Add(Readint64());
            this.Seek(next);
        }
        public void Readsint64s(List<long> lst)
        {
            int len = Readint32();
            int next = Position + len;
            while (Position < next)
                lst.Add(Readsint64());
            this.Seek(next);
        }
        public void Readfixed32s(List<uint> lst)
        {
            int len = Readint32();
            int next = Position + len;
            while (Position < next)
                lst.Add(Readfixed32());
            this.Seek(next);
        }
        public void Readsfixed32s(List<int> lst)
        {
            int len = Readint32();
            int next = Position + len;
            while (Position < next)
                lst.Add(Readsfixed32());
            this.Seek(next);
        }
        public void Readfixed64s(List<ulong> lst)
        {
            int len = Readint32();
            int next = Position + len;
            while (Position < next)
                lst.Add(Readfixed64());
            this.Seek(next);
        }
        public void Readsfixed64s(List<long> lst)
        {
            int len = Readint32();
            int next = Position + len;
            while (Position < next)
                lst.Add(Readsfixed64());
            this.Seek(next);
        }
        public void Readdoubles(List<double> lst)
        {
            int len = Readint32();
            int next = Position + len;
            while (Position < next)
                lst.Add(Readdouble());
            this.Seek(next);
        }
        public void Readfloats(List<float> lst)
        {
            int len = Readint32();
            int next = Position + len;
            while (Position < next)
                lst.Add(Readfloat());
            this.Seek(next);
        }
        public void Readstrings(int tag, List<string> lst)
        {
            int point;
            do
            {
                lst.Add(Readstring());
                point = Position;
            } while (ReadTag() == tag);
            this.Seek(point);
        }

        public void Seek(int index)
        {
            if (index < min)
                index = min;
            else if (index > max)
                index = max;
            stream.Seek(index, SeekOrigin.Begin);
        }
        public void SeekNext(int tag)
        {
            int type = tag & 7;
            if (type == 0)
                Readint64();
            else if (type == 1)
                this.Seek(Position + 8);
            else if (type == 2)
                this.Seek(this.Readint32() + Position);
            else if (type == 5)
                this.Seek(Position + 4);
            else
                throw new Exception("未实现类型=" + type);
        }
        public void SeekLast()
        {
            this.Seek(max);
        }

        public void Dispose()
        {
            stream.Dispose();
        }
        public void SetMinMax(int min, int max)
        {
            this.min = min;
            this.max = max;
            if (Position < min)
                Position = min;
            else if (Position > max)
                Position = max;
        }
        public void SetMin(int min)
        {
            this.min = min;
            if (Position < min)
                Position = min;
        }
        public void SetMax(int max)
        {
            this.max = max;
            if (Position > max)
                Position = max;
        }
    }
}
