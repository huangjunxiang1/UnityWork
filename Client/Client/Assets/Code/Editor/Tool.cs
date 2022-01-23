using UnityEditor;
using UnityEngine;
using System.IO;

public class Tool
{
    [MenuItem("Tools/CopyTexture")]
    static void DoIt()
    {
        var fs = Directory.GetFiles(Application.dataPath+"/../../../Art/Texture");
        for (int i = 0; i < fs.Length; i++)
        {
            FileInfo fi = new FileInfo(fs[i]);
            File.Copy(fs[i], Application.dataPath + "/Res/Texture/" + fi.Name, true);
        }
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("成功", "成功", "OK", "取消");
    }
}