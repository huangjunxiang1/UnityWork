using Core;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Profiling;

namespace Game
{
    internal class OnBeforeUpdate : MonoBehaviour
    {
        public Core.World world;
       
        void Update()
        {
            Profiler.BeginSample($"{nameof(World)}.{nameof(World.BeforeUpdate)}");
            try
            {
                world.BeforeUpdate(Time.deltaTime);
            }
            catch (Exception ex)
            {
                Loger.Error($"{nameof(World.BeforeUpdate)} error " + ex);
            }
            Profiler.EndSample();

        }
    }
}