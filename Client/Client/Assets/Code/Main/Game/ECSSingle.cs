using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public static class ECSSingle
    {
        public static readonly SharedStatic<Unity.Mathematics.Random> Random = SharedStatic<Unity.Mathematics.Random>.GetOrCreate<Unity.Mathematics.Random>();
        public static readonly SharedStatic<NativeList<FixedString128Bytes>> Strings = SharedStatic<NativeList<FixedString128Bytes>>.GetOrCreate<NativeList<FixedString128Bytes>>();

        public static void Init()
        {
            Random.Data.InitState((uint)DateTime.Now.Ticks);
            Strings.Data = new NativeList<FixedString128Bytes>(10, AllocatorManager.Persistent);
        }
        static Dictionary<string, int> stringsMap;
        public static int GetStringIndex(string k)
        {
            if (stringsMap == null)
                stringsMap = new Dictionary<string, int>();

            if (!stringsMap.TryGetValue(k, out int index))
            {
                index = stringsMap[k] = Strings.Data.Length;
                Strings.Data.Add(k);
            }
            return index;
        }
        public static void Dispose()
        {
            Strings.Data.Dispose();
        }
    }
}

