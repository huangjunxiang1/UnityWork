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
using DG.Tweening.Plugins.Core.PathCore;

public class Tool
{
    [MenuItem("Tools/MyTool/生成UGUI代码")]
    static void CreateUUICode()
    {
        StringBuilder code = new StringBuilder(100000);

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
        if (EditorUtility.DisplayDialog("完成", "创建完成", "确定"))
        {
            File.WriteAllText(Application.dataPath + $"/Code/HotFix/_Gen/UUI.cs", code.ToString());
            AssetDatabase.Refresh();
        }
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
            if (go.GetComponent<Canvas>() == null)
            {
                Debug.LogError(go.name + "不是Canvas对象");
                continue;
            }

            StringBuilder getUICode = new StringBuilder();
            StringBuilder vmCallCode = new StringBuilder();
            getUICode.AppendLine("        Transform c;");
            code.AppendLine(@"");
            if (go.name.StartsWith("UUI3D"))
                code.AppendLine(@$"partial class {go.name} : UUI3D");
            else if (go.name.StartsWith("UUI"))
                code.AppendLine(@$"partial class {go.name} : UUI");
             
            code.AppendLine(@"{");
            code.AppendLine($"    public sealed override string url => \"{AssetDatabase.GetAssetPath(go).Replace(SAsset.Directory, "")}\";");

            var childs = go.GetComponentsInChildren<Transform>(true);
            for (int j = 0; j < childs.Length; j++)
            {
                var child = childs[j];
                if (child.name.StartsWith("_") || child == go.transform)
                {
                    var coms = child.GetComponents<Component>().ToList();
                    if (child == go.transform)
                    {
                        coms.RemoveAll(t =>
                        {
                            if (t is Mask) return true;
                            if (t is ContentSizeFitter) return true;
                            if (t is Shadow) return true;
                            if (t is Outline) return true;
                            if (t is CanvasRenderer) return true;

                            if (t is Canvas) return true;
                            if (t is CanvasScaler) return true;
                            if (t is GraphicRaycaster) return true;
                            return false;
                        });
                    }
                    else
                    {
                        coms.RemoveAll(t =>
                        {
                            if (t is Mask) return true;
                            if (t is ContentSizeFitter) return true;
                            if (t is Shadow) return true;
                            if (t is Outline) return true;
                            if (t is CanvasRenderer) return true;
                            return false;
                        });
                    }

                    if (coms.Find(t => t is Selectable) != null)
                        coms.RemoveAll(t => t is UnityEngine.UI.Image);

                    if (coms.Count > 1 || child == go.transform)
                        coms.Remove(child.GetComponent<Transform>());

                    if (child == go.transform)
                    {
                        for (int i = 0; i < coms.Count; i++)
                        {
                            var item1 = coms[i];
                            code.AppendLine($@"    public {item1.GetType().FullName} UI_{item1.GetType().Name};");
                            getUICode.AppendLine($@"        this.UI_{item1.GetType().Name} = ({item1.GetType().FullName})ui.GetComponent(typeof({item1.GetType().FullName}));");
                        }
                    }
                    else
                    {
                        getUICode.Append($@"        c = ui");
                        var temp = child;
                        List<int> idxs = new List<int>();
                        idxs.Add(temp.GetSiblingIndex());
                        while (temp.transform.parent != go.transform)
                        {
                            temp = temp.transform.parent;
                            idxs.Add(temp.GetSiblingIndex());
                        }
                        for (int i = idxs.Count - 1; i >= 0; i--)
                        {
                            getUICode.Append($".GetChild({idxs[i]})");
                        }
                        getUICode.AppendLine(";");

                        if (child.name.EndsWith("_str")
                            || child.name.EndsWith("_int")
                            || child.name.EndsWith("_bool")
                            || child.name.EndsWith("_float"))
                        {
                            int idx = child.name.LastIndexOf('_');
                            string bType = child.name.Split('_').Last();
                            if (bType == "str")
                                bType = "string";
                            string name = child.name.Substring(0, idx);
                            for (int i = 0; i < coms.Count; i++)
                            {
                                var item1 = coms[i];
                                code.AppendLine($@"    public PropertyBinding<{item1.GetType().FullName}, {bType}> {name}{item1.GetType().Name}Binding;");
                                getUICode.AppendLine($@"        this.{name}{item1.GetType().Name}Binding = new PropertyBinding<{item1.GetType().FullName}, {bType}>(({item1.GetType().FullName})c.GetComponent(typeof({item1.GetType().FullName})));");
                                vmCallCode.AppendLine($"        this.{name}{item1.GetType().Name}Binding.CallEvent();");
                            }
                        }
                        else
                        {
                            for (int i = 0; i < coms.Count; i++)
                            {
                                var item1 = coms[i];
                                code.AppendLine($@"    public {item1.GetType().FullName} {item1.name}{item1.GetType().Name};");
                                getUICode.AppendLine($@"        this.{item1.name}{item1.GetType().Name} = ({item1.GetType().FullName})c.GetComponent(typeof({item1.GetType().FullName}));");
                            }
                        }
                    }
                }
            }

            code.AppendLine(@"");
            code.AppendLine(@"    protected sealed override void Binding()");
            code.AppendLine(@"    {");
            code.AppendLine(@"        RectTransform ui = this.UI;");
            code.Append(getUICode.ToString());
            code.AppendLine(@"        this.VMBinding();");
            code.Append(vmCallCode.ToString());
            code.AppendLine(@"    }");
            code.Append(@"}");
        }
    }

    [MenuItem("Tools/MyTool/生成FGUI代码")]
    static void CreateFUICode()
    {
        UIPackage.RemoveAllPackages();
        FontManager.Clear();
        FairyGUI.UIConfig.defaultFont = "Impact";
        var pkg = UIPackage.AddPackage($"{SAsset.Directory}/UI/FUI/ComPkg/ComPkg");
        StringBuilder code = new StringBuilder(100000);
        code.AppendLine("using FairyGUI;");
        code.AppendLine("using FairyGUI.Utils;");

        foreach (var item in pkg.GetItems())
        {
            if (item.exported)
            {
                if (!item.name.StartsWith("FUI") && !item.name.StartsWith("FUI3D"))
                {
                    code.AppendLine($"partial class G_{item.name}");
                    code.AppendLine("{");

                    appendExportedCode(code, item);

                    code.AppendLine("}");
                }
            }
        }
        foreach (var item in pkg.GetItems())
        {
            if (item.name.StartsWith("FUI") && !item.name.StartsWith("FUI3D"))
            {
                code.AppendLine($"partial class {item.name} : FUI");
                code.AppendLine("{");
                code.AppendLine($"    public sealed override string url => \"{item.name}\";");

                appendFUICode(code, item);

                code.AppendLine("}");
            }
        }
       
        appendFUI3DCode(code, Application.dataPath + "/Res/UI/FUI/3DUI", pkg);
        UIPackage.RemoveAllPackages();

        if (EditorUtility.DisplayDialog("完成", "创建完成", "确定"))
        {
            File.WriteAllText(Application.dataPath + $"/Code/HotFix/_Gen/FUI.cs", code.ToString());
            AssetDatabase.Refresh();
        }
    }
    static void appendExportedCode(StringBuilder code, PackageItem item)
    {
        var obj = item.owner.CreateObject(item.name).asCom;
        if (obj == null)
        {
            Debug.LogError(item.name + "不是FGUI组件类型");
            return;
        }

        code.AppendLine("    public GComponent ui { get; }");
        appendExportedChildCode("", obj, code);
        code.AppendLine($"    public G_{item.name}(GComponent ui)");
        code.AppendLine("    {");
        code.AppendLine("        this.ui = ui;");
        addChildBinding("", "ui", obj, code);
        code.AppendLine("    }");
        code.AppendLine($"    public static G_{item.name} Create()");
        code.AppendLine("    {");
        code.AppendLine($"        return new G_{item.name}(UIPackage.CreateObject(\"{item.owner.name}\", \"{item.name}\").asCom);");
        code.AppendLine("    }");

        obj.Dispose();
    }
    static void appendExportedChildCode(string name, GComponent obj, StringBuilder code)
    {
        var cs = obj.GetChildren();
        for (int i = 0; i < cs.Length; i++)
        {
            var c = cs[i];
            if (!c.name.StartsWith("_"))
                continue;

            if (c is GComponent)
            {
                if (c.packageItem != null && c.packageItem.exported)
                {
                    code.AppendLine($"    public {c.packageItem.name} {name + c.name} {{ get; }}");
                }
                else
                {
                    code.AppendLine($"    public {c.GetType().Name} {name + c.name} {{ get; }}");
                    addChildCode(name + c.name, c.asCom, code);
                }
            }
            else
                code.AppendLine($"    public {c.GetType().Name} {name + c.name} {{ get; }}");
        }
        for (int i = 0; i < obj.Controllers.Count; i++)
        {
            if (!obj.Controllers[i].name.StartsWith("_"))
                continue;
            code.AppendLine($"    public Controller {name + obj.Controllers[i].name} {{ get; }}");
        }
        for (int i = 0; i < obj.Transitions.Count; i++)
        {
            if (!obj.Transitions[i].name.StartsWith("_"))
                continue;
            code.AppendLine($"    public Transition {name + obj.Transitions[i].name} {{ get; }}");
        }
    }
    static void appendFUICode(StringBuilder code, PackageItem item)
    {
        var obj = item.owner.CreateObject(item.name).asCom;
        if (obj == null)
        {
            Debug.LogError(item.name + "不是FGUI组件类型");
            return;
        }

        addChildCode("", obj, code);

        code.AppendLine($"    protected sealed override void Binding()");
        code.AppendLine("    {");
        code.AppendLine("        GComponent ui = this.UI;");

        addChildBinding("", "ui", obj, code);

        code.AppendLine("    }");

        obj.Dispose();
    }
    static void addChildCode(string name, GComponent obj, StringBuilder code)
    {
        var cs = obj.GetChildren();
        for (int i = 0; i < cs.Length; i++)
        {
            var c = cs[i];
            if (!c.name.StartsWith("_"))
                continue;

            if (c is GComponent)
            {
                if (c.packageItem != null && c.packageItem.exported)
                {
                    code.AppendLine($"    public G_{c.packageItem.name} {name + c.name} {{ get; private set; }}");
                }
                else
                {
                    code.AppendLine($"    public {c.GetType().Name} {name + c.name} {{ get; private set; }}");
                    addChildCode(name + c.name, c.asCom, code);
                }
            }
            else
                code.AppendLine($"    public {c.GetType().Name} {name + c.name} {{ get; private set; }}");
        }
        for (int i = 0; i < obj.Controllers.Count; i++)
        {
            if (!obj.Controllers[i].name.StartsWith("_"))
                continue;
            code.AppendLine($"    public Controller {name + obj.Controllers[i].name} {{ get; private set; }}");
        }
        for (int i = 0; i < obj.Transitions.Count; i++)
        {
            if (!obj.Transitions[i].name.StartsWith("_"))
                continue;
            code.AppendLine($"    public Transition {name + obj.Transitions[i].name} {{ get; private set; }}");
        }
    }
    static void addChildBinding(string name, string path, GComponent obj, StringBuilder code)
    {
        var cs = obj.GetChildren();
        for (int i = 0; i < cs.Length; i++)
        {
            var c = cs[i];
            if (!c.name.StartsWith("_"))
                continue;

            if (c is GComponent)
            {
                if (c.packageItem != null && c.packageItem.exported)
                {
                    code.AppendLine($"        {name + c.name} = new G_{c.packageItem.name}(ui.GetChildAt({i}).asCom);");
                }
                else
                {
                    code.AppendLine($"        {name + c.name} = ({c.GetType().Name}){path}.GetChildAt({i});");
                    addChildBinding(name + c.name, name + c.name, c.asCom, code);
                }
            }
            else
                code.AppendLine($"        {name + c.name} = ({c.GetType().Name}){path}.GetChildAt({i});");
        }
        for (int i = 0; i < obj.Controllers.Count; i++)
        {
            if (!obj.Controllers[i].name.StartsWith("_"))
                continue;
            code.AppendLine($"        {name + obj.Controllers[i].name} = {path}.GetControllerAt({i});");
        }
        for (int i = 0; i < obj.Transitions.Count; i++)
        {
            if (!obj.Transitions[i].name.StartsWith("_"))
                continue;
            code.AppendLine($"        {name + obj.Transitions[i].name} = {path}.GetTransitionAt({i});");
        }
    }
    static void appendFUI3DCode(StringBuilder code, string path, UIPackage pkg)
    {
        var ds = Directory.GetDirectories(path);
        foreach (var d in ds)
        {
            appendFUI3DCode(code,d, pkg);
        }
        var fs = Directory.GetFiles(path);
        foreach (var f in fs)
        {
            if (!f.EndsWith(".prefab")) continue;
            var ff = "Assets" + f.Replace(Application.dataPath, "");
            ff = ff.Replace("\\", "/");
            var g = (GameObject)AssetDatabase.LoadMainAssetAtPath(ff);
            UIPanel panel = g.GetComponentInChildren<UIPanel>();
            if (panel == null)
            {
                Debug.LogError($"{ff}非FUI3D对象");
                continue;
            }
            if (string.IsNullOrEmpty(panel.packageName) || string.IsNullOrEmpty(panel.componentName))
            {
                Debug.LogError("目录:" + ff + "  包名或组件名为空");
                continue;
            }
            if (panel.packageName != pkg.name)
            {
                Debug.LogError($"包名不一致 prefab设置包名{panel.packageName} UI包名{pkg.name}");
                continue;
            }
            if (!panel.componentName.StartsWith("FUI3D"))
            {
                Debug.LogError("目录:" + ff + "  3DUI以FUI3D开头");
                continue;
            }
            string className = g.name;
            code.AppendLine($"partial class {className} : FUI3D");
            code.AppendLine("{");
            code.AppendLine($"    public sealed override string url => \"{ff}\";");

            var pi = pkg.GetItemByName(panel.componentName);
            appendFUICode(code, pi);

            code.AppendLine("}");
        }
    }

    [MenuItem("Tools/MyTool/CopyTexture")]
    static void CopyTexture()
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

    [MenuItem("Tools/MyTool/生成Config代码")]
    static void CreateConfigCode()
    {
        StringBuilder so = new StringBuilder();
        so.AppendLine("using UnityEngine;");
        so.AppendLine("using Game;");
        so.AppendLine("using Main;");
        so.AppendLine("");
        so.AppendLine("namespace Game");
        so.AppendLine("{");
        so.AppendLine("\tpublic static partial class Setting");
        so.AppendLine("\t{");

        StringBuilder input = new StringBuilder();
        input.AppendLine(@"using UnityEngine.InputSystem;");
        input.AppendLine(@"using Main;");
        input.AppendLine(@"using UnityEngine;");
        input.AppendLine(@"");

        appendConfigWithDirectory(Application.dataPath + "/Res/Config/SO/", so, input);

        so.AppendLine("\t}");
        so.AppendLine("}");

        if (!Directory.Exists(Application.dataPath + "/Code/HotFix/_Gen"))
            Directory.CreateDirectory(Application.dataPath + "/Code/HotFix/_Gen");

        File.WriteAllText(Application.dataPath + $"/Code/HotFix/_Gen/Setting.cs", so.ToString());
        File.WriteAllText(Application.dataPath + $"/Code/HotFix/_Gen/Inputs.cs", input.ToString());

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
        code.AppendLine($"\t    static {type} _{so.name};");
        code.AppendLine($"\t    public static {type} {so.name}");
        code.AppendLine("\t    {");
        code.AppendLine("\t        get");
        code.AppendLine("\t        {");
        code.AppendLine($"\t            if (!_{so.name})");
        code.AppendLine($"\t                _{so.name} = ({type})SAsset.Load<ScriptableObject>(\"{url.Replace(SAsset.Directory, "")}\");");
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
        str.AppendLine($"        this.Asset = SAsset.Load<InputActionAsset>(\"{url.Replace(SAsset.Directory, "")}\");");

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
        str.AppendLine("        SAsset.Release(Asset);");
        foreach (var item in input.actionMaps)
        {
            str.AppendLine($"        this.{item.name}.Dispose();");
        }
        str.AppendLine("    }");
        str.AppendLine(@"}");
    }

    [MenuItem("Tools/MyTool/热重载配置表")]
    static void ReloadConfig()
    {
        if (!Application.isPlaying) return;
        TabM.Init(new DBuffer(new MemoryStream(File.ReadAllBytes(Application.dataPath + "/Res/Config/Tabs/STabM.bytes"))), ConstDefM.Debug);
        TabL.Init(new DBuffer(new MemoryStream(File.ReadAllBytes(Application.dataPath + "/Res/Config/Tabs/STabL.bytes"))), ConstDefM.Debug);
        EditorUtility.DisplayDialog("完成", "重载完成", "确定");
    }
}