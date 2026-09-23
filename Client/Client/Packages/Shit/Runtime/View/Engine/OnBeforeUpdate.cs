using Core;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Profiling;

internal class OnBeforeUpdate : MonoBehaviour
{
    void Update()
    {
        Profiler.BeginSample($"{nameof(World)}.BeforeUpdate");
        try
        {
            Game.BeforeUpdate(Time.deltaTime);
        }
        catch (Exception ex)
        {
            Loger.Error($"BeforeUpdate error " + ex);
        }
        Profiler.EndSample();

    }
}