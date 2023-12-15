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
    public unsafe struct SceneConfig : IComponentData
    {
        public SharedStatic<Unity.Mathematics.Random> Random;
        public SharedStatic<NativeList<FixedString128Bytes>> Strings;

        static Dictionary<string, int> stringsMap;
        public int GetStringIndex(string k)
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
    }
    public static class ECSSingle
    {
        static Entity single;

        public static void Init()
        {
            single = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager.CreateSingleton<SceneConfig>();
            var sc = new SceneConfig()
            {
                Random = SharedStatic<Unity.Mathematics.Random>.GetOrCreate<Unity.Mathematics.Random>(),
                Strings = SharedStatic<NativeList<FixedString128Bytes>>.GetOrCreate<SharedStatic<NativeList<FixedString128Bytes>>>(),
            };
            sc.Random.Data.InitState((uint)DateTime.Now.Ticks);
            sc.Strings.Data = new NativeList<FixedString128Bytes>(10, AllocatorManager.Persistent);

            Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(single, sc);
        }
        public static void Dispose()
        {
            GetSingle<SceneConfig>().Strings.Data.Dispose();
        }
        public static T GetSingle<T>() where T : unmanaged, IComponentData
        {
            return Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<T>(single);
        }
        public static void SetSingle<T>(T c) where T : unmanaged, IComponentData
        {
            Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(single, c);
        }
    }
}

