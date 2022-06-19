using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using FairyGUI;
using Main;
using UnityEngine.InputSystem;

public class Tool
{
    [MenuItem("Tools/生成UGUI代码")]
    static void CreateUICode()
    {
        StringBuilder code = new StringBuilder();

        code.AppendLine(@"using System;");
        code.AppendLine(@"using System.Collections.Generic;");
        code.AppendLine(@"using System.Linq;");
        code.AppendLine(@"using System.Text;");
        code.AppendLine(@"using System.Threading.Tasks;");
        code.AppendLine(@"using Main;");
        code.AppendLine(@"using UnityEngine;");
        code.AppendLine(@"using UnityEngine.UI;");
        code.AppendLine(@"using Game;");

        appendUUICode(Application.dataPath + "/Res/UI/UUI/Prefab/", code);
        File.WriteAllText(Application.dataPath + $"/Code/HotFix/Game/_Gen/UUI.cs", code.ToString());
        AssetDatabase.Refresh();
    }
    static void appendUUICode(string path, StringBuilder code)
    {
        foreach (var d in Directory.GetDirectories(path))
        {
            appendUUICode(d, code);
        }

        List<GameObject> uis = new List<GameObject>();
        var fis = Directory.GetFiles(path);

        foreach (var f in fis)
        {
            if (!f.EndsWith(".prefab")) continue;
            var ff = "Assets" + f.Replace(Application.dataPath, "");
            var g = (GameObject)AssetDatabase.LoadMainAssetAtPath(ff);
            if (g is not GameObject go)
                continue;
            if (!g.name.StartsWith("UUI") && !g.name.StartsWith("UUI3D"))
                continue;
            uis.Add(go);
        }

        foreach (var go in uis)
        {
            if (go.GetComponentInChildren<Canvas>() == null)
            {
                Debug.LogError(go.name + "不是Canvas对象");
                continue;
            }

            StringBuilder str2 = new StringBuilder();
            code.AppendLine(@"");
            if (go.name.StartsWith("UUI3D"))
                code.AppendLine(@$"partial class {go.name} : UUI3D");
            else if (go.name.StartsWith("UUI"))
                code.AppendLine(@$"partial class {go.name} : UUI");
             
            code.AppendLine(@"{");
            code.AppendLine($"    public override string url => \"{AssetDatabase.GetAssetPath(go).Replace(AssetLoad.Directory, "")}\";");

            var childs = go.GetComponentsInChildren<Transform>(true);
            for (int j = 0; j < childs.Length; j++)
            {
                var child = childs[j];
                if (child.name.StartsWith("_"))
                {
                    var coms = child.GetComponents<Component>().ToList();
                    coms.RemoveAll(t =>
                    {
                        if (t is Mask) return true;
                        if (t is ContentSizeFitter) return true;
                        if (t is Shadow) return true;
                        if (t is Outline) return true;
                        if (t is CanvasRenderer) return true;
                        return false;
                    });
                    if (coms.Find(t => t is Selectable) != null)
                        coms.RemoveAll(t => t is UnityEngine.UI.Image);

                    if (coms.Count > 1)
                        coms.Remove(child.GetComponent<Transform>());

                    List<string> paths = new List<string>();
                    var temp = child;
                    paths.Add(temp.name);
                    while (temp.transform.parent != go.transform)
                    {
                        temp = temp.transform.parent;
                        paths.Add(temp.name);
                    }
                    string p = null;
                    for (int k = paths.Count - 1; k >= 0; k--)
                    {
                        p += paths[k];
                        if (k != 0) p += "/";
                    }

                    foreach (var item1 in coms)
                    {
                        code.AppendLine($@"    public {item1.GetType().FullName} {item1.name}{item1.GetType().Name};");
                        str2.AppendLine($@"        this.{item1.name}{item1.GetType().Name} = ({item1.GetType().FullName})this.UI.transform.Find(""{p}"").GetComponent(typeof({item1.GetType().FullName}));");
                    }
                }
            }

            code.AppendLine(@"");
            code.AppendLine(@"    protected override void Binding()");
            code.AppendLine(@"    {");
            code.AppendLine(str2.ToString());
            code.AppendLine(@"    }");
            code.Append(@"}");
        }
    }

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

