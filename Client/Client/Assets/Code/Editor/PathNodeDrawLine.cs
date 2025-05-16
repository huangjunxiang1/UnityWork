using Game;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
internal class PathNodeDrawLine
{
    static PathNodeDrawLine()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        var root = PathFindingNode.GetCurrentRoot();
        if (root == null || root.Nodes.Count == 0) return;

        Handles.color = Color.green;
        foreach (var node in root.Nodes.Values)
        {
            for (int i = 0; i < node.Next.Count; i++)
            {
                Handles.DrawLine(node.position, node.Next[i].Node.position);
            }
            Handles.SphereHandleCap(0, node.position, Quaternion.identity, 0.1f, EventType.Repaint);
        }
    }
}
