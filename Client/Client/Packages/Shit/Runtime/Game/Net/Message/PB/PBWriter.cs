using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Buffers;
using Unity.Mathematics;

namespace PB
{
    public unsafe class PBWriter : IDisposable
    {
        public PBWriter(Stream s)
        {
            stream = s;
        }

        Stream stream;

        public Stream Stream { get { return stream; } }
        public int Position { get => (int)stream.Position; private set => this.Seek(value); }

        public void WriteTag(int tag) { Writeint32(tag); }
        public void Writebool(bool v)
        {
            stream.WriteByte(v ? (byte)1 : (byte)0);
        }
        public void Writeint32(int v)
        {
            Writeint64(v);
        }
        public void Writeint2(int tag, int2 v)
        {
            if (v.Equals(0))
                return;
            PBWriter writer = PBBuffPool.Get();
            for (int i = 0; i < 2; i++)
                writer.Writeint32(v[i]);
            WriteBuff(tag, writer);
            PBBuffPool.Return(writer);
        }
        public void Writeint3(int tag, int3 v)
        {
            if (v.Equals(0))
                return;
            PBWriter writer = PBBuffPool.Get();
            for (int i = 0; i < 3; i++)
                writer.Writeint32(v[i]);
            WriteBuff(tag, writer);
            PBBuffPool.Return(writer);
        }
        public void Writeint4(int tag, int4 v)
        {
            if (v.Equals(0))
                return;
            PBWriter writer = PBBuffPool.Get();
            for (int i = 0; i < 4; i++)
                writer.Writeint32(v[i]);
            WriteBuff(tag, writer);
            PBBuffPool.Return(writer);
        }
        public void Writeuint32(uint v)
        {
            Writeint64(v);
        }
        public void Writesint32(int v)
        {
            v = (v >> 31) ^ (v << 1);
            Writeint32(v);
        }
        public void Writeint64(long v)
        {
            ulong uv = (ulong)v;
            while (uv > 127)
            {
                stream.WriteByte((byte)(uv | 0x80));
                uv >>= 7;
            }
            stream.WriteByte((byte)uv);
        }
        public void Writesint64(long v)
        {
            v = (v >> 63) ^ (v << 1);
            Writeint64(v);
        }
        public void Writefixed32(int v)
        {
            for (int i = 0; i < sizeof(uint); i++)
                stream.WriteByte((byte)(v >> (8 * i)));
        }
        public void Writefixed64(long v)
        {
            for (int i = 0; i < sizeof(ulong); i++)
                stream.WriteByte((byte)(v >> (8 * i)));
        }
        public void Writedouble(double v)
        {
            long uv = *(long*)&v;
            Writefixed64(uv);
        }
        public void Writefloat(float v)
        {
            int uv = *(int*)&v;
            Writefixed32(uv);
        }

