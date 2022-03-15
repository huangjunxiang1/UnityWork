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

    [MenuItem("Tools/生成InputSystem代码")]
    static void CreateInputSystemCode()
    {
        if (Selection.activeObject is not UnityEngine.InputSystem.InputActionAsset input) return;
        StringBuilder str = new StringBuilder();
        str.AppendLine(@"using UnityEngine.InputSystem;
using Main;

public class SInput
{
    public SInput(InputActionAsset asset)
    {
        this.Asset = asset;");

        foreach (var item in input.actionMaps)
        {
            str.AppendLine($"        this.{item.name} = asset.FindActionMap(\"{item.name}\", true);");
            foreach (var act in item.actions)
            {
                str.AppendLine($"        this.{item.name}{act.name} = this.{item.name}.FindAction(\"{act.name}\");");
            }
        }

        str.AppendLine(@"    }

    public InputActionAsset Asset { get; }
");

        foreach (var item in input.actionMaps)
        {
            str.AppendLine($"    public InputActionMap {item.name} {{ get; }}");
            foreach (var act in item.actions)
            {
                str.AppendLine($"    public InputAction {item.name}{act.name} {{ get; }}");
            }
        }

        str.AppendLine(@"
    public void Dispose()
    {
        AssetLoad.DefaultLoader.Release(Asset);
    }
}");
        if (!Directory.Exists(Application.dataPath + "/Code/HotFix/Game/Input/"))
            Directory.CreateDirectory(Application.dataPath + "/Code/HotFix/Game/Input/");
        File.WriteAllText(Application.dataPath + "/Code/HotFix/Game/Input/SInput.cs", str.ToString());
        AssetDatabase.Refresh();
    }
}