using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB
{
    public abstract class PBReader : IDisposable
    {
        public PBReader(int index, int length)
        {
            min = index;
            max = index + length;
        }

        public int min { get; private set; }
        public int max { get; private set; }

        public virtual int Position { get; protected set; }

        public int ReadTag()
        {
            if (Position >= max)
                return 0;
            return Readint32();
        }
        public abstract bool Readbool();
        public abstract int Readint32();
        public int Readsint32()
        {
            int v = Readint32();
            return (v >> 1) ^ -(v & 1);
        }
        public abstract long Readint64();
        public long Readsint64()
        {
            long v = Readint64();
            return (v >> 1) ^ -(v & 1);
        }
        public abstract uint Readfixed32();
        public int Readsfixed32()
        {
            return (int)Readfixed32();
        }
        public abstract ulong Readfixed64();
        public long Readsfixed64()
        {
            return (long)Readfixed64();
        }
        public abstract double Readdouble();
        public abstract float Readfloat();
        public abstract string Readstring();
        public void Readmessage(IPBMessage message)
        {
            int min = this.min;
            int max = this.max;
            int len = this.Readint32();
            this.SetLimit(Position, Position + len);
            message.Read(this);
            this.SetLimit(min, max);
        }
        public abstract byte[] Readbytes();
        public void Readbools(List<bool> lst)
        {
            int len = Readint32();
            int next = Position + len;
            for (int i = 0; i < len; i++)
                lst.Add(Readbool());
            this.Seek(next);
        }
        public void Readint32s(List<int> lst)
        {
            int len = Readint32();
            int next = Position + len;
            while (Position < next)
                lst.Add(Readint32());
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

        public abstract void Seek(int index);

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

        public void Dispose()
        {
           
        }
        public void SetLimit(int min, int max)
        {
            this.min = min;
            this.max = max;
            if (Position < min)
                Position = min;
            else if (Position > max)
                Position = max;
        }
    }
}
