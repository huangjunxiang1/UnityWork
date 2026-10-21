using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using Unity.Cinemachine;
using Unity.Mathematics;

// ============================================================================
//  六边形王国模拟 —— 使用项目自带 Hex 坐标系统 (int2 + XZ 平面)
//  操作：
//    WASD / 方向键   平移视角
//    滚轮            缩放
//    1 / 2           提高 / 降低 税率
//    3 / 4 / 5       基建 / 军费 / 民生 预算 +10%
//    空格            暂停 / 继续
//    R               打印王国日报
//    H               加快建房
//    Shift + W       宣战
// ============================================================================

#region ---------------- 枚举 ----------------

public enum Terrain : byte { Water = 0, Grass = 1, Forest = 2, Desert = 3, Stone = 4, Mountain = 5, River = 6 }
public enum Gender : byte { Male = 0, Female = 1 }

public enum Job : byte
{
    None = 0, Child = 1,
    Worker = 2, Farmer = 3, Soldier = 4,
    Weaver = 5, Housewife = 6, Logistic = 7, Merchant = 8,
}

public enum LifeStage : byte { Baby, Child, Teen, Adult, Elder }
public enum BuildingType : byte { House, Farm, Workshop, Barracks, Wall, Warehouse, Well }
public enum Budget : byte { Infrastructure, Military, Welfare, Reserve }

#endregion

#region ---------------- 实体 ----------------

public class Citizen
{
    public int Id;
    public string Name;
    public Gender Gender;
    public float Age;
    public float Health = 1f;
    public float Hunger = 0f;
    public int FamilyId = -1;
    public Job Job = Job.Child;
    public int2 Home;
    public int2 Workplace = new int2(int.MinValue, int.MinValue);
    public bool Alive = true;
    public float BirthCooldown;
    public GameObject View;
    public MaterialPropertyBlock Mpb;
    public Transform HeadAnchor;    // 头顶 UI 定位点
}

public class Family
{
    public int Id;
    public readonly List<int> Members = new List<int>();
    public float Funds;
    public int2 Home;
    public int HouseBuildingId = -1;
    public int2 HouseHex;
    public float LastBuildAttemptDay = -999f;
    public float FoodStore;
}

public class Building
{
    public int Id;
    public BuildingType Type;
    public int2 Hex;
    public bool Complete;
    public float WorkDone;
    public float WorkRequired;
    public int OwnerFamilyId = -1;
    public GameObject View;
    public MaterialPropertyBlock Mpb;
}

public class KingdomState
{
    public string Name = "无名王国";
    public float Treasury = 500f;
    public float TaxRate = 0.12f;
    public readonly float[] Allocation = { 0.40f, 0.20f, 0.30f, 0.10f };
    public bool AtWar = false;
    public int EnemyStrength = 0;
    public float WallRadius = 6f;
    public int2 Capital = int2.zero;
    public float DailyIncome;
    public float DailyExpense;
    public int TotalTaxpayers;

    public void NormalizeAllocation()
    {
        float sum = 0f;
        for (int i = 0; i < Allocation.Length; i++) sum += Allocation[i];
        if (sum <= 0.0001f) { Allocation[0] = 1f; return; }
        for (int i = 0; i < Allocation.Length; i++) Allocation[i] /= sum;
    }
}

#endregion

#region ---------------- 头顶 UI ----------------

/// <summary>
/// 用 OnGUI 做世界坐标标签。仅当有标签且相机可见时绘制。
/// </summary>
public class KingdomLabelRenderer : MonoBehaviour
{
    public Camera cam;

    public struct LabelEntry
    {
        public Vector3 WorldPos;
        public string Text;
        public Color Color;
    }

    public readonly List<LabelEntry> Labels = new List<LabelEntry>();

    GUIStyle _style;
    GUIStyle _shadow;

    void EnsureStyles()
    {
        if (_style != null) return;
        _style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 11,
            alignment = TextAnchor.LowerCenter,
            wordWrap = false,
        };
        _shadow = new GUIStyle(_style);
        _shadow.normal.textColor = new Color(0f, 0f, 0f, 0.9f);
    }

    void OnGUI()
    {
        if (cam == null || Labels.Count == 0) return;
        EnsureStyles();

        // 相机中心用于做屏幕外剔除
        float sw = Screen.width;
        float sh = Screen.height;

        for (int i = 0; i < Labels.Count; i++)
        {
            var e = Labels[i];
            Vector3 sp = cam.WorldToScreenPoint(e.WorldPos);
            if (sp.z <= 0f) continue;

            float gx = sp.x;
            float gy = sh - sp.y;
            if (gx < -120 || gx > sw + 120 || gy < -40 || gy > sh + 40) continue;

            var rect = new Rect(gx - 100f, gy - 22f, 200f, 18f);

            _style.normal.textColor = e.Color;
            GUI.Label(new Rect(rect.x + 1f, rect.y + 1f, rect.width, rect.height), e.Text, _shadow);
            GUI.Label(rect, e.Text, _style);
        }
    }
}

#endregion

#region ---------------- Cinemachine 桥接 ----------------

class CinemachineHelper
{
    public Camera cam;
    public CinemachineCamera vcam;
    public bool active;

    public void Init()
    {
        cam = Camera.main;
        if (cam == null)
        {
            var go = new GameObject("KingdomCamera") { tag = "MainCamera" };
            cam = go.AddComponent<Camera>();
            Debug.Log("[Kingdom] 未找到 Camera.main，创建了专用相机");
        }

        cam.orthographic = true;
        cam.nearClipPlane = 0.1f;
        cam.farClipPlane = 5000f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.06f, 0.08f, 0.13f, 1f);
        cam.enabled = true;

        try
        {
            var hd = cam.GetComponent<HDAdditionalCameraData>();
            if (hd == null) hd = cam.gameObject.AddComponent<HDAdditionalCameraData>();
            hd.clearColorMode = HDAdditionalCameraData.ClearColorMode.Color;
            hd.backgroundColorHDR = new Color(0.06f, 0.08f, 0.13f, 1f);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[Kingdom] HDRP 相机设置异常（可忽略）: {e.Message}");
        }

        if (cam.GetComponent<CinemachineBrain>() == null)
            cam.gameObject.AddComponent<CinemachineBrain>();

        vcam = null;
        var all = UnityEngine.Object.FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None);
        foreach (var c in all)
            if (c != null && c.gameObject.name == "KingdomVCam") { vcam = c; break; }

        if (vcam == null)
        {
            var go = new GameObject("KingdomVCam");
            vcam = go.AddComponent<CinemachineCamera>();
        }

        vcam.Priority = 1000;
        vcam.Follow = null;
        vcam.LookAt = null;

        var lens = vcam.Lens;
        lens.ModeOverride = LensSettings.OverrideModes.Orthographic;
        lens.OrthographicSize = 120f;
        lens.NearClipPlane = 0.1f;
        lens.FarClipPlane = 5000f;
        vcam.Lens = lens;

        // 俯视 XZ 平面：+X 向右，+Z 向上
        vcam.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        active = true;
        Debug.Log("[Kingdom] Cinemachine 接入成功 (CinemachineCamera)");
    }

    public void SetPosition(Vector3 pos)
    {
        if (active && vcam != null) vcam.transform.position = pos;
        else if (cam != null) cam.transform.position = pos;
    }

    public Vector3 GetPosition()
    {
        if (active && vcam != null) return vcam.transform.position;
        if (cam != null) return cam.transform.position;
        return Vector3.zero;
    }

    public void SetOrthoSize(float size)
    {
        if (cam != null) cam.orthographicSize = size;
        if (active && vcam != null)
        {
            var lens = vcam.Lens;
            lens.ModeOverride = LensSettings.OverrideModes.Orthographic;
            lens.OrthographicSize = size;
            vcam.Lens = lens;
        }
    }
}

#endregion

#region ---------------- 场景 ----------------

