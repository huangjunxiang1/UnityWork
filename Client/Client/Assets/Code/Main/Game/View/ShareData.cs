using Event;
using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Game
{
    public static class ShareData
    {
        public static readonly SharedStatic<Unity.Mathematics.Random> Random = SharedStatic<Unity.Mathematics.Random>.GetOrCreate<Unity.Mathematics.Random>();
        public static readonly SharedStatic<UnsafeList<FixedString128Bytes>> Strings = SharedStatic<UnsafeList<FixedString128Bytes>>.GetOrCreate<UnsafeList<FixedString128Bytes>>();

        [Event]
        static void Init(EC_ClientLanucher e)
        {
            Random.Data.InitState((uint)DateTime.Now.Ticks);
            Strings.Data = new UnsafeList<FixedString128Bytes>(10, AllocatorManager.Persistent);
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

