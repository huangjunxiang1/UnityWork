#if UGUI
using UnityEngine.UI;
#endif

#if FairyGUI
using FairyGUI;
#endif

using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System;
using Core;
using Debug = UnityEngine.Debug;
using HybridCLR.Editor;

using Mono.Cecil;
using Mono.Cecil.Cil;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;


public class Tool
{
    static bool tips = true;

#if UGUI
    [MenuItem("Shit/生成UGUI代码", false, int.MaxValue - 100)]
    static void CreateUUICode()
    {
        StringBuilder code = new StringBuilder(100000);

        Dictionary<Type, Type> typeMap = new();
        var arr = typeof(Program).Assembly.GetTypes();
        foreach (var item in arr)
        {
            if (item.BaseType != null && item.BaseType.IsGenericType && item.BaseType.GetGenericTypeDefinition() == typeof(UIPropertyBinding<,>) && typeof(UIPropertyBinding<,>) != item)
                typeMap[item.BaseType.GetGenericArguments()[0]] = item;
        }
        appendUUICode(Application.dataPath + "/Res/UI/UUI/Prefab/", code, typeMap);

        File.WriteAllText(Application.dataPath + $"/Code/HotFix/_Gen/UUI.cs", code.ToString());
        AssetDatabase.Refresh();

        if (tips)
            EditorUtility.DisplayDialog("完成", "创建完成", "确定");
    }
    static void appendUUICode(string path, StringBuilder code, Dictionary<Type, Type> typeMap)
    {
        foreach (var d in Directory.GetDirectories(path))
        {
            appendUUICode(d, code, typeMap);
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
            uis.Add(go);
        }
      
        foreach (var go in uis)
        {
            StringBuilder fieldCode = new StringBuilder();
            StringBuilder getUICode = new StringBuilder();
            StringBuilder disposeStr = new StringBuilder();

            var coms = go.GetComponents<Component>().ToList();
            Filter(coms, true);
            if (coms.Find(t => t is Selectable) != null)
                coms.RemoveAll(t => t is UnityEngine.UI.Image);
            coms.Remove(go.transform);
            for (int i = 0; i < coms.Count; i++)
            {
                var c = coms[i];
                fieldCode.AppendLine($@"    public {c.GetType().FullName} UI_{c.GetType().Name} {{ get; private set; }}");
                getUICode.AppendLine($@"        this.UI_{c.GetType().Name} = ({c.GetType().FullName})ui.GetComponent(typeof({c.GetType().FullName}));");
            }

            fieldCode.AppendLine();
            for (int i = 0; i < go.transform.childCount; i++)
                genFieldCode(go, go.transform.GetChild(i), fieldCode, typeMap, getUICode, disposeStr);

            if (go.name.StartsWith("UUI") || go.name.StartsWith("UUI3D"))
            {
                if (go.GetComponent<Canvas>() == null)
                {
                    Debug.LogError(go.name + "不是Canvas对象");
                    continue;
                }
                if (go.name.StartsWith("UUI"))
                    code.AppendLine($"partial class {go.name} : UUI");
                else if (go.name.StartsWith("UUI"))
                    code.AppendLine($"partial class {go.name} : UUI3D");

                code.AppendLine(@"{");
                code.AppendLine($"    public sealed override string url => \"UI_{go.name}\";");
                code.Append(fieldCode.ToString());
                code.AppendLine(@"");
                code.AppendLine(@"    protected sealed override void Binding()");
                code.AppendLine(@"    {");
                code.AppendLine(@"        UnityEngine.RectTransform ui = this.ui;");
                code.AppendLine($"        UnityEngine.Transform c;");
                code.Append(getUICode.ToString());
                code.AppendLine(@"    }");
                code.AppendLine(@"    public override void Dispose()");
                code.AppendLine(@"    {");
                code.Append(disposeStr.ToString());
                code.AppendLine(@"        base.Dispose();");
                code.AppendLine(@"    }");
                code.AppendLine(@"}");
            }
            else
            {
                if (go.name.StartsWith("_"))
                {
                    code.AppendLine($"partial class U{go.name}");
                    code.AppendLine($"{{");
                    code.AppendLine($"    public UnityEngine.RectTransform ui {{ get; private set; }}");
                    code.AppendLine(fieldCode.ToString());
                    code.AppendLine($"    public U{go.name}(UnityEngine.Transform ui)");
                    code.AppendLine($"    {{");
                    code.AppendLine($"        this.ui = (UnityEngine.RectTransform)ui;");
                    code.AppendLine($"        UnityEngine.Transform c;");
                    code.Append(getUICode.ToString());
                    code.AppendLine($"        this.Enter();");
                    code.AppendLine($"    }}");
                    code.AppendLine($"    public U{go.name}(Game.ReleaseMode mode = Game.ReleaseMode.Destroy) : this(Game.SAsset.LoadGameObject(\"UI_{go.name}\", mode).transform) {{ }}");
                    code.AppendLine($"    partial void Enter();");
                    code.AppendLine($"    public void Dispose()");
                    code.AppendLine($"    {{");
                    code.Append(disposeStr.ToString());
                    code.AppendLine($"    }}");
                    code.AppendLine($"}}");
                }
            }
        }
    }
    static void Filter(List<Component> coms,bool isRoot)
    {
        if (isRoot)
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
    }
    static void genFieldCode(GameObject go, Transform child, StringBuilder fieldCode, Dictionary<Type, Type> typeMap, StringBuilder getUICode, StringBuilder disposeStr)
    {
        var g = PrefabUtility.GetCorrespondingObjectFromSource(child.gameObject);
        string p = g ? AssetDatabase.GetAssetPath(go) : null;
        if (child.name.StartsWith("_"))
        {
            var temp = child;
            List<int> idxs = new List<int>();
            idxs.Add(temp.GetSiblingIndex());
            while (temp.transform.parent != go.transform)
            {
                temp = temp.transform.parent;
                idxs.Add(temp.GetSiblingIndex());
            }
            getUICode.Append($@"        c = ui");
            for (int i = idxs.Count - 1; i >= 0; i--)
                getUICode.Append($".GetChild({idxs[i]})");
            getUICode.AppendLine(";");

            if (g && g.name.StartsWith("_") && p != null && p.StartsWith("Assets/Res/UI/UUI/"))
            {
                fieldCode.AppendLine($"    public U{g.name} {child.name} {{ get; private set; }}");
                getUICode.AppendLine($"        this.{child.name} = new(c);");
                disposeStr.AppendLine($"        this.{child.name}.Dispose();");
                return;
            }
            var coms = child.GetComponents<Component>().ToList();
            Filter(coms, false);

            if (coms.Find(t => t is Selectable) != null)
                coms.RemoveAll(t => t is UnityEngine.UI.Image);

            if (coms.Count > 1)
                coms.Remove(child.GetComponent<Transform>());


            for (int i = 0; i < coms.Count; i++)
            {
                var c = coms[i];
                if (c.name.Length >= 4 && c.name.EndsWith("_b") && typeMap.TryGetValue(c.GetType(), out var bindingType))
                {
                    string fieldName = c.name[..^2] + c.GetType().Name;
                    fieldCode.AppendLine($@"    public {bindingType.FullName} {fieldName} {{ get; private set; }}");
                    getUICode.AppendLine($@"        this.{fieldName} = new(({c.GetType().FullName})c.GetComponent(typeof({c.GetType().FullName})));");
                    disposeStr.AppendLine($"        this.{fieldName}.Dispose();");
                }
                else
                {
                    string fieldName = c.name + c.GetType().Name;
                    fieldCode.AppendLine($@"    public {c.GetType().FullName} {fieldName} {{ get; private set; }}");
                    getUICode.AppendLine($@"        this.{fieldName} = ({c.GetType().FullName})c.GetComponent(typeof({c.GetType().FullName}));");
                }
            }
        }

        if (!g)
        {
            for (int i = 0; i < child.childCount; i++)
                genFieldCode(go, child.GetChild(i), fieldCode, typeMap, getUICode, disposeStr);
        }
    }
#endif

#if FairyGUI
    [MenuItem("Shit/生成FGUI代码", false, int.MaxValue - 100)]
    static void CreateFUICode()
    {
        UIObjectFactory.Clear();
        UIPackage.RemoveAllPackages();
        FontManager.Clear();
        FairyGUI.UIConfig.defaultFont = "Impact";
        var pkg = UIPackage.AddPackage($"Assets/Res/Config/raw/ComPkg/ComPkg");
        StringBuilder code = new StringBuilder(100000);
        code.AppendLine("using FairyGUI;");
        code.AppendLine("using FairyGUI.Utils;");

        Dictionary<Type, Type> typeMap = new();
        foreach (var item in typeof(Program).Assembly.GetTypes())
        {
            if (item.BaseType != null && item.BaseType.IsGenericType && item.BaseType.GetGenericTypeDefinition() == typeof(UIPropertyBinding<,>) && typeof(UIPropertyBinding<,>) != item)
                typeMap[item.BaseType.GetGenericArguments()[0]] = item;
        }
        code.AppendLine("class FUIBinder");
        code.AppendLine("{");
        code.AppendLine("    public static void Binding()");
        code.AppendLine("    {");
        foreach (var item in pkg.GetItems())
        {
            if (item.exported)
            {
                if (item.name.StartsWith("FUI"))
                    continue;
                code.AppendLine($"        UIObjectFactory.SetPackageItemExtension(G_{item.name}.URL, typeof(G_{item.name}));");
            }
        }
        code.AppendLine("    }");
        code.AppendLine("}");
        foreach (var item in pkg.GetItems())
        {
            if (item.exported)
            {
                if (item.name.StartsWith("FUI3D"))
                    continue;
                var obj = item.owner.CreateObject(item.name).asCom;
                if (obj == null)
                {
                    obj.Dispose();
                    Debug.LogError(item.name + "不是FGUI组件类型");
                    continue;
                }
                StringBuilder fieldCode = new();
                StringBuilder bindingCode = new();
                StringBuilder disposeCode = new();

                if (item.name.StartsWith("FUI"))
                {
                    appendField("", "ui", obj, fieldCode, bindingCode, disposeCode, typeMap);
                    code.AppendLine($"partial class {item.name} : FUI");
                    code.AppendLine("{");
                    code.AppendLine($"    public sealed override string url => \"{UIPackage.GetItemURL(item.owner.name, item.name)}\";");
                    code.AppendLine(fieldCode.ToString());

                    code.AppendLine($"    protected sealed override void Binding()");
                    code.AppendLine("    {");
                    code.AppendLine("        GComponent ui = this.ui;");
                    code.Append(bindingCode.ToString());
                    code.AppendLine("    }");
                    code.AppendLine("    public override void Dispose()");
                    code.AppendLine("    {");
                    code.AppendLine("        base.Dispose();");
                    code.Append(disposeCode.ToString());
                    code.AppendLine("    }");
                    code.AppendLine("}");
                }
                else
                {
                    appendField("", "this", obj, fieldCode, bindingCode, disposeCode, typeMap);
                    code.AppendLine($"partial class G_{item.name} : {obj.GetType().Name}");
                    code.AppendLine("{");
                    code.AppendLine($"    static G_{item.name} _inst;");
                    code.AppendLine($"    public static G_{item.name} Inst");
                    code.AppendLine($"    {{");
                    code.AppendLine($"        get");
                    code.AppendLine($"        {{");
                    code.AppendLine($"            if (_inst == null || _inst.isDisposed) ");
                    code.AppendLine($"                _inst = Create();");
                    code.AppendLine($"            return _inst;");
                    code.AppendLine($"        }}");
                    code.AppendLine($"    }}");
                    code.AppendLine($"    public static readonly string URL = \"{UIPackage.GetItemURL(item.owner.name, item.name)}\";");
                    code.AppendLine(fieldCode.ToString());

                    code.AppendLine($"    public override void ConstructFromXML(XML xml)");
                    code.AppendLine("    {");
                    code.Append(bindingCode.ToString());
                    code.AppendLine("        this.OnEnter();");
                    code.AppendLine("    }");
                    code.AppendLine("    partial void OnEnter();");
                    code.AppendLine("    partial void OnExit();");
                    code.AppendLine($"    public static G_{item.name} Create() => (G_{item.name})UIPackage.CreateObjectFromURL(URL);");
                    code.AppendLine("    public override void Dispose()");
                    code.AppendLine("    {");
                    code.AppendLine("        base.Dispose();");
                    code.Append(disposeCode.ToString());
                    code.AppendLine("        this.OnExit();");
                    code.AppendLine("    }");

                    code.AppendLine("}");
                }
                obj.Dispose();
            }
        }

        appendFUI3DCode(code, Application.dataPath + "/Res/UI/FUI/3DUI", pkg, typeMap);
        UIPackage.RemoveAllPackages();
        File.WriteAllText(Application.dataPath + $"/Code/HotFix/_Gen/FUI.cs", code.ToString());
        AssetDatabase.Refresh();

        if (tips)
            EditorUtility.DisplayDialog("完成", "创建完成", "确定");
    }
    static void appendField(string name, string path, FairyGUI.GComponent obj, StringBuilder field, StringBuilder binding, StringBuilder dispose, Dictionary<Type, Type> typeMap)
    {
        var cs = obj.GetChildren();
        for (int i = 0; i < cs.Length; i++)
        {
            var c = cs[i];
            if (!c.name.StartsWith("_"))
                continue;

            if (c.name.Length >= 4 && c.name.EndsWith("_b") && typeMap.TryGetValue(c.GetType(), out var value))
            {
                field.AppendLine($"    public {value.FullName} {name + c.name[..^2]} {{ get; private set; }}");
                binding.AppendLine($"        {name + c.name[..^2]} = new(({c.GetType().Name}){path}.GetChildAt({i}));");
                dispose.AppendLine($"        {name + c.name[..^2]}.Dispose();");
                continue;
            }
            if (c is GComponent)
            {
                if (c.packageItem != null && c.packageItem.exported)
                {
                    field.AppendLine($"    public G_{c.packageItem.name} {name + c.name} {{ get; private set; }}");
                    binding.AppendLine($"        {name + c.name} = (G_{c.packageItem.name}){path}.GetChildAt({i});");
                }
                else
                {
                    field.AppendLine($"    public {c.GetType().Name} {name + c.name} {{ get; private set; }}");
                    binding.AppendLine($"        {name + c.name} = ({c.GetType().Name}){path}.GetChildAt({i});");
                    appendField(name + c.name, path + c.name, c.asCom, field, binding, dispose, typeMap);
                }
            }
            else
            {
                field.AppendLine($"    public {c.GetType().Name} {name + c.name} {{ get; private set; }}");
                binding.AppendLine($"        {name + c.name} = ({c.GetType().Name}){path}.GetChildAt({i});");
            }
        }
        for (int i = 0; i < obj.Controllers.Count; i++)
        {
            if (!obj.Controllers[i].name.StartsWith("_"))
                continue;
            field.AppendLine($"    public Controller {name + obj.Controllers[i].name} {{ get; private set; }}");
            binding.AppendLine($"        {name + obj.Controllers[i].name} = {path}.GetControllerAt({i});");
        }
        for (int i = 0; i < obj.Transitions.Count; i++)
        {
            if (!obj.Transitions[i].name.StartsWith("_"))
                continue;
            field.AppendLine($"    public Transition {name + obj.Transitions[i].name} {{ get; private set; }}");
            binding.AppendLine($"        {name + obj.Transitions[i].name} = {path}.GetTransitionAt({i});");
        }
    }
    static void appendFUI3DCode(StringBuilder code, string path, FairyGUI.UIPackage pkg, Dictionary<Type, Type> typeMap)
    {
        var ds = Directory.GetDirectories(path);
        foreach (var d in ds)
        {
            appendFUI3DCode(code, d, pkg, typeMap);
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

            var obj = UIPackage.CreateObject(panel.packageName, panel.componentName).asCom;
            StringBuilder fieldCode = new();
            StringBuilder bindingCode = new();
            StringBuilder disposeCode = new();
            appendField("", "ui", obj, fieldCode, bindingCode, disposeCode, typeMap);

            code.AppendLine($"partial class {g.name} : FUI3D");
            code.AppendLine("{");
            code.AppendLine($"    public sealed override string url => \"UI_{g.name}\";");
            code.AppendLine(fieldCode.ToString());

            code.AppendLine($"    protected sealed override void Binding()");
            code.AppendLine("    {");
            code.AppendLine("        GComponent ui = this.ui;");
            code.Append(bindingCode.ToString());
            code.AppendLine("    }");
            code.AppendLine("    public override void Dispose()");
            code.AppendLine("    {");
            code.AppendLine("        base.Dispose();");
            code.Append(disposeCode.ToString());
            code.AppendLine("    }");
            code.AppendLine("}");

            obj.Dispose();
        }
    }
#endif