[Scene("Virus")]
class KingdomScene : Scene
{
    const int MAP_HALF = 500;
    const int MAP_SIZE = MAP_HALF * 2;      // 1000 x 1000 格
    const int CHUNK_SIZE = 32;

    const int MAX_POPULATION = 4000;
    const int DAYS_PER_YEAR = 365;
    const float DAY_LENGTH = 0.08f;
    const int MAX_TICKS_PER_FRAME = 24;

    const float WAGE_FARMER = 3.2f;
    const float WAGE_WORKER = 4.0f;
    const float WAGE_WEAVER = 3.4f;
    const float WAGE_LOGISTIC = 3.0f;
    const float WAGE_SOLDIER = 2.4f;
    const float FOOD_COST_PER_PERSON = 1.2f;
    const float HOUSE_COST = 60f;
    const float HOUSE_WORK = 40f;

    // 相机缩放范围
    const float CAM_MIN = 5f;
    const float CAM_MAX = 500f;
    const float CAM_START = 120f;

    // ---------- 世界数据 ----------
    int2 _mapMin = new int2(-MAP_HALF, -MAP_HALF);
    int2 _mapMax = new int2(MAP_HALF, MAP_HALF);
    Terrain[] _terrain;

    // ---------- 实体 ----------
    readonly List<Citizen> _citizens = new List<Citizen>();
    readonly Dictionary<int, Family> _families = new Dictionary<int, Family>();
    readonly List<Building> _buildings = new List<Building>();
    readonly Dictionary<int2, Building> _buildingAt = new Dictionary<int2, Building>();

    int _nextCitizenId = 1;
    int _nextFamilyId = 1;
    int _nextBuildingId = 1;

    KingdomState _kingdom = new KingdomState();

    // ---------- 时间 ----------
    float _dayAcc;
    float _day = 1f;
    bool _paused = false;

    // ---------- 渲染 ----------
    CinemachineHelper _cinemachine = new CinemachineHelper();
    GameObject _chunkRoot;
    GameObject _unitRoot;
    GameObject _buildingRoot;
    KingdomLabelRenderer _labelRenderer;

    Texture2D _mapTex;
    Material _mapMaterial;
    Material _unitMaterial;

    Mesh _citizenMesh;       // 八棱柱（人）
    Mesh _buildingMesh;      // 四棱柱（建筑）

    System.Random _rng = new System.Random(20240601);

    // ---------- 相机 ----------
    Vector3 _camTarget;
    float _camSize = CAM_START;
    bool _cameraReady = false;

    // ========================================================================
    //  生命周期
    // ========================================================================

    public override void OnEnter()
    {
        Debug.Log("[Kingdom] OnEnter 开始");

        SetupAssets();
        GenerateWorld(20240601);
        BuildMapTexture();
        BuildChunkedMapMesh();
        SpawnInitialVillage();

        float3 capW = Hex.GetPositon(_kingdom.Capital);
        _camTarget = new Vector3(capW.x, 100f, capW.z);

        _cinemachine.Init();
        _cinemachine.SetOrthoSize(_camSize);
        _cinemachine.SetPosition(_camTarget);
        _cameraReady = true;

        Debug.Log($"[Kingdom] 首都格 {_kingdom.Capital} -> 世界坐标 ({capW.x:F1},{capW.z:F1})");
        Debug.Log($"[Kingdom] 世界生成完毕，地块 {_terrain.Length} 个。");
    }

    public override void Dispose()
    {
        if (_mapTex) UnityEngine.Object.Destroy(_mapTex);
        if (_chunkRoot) UnityEngine.Object.Destroy(_chunkRoot);
        if (_unitRoot) UnityEngine.Object.Destroy(_unitRoot);
        if (_buildingRoot) UnityEngine.Object.Destroy(_buildingRoot);
        if (_citizenMesh) UnityEngine.Object.Destroy(_citizenMesh);
        if (_buildingMesh) UnityEngine.Object.Destroy(_buildingMesh);
        if (_mapMaterial) UnityEngine.Object.Destroy(_mapMaterial);
        if (_unitMaterial) UnityEngine.Object.Destroy(_unitMaterial);
    }

    [Timer(0, -1)]
    void update()
    {
        float dt = Time.deltaTime;

        if (!_cameraReady)
        {
            float3 capW = Hex.GetPositon(_kingdom.Capital);
            _camTarget = new Vector3(capW.x, 100f, capW.z);
            _cinemachine.Init();
            _cinemachine.SetOrthoSize(_camSize);
            _cinemachine.SetPosition(_camTarget);
            _cameraReady = true;
        }

        HandleInput(dt);
        if (!_paused) UpdateSimulation(dt);
        UpdateCamera(dt);
        UpdateUnitVisuals();
    }

    // ========================================================================
    //  资源
    // ========================================================================

    void SetupAssets()
    {
        _citizenMesh = CreatePrismMesh(8, 0.20f, 0.17f, 0.55f);   // 八棱柱当人
        _buildingMesh = CreatePrismMesh(4, 0.72f, 0.72f, 1.1f);    // 四棱柱当房子

        var mapShader = FindShader("HDRP/Unlit", "Universal Render Pipeline/Unlit", "Unlit/Texture", "Sprites/Default");
        var unitShader = FindShader("HDRP/Lit", "Universal Render Pipeline/Lit", "Standard", "HDRP/Unlit", "Sprites/Default");

        _mapMaterial = new Material(mapShader);

        _unitMaterial = new Material(unitShader);
        _unitMaterial.enableInstancing = true;
        if (_unitMaterial.HasProperty("_BaseColor")) _unitMaterial.SetColor("_BaseColor", Color.white);
        if (_unitMaterial.HasProperty("_UnlitColor")) _unitMaterial.SetColor("_UnlitColor", Color.white);
        if (_unitMaterial.HasProperty("_Color")) _unitMaterial.SetColor("_Color", Color.white);
        if (_unitMaterial.HasProperty("_Smoothness")) _unitMaterial.SetFloat("_Smoothness", 0.05f);

        _chunkRoot = new GameObject("KingdomMapChunks");
        _unitRoot = new GameObject("KingdomUnits");
        _buildingRoot = new GameObject("KingdomBuildings");

        // 头顶 UI
        var labelGO = new GameObject("KingdomLabels");
        _labelRenderer = labelGO.AddComponent<KingdomLabelRenderer>();

        Debug.Log($"[Kingdom] 地图 shader = {_mapMaterial.shader.name}, 单位 shader = {_unitMaterial.shader.name}");
    }

    static Shader FindShader(params string[] names)
    {
        foreach (var n in names)
        {
            var s = Shader.Find(n);
            if (s != null) return s;
        }
        return Shader.Find("Diffuse");
    }

    /// <summary>
    /// 生成一个上大下小可调的 N 边棱柱（底在 y=0，顶在 y=height）。
    /// </summary>
    static Mesh CreatePrismMesh(int sides, float radiusBottom, float radiusTop, float height)
    {
        sides = Mathf.Max(3, sides);
        int vCount = sides * 2 + 2;
        var verts = new Vector3[vCount];
        var uvs = new Vector2[vCount];

        for (int i = 0; i < sides; i++)
        {
            float a = Mathf.PI * 2f * i / sides;
            float c = Mathf.Cos(a), s = Mathf.Sin(a);
            verts[i] = new Vector3(c * radiusBottom, 0f, s * radiusBottom);
            verts[i + sides] = new Vector3(c * radiusTop, height, s * radiusTop);
            uvs[i] = new Vector2(i / (float)sides, 0f);
            uvs[i + sides] = new Vector2(i / (float)sides, 1f);
        }
        int bCenter = vCount - 2;
        int tCenter = vCount - 1;
        verts[bCenter] = Vector3.zero;
        verts[tCenter] = new Vector3(0f, height, 0f);
        uvs[bCenter] = new Vector2(0.5f, 0.5f);
        uvs[tCenter] = new Vector2(0.5f, 0.5f);

        var tris = new int[sides * 4 * 3];
        int t = 0;
        for (int i = 0; i < sides; i++)
        {
            int b0 = i;
            int b1 = (i + 1) % sides;
            int t0 = i + sides;
            int t1 = ((i + 1) % sides) + sides;

            // 侧面（外法线）
            tris[t++] = b0; tris[t++] = t0; tris[t++] = b1;
            tris[t++] = b1; tris[t++] = t0; tris[t++] = t1;

            // 底面（法线 -Y）
            tris[t++] = bCenter; tris[t++] = b0; tris[t++] = b1;

            // 顶面（法线 +Y）
            tris[t++] = tCenter; tris[t++] = t1; tris[t++] = t0;
        }

        var mesh = new Mesh { name = $"Prism_{sides}" };
        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }

