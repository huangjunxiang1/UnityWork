using System;
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

        static Queue<PBWriter> bytes = new Queue<PBWriter>();

        public static PBWriter Get()
        {
            if (bytes.Count > 0)
            {
                PBWriter writer = bytes.Dequeue();
                writer.Seek(0);
                return writer;
            }
            return new PBWriter(new MemoryStream(MaxSize));
        }
        public static void Return(PBWriter writer)
        {
            if (writer == null)
                throw new NullReferenceException("空 PBBytesWriter");
            
            bytes.Enqueue(writer);
        }
    }
}