    [MenuItem("Tools/生成Config代码")]
    static void CreateConfigCode()
    {
        //main
        {
            StringBuilder so = new StringBuilder();
            so.AppendLine("using UnityEngine;");
            so.AppendLine("using Game;");
            so.AppendLine("using Main;");
            so.AppendLine("");
            so.AppendLine("namespace Game");
            so.AppendLine("{");
            so.AppendLine("\tpublic partial class SettingM");
            so.AppendLine("\t{");

            StringBuilder input = new StringBuilder();
            input.AppendLine(@"using UnityEngine.InputSystem;");
            input.AppendLine(@"using Main;");
            input.AppendLine(@"using UnityEngine;");
            input.AppendLine(@"");

            appendConfigWithDirectory(Application.dataPath + "/Res/Config/SO/Main", so, input);

            so.AppendLine("\t}");
            so.AppendLine("}");

            if (!Directory.Exists(Application.dataPath + "/Code/Main/_Gen"))
                Directory.CreateDirectory(Application.dataPath + "/Code/Main/_Gen");

            File.WriteAllText(Application.dataPath + $"/Code/Main/_Gen/Setting.cs", so.ToString());
            File.WriteAllText(Application.dataPath + $"/Code/Main/_Gen/Inputs.cs", input.ToString());
        }
        //hot
        {
            StringBuilder so = new StringBuilder();
            so.AppendLine("using UnityEngine;");
            so.AppendLine("using Game;");
            so.AppendLine("using Main;");
            so.AppendLine("");
            so.AppendLine("namespace Game");
            so.AppendLine("{");
            so.AppendLine("\tpublic partial class SettingL");
            so.AppendLine("\t{");

            StringBuilder input = new StringBuilder();
            input.AppendLine(@"using UnityEngine.InputSystem;");
            input.AppendLine(@"using Main;");
            input.AppendLine(@"using UnityEngine;");
            input.AppendLine(@"");

            appendConfigWithDirectory(Application.dataPath + "/Res/Config/SO/Hot", so, input);

            so.AppendLine("\t}");
            so.AppendLine("}");

            if (!Directory.Exists(Application.dataPath + "/Code/HotFix/Game/Config"))
                Directory.CreateDirectory(Application.dataPath + "/Code/HotFix/Game/Config");

            File.WriteAllText(Application.dataPath + $"/Code/HotFix/Game/_Gen/Setting.cs", so.ToString());
            File.WriteAllText(Application.dataPath + $"/Code/HotFix/Game/_Gen/Inputs.cs", input.ToString());
        }


        AssetDatabase.Refresh();
    }
    static void appendConfigWithDirectory(string path, StringBuilder soCode, StringBuilder inputCode)
    {
        foreach (var d in Directory.GetDirectories(path))
        {
            appendConfigWithDirectory(d, soCode,inputCode);
        }

        foreach (var f in Directory.GetFiles(path))
        {
            if (f.EndsWith(".meta"))
                continue;
            var ff = "Assets" + f.Replace(Application.dataPath, "");
            var o = AssetDatabase.LoadMainAssetAtPath(ff);
            if (o is InputActionAsset input)
                appendInputSystemCode(input, inputCode);
            else if (o is ScriptableObject so)
                appendScriptObjectCode(so, soCode);
        }
    }
    static void appendScriptObjectCode(ScriptableObject so, StringBuilder code)
    {
        var url = AssetDatabase.GetAssetPath(so);
        string type = so.GetType().FullName;
        code.AppendLine($"\t    {type} _{so.name};");
        code.AppendLine($"\t    public {type} {so.name}");
        code.AppendLine("\t    {");
        code.AppendLine("\t        get");
        code.AppendLine("\t        {");
        code.AppendLine($"\t            if (!_{so.name})");
        code.AppendLine($"\t                _{so.name} = ({type})AssetLoad.Load<ScriptableObject>(\"{url.Replace(AssetLoad.Directory, "")}\");");
        code.AppendLine($"\t            return _{so.name};");
        code.AppendLine("\t        }");
        code.AppendLine("\t    }");
    }
    static void appendInputSystemCode(InputActionAsset input, StringBuilder str)
    {
        str.AppendLine($"public class {input.name}");
        str.AppendLine("{");
        str.AppendLine($"    public {input.name}()");
        str.AppendLine("    {");
        var url = AssetDatabase.GetAssetPath(input);
        str.AppendLine($"        this.Asset = AssetLoad.Load<InputActionAsset>(\"{url.Replace(AssetLoad.Directory, "")}\");");

        foreach (var item in input.actionMaps)
        {
            str.AppendLine($"        this.{item.name} = this.Asset.FindActionMap(\"{item.name}\", true);");
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

        str.AppendLine();
        str.AppendLine("    public void Dispose()");
        str.AppendLine("    {");
        str.AppendLine("        AssetLoad.Release(Asset);");
        foreach (var item in input.actionMaps)
        {
            str.AppendLine($"        this.{item.name}.Dispose();");
        }
        str.AppendLine("    }");
        str.AppendLine(@"}");
    }

    class FUI3DCodeTemp
    {
        public string path;
        public string name;
    }
    [MenuItem("Tools/生成FUI3D代码")]
    static void CreateFUI3DCode()
    {
        StringBuilder code = new StringBuilder();
        List<FUI3DCodeTemp> components = new List<FUI3DCodeTemp>();
        appendFUI3DCode(Application.dataPath + "/Res/UI/FUI/3DUI", code, components);
        EditorUtility.DisplayDialog("完成", "创建完成", "确定");
        File.WriteAllText(Application.dataPath + @"\Code\HotFix\Game\_Gen\FUI3D.cs", code.ToString());
        AssetDatabase.Refresh();
    }
    static void appendFUI3DCode(string path,StringBuilder code, List<FUI3DCodeTemp> components)
    {
        var ds = Directory.GetDirectories(path);
        foreach (var d in ds)
        {
            appendFUI3DCode(d, code, components);
        }
        var fs = Directory.GetFiles(path);
        foreach (var f in fs)
        {
            if (!f.EndsWith(".prefab")) continue;
            var ff = "Assets" + f.Replace(Application.dataPath, "");
            var g = (GameObject)AssetDatabase.LoadMainAssetAtPath(ff);
            UIPanel panel = g.GetComponentInChildren<UIPanel>();
            if (panel == null) continue;
            if (string.IsNullOrEmpty(panel.packageName) || string.IsNullOrEmpty(panel.componentName))
            {
                Debug.LogError("目录:" + ff + "  包名或组件名为空");
                continue;
            }
            if (!panel.componentName.StartsWith("FUI3D"))
            {
                Debug.LogError("目录:" + ff + "  3DUI必须以FUI3D开头");
                continue;
            }
            var c = components.Find(t => t.name == panel.componentName);
            if (c != null)
            {
                Debug.LogError($"目录:{ff}  和目录{c.path} 使用了相同的组件");
                continue;
            }
            c = new FUI3DCodeTemp();
            c.path = ff;
            c.name = panel.componentName;
            components.Add(c);
            code.AppendLine($"partial class {panel.componentName}");
            code.AppendLine("{");
            code.AppendLine($"    public override string url => \"{AssetDatabase.GetAssetPath(g).Replace(AssetLoad.Directory, "")}\";");
            code.AppendLine("}");
        }
    }

    [MenuItem("Tools/热重载配置表")]
    static void ReloadConfig()
    {
        if (!Application.isPlaying) return;
        TabM.Init(new DBuffer(File.ReadAllBytes(Application.dataPath+ "/Res/Config/Tabs/TabM.bytes")));
        TabL.Init(new DBuffer(File.ReadAllBytes(Application.dataPath + "/Res/Config/Tabs/TabL.bytes")));
        LanguageS.Clear();
        LanguageS.Load((int)SystemLanguage.Chinese, new DBuffer(File.ReadAllBytes(Application.dataPath + "/Res/Config/Tabs/Language_cn.bytes")));
        LanguageS.Load((int)SystemLanguage.English, new DBuffer(File.ReadAllBytes(Application.dataPath + "/Res/Config/Tabs/Language_en.bytes")));
        EditorUtility.DisplayDialog("完成", "重载完成", "确定");
    }
}