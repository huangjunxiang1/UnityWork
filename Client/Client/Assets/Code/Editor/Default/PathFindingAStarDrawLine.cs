using FairyGUI;
using Game;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.UI.CanvasScaler;

[CustomEditor(typeof(PathFindingAStar))]
class PathFindingAStarDrawLine : Editor
{
    static GUIStyle gui = new();
    static GUIStyle gui2 = new();

    bool isInit = false;
    GameObject quad;
    Texture2D texture;
    static int selectedOption = 0;
    private string[] options = new string[] { "Enable", "Disable", "Cost" };
    private string[] viewXYStyle = new string[] { "只显示轴", "全部格子显示" };
    static int viewXYStyleIndex = 0;
    float3 start;
    float3 end;
    PathFindingAStar root;
    static bool viewCost = false;
    static bool viewXY = false;
    static int cost = 1;

    private void OnEnable()
    {
        gui.normal.textColor = Color.yellow;
        gui.fontSize = 30;
        gui2.normal.textColor = Color.blue;
        gui2.fontSize = 20;
        root = (PathFindingAStar)this.target;
        if (!quad)
            quad = EditorUtility.CreateGameObjectWithHideFlags("", HideFlags.HideAndDontSave, typeof(MeshFilter), typeof(MeshRenderer));

        isInit = false;
        Load();
    }
    private void OnDisable()
    {
        if (quad)
            GameObject.DestroyImmediate(quad);
        quad = null;
    }
    private void OnSceneGUI()
    {
        if (root.data == null) return;
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        var currentEvent = UnityEngine.Event.current;
        if (currentEvent != null)
        {
            if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    start = hit.point;
                    currentEvent.Use();
                }
            }
            if (currentEvent.type == EventType.MouseUp && currentEvent.button == 0)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    end = hit.point;

                    float3 min = math.min(start, end);
                    float3 max = math.max(start, end);
                    int2 min_i = math.clamp((int2)((min - (float3)root.transform.position) / root.size).xz, -1, root.aStarSize + 1);
                    int2 max_i = math.clamp((int2)((max - (float3)root.transform.position) / root.size).xz, -1, root.aStarSize + 1);