        public void Writebool(int tag, bool v)
        {
            if (!v) return;
            WriteTag(tag);
            Writebool(v);
        }
        public void Writeint32(int tag, int v)
        {
            if (v == 0) return;
            WriteTag(tag);
            Writeint32(v);
        }
        public void Writesint32(int tag, int v)
        {
            if (v == 0) return;
            WriteTag(tag);
            Writesint32(v);
        }
        public void Writeuint32(int tag, int v)
        {
            Writeint64(tag, v);
        }
        public void Writeint64(int tag, long v)
        {
            if (v == 0) return;
            WriteTag(tag);
            Writeint64(v);
        }
        public void Writesint64(int tag, long v)
        {
            if (v == 0) return;
            WriteTag(tag);
            Writesint64(v);
        }
        public void Writefixed32(int tag, int v)
        {
            if (v == 0) return;
            WriteTag(tag);
            Writefixed32(v);
        }
        public void Writefixed64(int tag, long v)
        {
            if (v == 0) return;
            WriteTag(tag);
            Writefixed64(v);
        }
        public void Writedouble(int tag, double v)
        {
            if (v == 0) return;
            WriteTag(tag);
            Writedouble(v);
        }
        public void Writefloat(int tag, float v)
        {
            if (v == 0) return;
            WriteTag(tag);
            Writefloat(v);
        }
        public void Writefloat2(int tag, float2 v)
        {
            if (math.all(v == float2.zero))
                return;
            WriteTag(tag);
            Writeint32(8);
            Writefloat(v.x);
            Writefloat(v.y);
        }
        public void Writefloat3(int tag, float3 v)
        {
            if (math.all(v == float3.zero))
                return;
            WriteTag(tag);
            Writeint32(12);
            Writefloat(v.x);
            Writefloat(v.y);
            Writefloat(v.z);
        }
        public void Writefloat4(int tag, float4 v)
        {
            if (math.all(v == float4.zero))
                return;
            WriteTag(tag);
            Writeint32(16);
            Writefloat(v.x);
            Writefloat(v.y);
            Writefloat(v.z);
            Writefloat(v.w);
        }
        public void Writestring(int tag, string v)
        {
            if (string.IsNullOrEmpty(v))
                return;

            int len = Encoding.UTF8.GetByteCount(v);
            WriteTag(tag);
            Writeint32(len);

            byte[] buffer = ArrayPool<byte>.Shared.Rent(len);
            try
            {
                Encoding.UTF8.GetBytes(v, 0, v.Length, buffer, 0);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
            stream.Write(buffer, 0, len);
        }
        public void Writestring(string v)
        {
            int len = string.IsNullOrEmpty(v) ? 0 : Encoding.UTF8.GetByteCount(v);
            Writeint32(len);

            if (len > 0)
            {
                byte[] buffer = ArrayPool<byte>.Shared.Rent(len);
                try
                {
                    Encoding.UTF8.GetBytes(v, 0, v.Length, buffer, 0);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(buffer);
                }
                stream.Write(buffer, 0, len);
            }
        }
        public void Writebytes(int tag, byte[] v)
        {
            if (v == null)
                return;
            Writebytes(tag, v, 0, v.Length);
        }
        public void Writebytes(int tag, byte[] v, int index, int length)
        {
            if (length == 0) return;
            if (index < 0 || length < 0 || index + length > v.Length)
                throw new ArgumentOutOfRangeException();

            WriteTag(tag);
            Writeint32(length);
            stream.Write(v, index, length);
        }
        public void WriteBuff(int tag, PBWriter writer)
        {
            int size = writer.Position;
            if (size == 0) return;

            WriteTag(tag);
            Writeint32(size);
            writer.Seek(0);

            byte[] buffer = ArrayPool<byte>.Shared.Rent(size);
            try
            {
                writer.stream.Read(buffer, 0, size);
                stream.Write(buffer, 0, size);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

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
            PBWriter writer = PBBuffPool.Get();
            int len = v.Count;
            for (int i = 0; i < len; i++)
                writer.Writeint32(v[i]);
            WriteBuff(tag, writer);
            PBBuffPool.Return(writer);
        }
        public void Writeuint32s(int tag, List<uint> v)
        {
            if (v == null || v.Count == 0)
                return;
            PBWriter writer = PBBuffPool.Get();
            int len = v.Count;
            for (int i = 0; i < len; i++)
                writer.Writeuint32(v[i]);
            WriteBuff(tag, writer);
            PBBuffPool.Return(writer);
        }
        public void Writesint32s(int tag, List<int> v)
        {
            if (v == null || v.Count == 0)
                return;
            PBWriter writer = PBBuffPool.Get();
            int len = v.Count;
            for (int i = 0; i < len; i++)
                writer.Writesint32(v[i]);
            WriteBuff(tag, writer);
            PBBuffPool.Return(writer);
        }
        public void Writeint64s(int tag, List<long> v)
        {
            if (v == null || v.Count == 0)
                return;
            PBWriter writer = PBBuffPool.Get();
            int len = v.Count;
            for (int i = 0; i < len; i++)
                writer.Writeint64(v[i]);
            WriteBuff(tag, writer);
            PBBuffPool.Return(writer);
        }
        public void Writesint64s(int tag, List<long> v)
        {
            if (v == null || v.Count == 0)
                return;
            PBWriter writer = PBBuffPool.Get();
            int len = v.Count;
            for (int i = 0; i < len; i++)
                writer.Writesint64(v[i]);
            WriteBuff(tag, writer);
            PBBuffPool.Return(writer);
        }
        public void Writefixed32s(int tag, List<int> v)
        {
            if (v == null || v.Count == 0)
                return;
            PBWriter writer = PBBuffPool.Get();
            int len = v.Count;
            for (int i = 0; i < len; i++)
                writer.Writefixed32(v[i]);
            WriteBuff(tag, writer);
            PBBuffPool.Return(writer);
        }
        public void Writefixed64s(int tag, List<long> v)
        {
            if (v == null || v.Count == 0)
                return;
            PBWriter writer = PBBuffPool.Get();
            int len = v.Count;
            for (int i = 0; i < len; i++)
                writer.Writefixed64(v[i]);
            WriteBuff(tag, writer);
            PBBuffPool.Return(writer);
        }
        public void Writedoubles(int tag, List<double> v)
        {
            if (v == null || v.Count == 0)
                return;
            PBWriter writer = PBBuffPool.Get();
            int len = v.Count;
            for (int i = 0; i < len; i++)
                writer.Writedouble(v[i]);
            WriteBuff(tag, writer);
            PBBuffPool.Return(writer);
        }
        public void Writefloats(int tag, List<float> v)
        {
            if (v == null || v.Count == 0)
                return;
            PBWriter writer = PBBuffPool.Get();
            int len = v.Count;
            for (int i = 0; i < len; i++)
                writer.Writefloat(v[i]);
            WriteBuff(tag, writer);
            PBBuffPool.Return(writer);
        }
        public void Writestrings(int tag, List<string> v)
        {
            if (v == null || v.Count == 0)
                return;
            int len = v.Count;
            for (int i = 0; i < len; i++)
            {
                if (string.IsNullOrEmpty(v[i]))
                {
                    WriteTag(tag);
                    Writeint32(0);
                    continue;
                }
                Writestring(tag, v[i]);
            }
        }
        public void Writemessage(int tag, PBMessage message)
        {
            if (message == null)
                return;
            PBWriter writer = PBBuffPool.Get();
            message.Write(writer);
            WriteBuff(tag, writer);
            PBBuffPool.Return(writer);
        }

        public void Seek(int index)
        {
            stream.Seek(index, SeekOrigin.Begin);
        }

        public void Dispose()
        {
            stream.Dispose();
        }
    }
}