    // ========================================================================
    //  世界生成
    // ========================================================================

    void GenerateWorld(int seed)
    {
        _terrain = new Terrain[MAP_SIZE * MAP_SIZE];

        System.Random rng = new System.Random(seed);
        float ox = (float)rng.NextDouble() * 10000f;
        float oz = (float)rng.NextDouble() * 10000f;

        for (int y = 0; y < MAP_SIZE; y++)
        {
            int gy = _mapMin.y + y;
            for (int x = 0; x < MAP_SIZE; x++)
            {
                int gx = _mapMin.x + x;
                float3 wp = Hex.GetPositon(new int2(gx, gy));

                float nx = wp.x * 0.02f + ox;
                float nz = wp.z * 0.02f + oz;

                float height = FractalNoise(nx, nz, 5, 2.0f, 0.5f);
                float moisture = FractalNoise(nx * 1.7f + 331.7f, nz * 1.7f - 118.3f, 3, 2.0f, 0.5f);
                float river = Mathf.Abs(FractalNoise(nx * 2.2f + 900f, nz * 2.2f + 400f, 2, 2f, 0.5f) - 0.5f) * 2f;

                float edgeX = Mathf.Min(x, MAP_SIZE - 1 - x) / (MAP_SIZE * 0.5f);
                float edgeY = Mathf.Min(y, MAP_SIZE - 1 - y) / (MAP_SIZE * 0.5f);
                float edge = Mathf.Min(1f, Mathf.Min(edgeX, edgeY) * 1.6f);
                height = height * edge + height * (1f - edge) * 0.35f - (1f - edge) * 0.15f;

                Terrain t;
                if (height < 0.34f) t = Terrain.Water;
                else if (height > 0.72f) t = Terrain.Mountain;
                else if (river < 0.018f && height < 0.68f) t = Terrain.River;
                else if (moisture < 0.34f) t = Terrain.Desert;
                else if (moisture > 0.60f && height < 0.66f) t = Terrain.Forest;
                else if (height > 0.64f) t = Terrain.Stone;
                else t = Terrain.Grass;

                _terrain[y * MAP_SIZE + x] = t;
            }
        }
    }

    static float FractalNoise(float x, float y, int octaves, float lacunarity, float gain)
    {
        float sum = 0f, amp = 1f, freq = 1f, norm = 0f;
        for (int i = 0; i < octaves; i++)
        {
            sum += Mathf.PerlinNoise(x * freq, y * freq) * amp;
            norm += amp;
            amp *= gain;
            freq *= lacunarity;
        }
        return norm > 0f ? sum / norm : 0f;
    }

    // ========================================================================
    //  地图纹理
    // ========================================================================

    void BuildMapTexture()
    {
        _mapTex = new Texture2D(MAP_SIZE, MAP_SIZE, TextureFormat.RGBA32, false);
        _mapTex.name = "KingdomMapTex";
        _mapTex.filterMode = FilterMode.Point;
        _mapTex.wrapMode = TextureWrapMode.Clamp;

        var pixels = new Color32[MAP_SIZE * MAP_SIZE];
        for (int i = 0; i < _terrain.Length; i++)
            pixels[i] = TerrainColor(_terrain[i], i);

        _mapTex.SetPixels32(pixels);
        _mapTex.Apply(false, false);

        if (_mapMaterial.HasProperty("_BaseColorMap")) _mapMaterial.SetTexture("_BaseColorMap", _mapTex);
        if (_mapMaterial.HasProperty("_MainTex")) _mapMaterial.SetTexture("_MainTex", _mapTex);
        if (_mapMaterial.HasProperty("_UnlitColorMap")) _mapMaterial.SetTexture("_UnlitColorMap", _mapTex);
        _mapMaterial.mainTexture = _mapTex;
    }

    Color32 TerrainColor(Terrain t, int seedIdx)
    {
        int n = (seedIdx * 1103515245 + 12345) & 0xFF;
        int d = (n % 17) - 8;

        switch (t)
        {
            case Terrain.Water: return new Color32((byte)(46 + d), (byte)(92 + d), (byte)(138 + d), 255);
            case Terrain.River: return new Color32((byte)(62 + d), (byte)(124 + d), (byte)(177 + d), 255);
            case Terrain.Grass: return new Color32((byte)(107 + d), (byte)(158 + d), (byte)(74 + d), 255);
            case Terrain.Forest: return new Color32((byte)(47 + d), (byte)(94 + d), (byte)(42 + d), 255);
            case Terrain.Desert: return new Color32((byte)(217 + d), (byte)(192 + d), (byte)(122 + d), 255);
            case Terrain.Stone: return new Color32((byte)(122 + d), (byte)(122 + d), (byte)(114 + d), 255);
            case Terrain.Mountain: return new Color32((byte)(92 + d), (byte)(92 + d), (byte)(86 + d), 255);
            default: return new Color32(255, 0, 255, 255);
        }
    }

    // ========================================================================
    //  分块六边形网格
    // ========================================================================

    void BuildChunkedMapMesh()
    {
        int chunksPerSide = Mathf.CeilToInt((float)MAP_SIZE / CHUNK_SIZE);
        float hexRadius = Define.HexWidth / Mathf.Sqrt(3f);

        int total = 0;

        for (int cy = 0; cy < chunksPerSide; cy++)
        {
            for (int cx = 0; cx < chunksPerSide; cx++)
            {
                int startX = _mapMin.x + cx * CHUNK_SIZE;
                int startY = _mapMin.y + cy * CHUNK_SIZE;
                int endX = Mathf.Min(startX + CHUNK_SIZE, _mapMin.x + MAP_SIZE);
                int endY = Mathf.Min(startY + CHUNK_SIZE, _mapMin.y + MAP_SIZE);
                int w = endX - startX, h = endY - startY;
                if (w <= 0 || h <= 0) continue;

                int hexCount = w * h;
                var verts = new Vector3[hexCount * 7];
                var uvs = new Vector2[hexCount * 7];
                var tris = new int[hexCount * 6 * 3];

                int vIdx = 0, tIdx = 0;

                for (int gy = startY; gy < endY; gy++)
                {
                    for (int gx = startX; gx < endX; gx++)
                    {
                        float3 c = Hex.GetPositon(new int2(gx, gy));

                        float u = (gx - _mapMin.x + 0.5f) / MAP_SIZE;
                        float v = (gy - _mapMin.y + 0.5f) / MAP_SIZE;
                        var uv = new Vector2(u, v);

                        int centerIdx = vIdx;
                        verts[vIdx] = new Vector3(c.x, 0f, c.z);
                        uvs[vIdx] = uv;
                        vIdx++;

                        int firstRim = vIdx;
                        for (int i = 0; i < 6; i++)
                        {
                            float a = Mathf.Deg2Rad * (30f - 60f * i);
                            verts[vIdx] = new Vector3(
                                c.x + Mathf.Cos(a) * hexRadius,
                                0f,
                                c.z + Mathf.Sin(a) * hexRadius);
                            uvs[vIdx] = uv;
                            vIdx++;
                        }

                        for (int i = 0; i < 6; i++)
                        {
                            int a = firstRim + i;
                            int b = firstRim + (i + 1) % 6;
                            tris[tIdx++] = centerIdx;
                            tris[tIdx++] = a;
                            tris[tIdx++] = b;
                        }
                    }
                }

                var mesh = new Mesh { name = $"Chunk_{cx}_{cy}" };
                mesh.indexFormat = IndexFormat.UInt32;
                mesh.vertices = verts;
                mesh.uv = uvs;
                mesh.triangles = tris;
                mesh.RecalculateBounds();

                var go = new GameObject($"Chunk_{cx}_{cy}");
                go.transform.SetParent(_chunkRoot.transform, false);
                var mf = go.AddComponent<MeshFilter>();
                var mr = go.AddComponent<MeshRenderer>();
                mf.sharedMesh = mesh;
                mr.sharedMaterial = _mapMaterial;
                mr.shadowCastingMode = ShadowCastingMode.Off;
                mr.receiveShadows = false;
                total++;
            }
        }

        Debug.Log($"[Kingdom] 生成 {total} 个地图分块（每块 {CHUNK_SIZE}×{CHUNK_SIZE}）");
    }

