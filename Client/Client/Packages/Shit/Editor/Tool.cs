using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using FairyGUI;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System;



#if InputSystem
using UnityEngine.InputSystem;
#endif

public class Tool
{
    static bool tips = true;
    [MenuItem("Tools/MyTool/生成UGUI代码", false, int.MaxValue - 100)]
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

        File.WriteAllText(Application.dataPath + $"/Code/HotFix/_Gen/UUI.cs", code.ToString());
        AssetDatabase.Refresh();

        if (tips)
            EditorUtility.DisplayDialog("完成", "创建完成", "确定");
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
            code.AppendLine($"    public sealed override string url => \"{AssetDatabase.GetAssetPath(go).Replace(Game.SAsset.Directory, "")}\";");

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

#if FairyGUI

    [MenuItem("Tools/MyTool/生成FGUI代码", false, int.MaxValue - 100)]
    static void CreateFUICode()
    {
        UIPackage.RemoveAllPackages();
        FontManager.Clear();
        FairyGUI.UIConfig.defaultFont = "Impact";
        var pkg = UIPackage.AddPackage($"{Game.SAsset.Directory}/UI/FUI/ComPkg/ComPkg");
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
        File.WriteAllText(Application.dataPath + $"/Code/HotFix/_Gen/FUI.cs", code.ToString());
        AssetDatabase.Refresh();

        if (tips)
            EditorUtility.DisplayDialog("完成", "创建完成", "确定");
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
            appendFUI3DCode(code, d, pkg);
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

#endif

    [MenuItem("Tools/MyTool/CopyTexture")]
    static void CopyTexture()
    {
        var fs = Directory.GetFiles(Application.dataPath + "/../../../Art/Texture");
        for (int i = 0; i < fs.Length; i++)
        {
            FileInfo fi = new FileInfo(fs[i]);
            File.Copy(fs[i], Application.dataPath + "/Res/Texture/" + fi.Name, true);
        }
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("成功", "成功", "OK", "取消");
    }

    [MenuItem("Tools/MyTool/生成Config代码", false, int.MaxValue - 100)]
    static void CreateConfigCode()
    {
        {
            StringBuilder so = new StringBuilder();
            so.AppendLine("using UnityEngine;");
            so.AppendLine("using Game;");
            so.AppendLine("using System;");
#if InputSystem
            so.AppendLine(@"using UnityEngine.InputSystem;");
#endif
            so.AppendLine("");
            so.AppendLine("public static partial class SettingM");
            so.AppendLine("{");

            StringBuilder input = new StringBuilder();
            input.AppendLine(@"");

            appendConfigWithDirectory(Application.dataPath + "/Res/Config/SO/Main/", so, input);

            so.AppendLine("}");
            so.Append(input);

            if (!Directory.Exists(Application.dataPath + "/Code/Main/_Gen"))
                Directory.CreateDirectory(Application.dataPath + "/Code/Main/_Gen");

            File.WriteAllText(Application.dataPath + $"/Code/Main/_Gen/Config.cs", so.ToString());
        }
        {
            StringBuilder so = new StringBuilder();
            so.AppendLine("using UnityEngine;");
            so.AppendLine("using Game;");
            so.AppendLine("using System;");
#if InputSystem
            so.AppendLine(@"using UnityEngine.InputSystem;");
#endif
            so.AppendLine("");
            so.AppendLine("public static partial class SettingL");
            so.AppendLine("{");

            StringBuilder input = new StringBuilder();
            input.AppendLine(@"");

            appendConfigWithDirectory(Application.dataPath + "/Res/Config/SO/Hotfix/", so, input);

            so.AppendLine("}");
            so.Append(input);

            if (!Directory.Exists(Application.dataPath + "/Code/HotFix/_Gen"))
                Directory.CreateDirectory(Application.dataPath + "/Code/HotFix/_Gen");

            File.WriteAllText(Application.dataPath + $"/Code/HotFix/_Gen/Config.cs", so.ToString());
        }

        AssetDatabase.Refresh();
        if (tips)
            EditorUtility.DisplayDialog("成功", "成功", "OK", "取消");
    }
    static void appendConfigWithDirectory(string path, StringBuilder soCode, StringBuilder inputCode)
    {
        foreach (var d in Directory.GetDirectories(path))
        {
            appendConfigWithDirectory(d, soCode, inputCode);
        }

        foreach (var f in Directory.GetFiles(path))
        {
            if (f.EndsWith(".meta"))
                continue;
            var ff = "Assets" + f.Replace(Application.dataPath, "");
            var o = AssetDatabase.LoadMainAssetAtPath(ff);
#if InputSystem
            if (o is InputActionAsset input)
                appendInputSystemCode(input, inputCode);
            else
#endif
            if (o is ScriptableObject so)
                appendScriptObjectCode(so, soCode);
        }
    }
    static void appendScriptObjectCode(ScriptableObject so, StringBuilder code)
    {
        var url = AssetDatabase.GetAssetPath(so);
        string type = so.GetType().FullName;
        code.AppendLine($"\tstatic {type} _{so.name};");
        code.AppendLine($"\tpublic static {type} {so.name} => _{so.name} ??= ({type})SAsset.Load<ScriptableObject>(\"{url.Replace(Game.SAsset.Directory, "")}\");");
    }
#if InputSystem
    static void appendInputSystemCode(InputActionAsset input, StringBuilder str)
    {
        str.AppendLine($"public class {input.name}");
        str.AppendLine("{");
        str.AppendLine($"    public {input.name}()");
        str.AppendLine("    {");
        var url = AssetDatabase.GetAssetPath(input);
        str.AppendLine($"        this.Asset = SAsset.Load<InputActionAsset>(\"{url.Replace(Game.SAsset.Directory, "")}\");");
        str.AppendLine("        this.Asset.Enable();");

        foreach (var item in input.actionMaps)
        {
            var bs = item.id.ToByteArray();
            str.AppendLine($"        this.{item.name} = this.Asset.FindActionMap(new Guid(0x{bs[3]:x2}{bs[2]:x2}{bs[1]:x2}{bs[0]:x2}, 0x{bs[5]:x2}{bs[4]:x2}, 0x{bs[7]:x2}{bs[6]:x2}, 0x{bs[8]:x2}, 0x{bs[9]:x2}, 0x{bs[10]:x2}, 0x{bs[11]:x2}, 0x{bs[12]:x2}, 0x{bs[13]:x2}, 0x{bs[14]:x2}, 0x{bs[15]:x2}));");
            foreach (var act in item.actions)
            {
                bs = act.id.ToByteArray();
                str.AppendLine($"        this.{item.name}{act.name} = this.{item.name}.FindAction(new Guid(0x{bs[3]:x2}{bs[2]:x2}{bs[1]:x2}{bs[0]:x2}, 0x{bs[5]:x2}{bs[4]:x2}, 0x{bs[7]:x2}{bs[6]:x2}, 0x{bs[8]:x2}, 0x{bs[9]:x2}, 0x{bs[10]:x2}, 0x{bs[11]:x2}, 0x{bs[12]:x2}, 0x{bs[13]:x2}, 0x{bs[14]:x2}, 0x{bs[15]:x2}));");
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
#endif

    static void calBat(string path, bool throwEx = false)
    {
        // 创建一个Process对象
        Process process = new Process();
        // 设置要执行的批处理文件路径
        process.StartInfo.FileName = path;
        // 设置为不使用Shell执行
        process.StartInfo.UseShellExecute = false;
        // 重定向标准输出流和标准错误流，以便读取批处理文件的输出和错误信息
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        // 启动批处理文件
        process.Start();
        // 读取批处理文件的输出和错误信息
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        // 等待批处理文件执行完毕
        process.WaitForExit();
        // 关闭Process对象
        process.Close();

        if (!string.IsNullOrEmpty(error))
        {
            EditorUtility.DisplayDialog("失败", error, "OK", "取消");
            if (throwEx)
                throw new System.Exception(error);
        }
        else if (tips)
            EditorUtility.DisplayDialog("成功", output, "OK", "取消");
    }
    [MenuItem("Tools/MyTool/生成配置表", false, int.MaxValue - 10)]
    static void GenTabs()
    {
        calBat(Application.dataPath + "/../../../Excel/ExcelToDB.bat");
    }

    [MenuItem("Tools/MyTool/生成pb", false, int.MaxValue - 10)]
    static void GenPB()
    {
        calBat(Application.dataPath + "/../../../PB/pb_cs.bat");
    }

    [MenuItem("Tools/MyTool/全部事项生成")]
    static void GenAll()
    {
        tips = false;
        try
        {
            CreateUUICode();
#if FairyGUI
            CreateFUICode();
#endif
            CreateConfigCode();
            calBat(Application.dataPath + "/../../../Excel/ExcelToDB.bat", true);
            calBat(Application.dataPath + "/../../../Excel/ExcelToDB.bat", true);
        }
        catch (System.Exception)
        {
            return;
        }
        finally
        {
            tips = true;
        }
        EditorUtility.DisplayDialog("成功", "成功", "OK", "取消");
    }
}