using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

static class Other
{

    [MenuItem("Tools/热重载配置表")]
    static void ReloadConfig()
    {
        if (!Application.isPlaying) return;

        {
            var buff = new DBuffer(new MemoryStream(File.ReadAllBytes(Application.dataPath + $"/Res/Config/raw/Tabs/{nameof(TabM)}.bytes")));
            if (!buff.ReadHeaderInfo())
                throw new System.Exception("数据错误");
            TabM.Init(buff, SSetting.CoreSetting.Debug);
        }
        {
            var buff = new DBuffer(new MemoryStream(File.ReadAllBytes(Application.dataPath + $"/Res/Config/raw/Tabs/{nameof(TabL)}.bytes")));
            if (!buff.ReadHeaderInfo())
                throw new System.Exception("数据错误");
            TabL.Init(buff, SSetting.CoreSetting.Debug);
        }
        {
            var buff = new DBuffer(new MemoryStream(File.ReadAllBytes(Application.dataPath + $"/Res/Config/raw/Tabs/Language_{SettingL.LanguageType}.bytes")));
            if (!buff.ReadHeaderInfo())
                throw new System.Exception("数据错误");
            LanguageUtil.Load((int)SettingL.LanguageType, buff, SSetting.CoreSetting.Debug);
        }
        EditorUtility.DisplayDialog("完成", "重载完成", "确定");
    }


    static List<string> dirs = new()
    {
        "/Code/Editor/Default",
        "/Code/Main/Core",
        "/Code/HotFix/Core",
        "/../Packages/Shit"
    };
    [MenuItem("Tools/文件夹 同步到x", priority = int.MaxValue - 10)]
    static void SyncDirsTo()
    {
        var src = ShitSettings.Inst.src;
        for (int i = 0; i < dirs.Count; i++)
            FileHelper.SyncDirectories(Application.dataPath + dirs[i], Application.dataPath + src + dirs[i]);
    }
    [MenuItem("Tools/文件夹 从x同步", priority = int.MaxValue - 10)]
    static void SyncDirsFrom()
    {
        var src = ShitSettings.Inst.src;
        for (int i = 0; i < dirs.Count; i++)
            FileHelper.SyncDirectories(Application.dataPath + src + dirs[i], Application.dataPath + dirs[i]);
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/生成mesh")]
    static void gen_mesh()
    {
        Mesh mesh = new Mesh();
        float distance = Hex.HexWidth / 2;
        float sqrt = math.sqrt(3);
        mesh.vertices = new Vector3[6]
        {
            new Vector3(0,0,-2*distance/sqrt),
            new Vector3(-distance,0,-distance/sqrt),
            new Vector3(-distance,0,distance/sqrt),
            new Vector3(0,0,2*distance/sqrt),
            new Vector3(distance,0,distance/sqrt),
            new Vector3(distance,0,-distance/sqrt),
        };
        mesh.triangles = new int[12] 
        {
            0,1,2,
            0,2,3,
            0,3,4,
            0,4,5,
        }; 
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        AssetDatabase.CreateAsset(mesh, "Assets/Res/3D/World/grid/hex.mesh");
    }
}