    // ========================================================================
    //  初始村落
    // ========================================================================

    void SpawnInitialVillage()
    {
        int2 start = FindSettlementSite();
        _kingdom.Capital = start;

        PlaceBuilding(BuildingType.Well, start, -1, true);
        PlaceBuilding(BuildingType.Warehouse, start + new int2(1, 0), -1, true);
        PlaceBuilding(BuildingType.Farm, start + new int2(0, 1), -1, true);
        PlaceBuilding(BuildingType.Farm, start + new int2(-1, 1), -1, true);

        for (int i = 0; i < 3; i++)
        {
            int fid = CreateFamily(start);
            var husband = CreateCitizen(Gender.Male, 22f + i * 3f, fid);
            var wife = CreateCitizen(Gender.Female, 20f + i * 2f, fid);
            husband.Home = wife.Home = start;
            wife.BirthCooldown = 30f;
            _families[fid].Funds = 45f + i * 10f;

            if (i < 2)
            {
                var child = CreateCitizen(i == 0 ? Gender.Male : Gender.Female, 4f + i * 4f, fid);
                child.Home = start;
            }
        }

        for (int i = 0; i < 2; i++)
        {
            int fid = CreateFamily(start);
            var man = CreateCitizen(Gender.Male, 19f + i * 4f, fid);
            man.Home = start;
            _families[fid].Funds = 25f;
        }

        Debug.Log($"[Kingdom] 初始人口 {_citizens.Count}，家庭 {_families.Count} 户。");
    }

    int2 FindSettlementSite()
    {
        int2 best = int2.zero;
        float bestScore = float.MinValue;

        for (int radius = 0; radius < 200; radius += 4)
        {
            for (int dx = -radius; dx <= radius; dx += 2)
            {
                for (int dy = -radius; dy <= radius; dy += 2)
                {
                    if (Mathf.Abs(dx) != radius && Mathf.Abs(dy) != radius) continue;
                    int2 h = new int2(dx, dy);
                    if (!InBounds(h)) continue;

                    float s = ScoreSettlement(h);
                    if (s > bestScore) { bestScore = s; best = h; }
                }
            }
            if (bestScore > 0) break;
        }
        return best;
    }

    float ScoreSettlement(int2 h)
    {
        if (GetTerrain(h) != Terrain.Grass) return -999f;

        int grass = 0, forest = 0, water = 0, mountain = 0, stone = 0;
        int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
        int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };
        for (int i = 0; i < 8; i++)
        {
            int2 n = new int2(h.x + dx[i], h.y + dy[i]);
            if (!InBounds(n)) continue;
            switch (GetTerrain(n))
            {
                case Terrain.Grass: grass++; break;
                case Terrain.Forest: forest++; break;
                case Terrain.Water:
                case Terrain.River: water++; break;
                case Terrain.Mountain: mountain++; break;
                case Terrain.Stone: stone++; break;
            }
        }

