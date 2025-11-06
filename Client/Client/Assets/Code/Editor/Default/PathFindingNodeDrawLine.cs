using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
class PathFindingNodeDrawLine
{
    static GUIStyle gui = new();
    static PathFindingNodeDrawLine()
    {
        gui.normal.textColor = Color.yellow;
        gui.fontSize = 30;
        SceneView.duringSceneGui += OnSceneGUI;
        EditorSceneManager.activeSceneChangedInEditMode += OnEditorSceneChanged;
    }

    private static void OnEditorSceneChanged(UnityEngine.SceneManagement.Scene previousScene, UnityEngine.SceneManagement.Scene newScene)
    {
        PathFindingNode.Clear();
    }
    private static void OnSceneGUI(SceneView sceneView)
    {
        var root = PathFindingNode.GetCurrent();
        if (root == null || root.Nodes.Count == 0) return;

        Handles.color = Color.green;
        foreach (var node in root.Nodes.Values)
        {
            for (int i = 0; i < node.Next.Count; i++)
            {
                Handles.DrawLine(node.position, node.Next[i].Node.position);
            }
            Handles.SphereHandleCap(0, node.position, Quaternion.identity, ShitSettings.Inst.PathNodeDrawLineRadius, EventType.Repaint);
        }
        foreach (var node in root.Nodes.Values)
            Handles.Label(node.position + new Unity.Mathematics.float3(0, ShitSettings.Inst.PathNodeDrawLineRadius, 0), node.id.ToString(), gui);
    }
}
