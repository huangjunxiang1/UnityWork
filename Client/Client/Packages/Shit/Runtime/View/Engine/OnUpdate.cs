using Core;
using Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

namespace Game
{
    internal class OnUpdate : MonoBehaviour
    {
        public Core.World world;
        void Update()
        {
            Profiler.BeginSample($"{nameof(World)}.{nameof(World.Update)}");
            try
            {
                world.Update();
            }
            catch (Exception ex)
            {
                Loger.Error($"{nameof(World.Update)} error " + ex);
            }
            Profiler.EndSample();
        }
        void LateUpdate()
        {
            Profiler.BeginSample($"{nameof(World)}.{nameof(World.LateUpdate)}");
            try
            {
                world.LateUpdate();
            }
            catch (Exception ex)
            {
                Loger.Error($"{nameof(World.LateUpdate)} error " + ex);
            }
            Profiler.EndSample();
        }
        void OnApplicationQuit() => world?.Event?.RunEvent(new EC_QuitGame());
    }
}