        if (water == 0) return -1f;
        return grass * 3f + forest * 2f + water * 4f + stone * 1f - mountain * 1.5f;
    }

    // ========================================================================
    //  实体创建
    // ========================================================================

    int CreateFamily(int2 home)
    {
        int id = _nextFamilyId++;
        _families[id] = new Family { Id = id, Home = home, Funds = 20f };
        return id;
    }

    Citizen CreateCitizen(Gender gender, float age, int familyId)
    {
        if (_citizens.Count >= MAX_POPULATION) return null;

        var c = new Citizen
        {
            Id = _nextCitizenId++,
            Name = GenerateName(gender),
            Gender = gender,
            Age = age,
            FamilyId = familyId,
            Health = 1f,
        };
        _citizens.Add(c);

        if (familyId >= 0 && _families.TryGetValue(familyId, out var fam))
            fam.Members.Add(c.Id);

        AssignJobByAge(c);
        return c;
    }

    string GenerateName(Gender g)
    {
        string[] maleFirst = { "阿尔", "巴德", "卡尔", "德文", "埃德", "芬恩", "格伦", "汉斯", "伊恩", "乔", "凯", "卢克", "马克", "诺尔", "奥丁" };
        string[] femaleFirst = { "艾拉", "贝拉", "卡拉", "黛娜", "艾玛", "菲娅", "格蕾", "海伦", "艾薇", "简", "凯特", "莉娜", "米娅", "妮娜", "奥拉" };
        string[] lastNames = { "石匠", "铁锤", "麦田", "林地", "河畔", "牧羊", "炭火", "织布", "远行", "寒松", "灰岩", "晨星" };

        string first = g == Gender.Male
            ? maleFirst[_rng.Next(maleFirst.Length)]
            : femaleFirst[_rng.Next(femaleFirst.Length)];
        return first + "·" + lastNames[_rng.Next(lastNames.Length)];
    }

    // ========================================================================
    //  主循环
    // ========================================================================

    void UpdateSimulation(float dt)
    {
        _dayAcc += dt;
        int guard = 0;
        while (_dayAcc >= DAY_LENGTH && guard++ < MAX_TICKS_PER_FRAME)
        {
            _dayAcc -= DAY_LENGTH;
            TickDay();
        }
        if (guard >= MAX_TICKS_PER_FRAME) _dayAcc = 0f;
    }

    void TickDay()
    {
        _day += 1f;
        float yearsPerDay = 1f / DAYS_PER_YEAR;

        for (int i = _citizens.Count - 1; i >= 0; i--)
        {
            var c = _citizens[i];
            if (!c.Alive) continue;
            c.Age += yearsPerDay;
            c.BirthCooldown = Mathf.Max(0f, c.BirthCooldown - 1f);

            UpdateLifeStage(c);
            UpdateHealth(c, yearsPerDay);

            if (c.Health <= 0f) { KillCitizen(c); continue; }
            DoDailyWork(c);
            ConsumeDailyFood(c);
        }

        UpdateFamilies();
        TryMarriage();
        TryBirth();
        UpdateConstruction();
        CollectTaxes();
        SpendBudget();

        if (_kingdom.AtWar) TickWar();
        TickTerritoryExpansion();

        if ((int)_day % 30 == 0) LogStatus();
    }

    // ========================================================================
    //  市民 AI
    // ========================================================================

    void UpdateLifeStage(Citizen c)
    {
        LifeStage stage;
        if (c.Age < 3f) stage = LifeStage.Baby;
        else if (c.Age < 13f) stage = LifeStage.Child;
        else if (c.Age < 18f) stage = LifeStage.Teen;
        else if (c.Age < 60f) stage = LifeStage.Adult;
        else stage = LifeStage.Elder;

        bool wasChild = c.Job == Job.Child || c.Job == Job.None;

        if (stage == LifeStage.Baby || stage == LifeStage.Child || stage == LifeStage.Teen)
            c.Job = Job.Child;
        else if (wasChild)
            AssignJobByAge(c);

        if (stage == LifeStage.Elder && c.Job != Job.Housewife)
            if (_rng.NextDouble() < 0.02) c.Job = Job.None;
    }

    void AssignJobByAge(Citizen c)
    {
        if (c.Age < 16f) { c.Job = Job.Child; return; }

        if (c.Gender == Gender.Male)
        {
            double r = _rng.NextDouble();
            if (c.Age < 45f)
                c.Job = r < 0.45 ? Job.Farmer : (r < 0.80 ? Job.Worker : Job.Soldier);
            else
                c.Job = r < 0.65 ? Job.Farmer : Job.Worker;
        }
        else
        {
            double r = _rng.NextDouble();
            c.Job = r < 0.35 ? Job.Weaver : (r < 0.80 ? Job.Housewife : Job.Logistic);
        }
    }

    void UpdateHealth(Citizen c, float yearsPerDay)
    {
        float decline = 0f;
        if (c.Age > 50f) decline += (c.Age - 50f) * 0.00004f * DAYS_PER_YEAR * yearsPerDay;
        if (c.Age > 75f) decline += 0.0012f;
        if (c.Hunger > 0.7f) decline += (c.Hunger - 0.7f) * 0.02f;

        c.Health -= decline;
        c.Health += _kingdom.Allocation[(int)Budget.Welfare] * 0.00035f;
        c.Health = Mathf.Clamp01(c.Health);

        if (c.FamilyId >= 0 && _families.TryGetValue(c.FamilyId, out var f) && f.HouseBuildingId >= 0)
            c.Health = Mathf.Min(1f, c.Health + 0.0004f);
    }

    void DoDailyWork(Citizen c)
    {
        if (c.Job == Job.Child || c.Job == Job.None) return;

        float wage = c.Job switch
        {
            Job.Farmer => WAGE_FARMER,
            Job.Worker => WAGE_WORKER,
            Job.Weaver => WAGE_WEAVER,
            Job.Logistic => WAGE_LOGISTIC,
            Job.Soldier => WAGE_SOLDIER,
            Job.Housewife => 0f,
            _ => 1.5f
        };

        float eff = Mathf.Clamp01(c.Health) * Mathf.Clamp01(1f - c.Hunger * 0.8f);
        wage *= eff;

        if (c.Job == Job.Soldier)
        {
            float pay = Mathf.Min(wage, _kingdom.Treasury * 0.01f);
            _kingdom.Treasury -= pay;
            wage = pay + wage * 0.4f;
        }

        if (wage > 0f && c.FamilyId >= 0 && _families.TryGetValue(c.FamilyId, out var f))
            f.Funds += wage;
    }

    void ConsumeDailyFood(Citizen c)
    {
        float need = FOOD_COST_PER_PERSON * (c.Age < 12f ? 0.6f : (c.Age > 60f ? 0.8f : 1f));

        if (c.FamilyId >= 0 && _families.TryGetValue(c.FamilyId, out var f))
        {
            if (f.FoodStore >= need) { f.FoodStore -= need; c.Hunger = Mathf.Max(0f, c.Hunger - 0.25f); return; }
            float price = need * 1.6f;
            if (f.Funds >= price) { f.Funds -= price; c.Hunger = Mathf.Max(0f, c.Hunger - 0.25f); return; }
        }
        c.Hunger = Mathf.Min(1f, c.Hunger + 0.06f);
    }

    void KillCitizen(Citizen c)
    {
        c.Alive = false;
        c.Health = 0f;
        if (c.FamilyId >= 0 && _families.TryGetValue(c.FamilyId, out var f))
            f.Members.Remove(c.Id);
        if (c.View != null) { UnityEngine.Object.Destroy(c.View); c.View = null; }
        _citizens.Remove(c);

        if (c.Job == Job.Soldier && _kingdom.AtWar)
            _kingdom.EnemyStrength = Mathf.Max(0, _kingdom.EnemyStrength - 1);
    }

    // ========================================================================
    //  家庭
    // ========================================================================

    void UpdateFamilies()
    {
        List<int> empty = null;
        foreach (var kv in _families)
        {
            var f = kv.Value;
            if (f.Members.Count == 0) { (empty ??= new List<int>()).Add(kv.Key); continue; }

            int adults = 0;
            foreach (var id in f.Members)
            {
                var c = FindCitizen(id);
                if (c != null && c.Age >= 16f && c.Age < 60f) adults++;
            }
            f.Funds += adults * 0.35f;
            TryRequestHouse(f);
        }
        if (empty != null) foreach (var id in empty) _families.Remove(id);
    }

    void TryRequestHouse(Family f)
    {
        if (f.HouseBuildingId >= 0) return;
        if (_day - f.LastBuildAttemptDay < 20f) return;
        if (f.Funds < HOUSE_COST * 0.6f) return;

        int adults = 0;
        foreach (var id in f.Members)
        {
            var c = FindCitizen(id);
            if (c != null && c.Age >= 16f) adults++;
        }
        if (adults < 1) return;

        int2 plot = FindBuildPlot(f.Home, 10);
        if (!InBounds(plot)) { f.LastBuildAttemptDay = _day; return; }

        var b = PlaceBuilding(BuildingType.House, plot, f.Id, false);
        if (b == null) { f.LastBuildAttemptDay = _day; return; }

        f.HouseBuildingId = b.Id;
        f.LastBuildAttemptDay = _day;
        f.Funds -= HOUSE_COST * 0.5f;
        Debug.Log($"[家庭 {f.Id}] 申请建造房屋 @ {plot}");
    }

    // ========================================================================
    //  婚配 / 生育
    // ========================================================================

    void TryMarriage()
    {
        List<Citizen> singleMen = null, singleWomen = null;
        foreach (var c in _citizens)
        {
            if (!c.Alive || c.Age < 18f || c.Age > 55f || c.Job == Job.Child) continue;
            if (HasPartnerInFamily(c)) continue;
            if (c.Gender == Gender.Male) (singleMen ??= new List<Citizen>()).Add(c);
            else (singleWomen ??= new List<Citizen>()).Add(c);
        }
        if (singleMen == null || singleWomen == null) return;

        int pairs = Mathf.Min(singleMen.Count, singleWomen.Count);
        for (int i = 0; i < pairs; i++)
        {
            var m = singleMen[i];
            var w = singleWomen[i];
            if (Mathf.Abs(m.Age - w.Age) > 18f) continue;
            if (_rng.NextDouble() > 0.35) continue;
            Marry(m, w);
        }
    }

    bool HasPartnerInFamily(Citizen c)
    {
        if (c.FamilyId < 0) return false;
        if (!_families.TryGetValue(c.FamilyId, out var f)) return false;
        foreach (var id in f.Members)
        {
            if (id == c.Id) continue;
            var o = FindCitizen(id);
            if (o != null && o.Gender != c.Gender && o.Age >= 16f) return true;
        }
        return false;
    }

    void Marry(Citizen a, Citizen b)
    {
        int fid = a.FamilyId >= 0 ? a.FamilyId : CreateFamily(a.Home);
        if (a.FamilyId != fid && a.FamilyId >= 0) _families[a.FamilyId].Members.Remove(a.Id);
        if (b.FamilyId >= 0 && _families.TryGetValue(b.FamilyId, out var bf)) bf.Members.Remove(b.Id);

        a.FamilyId = fid; b.FamilyId = fid;
        if (!_families[fid].Members.Contains(a.Id)) _families[fid].Members.Add(a.Id);
        if (!_families[fid].Members.Contains(b.Id)) _families[fid].Members.Add(b.Id);

        if (b.Gender == Gender.Female && _rng.NextDouble() < 0.6) b.Job = Job.Housewife;
        b.BirthCooldown = 20f;
        _families[fid].Funds += 15f;
        Debug.Log($"[婚姻] {a.Name} 与 {b.Name} 组建家庭 {fid}");
    }

    void TryBirth()
    {
        if (_citizens.Count >= MAX_POPULATION) return;

        foreach (var kv in _families)
        {
            var f = kv.Value;
            if (f.Members.Count < 2) continue;

            Citizen mother = null, father = null;
            foreach (var id in f.Members)
            {
                var c = FindCitizen(id);
                if (c == null) continue;
                if (c.Gender == Gender.Female && c.Age >= 18f && c.Age <= 42f) mother = c;
                if (c.Gender == Gender.Male && c.Age >= 18f && c.Age <= 60f) father = c;
            }
            if (mother == null || father == null || mother.BirthCooldown > 0f) continue;

            bool hasHouse = f.HouseBuildingId >= 0 &&
                            _buildingAt.TryGetValue(f.HouseHex, out var hb) && hb.Complete;

            float w = 0.0025f;
            w *= Mathf.Clamp01(mother.Health) * Mathf.Clamp01(father.Health);
            w *= hasHouse ? 2.2f : 1.0f;
            w *= Mathf.Clamp(f.Funds / 80f, 0.4f, 2.0f);
            w *= 1f + _kingdom.Allocation[(int)Budget.Welfare] * 1.5f;

            if (_rng.NextDouble() < w)
            {
                Gender g = _rng.NextDouble() < 0.5 ? Gender.Male : Gender.Female;
                var baby = CreateCitizen(g, 0f, f.Id);
                if (baby != null)
                {
                    baby.Home = f.Home;
                    mother.BirthCooldown = 260f + (float)_rng.NextDouble() * 200f;
                    Debug.Log($"[生育] {mother.Name} 生下{(g == Gender.Male ? "男孩" : "女孩")}，家庭 {f.Id} 现有 {f.Members.Count} 人");
                }
            }
        }
    }

    // ========================================================================
    //  建造
    // ========================================================================

    Building PlaceBuilding(BuildingType type, int2 hex, int ownerFamilyId, bool instantComplete)
    {
        if (!InBounds(hex) || _buildingAt.ContainsKey(hex)) return null;
        Terrain t = GetTerrain(hex);
        if (t == Terrain.Water || t == Terrain.Mountain) return null;

        float work = type switch
        {
            BuildingType.House => HOUSE_WORK,
            BuildingType.Farm => 25f,
            BuildingType.Workshop => 45f,
            BuildingType.Barracks => 60f,
            BuildingType.Wall => 20f,
            BuildingType.Warehouse => 35f,
            BuildingType.Well => 15f,
            _ => 30f
        };

        var b = new Building
        {
            Id = _nextBuildingId++,
            Type = type,
            Hex = hex,
            WorkRequired = work,
            WorkDone = instantComplete ? work : 0f,
            Complete = instantComplete,
            OwnerFamilyId = ownerFamilyId,
        };
        _buildings.Add(b);
        _buildingAt[hex] = b;
        CreateBuildingView(b);

        if (type == BuildingType.House && ownerFamilyId >= 0 && instantComplete &&
            _families.TryGetValue(ownerFamilyId, out var f))
        {
            f.HouseBuildingId = b.Id;
            f.HouseHex = hex;
        }
        return b;
    }

    void UpdateConstruction()
    {
        if (_buildings.Count == 0) return;

        int totalWorkers = 0;
        foreach (var c in _citizens)
            if (c.Alive && c.Job == Job.Worker && c.Age >= 16f && c.Age < 65f) totalWorkers++;
        if (totalWorkers == 0) return;

        List<Building> sites = null;
        foreach (var b in _buildings)
            if (!b.Complete) (sites ??= new List<Building>()).Add(b);
        if (sites == null) return;

        sites.Sort((x, y) => Priority(x.Type).CompareTo(Priority(y.Type)));

        float power = totalWorkers * 1.0f * (1f + _kingdom.Allocation[(int)Budget.Infrastructure] * 0.8f);
        foreach (var b in sites)
        {
            if (power <= 0f) break;
            float need = b.WorkRequired - b.WorkDone;
            float assign = Mathf.Min(power, need);
            b.WorkDone += assign;
            power -= assign;
            if (b.WorkDone >= b.WorkRequired - 0.001f) CompleteBuilding(b);
        }
    }

    static int Priority(BuildingType t) => t switch
    {
        BuildingType.House => 0,
        BuildingType.Farm => 1,
        BuildingType.Well => 2,
        BuildingType.Warehouse => 3,
        BuildingType.Wall => 4,
        BuildingType.Workshop => 5,
        BuildingType.Barracks => 6,
        _ => 9
    };

    void CompleteBuilding(Building b)
    {
        b.Complete = true;
        b.WorkDone = b.WorkRequired;

        if (b.Type == BuildingType.House && b.OwnerFamilyId >= 0 &&
            _families.TryGetValue(b.OwnerFamilyId, out var f))
        {
            f.HouseBuildingId = b.Id;
            f.HouseHex = b.Hex;
            f.Home = b.Hex;
            f.Funds = Mathf.Max(0f, f.Funds - HOUSE_COST * 0.5f);
            foreach (var id in f.Members)
            {
                var c = FindCitizen(id);
                if (c != null) c.Home = b.Hex;
            }
            Debug.Log($"[建造] 家庭 {f.Id} 的房屋完工 @ {b.Hex}");
        }
        else if (b.Type == BuildingType.Wall)
        {
            float d = GridDistance(b.Hex, _kingdom.Capital);
            if (d > _kingdom.WallRadius)
            {
                _kingdom.WallRadius = d;
                Debug.Log($"[领地] 城墙扩张至半径 {_kingdom.WallRadius:F0} 格");
            }
        }
    }

    void TickTerritoryExpansion()
    {
        int pop = _citizens.Count;
        float desired = 4f + Mathf.Sqrt(pop) * 1.4f;
        if (desired <= _kingdom.WallRadius + 2f) return;

        float angle = (float)_rng.NextDouble() * Mathf.PI * 2f;
        int radius = Mathf.CeilToInt(_kingdom.WallRadius) + 1;
        int2 target = HexRoundPolar(_kingdom.Capital, angle, radius);
        if (InBounds(target) && !_buildingAt.ContainsKey(target) && GetTerrain(target) != Terrain.Water)
            PlaceBuilding(BuildingType.Wall, target, -1, false);
    }

    static int2 HexRoundPolar(int2 center, float angle, float radius)
    {
        float3 wc = Hex.GetPositon(center);
        float wx = wc.x + Mathf.Cos(angle) * radius;
        float wz = wc.z + Mathf.Sin(angle) * radius;
        return Hex.GetGridxy(new float2(wx, wz));
    }

    int2 FindBuildPlot(int2 near, int searchRadius)
    {
        for (int attempt = 0; attempt < 40; attempt++)
        {
            int dx = _rng.Next(-searchRadius, searchRadius + 1);
            int dy = _rng.Next(-searchRadius, searchRadius + 1);
            int2 p = new int2(near.x + dx, near.y + dy);

            if (!InBounds(p) || _buildingAt.ContainsKey(p)) continue;
            Terrain t = GetTerrain(p);
            if (t == Terrain.Water || t == Terrain.Mountain || t == Terrain.River) continue;

            int[] ddx = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] ddy = { -1, -1, -1, 0, 0, 1, 1, 1 };
            bool tooClose = false;
            for (int i = 0; i < 8; i++)
            {
                int2 n = new int2(p.x + ddx[i], p.y + ddy[i]);
                if (_buildingAt.ContainsKey(n)) { tooClose = true; break; }
            }
            if (tooClose && _rng.NextDouble() < 0.7) continue;
            return p;
        }
        return new int2(int.MinValue, int.MinValue);
    }

    // ========================================================================
    //  经济
    // ========================================================================

    void CollectTaxes()
    {
        _kingdom.DailyIncome = 0f;
        _kingdom.TotalTaxpayers = 0;
        float rate = Mathf.Clamp(_kingdom.TaxRate, 0f, 0.75f);

        foreach (var kv in _families)
        {
            var f = kv.Value;
            if (f.Funds <= 0f) continue;
            float tax = f.Funds * rate * 0.02f;
            if (tax <= 0f) continue;
            f.Funds -= tax;
            _kingdom.Treasury += tax;
            _kingdom.DailyIncome += tax;
            _kingdom.TotalTaxpayers += f.Members.Count;
        }

        if (rate > 0.35f)
            foreach (var c in _citizens)
                c.Health = Mathf.Max(0f, c.Health - (rate - 0.35f) * 0.0004f);
    }

    void SpendBudget()
    {
        _kingdom.DailyExpense = 0f;
        float treasury = _kingdom.Treasury;
        if (treasury <= 0f) return;

        float infra = _kingdom.Allocation[(int)Budget.Infrastructure] * treasury * 0.02f;
        float mil = _kingdom.Allocation[(int)Budget.Military] * treasury * 0.02f;
        float welfare = _kingdom.Allocation[(int)Budget.Welfare] * treasury * 0.02f;

        if (infra > 0f)
        {
            float remaining = infra;
            foreach (var b in _buildings)
            {
                if (b.Complete || remaining <= 0f) continue;
                float need = b.WorkRequired - b.WorkDone;
                float add = Mathf.Min(need, remaining * 2f);
                b.WorkDone += add;
                remaining -= add * 0.5f;
                if (b.WorkDone >= b.WorkRequired - 0.001f) CompleteBuilding(b);
            }
            _kingdom.Treasury -= infra;
            _kingdom.DailyExpense += infra;
        }

        if (mil > 0f)
        {
            int target = Mathf.RoundToInt(mil * 1.5f);
            for (int i = 0; i < target && i < 4; i++)
            {
                var c = FindRecruitable();
                if (c == null) break;
                c.Job = Job.Soldier;
            }
            _kingdom.Treasury -= mil;
            _kingdom.DailyExpense += mil;
        }

        if (welfare > 0f)
        {
            float per = welfare / Mathf.Max(1, _families.Count);
            foreach (var kv in _families) kv.Value.FoodStore += per * 0.5f;
            _kingdom.Treasury -= welfare;
            _kingdom.DailyExpense += welfare;
        }

        if (_kingdom.Treasury < 0f) _kingdom.Treasury = 0f;
    }

    Citizen FindRecruitable()
    {
        foreach (var c in _citizens)
        {
            if (!c.Alive || c.Gender != Gender.Male) continue;
            if (c.Age < 18f || c.Age > 40f || c.Job == Job.Soldier || c.Health < 0.6f) continue;
            return c;
        }
        return null;
    }

    // ========================================================================
    //  战争
    // ========================================================================

    void TickWar()
    {
        int soldiers = 0;
        foreach (var c in _citizens) if (c.Alive && c.Job == Job.Soldier) soldiers++;

        float myPower = soldiers * (1f + _kingdom.Allocation[(int)Budget.Military] * 2f);
        if (myPower > _kingdom.EnemyStrength)
        {
            _kingdom.EnemyStrength -= Mathf.RoundToInt(myPower * 0.05f);
            if (_kingdom.EnemyStrength <= 0)
            {
                _kingdom.AtWar = false;
                _kingdom.Treasury += 300f;
                Debug.Log("[战争] 胜利！获得战利品 300 金。");
            }
        }
        else if (_rng.NextDouble() < 0.05)
        {
            var v = FindRandomSoldier();
            if (v != null) { KillCitizen(v); Debug.Log($"[战争] {v.Name} 阵亡。"); }
        }
    }

    Citizen FindRandomSoldier()
    {
        List<Citizen> pool = null;
        foreach (var c in _citizens)
            if (c.Alive && c.Job == Job.Soldier)
                (pool ??= new List<Citizen>()).Add(c);
        if (pool == null) return null;
        return pool[_rng.Next(pool.Count)];
    }

    public void DeclareWar(int enemy)
    {
        _kingdom.AtWar = true;
        _kingdom.EnemyStrength = enemy;
        Debug.Log($"[国王] 对敌宣战！敌军强度 {enemy}");
    }

    // ========================================================================
    //  视觉
    // ========================================================================

    void CreateBuildingView(Building b)
    {
        var go = new GameObject($"B_{b.Type}_{b.Id}");
        go.transform.SetParent(_buildingRoot.transform, false);

        var mf = go.AddComponent<MeshFilter>();
        var mr = go.AddComponent<MeshRenderer>();
        mf.sharedMesh = _buildingMesh;

        var mpb = new MaterialPropertyBlock();
        Color col = BuildingColor(b.Type);
        SetColor(mpb, col, b.Complete ? 1f : 0.55f);
        mr.SetPropertyBlock(mpb);
        mr.sharedMaterial = _unitMaterial;
        mr.shadowCastingMode = ShadowCastingMode.Off;
        mr.receiveShadows = false;

        float3 wp = Hex.GetPositon(b.Hex);
        float size = b.Type == BuildingType.Wall ? 1.05f : 1.35f;
        float hgt = b.Type == BuildingType.Wall ? 0.35f
                  : b.Type == BuildingType.Warehouse ? 1.1f
                  : b.Type == BuildingType.Barracks ? 1.15f
                  : 0.9f;

        go.transform.position = new Vector3(wp.x, 0.02f, wp.z);
        go.transform.localScale = new Vector3(size, hgt, size);

        b.View = go;
        b.Mpb = mpb;
    }

    static Color BuildingColor(BuildingType t) => t switch
    {
        BuildingType.House => new Color(0.85f, 0.55f, 0.32f),
        BuildingType.Farm => new Color(0.86f, 0.80f, 0.36f),
        BuildingType.Workshop => new Color(0.55f, 0.42f, 0.34f),
        BuildingType.Barracks => new Color(0.70f, 0.24f, 0.22f),
        BuildingType.Wall => new Color(0.60f, 0.60f, 0.64f),
        BuildingType.Warehouse => new Color(0.66f, 0.52f, 0.30f),
        BuildingType.Well => new Color(0.32f, 0.60f, 0.85f),
        _ => Color.white
    };

    void UpdateUnitVisuals()
    {
        _labelRenderer.cam = _cinemachine.cam != null ? _cinemachine.cam : Camera.main;
        _labelRenderer.Labels.Clear();

        bool showLabels = _camSize <= 200f;
        int labelBudget = 200;

        foreach (var c in _citizens)
        {
            if (!c.Alive) continue;

            if (c.View == null)
            {
                var go = new GameObject($"C_{c.Id}");
                go.transform.SetParent(_unitRoot.transform, false);
                var mf = go.AddComponent<MeshFilter>();
                var mr = go.AddComponent<MeshRenderer>();
                mf.sharedMesh = _citizenMesh;
                mr.sharedMaterial = _unitMaterial;
                mr.shadowCastingMode = ShadowCastingMode.Off;
                mr.receiveShadows = false;
                c.View = go;
                c.Mpb = new MaterialPropertyBlock();
            }

            SetColor(c.Mpb, CitizenColor(c), 1f);
            c.View.GetComponent<MeshRenderer>().SetPropertyBlock(c.Mpb);

            float3 wp = Hex.GetPositon(c.Home);
            float jx = ((c.Id * 37) % 100) / 100f - 0.5f;
            float jz = ((c.Id * 71) % 100) / 100f - 0.5f;
            var pos = new Vector3(wp.x + jx * 0.55f, 0.02f, wp.z + jz * 0.55f);
            c.View.transform.position = pos;

            float size = c.Age < 13f ? 0.7f : (c.Age > 60f ? 0.85f : 1f);
            float height = c.Age < 13f ? 0.7f : (c.Age > 60f ? 0.9f : 1.1f);
            c.View.transform.localScale = new Vector3(size, height, size);

            // 头顶 UI
            if (showLabels && _labelRenderer.Labels.Count < labelBudget)
            {
                var head = pos + new Vector3(0f, 0.55f * height + 0.15f, 0f);
                _labelRenderer.Labels.Add(new KingdomLabelRenderer.LabelEntry
                {
                    WorldPos = head,
                    Text = $"{c.Name} {c.Age:F0}·{ShortJob(c.Job)}",
                    Color = CitizenLabelColor(c)
                });
            }
        }
    }

    static string ShortJob(Job j) => j switch
    {
        Job.Child => "幼",
        Job.Worker => "工",
        Job.Farmer => "农",
        Job.Soldier => "兵",
        Job.Weaver => "织",
        Job.Housewife => "家",
        Job.Logistic => "勤",
        Job.Merchant => "商",
        _ => "闲"
    };

    static Color CitizenLabelColor(Citizen c)
    {
        if (c.Hunger > 0.6f || c.Health < 0.4f) return new Color(1f, 0.45f, 0.45f);
        if (c.Age < 13f) return new Color(0.95f, 0.90f, 0.65f);
        if (c.Age > 60f) return new Color(0.80f, 0.80f, 0.85f);
        return c.Gender == Gender.Male
            ? new Color(0.60f, 0.78f, 1f)
            : new Color(1f, 0.72f, 0.82f);
    }

    Color CitizenColor(Citizen c)
    {
        Color baseCol;
        if (c.Age < 13f) baseCol = new Color(0.95f, 0.85f, 0.55f);
        else if (c.Age > 60f) baseCol = new Color(0.72f, 0.72f, 0.78f);
        else
            baseCol = c.Gender == Gender.Male
                ? new Color(0.28f, 0.45f, 0.80f)
                : new Color(0.88f, 0.42f, 0.58f);

        switch (c.Job)
        {
            case Job.Soldier: baseCol = Color.Lerp(baseCol, new Color(0.85f, 0.15f, 0.15f), 0.65f); break;
            case Job.Worker: baseCol = Color.Lerp(baseCol, new Color(0.95f, 0.65f, 0.15f), 0.55f); break;
            case Job.Farmer: baseCol = Color.Lerp(baseCol, new Color(0.35f, 0.78f, 0.28f), 0.55f); break;
            case Job.Weaver: baseCol = Color.Lerp(baseCol, new Color(0.82f, 0.35f, 0.88f), 0.55f); break;
            case Job.Logistic: baseCol = Color.Lerp(baseCol, new Color(0.30f, 0.82f, 0.88f), 0.55f); break;
        }

        if (c.Hunger > 0.6f || c.Health < 0.4f) baseCol *= 0.55f;
        return baseCol;
    }

    static void SetColor(MaterialPropertyBlock mpb, Color c, float alpha)
    {
        Color cc = c; cc.a = alpha;
        mpb.SetColor("_BaseColor", cc);
        mpb.SetColor("_UnlitColor", cc);
        mpb.SetColor("_Color", cc);
        mpb.SetColor("_EmissiveColor", c * 0.25f);
    }

    // ========================================================================
    //  输入 / 相机
    // ========================================================================

    void HandleInput(float dt)
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.spaceKey.wasPressedThisFrame) { _paused = !_paused; Debug.Log(_paused ? "[国王] 暂停" : "[国王] 继续"); }
        if (kb.digit1Key.wasPressedThisFrame) { _kingdom.TaxRate = Mathf.Clamp(_kingdom.TaxRate + 0.03f, 0f, 0.75f); Debug.Log($"[国王] 税率 -> {_kingdom.TaxRate:P0}"); }
        if (kb.digit2Key.wasPressedThisFrame) { _kingdom.TaxRate = Mathf.Clamp(_kingdom.TaxRate - 0.03f, 0f, 0.75f); Debug.Log($"[国王] 税率 -> {_kingdom.TaxRate:P0}"); }
        if (kb.digit3Key.wasPressedThisFrame) { _kingdom.Allocation[0] += 0.1f; _kingdom.NormalizeAllocation(); Debug.Log($"[国王] 基建 -> {_kingdom.Allocation[0]:P0}"); }
        if (kb.digit4Key.wasPressedThisFrame) { _kingdom.Allocation[1] += 0.1f; _kingdom.NormalizeAllocation(); Debug.Log($"[国王] 军费 -> {_kingdom.Allocation[1]:P0}"); }
        if (kb.digit5Key.wasPressedThisFrame) { _kingdom.Allocation[2] += 0.1f; _kingdom.NormalizeAllocation(); Debug.Log($"[国王] 民生 -> {_kingdom.Allocation[2]:P0}"); }
        if (kb.rKey.wasPressedThisFrame) LogStatus();
        if (kb.hKey.wasPressedThisFrame)
        {
            foreach (var kv in _families)
            {
                kv.Value.LastBuildAttemptDay = -999f;
                kv.Value.Funds = Mathf.Max(kv.Value.Funds, HOUSE_COST);
            }
            Debug.Log("[国王] 已下令加快建房");
        }
        if (kb.wKey.wasPressedThisFrame && kb.leftShiftKey.isPressed) DeclareWar(20 + _citizens.Count / 5);
    }

    void UpdateCamera(float dt)
    {
        var kb = Keyboard.current;
        float speed = _camSize * 0.9f * dt;

        if (kb != null)
        {
            if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) _camTarget.x -= speed;
            if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) _camTarget.x += speed;
            if (kb.upArrowKey.isPressed) _camTarget.z += speed;
            if (kb.downArrowKey.isPressed) _camTarget.z -= speed;
        }

        var mouse = Mouse.current;
        if (mouse != null)
        {
            float scroll = mouse.scroll.ReadValue().y;
            if (Mathf.Abs(scroll) > 0.01f)
                _camSize = Mathf.Clamp(_camSize * (1f - scroll * 0.10f), CAM_MIN, CAM_MAX);
        }

        Vector3 cur = _cinemachine.GetPosition();
        Vector3 next = Vector3.Lerp(cur, _camTarget, 1f - Mathf.Exp(-10f * dt));
        next.y = 100f;
        _cinemachine.SetPosition(next);
        _cinemachine.SetOrthoSize(_camSize);
    }

    // ========================================================================
    //  工具
    // ========================================================================

    bool InBounds(int2 p) => p.x >= _mapMin.x && p.x < _mapMax.x && p.y >= _mapMin.y && p.y < _mapMax.y;

    Terrain GetTerrain(int2 p)
    {
        if (!InBounds(p)) return Terrain.Water;
        return _terrain[(p.y - _mapMin.y) * MAP_SIZE + (p.x - _mapMin.x)];
    }

    Citizen FindCitizen(int id)
    {
        for (int i = 0; i < _citizens.Count; i++)
            if (_citizens[i].Id == id) return _citizens[i];
        return null;
    }

    static float GridDistance(int2 a, int2 b)
    {
        float3 wa = Hex.GetPositon(a);
        float3 wb = Hex.GetPositon(b);
        return math.distance(wa.xz, wb.xz);
    }

    void LogStatus()
    {
        int males = 0, females = 0, children = 0, elders = 0, soldiers = 0, workers = 0;
        foreach (var c in _citizens)
        {
            if (c.Gender == Gender.Male) males++; else females++;
            if (c.Age < 16f) children++;
            else if (c.Age > 60f) elders++;
            if (c.Job == Job.Soldier) soldiers++;
            if (c.Job == Job.Worker) workers++;
        }

        int done = 0, pending = 0;
        foreach (var b in _buildings) { if (b.Complete) done++; else pending++; }

        Debug.Log(
            $"===== 王国日报 · 第 {(int)_day} 天 =====\n" +
            $"人口 {_citizens.Count}（男 {males} / 女 {females} / 儿童 {children} / 老人 {elders}）\n" +
            $"家庭 {_families.Count} 户 · 建筑 {done} 座（在建 {pending}）\n" +
            $"国库 {_kingdom.Treasury:F1} · 税率 {_kingdom.TaxRate:P0} · 日税收 {_kingdom.DailyIncome:F2}\n" +
            $"预算  基建 {_kingdom.Allocation[0]:P0} / 军费 {_kingdom.Allocation[1]:P0} / 民生 {_kingdom.Allocation[2]:P0} / 储备 {_kingdom.Allocation[3]:P0}\n" +
            $"军队 {soldiers} 人 · 工人 {workers} 人 · 领地半径 {_kingdom.WallRadius:F0} 格 · {(_kingdom.AtWar ? "【战争中】" : "和平")}"
        );
    }
}

#endregion