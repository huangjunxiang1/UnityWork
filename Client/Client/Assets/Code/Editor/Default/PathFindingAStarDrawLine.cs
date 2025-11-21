using Game;
using System.IO;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathFindingAStar))]
class PathFindingAStarDrawLine : Editor
{
    static GUIStyle gui = new();
    static GUIStyle gui2 = new();

    static int selectedOption = 0;
    private string[] options = new string[] { "None", "Enable", "Disable", "Cost" };
    float3 start;
    float3 end;
    PathFindingAStar root;
    static bool viewAstar = true;
    static int cost = 1;

    private void OnEnable()
    {
        gui.normal.textColor = Color.yellow;
        gui.fontSize = 30;
        gui2.normal.textColor = Color.blue;
        gui2.fontSize = 20;
        root = (PathFindingAStar)this.target;

        if (root.data == null)
            Load();
        root.View(viewAstar);
    }
    private void OnSceneGUI()
    {
        if (root.data == null) return;
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        var currentEvent = UnityEngine.Event.current;
        if (currentEvent != null && selectedOption > 0)
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
                    int2 min_i = math.clamp((int2)((min - (float3)root.transform.position) / root.size).xz, 0, root.aStarSize - 1);
                    int2 max_i = math.clamp((int2)((max - (float3)root.transform.position) / root.size).xz, 0, root.aStarSize - 1);

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
                        root.View(viewAstar);
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
        viewAstar = EditorGUILayout.Toggle("显示A星数据", viewAstar);
        EditorGUILayout.Space();
        if (EditorGUI.EndChangeCheck())
            root.View(viewAstar);

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
            root.View(viewAstar);
        }
        if (GUILayout.Button("加载数据"))
        {
            Load();
            root.View(viewAstar);
        }
        if (GUILayout.Button("保存数据"))
        {
            if (string.IsNullOrEmpty(root.savePath))
            {
                Debug.LogError("path is null");
                return;
            }
            DBuffer buffer = new(10000);
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
        if (!Directory.Exists($"{Application.dataPath}/{root.savePath.Split("Assets").LastOrDefault()}"))
            Directory.CreateDirectory($"{Application.dataPath}/{root.savePath.Split("Assets").LastOrDefault()}");
        if (File.Exists(path))
        {
            var buffer = new DBuffer(File.ReadAllBytes(path));
            root.transform.position = buffer.Readfloat3();
            root.size = buffer.Readfloat3();
            root.aStarSize = math.max(buffer.Readint2(), 1);
            root.data = buffer.Readbytes();
            buffer.Seek(0);
            root.astar = new(buffer);
        }
        else
            root.data = new byte[root.aStarSize.x * root.aStarSize.y];
        root.dataSize = root.aStarSize;
    }
}
