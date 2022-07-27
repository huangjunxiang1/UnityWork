using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PB
{
    public abstract class PBWriter : IDisposable
    {
        public virtual int Position { get; protected set; }

        public void WriteTag(int tag) { Writeint32(tag); }
        public abstract void Writebool(bool v);
        public abstract void Writeint32(int v);
        public void Writesint32(int v)
        {
            v = (v >> 31) ^ (v << 1);
            Writeint32(v);
        }
        public abstract void Writeint64(long v);
        public void Writesint64(long v)
        {
            v = (v >> 63) ^ (v << 1);
            Writeint64(v);
        }
        public abstract void Writefixed32(uint v);
        public void Writesfixed32(int v)
        {
            Writefixed32((uint)v);
        }
        public abstract void Writefixed64(ulong v);
        public void Writesfixed64(long v)
        {
            Writefixed64((ulong)v);
        }
        public abstract void Writedouble(double v);
        public abstract void Writefloat(float v);
        public abstract void Writestring(int tag, string v);
        public void Writebytes(int tag, byte[] v)
        {
            if (v == null)
                return;
            Writebytes(tag, v, 0, v.Length);
        }
        public abstract void Writebytes(int tag, byte[] v, int index, int length);

        public void Writebools(int tag, List<bool> v)
        {
            if (v == null || v.Count == 0)
                return;
            WriteTag(tag);
            int size = v.Count;
            Writeint32(size);
            for (int i = 0; i < size; i++)
                Writebool(v[i]);
        }
        public void Writeint32s(int tag, List<int> v)
        {
            if (v == null || v.Count == 0)
                return;
            PBBytesWriter writer = PBBuffPool.Get();
            int len = v.Count;
            for (int i = 0; i < len; i++)
                writer.Writeint32(v[i]);
            Writebytes(tag, writer.GetNativeBytes(), 0, writer.Position);
            PBBuffPool.Return(writer);
        }
        public void Writesint32s(int tag, List<int> v)
        {
            if (v == null || v.Count == 0)
                return;
            PBBytesWriter writer = PBBuffPool.Get();
            int len = v.Count;
            for (int i = 0; i < len; i++)
                writer.Writesint32(v[i]);
            Writebytes(tag, writer.GetNativeBytes(), 0, writer.Position);
            PBBuffPool.Return(writer);
        }
        public void Writeint64s(int tag, List<long> v)
        {
            if (v == null || v.Count == 0)
                return;
            PBBytesWriter writer = PBBuffPool.Get();
            int len = v.Count;
            for (int i = 0; i < len; i++)
                writer.Writeint64(v[i]);
            Writebytes(tag, writer.GetNativeBytes(), 0, writer.Position);
            PBBuffPool.Return(writer);
        }
        public void Writesint64s(int tag, List<long> v)
        {
            if (v == null || v.Count == 0)
                return;
            PBBytesWriter writer = PBBuffPool.Get();
            int len = v.Count;
            for (int i = 0; i < len; i++)
                writer.Writesint64(v[i]);
            Writebytes(tag, writer.GetNativeBytes(), 0, writer.Position);
            PBBuffPool.Return(writer);
        }
        public void Writefixed32s(int tag, List<uint> v)
        {
            if (v == null || v.Count == 0)
                return;
            PBBytesWriter writer = PBBuffPool.Get();
            int len = v.Count;
            for (int i = 0; i < len; i++)
                writer.Writefixed32(v[i]);
            Writebytes(tag, writer.GetNativeBytes(), 0, writer.Position);
            PBBuffPool.Return(writer);
        }
        public void Writesfixed32s(int tag, List<int> v)
        {
            if (v == null || v.Count == 0)
                return;
            PBBytesWriter writer = PBBuffPool.Get();
            int len = v.Count;
            for (int i = 0; i < len; i++)
                writer.Writesfixed32(v[i]);
            Writebytes(tag, writer.GetNativeBytes(), 0, writer.Position);
            PBBuffPool.Return(writer);
        }
        public void Writefixed64s(int tag, List<ulong> v)
        {
            if (v == null || v.Count == 0)
                return;
            PBBytesWriter writer = PBBuffPool.Get();
            int len = v.Count;
            for (int i = 0; i < len; i++)
                writer.Writefixed64(v[i]);
            Writebytes(tag, writer.GetNativeBytes(), 0, writer.Position);
            PBBuffPool.Return(writer);
        }
        public void Writesfixed64s(int tag, List<long> v)
        {
            if (v == null || v.Count == 0)
                return;
            PBBytesWriter writer = PBBuffPool.Get();
            int len = v.Count;
            for (int i = 0; i < len; i++)
                writer.Writesfixed64(v[i]);
            Writebytes(tag, writer.GetNativeBytes(), 0, writer.Position);
            PBBuffPool.Return(writer);
        }
        public void Writedoubles(int tag, List<double> v)
        {
            if (v == null || v.Count == 0)
                return;
            PBBytesWriter writer = PBBuffPool.Get();
            int len = v.Count;
            for (int i = 0; i < len; i++)
                writer.Writedouble(v[i]);
            Writebytes(tag, writer.GetNativeBytes(), 0, writer.Position);
            PBBuffPool.Return(writer);
        }
        public void Writefloats(int tag, List<float> v)
        {
            if (v == null || v.Count == 0)
                return;
            PBBytesWriter writer = PBBuffPool.Get();
            int len = v.Count;
            for (int i = 0; i < len; i++)
                writer.Writefloat(v[i]);
            Writebytes(tag, writer.GetNativeBytes(), 0, writer.Position);
            PBBuffPool.Return(writer);
        }
        public void Writestrings(int tag, List<string> v)
        {
            if (v == null || v.Count == 0)
                return;
            int len = v.Count;
            for (int i = 0; i < len; i++)
                Writestring(tag, v[i]);
        }
        public void Writemessage(int tag, IPBMessage message)
        {
            if (message == null)
                return;
            PBBytesWriter writer = PBBuffPool.Get();
            message.Write(writer);
            Writebytes(tag, writer.GetNativeBytes(), 0, writer.Position);
            PBBuffPool.Return(writer);
        }

        public abstract void Seek(int index);

        public void Dispose()
        {

        }
    }
}
