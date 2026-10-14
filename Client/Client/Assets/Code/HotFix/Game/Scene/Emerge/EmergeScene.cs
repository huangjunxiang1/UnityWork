using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using static ShaderProperties;

[Scene("Emerge")]
public class EmergeScene : Scene
{
    ComputeShader_Emerge emerge = new();
    public override void OnEnter()
    {
        emerge.kingLand = new UnityEngine.GraphicsBuffer(UnityEngine.GraphicsBuffer.Target.Structured, Define.Emerge_SceneSize.x * Define.Emerge_SceneSize.y, sizeof(int));
        emerge.kingLand.Clear();

        emerge.unitLand = new UnityEngine.GraphicsBuffer(UnityEngine.GraphicsBuffer.Target.Structured, Define.Emerge_SceneSize.x * Define.Emerge_SceneSize.y, sizeof(int));
        emerge.unitLand.Clear();

        emerge.unitCenter = new UnityEngine.GraphicsBuffer(UnityEngine.GraphicsBuffer.Target.Structured, Define.Emerge_SceneSize.x * Define.Emerge_SceneSize.y, sizeof(int));
        emerge.unitCenter.Clear();

        emerge.pheromoneLand = new UnityEngine.GraphicsBuffer(UnityEngine.GraphicsBuffer.Target.Structured, Define.Emerge_SceneSize.x * Define.Emerge_SceneSize.y, sizeof(int));
        emerge.pheromoneLand.Clear();

        emerge.unitDir = new UnityEngine.GraphicsBuffer(UnityEngine.GraphicsBuffer.Target.Structured, Define.Emerge_SceneSize.x * Define.Emerge_SceneSize.y, sizeof(int));
        emerge.unitDir.Clear();

        List<CustomPassVolume> activeVolumes = new List<CustomPassVolume>();
        foreach (CustomPassInjectionPoint point in Enum.GetValues(typeof(CustomPassInjectionPoint)))
        {
            List<CustomPassVolume> tempVolumes = new List<CustomPassVolume>();
            CustomPassVolume.GetActivePassVolumes(point, tempVolumes);
            activeVolumes.AddRange(tempVolumes);
        }
        var pass = activeVolumes.Find(t => t.customPasses?.Find(x => x is FullScreenCustomPass) != null).customPasses.Find(t => t is FullScreenCustomPass) as FullScreenCustomPass;
        pass.fullscreenPassMaterial.SetBuffer(ShaderProperties.Emerge.PT_kingLand, emerge.kingLand);
        pass.fullscreenPassMaterial.SetBuffer(ShaderProperties.Emerge.PT_unitLand, emerge.unitLand);

        int[] tmp = new int[emerge.kingLand.count];
        for (int i = 0; i < 3; i++)
        {
            int index = Util.RandomInt(0, emerge.kingLand.count);
            tmp[index] = i + 1;
        }
        emerge.kingLand.SetData(tmp, 0, 0, tmp.Length);
    }
    public override void Dispose()
    {
        base.Dispose();
        emerge.Dispose();
    }

    [Timer(0, -1)]
    void update0()
    {
        emerge.kn_unitAction_Dispatch(Define.Emerge_SceneSize);
    }
    [Timer(1,-1)]
    void update()
    {
        emerge._RandomSeed = Util.RandomInt();
        emerge._ExpandChance = 0.5f;
        emerge.kn_mapReSize_Dispatch(Define.Emerge_SceneSize);
    }
    [Timer(5, -1)]
    void update2()
    {
        emerge._SpawnSeed = Util.RandomInt();
        emerge._SpawnChance = 0.001f;
        emerge._GeneralRate = 0.01f;
        emerge.kn_spawnUnit_Dispatch(Define.Emerge_SceneSize);
    }
}
