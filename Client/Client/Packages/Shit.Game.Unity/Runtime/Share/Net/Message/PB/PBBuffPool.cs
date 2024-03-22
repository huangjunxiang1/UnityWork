using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB
{
    public static class PBBuffPool
    {
        public static int MaxSize = 1024;

        static ConcurrentQueue<PBWriter> queue = new();

        public static PBWriter Get()
        {
            if (queue.TryDequeue(out var writer))
            {
                writer.Seek(0);
                return writer;
            }
            return new PBWriter(new MemoryStream(MaxSize));
        }
        public static void Return(PBWriter writer)
        {
            if (writer == null)
                throw new NullReferenceException("空 PBBytesWriter");

            queue.Enqueue(writer);
        }
    }
}