                    if (min_i.x >= 0 && min_i.y >= 0 && max_i.x < root.aStarSize.x && max_i.y < root.aStarSize.y)
                    {
                        if (selectedOption == 0)
                        {
                            for (int i = min_i.x; i <= max_i.x; i++)
                            {
                                for (int j = min_i.y; j <= max_i.y; j++)
                                {
                                    root.data[j * root.aStarSize.x + i] |= 1;
                                }
                            }
                        }
                        else if (selectedOption == 1)
                        {
                            for (int i = min_i.x; i <= max_i.x; i++)
                            {
                                for (int j = min_i.y; j <= max_i.y; j++)
                                {
                                    root.data[j * root.aStarSize.x + i] &= 254;
                                }
                            }
                        }
                        else if (selectedOption == 2)
                        {
                            if (cost < 128)
                            {
                                for (int i = min_i.x; i <= max_i.x; i++)
                                {
                                    for (int j = min_i.y; j <= max_i.y; j++)
                                    {
                                        var v = root.data[j * root.aStarSize.x + i];
                                        if ((v & 1) != 0)
                                            root.data[j * root.aStarSize.x + i] = (byte)((cost << 1) | 1);
                                    }
                                }
                            }
                            else
                            {
                                Debug.LogError("Cost max = 127");
                            }
                        }
                        Init();
                    }
                    currentEvent.Use();
                }
            }
        }
        for (int i = 0; i < root.aStarSize.x; i++)
        {
            for (int j = 0; j < root.aStarSize.y; j++)
            {
                if (viewCost)
                {
                    int index = j * root.aStarSize.x + i;
                    if (index< root.data.Length)
                    {
                        var d = root.data[index];
                        if ((d & 1) == 1 && d >> 1 != 0)
                            Handles.Label((float3)root.transform.position + new float3(i, 0, j) * root.size + new float3(0, 0, root.size.z), (d >> 1).ToString(), gui);
                    }
                }
                if (viewXY)
                {
                    if (viewXYStyleIndex == 0)
                    {
                        if (i == 0 || j == 0 || i == root.aStarSize.x - 1 || j == root.aStarSize.y - 1)
                            Handles.Label((float3)root.transform.position + new float3(i, 0, j) * root.size + new float3(0, 0, root.size.z / 2), $"({i},{j})", gui2);
                    }
                    else if (viewXYStyleIndex == 1)
                        Handles.Label((float3)root.transform.position + new float3(i, 0, j) * root.size + new float3(0, 0, root.size.z / 2), $"({i},{j})", gui2);
                }
            }
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (root.data == null) return;
        if (!isInit)
        {
            isInit = true;
            Init();
        }
        selectedOption = GUILayout.SelectionGrid(selectedOption, options, 1, EditorStyles.radioButton);
        if (selectedOption == 2)
            cost = EditorGUILayout.IntField(cost);

        EditorGUILayout.Space();
        viewCost = EditorGUILayout.Toggle("显示行动力消耗", viewCost);
        EditorGUILayout.Space();
        viewXY = EditorGUILayout.Toggle("显示坐标", viewXY);
        if (viewXY)
        {
            viewXYStyleIndex = GUILayout.SelectionGrid(viewXYStyleIndex, viewXYStyle, 1, EditorStyles.radioButton);
        }
        EditorGUILayout.Space();

        if (GUILayout.Button("刷新显示"))
        {
            if (!root.dataSize.Equals(root.aStarSize))
            {
                var d2 = new byte[root.aStarSize.x * root.aStarSize.y];
                int2 min = math.min(root.aStarSize, root.dataSize);
                for (int i = 0; i < min.x; i++)
                {
                    for (int j = 0; j < min.y; j++)
                    {
                        d2[j * root.aStarSize.x + i] = root.data[j * root.dataSize.x + i];
                    }
                }
                root.data = d2;
                root.dataSize = root.aStarSize;
            }
            Init();
        }
        if (GUILayout.Button("加载数据"))
        {
            Load();
            Init();
        }
        if (GUILayout.Button("保存数据"))
        {
            if (string.IsNullOrEmpty(root.savePath))
            {
                Debug.LogError("path is null");
                return;
            }
            DBuffer buffer = new(10000);
            buffer.Compress = false;
            buffer.Write(root.transform.position);
            buffer.Write(root.size);
            buffer.Write(root.aStarSize);
            buffer.Write(root.data);

            var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var dir = $"{Application.dataPath}/{root.savePath.Split("Assets").LastOrDefault()}";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string path = $"{dir}/{currentScene.name}.bytes";
            File.WriteAllBytes(path, buffer.ToBytes());
            AssetDatabase.Refresh();
        }
    }

    void Load()
    {
        if (string.IsNullOrEmpty(root.savePath))
        {
            Debug.LogError("path is null");
            return;
        }
        var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        string path = $"{Application.dataPath}/{root.savePath.Split("Assets").LastOrDefault()}/{currentScene.name}.bytes";
        if (File.Exists(path))
        {
            var buffer = new DBuffer(File.ReadAllBytes(path));
            buffer.Compress = false;
            root.transform.position = buffer.Readfloat3();
            root.size = buffer.Readfloat3();
            root.aStarSize = math.max(buffer.Readint2(), 1);
            root.data = buffer.Readbytes();
        }
        else
            root.data = new byte[root.aStarSize.x * root.aStarSize.y];
        root.dataSize = root.aStarSize;
    }
    void Init()
    {
        Mesh mesh = new Mesh();
        var start = (float3)root.transform.position;
        Vector3[] verts = new Vector3[4]
        {
            start,
            start+new float3(root.size.x * root.aStarSize.x,0,0),
            start+new float3(0,0,root.size.z * root.aStarSize.y),
            start+new float3(root.size.x * root.aStarSize.x,0,root.size.z * root.aStarSize.y)
        };
        mesh.vertices = verts;
        mesh.triangles = new int[] { 0, 2, 1, 1, 2, 3 };
        mesh.uv = new Vector2[4]
        {
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(0,1),
            new Vector2(1,1),
        };

        if (texture)
            GameObject.DestroyImmediate(texture);
        texture = new Texture2D(root.aStarSize.x, root.aStarSize.y);
        texture.filterMode = FilterMode.Point;
        for (int i = 0; i < root.data.Length; i++)
        {
            var b = root.data[i];
            texture.SetPixel(i % root.aStarSize.x, i / root.aStarSize.x, ((b & 1) == 0) ? new Color(1, 0, 0, 0.5f) : new Color(0, 1, 0, 0.5f));
        }
        texture.Apply();

        // 应用 Mesh
        quad.GetComponent<MeshFilter>().mesh = mesh;
        var r = quad.GetComponent<MeshRenderer>();
        var mat = new Material(Shader.Find("Editor/AStar"));
        mat.SetTexture("_MainTex", texture);
        mat.SetVector("_Size", new Vector4(root.aStarSize.x, root.aStarSize.y, 0, 0));
        r.sharedMaterial = mat;
    }
}