    [MenuItem("Shit/CopyTexture")]
    static void CopyTexture()
    {
        FileHelper.SyncDirectories(Application.dataPath + "/../../../Art/UI/Texture", Application.dataPath + "/Res/Texture/", deleteFilter: s => !s.EndsWith(".meta"));
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("成功", "成功", "OK", "取消");
    }

    [MenuItem("Shit/生成Config代码", false, int.MaxValue - 100)]
    static void CreateConfigCode()
    {
        {
            StringBuilder so = new StringBuilder();
            so.AppendLine("using UnityEngine;");
            so.AppendLine("using Game;");
            so.AppendLine("using System;");
            so.AppendLine("");
            so.AppendLine("public static partial class SettingM");
            so.AppendLine("{");

            StringBuilder input = new StringBuilder();
            input.AppendLine(@"");

            appendConfigWithDirectory(Application.dataPath + "/Res/Config/res/SO/Main/", so, input);

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
            so.AppendLine("");
            so.AppendLine("public static partial class SettingL");
            so.AppendLine("{");

            StringBuilder input = new StringBuilder();
            input.AppendLine(@"");

            appendConfigWithDirectory(Application.dataPath + "/Res/Config/res/SO/Hotfix/", so, input);

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
#if ENABLE_INPUT_SYSTEM
            if (o is UnityEngine.InputSystem.InputActionAsset input)
                appendInputSystemCode(input, inputCode);
            else
#endif
            if (o is ScriptableObject so)
                appendScriptObjectCode(so, soCode);
        }
    }
    static void appendScriptObjectCode(ScriptableObject so, StringBuilder code)
    {
        string type = so.GetType().FullName;
        code.AppendLine($"\tstatic {type} _{so.name};");
        code.AppendLine($"\tpublic static {type} {so.name} => _{so.name} ??= ({type})SAsset.Load<ScriptableObject>(\"config_{so.name}\");");
    }

#if ENABLE_INPUT_SYSTEM
    static void appendInputSystemCode(UnityEngine.InputSystem.InputActionAsset input, StringBuilder str)
    {
        str.AppendLine($"public class {input.name}");
        str.AppendLine("{");
        str.AppendLine($"    public {input.name}()");
        str.AppendLine("    {");
        str.AppendLine($"        this.Asset = SAsset.Load<UnityEngine.InputSystem.InputActionAsset>(\"config_{input.name}\");");
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

    public UnityEngine.InputSystem.InputActionAsset Asset { get; }
");

        foreach (var item in input.actionMaps)
        {
            str.AppendLine($"    public UnityEngine.InputSystem.InputActionMap {item.name} {{ get; }}");
            foreach (var act in item.actions)
            {
                str.AppendLine($"    public UnityEngine.InputSystem.InputAction {item.name}{act.name} {{ get; }}");
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
    [MenuItem("Shit/生成配置表", false, int.MaxValue - 10)]
    static void GenTabs()
    {
        calBat(Application.dataPath + "/../../../Excel/ExcelToDB.bat");
    }

    [MenuItem("Shit/生成pb", false, int.MaxValue - 10)]
    static void GenPB()
    {
        calBat(Application.dataPath + "/../../../PB/pb_cs.bat");
    }

    [MenuItem("Shit/全部事项生成")]
    static void GenAll()
    {
        tips = false;
        try
        {
#if UGUI
            CreateUUICode();
#endif
#if FairyGUI
            CreateFUICode();
#endif
            CreateConfigCode();
            GenTabs();
            GenPB();
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


    [MenuItem("Tools/AOT_Copy")]
    static void copyAotDll()
    {
        string srcDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(EditorUserBuildSettings.activeBuildTarget);
        var dstDir = $"{Application.dataPath}/Resources/AOT";
        if (!Directory.Exists(dstDir))
            Directory.CreateDirectory(dstDir);
        else
        {
            foreach (var item in Directory.GetFiles(dstDir))
                File.Delete(item);
        }
        for (int i = 0; i < AOTGenericReferences.PatchedAOTAssemblyList.Count; i++)
        {
            File.Copy($"{srcDir}/{AOTGenericReferences.PatchedAOTAssemblyList[i]}", $"{dstDir}/{AOTGenericReferences.PatchedAOTAssemblyList[i]}.bytes");
        }
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/Code_Copy")]
    static void copyCode()
    {
        var dstDir = $"{Application.dataPath}/Res/Config/raw/Code";
        if (!Directory.Exists(dstDir))
            Directory.CreateDirectory(dstDir);
        var bs = File.ReadAllBytes($"{Application.dataPath}/../Library/ScriptAssemblies/Game.HotFix.dll");
        File.WriteAllBytes($"{dstDir}/code.bytes", bs);
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/Gen_ToXLanString")]
    static void GenForLanString()
    {
        if (!File.Exists($"{Application.dataPath}/../Library/ScriptAssemblies/Game.HotFix.dll"))
        {
            EditorUtility.DisplayDialog("失败", "没有dll", "OK", "取消");
            return;
        }

        var dll = File.ReadAllBytes($"{Application.dataPath}/../Library/ScriptAssemblies/Game.HotFix.dll");
        var asm = AssemblyDefinition.ReadAssembly(new MemoryStream(dll));

        var types = new List<TypeDefinition>();
        void addTypes(TypeDefinition type)
        {
            types.Add(type);

            //遍历嵌套类型
            var arr = type.NestedTypes;
            for (int i = 0; i < arr.Count; i++)
                addTypes(arr[i]);
        }
        var arr = asm.MainModule.Types;
        for (int i = 0; i < arr.Count; i++)
            addTypes(arr[i]);

        Dictionary<string,string> ret = new(10000);
        for (int i = 0; i < types.Count; i++)
        {
            var type = types[i];
            var methods = type.Methods;
            for (int j = 0; j < methods.Count; j++)
            {
                var method = methods[j];
                if (!method.HasBody)
                    continue;
                var body = method.Body;
                var instructions = body.Instructions;
                for (int k = 0; k < instructions.Count; k++)
                {
                    var instr = instructions[k];
                    if (instr.OpCode == OpCodes.Call)
                    {
                        var member = instr.Operand as MemberReference;
                        if (member == null)
                        {
                            Debug.LogError("未知 op");
                            continue;
                        }
                        if (member.FullName.Contains($"System.String {nameof(LanguageUtil)}::{nameof(LanguageUtil.ToXLan)}(System.String,System.Object[])"))
                        {
                            var last = instr.Previous;
                            while (last != null && last.OpCode != OpCodes.Newarr)
                                last = last.Previous;
                            if (last == null)
                            {
                                Debug.LogError($"未找到 Newarr {type.Name} {method.Name}");
                                continue;
                            }
                            last = last.Previous?.Previous;
                            if (last != null && last.OpCode == OpCodes.Ldstr)
                                ret.Add((string)last.Operand, $"{(type.DeclaringType ?? type).Name}[{method.Name}]");
                            else
                                Debug.LogError($"未识别OpCode {last?.OpCode} {last?.Operand} {type.Name} {method.Name}");
                        }
                        else if (member.FullName.Contains($"System.String {nameof(LanguageUtil)}::{nameof(LanguageUtil.ToXLan)}(System.String)"))
                        {
                            var last = instr.Previous;
                            if (last != null && last.OpCode == OpCodes.Ldstr)
                                ret.Add((string)last.Operand, $"{(type.DeclaringType ?? type).Name}[{method.Name}]");
                            else
                                Debug.LogError($"未识别OpCode {last?.OpCode} {last?.Operand} {(type.DeclaringType ?? type).Name} {method.Name}");
                        }
                    }
                }
            }
        }

        FileInfo fi = new FileInfo($"{Application.dataPath}/../../../Excel/Language/#genFromCode.xlsx");

        IWorkbook workbook;
        using (var fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            workbook = new XSSFWorkbook(fs);
            ISheet sheet = workbook.GetSheetAt(0);
            HashSet<string> keys = new();
            for (int row = 2; row <= sheet.LastRowNum; row++)
            {
                var rowInst = sheet.GetRow(row);
                if (rowInst != null) //null is when the row only contains empty cells
                {
                    var key = rowInst.GetCell(0)?.ToString();
                    if (key != null && ret.TryGetValue(key, out var method))
                        (rowInst.GetCell(1) ?? rowInst.CreateCell(1)).SetCellValue(method);
                    else
                        (rowInst.GetCell(1) ?? rowInst.CreateCell(1)).SetCellValue("");
                    ret.Remove(key);
                }
            }
            int len = sheet.LastRowNum;
            foreach (var item in ret)
            {
                int index = ++len;
                var rowInst = sheet.CreateRow(index);
                (rowInst.GetCell(0) ?? rowInst.CreateCell(0)).SetCellValue(item.Key);
                (rowInst.GetCell(1) ?? rowInst.CreateCell(1)).SetCellValue(item.Value);
            }
        }
       

        using (FileStream fs2 = new FileStream(fi.FullName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
        {
            workbook.Write(fs2); // 保存到文件
        }

        EditorUtility.DisplayDialog("完成", "完成导出", "OK", "取消");
    }
}