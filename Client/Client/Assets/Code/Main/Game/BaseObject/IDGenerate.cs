using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public static class IDGenerate
    {
        static long generate = 0;

        public static long GenerateID()
        {
            return ++generate;
        }
    }
}
