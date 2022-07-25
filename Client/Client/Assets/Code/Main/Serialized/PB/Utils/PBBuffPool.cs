using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB
{
    public static class PBBuffPool
    {
        public static int MaxSize = 2048;

        static Queue<PBBytesWriter> bytes = new Queue<PBBytesWriter>();

        public static PBBytesWriter Get()
        {
            if (bytes.Count > 0)
            {
                PBBytesWriter writer = bytes.Dequeue();
                writer.Seek(0);
                return writer;
            }
            return new PBBytesWriter(new byte[MaxSize]);
        }
        public static void Return(PBBytesWriter writer)
        {
            if (writer == null)
                throw new NullReferenceException("空 PBBytesWriter");
            
            bytes.Enqueue(writer);
        }
    }
}
