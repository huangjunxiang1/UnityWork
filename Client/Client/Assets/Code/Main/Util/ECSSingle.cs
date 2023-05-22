using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public static class ECSSingle
    {
        static ECSSingle()
        {
            single = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager.CreateSingleton<TabM_ST>();
        }

        static Entity single;

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

