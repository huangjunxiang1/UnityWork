using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Build.Profile;
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
        AssetDatabase.Refresh();
    }
    [MenuItem("Tools/生成mesh2")]
    static void gen_mesh2()
    {
        // 六边形棱柱：原点在底部中心，底面 y=0，顶面 y=1
        const int sides = 6;
        float height = 1f;
        float radius = 1f;

        // 顶点布局（为获得平面分明的法线，侧面每个面使用独立的 4 个顶点）：
        // 0 = bottom center
        // 1..6 = bottom rim (cap)
        // 7 = top center (cap)
        // 8..13 = top rim (cap)
        // sideStart .. sideStart + sides*4 - 1 = 每个面 4 个顶点 (b0, b1, t1, t0)
        int vertCount = 2 + sides * 2 + sides * 4;
        Vector3[] verts = new Vector3[vertCount];

        // 底心
        verts[0] = new Vector3(0f, 0f, 0f);

        // 底圈 (cap)
        for (int i = 0; i < sides; i++)
        {
            float ang = Mathf.Deg2Rad * (i * 360f / sides);
            float x = Mathf.Cos(ang) * radius;
            float z = Mathf.Sin(ang) * radius;
            verts[1 + i] = new Vector3(x, 0f, z); // bottom rim cap 1..6
        }

        // 顶心 (cap)
        int topCenterCap = 1 + sides; // 7
        verts[topCenterCap] = new Vector3(0f, height, 0f);

        // 顶圈 (cap)
        int topRimCapStart = topCenterCap + 1; // 8
        for (int i = 0; i < sides; i++)
        {
            float ang = Mathf.Deg2Rad * (i * 360f / sides);
            float x = Mathf.Cos(ang) * radius;
            float z = Mathf.Sin(ang) * radius;
            verts[topRimCapStart + i] = new Vector3(x, height, z); // 8..13
        }

        // 侧面：每个面 4 个独立顶点，顺序为 b0, b1, t1, t0（便于构建两三角形）
        int sideStart = topRimCapStart + sides; // 14
        for (int i = 0; i < sides; i++)
        {
            int next = (i + 1) % sides;
            Vector3 b0 = verts[1 + i];              // bottom rim i
            Vector3 b1 = verts[1 + next];           // bottom rim next
            Vector3 t0 = verts[topRimCapStart + i]; // top rim i
            Vector3 t1 = verts[topRimCapStart + next]; // top rim next

            int baseIndex = sideStart + i * 4;
            verts[baseIndex + 0] = b0;
            verts[baseIndex + 1] = b1;
            verts[baseIndex + 2] = t1;
            verts[baseIndex + 3] = t0;
        }

        // 将所有顶点绕 Y 轴旋转 30 度
        Quaternion rot = Quaternion.Euler(0f, 30f, 0f);
        for (int i = 0; i < verts.Length; i++)
        {
            verts[i] = rot * verts[i];
        }

        // 三角形数量：底盖 sides, 顶盖 sides, 侧面每边 2 个三角形 => total tris = 4*sides
        int triCount = 4 * sides;
        int[] tris = new int[triCount * 3];
        int t = 0;

        // 底盖（扇形），从中心到每个边：使底面法线朝下
        for (int i = 0; i < sides; i++)
        {
            int a = 0;
            int b = 1 + i;
            int c = 1 + ((i + 1) % sides);
            tris[t++] = a;
            tris[t++] = b;
            tris[t++] = c;
        }

        // 顶盖（扇形），从中心到每个边：使顶面法线朝上
        for (int i = 0; i < sides; i++)
        {
            int a = topCenterCap;
            int b = topRimCapStart + i;
            int c = topRimCapStart + ((i + 1) % sides);
            // 顶面使用 a, c, b 保持法线朝上
            tris[t++] = a;
            tris[t++] = c;
            tris[t++] = b;
        }

        // 侧面，使用独立的侧面顶点（每面 4 个顶点），以保证每个面的法线不被相邻面共享
        for (int i = 0; i < sides; i++)
        {
            int baseIndex = sideStart + i * 4;
            int v_b0 = baseIndex + 0; // b0
            int v_b1 = baseIndex + 1; // b1
            int v_t1 = baseIndex + 2; // t1
            int v_t0 = baseIndex + 3; // t0

            // 三角形 1: b0, t0, t1  (保持外法线)
            tris[t++] = v_b0;
            tris[t++] = v_t0;
            tris[t++] = v_t1;

            // 三角形 2: b0, t1, b1
            tris[t++] = v_b0;
            tris[t++] = v_t1;
            tris[t++] = v_b1;
        }

        // 简单 UV：为每组顶点分配合适的 UV（可根据需要改进）
        Vector2[] uvs = new Vector2[verts.Length];
        // 底心
        uvs[0] = new Vector2(0.5f, 0f);
        // 底盖圈 UV
        for (int i = 0; i < sides; i++)
        {
            float ang = (i * 360f / sides) * Mathf.Deg2Rad;
            uvs[1 + i] = new Vector2(0.5f + Mathf.Cos(ang) * 0.5f, 0.25f); // 底圈 cap
        }
        // 顶心
        uvs[topCenterCap] = new Vector2(0.5f, 0.75f);
        // 顶盖圈 UV
        for (int i = 0; i < sides; i++)
        {
            float ang = (i * 360f / sides) * Mathf.Deg2Rad;
            uvs[topRimCapStart + i] = new Vector2(0.5f + Mathf.Cos(ang) * 0.5f, 0.75f); // 顶圈 cap
        }
        // 侧面 UV（每面独立展开）
        for (int i = 0; i < sides; i++)
        {
            int baseIndex = sideStart + i * 4;
            float u0 = (float)i / sides;
            float u1 = (float)(i + 1) / sides;
            uvs[baseIndex + 0] = new Vector2(u0, 0f); // b0
            uvs[baseIndex + 1] = new Vector2(u1, 0f); // b1
            uvs[baseIndex + 2] = new Vector2(u1, 1f); // t1
            uvs[baseIndex + 3] = new Vector2(u0, 1f); // t0
        }

        // 构建 Mesh
        Mesh mesh = new Mesh();
        mesh.name = "HexPrism";
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        AssetDatabase.CreateAsset(mesh, "Assets/Res/3D/World/grid/hex2.mesh");
        AssetDatabase.Refresh();
    }
}