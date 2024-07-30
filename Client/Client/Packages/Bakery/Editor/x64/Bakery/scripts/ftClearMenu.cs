// Disable 'obsolete' warnings
#pragma warning disable 0618

using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ftClearMenu : EditorWindow
{
    public enum SceneClearingMode
    {
        nothing = 0,
        lightmapReferences = 1,
        lightmapReferencesAndBakeSettings = 2
    }

    static public string[] options = new string[] {"Nothing", "Baked data references", "All (data and bake settings)"};

    public SceneClearingMode sceneClearingMode = SceneClearingMode.lightmapReferences;
    public bool clearLightmapFiles = false;
    public bool clearVertexStreams = false;

    [MenuItem("Bakery/Utilities/Clear baked data", false, 44)]
    private static void ClearBakedDataShow()
    {
        var instance = (ftClearMenu)GetWindow(typeof(ftClearMenu));
        instance.titleContent.text = "Clear Baked Data";
        instance.minSize = new Vector2(250, 180);
        instance.maxSize = new Vector2(instance.minSize.x, instance.minSize.y + 1);
        instance.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins);
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Clear from scenes:", EditorStyles.boldLabel);
        GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
        sceneClearingMode = (SceneClearingMode)EditorGUILayout.Popup("", (int)sceneClearingMode, options, GUILayout.ExpandWidth(true));
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Delete:", EditorStyles.boldLabel);
        GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
        EditorGUI.indentLevel++;
        clearLightmapFiles = EditorGUILayout.ToggleLeft(" Lightmap Files", clearLightmapFiles);
        clearVertexStreams = EditorGUILayout.ToggleLeft(" Vertex Lightmap Streams", clearVertexStreams);
        EditorGUI.indentLevel--;
        GUILayout.Space(20);


        if (GUILayout.Button("Clear", GUILayout.Height(24)))
        {
            string txt = "";
            if (sceneClearingMode == SceneClearingMode.nothing)
            {
                if (clearLightmapFiles) { txt += "Delete currently used lightmap files"; }

                if (clearVertexStreams) { txt += " and vertex lightmap stream assets"; }
                else
                {
                    EditorGUILayout.EndVertical();
                    return;
                }
            }
            else
            {
                if (sceneClearingMode == SceneClearingMode.lightmapReferences) { txt = "Clear all Bakery data for currently loaded scenes"; }
                else { txt = "Clear all Bakery data and settings for currently loaded scenes"; }

                if (clearLightmapFiles && clearVertexStreams) txt += ", currently used lightmap files, and vertex lightmap stream assets";
                if (clearLightmapFiles && !clearVertexStreams) txt += " and delete currently used lightmap files";
                if (clearVertexStreams && !clearLightmapFiles) txt += " and vertex lightmap stream assets";
            }

            if (EditorUtility.DisplayDialog("Bakery", txt + "?", "Yes", "No")) { ClearBakedData(sceneClearingMode, clearLightmapFiles, clearVertexStreams); }
        }

        EditorGUILayout.EndVertical();
    }

    static void RemoveFiles(Texture2D map)
    {
        var path = AssetDatabase.GetAssetPath(map);
        AssetDatabase.DeleteAsset(path);
        ftRenderLightmap.DebugLogInfo("Deleted " + path);
    }

    static void RemoveFiles(List<Texture2D> maps)
    {
        for(int i=0; i<maps.Count; i++)
        {
            RemoveFiles(maps[i]);
        }
    }
    
    static void RemoveFiles(Mesh mesh)
    {
        var path = AssetDatabase.GetAssetPath(mesh);
        AssetDatabase.DeleteAsset(path);
        ftRenderLightmap.DebugLogInfo("Deleted " + path);
    }
    
    static void RemoveFiles(List<Mesh> meshes)
    {
        for(int i=0; i<meshes.Count; i++)
        {
            var mesh = meshes[i];
            if (mesh == null) continue;
            RemoveFiles(mesh);
        }
    }

    public static void ClearBakedData(SceneClearingMode sceneClearMode, bool removeLightmapFiles, bool removeVertexStreams = false)
    {
        if (removeLightmapFiles || removeVertexStreams)
        {
            var sceneCount = SceneManager.sceneCount;
            for(int i=0; i<sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded) continue;
                var go = ftLightmaps.FindInScene("!ftraceLightmaps", scene);
                if (go == null) continue;
                var storage = go.GetComponent<ftLightmapsStorage>();
                if (storage == null) continue;

                RemoveFiles(storage.maps);
                RemoveFiles(storage.masks);
                RemoveFiles(storage.dirMaps);
                RemoveFiles(storage.rnmMaps0);
                RemoveFiles(storage.rnmMaps1);
                RemoveFiles(storage.rnmMaps2);
                if (removeVertexStreams) { RemoveFiles(storage.bakedVertexColorMesh); }
            }
        }

        if (sceneClearMode == SceneClearingMode.lightmapReferences)
        {
            var newStorages = new List<GameObject>();
            var sceneCount = SceneManager.sceneCount;
            for(int i=0; i<sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded) continue;
                var go = ftLightmaps.FindInScene("!ftraceLightmaps", scene);
                if (go == null) continue;
                var storage = go.GetComponent<ftLightmapsStorage>();
                if (storage != null)
                {
                    var newGO = new GameObject();
                    newGO.hideFlags = HideFlags.HideInHierarchy;
                    var newStorage = newGO.AddComponent<ftLightmapsStorage>();
                    ftLightmapsStorage.CopySettings(storage, newStorage);
                    newStorages.Add(newGO);
                }
                Undo.DestroyObjectImmediate(go);
            }
            LightmapSettings.lightmaps = new LightmapData[0];
            for(int i=0; i<newStorages.Count; i++)
            {
                newStorages[i].name = "!ftraceLightmaps";
            }
        }
        else if (sceneClearMode == SceneClearingMode.lightmapReferencesAndBakeSettings)
        {
            var sceneCount = SceneManager.sceneCount;
            for(int i=0; i<sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded) continue;
                var go = ftLightmaps.FindInScene("!ftraceLightmaps", scene);
                if (go == null) continue;
                Undo.DestroyObjectImmediate(go);
            }
            LightmapSettings.lightmaps = new LightmapData[0];
        }

#if UNITY_2017_3_OR_NEWER
        var lights = FindObjectsOfType<Light>() as Light[];
        for(int i=0; i<lights.Length; i++)
        {
            var output = lights[i].bakingOutput;
            output.isBaked = false;
            lights[i].bakingOutput = output;
        }
#endif
    }
}

