using Core;
using System;
using System.IO;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathFindingAStar))]
class PathFindingAStarDrawLine : Editor
{
    static int selectedOption = 0;
    private string[] options = new string[] { "None", "Enable", "Disable", "Cost" };
    static float3 start;
    static float3 end;
    PathFindingAStar root;
    static int cost = 1;

    private void OnEnable()
    {
        SceneView.duringSceneGui += onScene;

        root = (PathFindingAStar)this.target;
    }
    private void OnDisable()
    {
        SceneView.duringSceneGui -= onScene;
    }
    private void onScene(SceneView sceneView)
    {
        if (root.data == null) return;
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        var currentEvent = UnityEngine.Event.current;
        if (currentEvent != null && selectedOption > 0)
        {
            if (currentEvent.control&& currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    start = hit.point;
                    currentEvent.Use();
                }
            }
            if (currentEvent.control && currentEvent.type == EventType.MouseUp && currentEvent.button == 0)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    end = hit.point;

                    float3 min = math.min(start, end);
                    float3 max = math.max(start, end);
                    int2 min_i = 0; 
                    int2 max_i = 0;
                    if (root.gridType == PathGridType.Rect)
                    {
                        min_i = math.clamp((int2)((min - (float3)root.transform.position) / root.gridSize).xz, 0, root.aStarSize - 1);
                        max_i = math.clamp((int2)((max - (float3)root.transform.position) / root.gridSize).xz, 0, root.aStarSize - 1);
                    }
                    else
                    {
                        min_i = math.clamp(Hex.GetGridxy((min - (float3)root.transform.position).xz, root.gridSize.x, root.hexParityType, root.hexFacingType), 0, root.aStarSize - 1);
                        max_i = math.clamp(Hex.GetGridxy((max - (float3)root.transform.position).xz, root.gridSize.x, root.hexParityType, root.hexFacingType), 0, root.aStarSize - 1);
                    }
                    if (min_i.x >= 0 && min_i.y >= 0 && max_i.x < root.aStarSize.x && max_i.y < root.aStarSize.y)
                    {
                        if (selectedOption == 1)
                        {
                            for (int i = min_i.x; i <= max_i.x; i++)
                            {
                                for (int j = min_i.y; j <= max_i.y; j++)
                                {
                                    root.data[j * root.aStarSize.x + i] |= 1;
                                }
                            }
                        }
                        else if (selectedOption == 2)
                        {
                            for (int i = min_i.x; i <= max_i.x; i++)
                            {
                                for (int j = min_i.y; j <= max_i.y; j++)
                                {
                                    root.data[j * root.aStarSize.x + i] &= 254;
                                }
                            }
                        }
                        else if (selectedOption == 3)
                        {
                            if (cost < 128)
                            {
                                for (int i = min_i.x; i <= max_i.x; i++)
                                {
                                    for (int j = min_i.y; j <= max_i.y; j++)
                                    {
                                        var v = root.data[j * root.aStarSize.x + i];
                                        root.data[j * root.aStarSize.x + i] = (byte)((cost << 1) | (v & 1));
                                    }
                                }
                            }
                            else
                            {
                                Debug.LogError("Cost max = 127");
                            }
                        }
                        root.View(root.view);
                    }
                    currentEvent.Use();
                }
            }
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (root.data == null) return;
        selectedOption = GUILayout.SelectionGrid(selectedOption, options, 1, EditorStyles.radioButton);
        if (selectedOption == 3)
            cost = EditorGUILayout.IntField(cost);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.Space();
        root.view = EditorGUILayout.Toggle("显示A星数据", root.view);
        EditorGUILayout.Space();
        if (EditorGUI.EndChangeCheck())
            root.View(root.view);

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
            root.View(root.view);
        }
        if (GUILayout.Button("加载数据"))
        {
            root.Load();
            root.View(root.view);
        }
        if (GUILayout.Button("保存数据"))
        {
            if (string.IsNullOrEmpty(root.savePath))
            {
                Debug.LogError("path is null");
                return;
            }
            DBuffer buffer = new(10000);
            buffer.Write((int)root.gridType);
            buffer.Write((int)root.hexParityType);
            buffer.Write((int)root.hexFacingType);
            buffer.Write(root.transform.position);
            buffer.Write(root.gridSize);
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
        if (GUILayout.Button("测试"))
        {
            AStarFinder Finder = new();
            Finder.Init(root.astar);
            Finder.Finding(0, new int2(2, 3));
            FastList<int2> point_int = new();
            Finder.GetGrids(point_int);
            if (point_int.Count > 0)
            {
                for (int i = 0; i < point_int.Count; i++)
                    Finder.astar.SetPathOccupation(point_int[i], true, true);
            }
            for (int i = 0; i < Finder.job.nodes.Length; i++)
                Finder.astar.SetPathOccupation(Finder.job.nodes[i].xy, true, false);
            Finder.astar.ChangeHandle();
        }
    }
}
