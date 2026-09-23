using Core;
using Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

internal class OnUpdate : MonoBehaviour
{
    void Update()
    {
        Profiler.BeginSample($"{nameof(World)}.Update");
        try
        {
            Game.Update();
        }
        catch (Exception ex)
        {
            Loger.Error($"Update error " + ex);
        }
        Profiler.EndSample();
    }
    void LateUpdate()
    {
        Profiler.BeginSample($"{nameof(World)}.LateUpdate");
        try
        {
            Game.LateUpdate();
        }
        catch (Exception ex)
        {
            Loger.Error($"LateUpdate error " + ex);
        }
        Profiler.EndSample();
    }
    void OnApplicationQuit() => Game.Event?.RunEvent(new EC_QuitGame());
}
