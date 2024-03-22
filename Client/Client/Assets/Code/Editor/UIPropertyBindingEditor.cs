using Game;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.Search;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using Sirenix.OdinInspector.Editor;
using System.Linq;
using PB;
using System.Collections;
using UnityEngine.Rendering;
using Sirenix.OdinInspector;
using static UnityEditor.Progress;
using FairyGUI;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Reflection;
using System.Xml.Schema;
using UnityEngine.UI;
using hot;
using System.IO;
using Newtonsoft.Json;
using UnityEditor.UI;
using static UnityEngine.Rendering.VolumeComponent;
using UnityEditor.PackageManager.UI;
using UnityEditor.Compilation;
using DG.Tweening.Plugins.Core.PathCore;
using System.Text;
using Core;

class ABCode
{
    public static ABCode Inst;

    public Type uiProType;

    public Type uiType;
    public Type dataType;
    public Type getterType;

    public string getterPath;
}
static class UIPropertyBindingUtil
{
    public static ABCode GetABCode(this Type gType, string piName)
    {
        if (gType == null) return new();
        var pi = gType.GetProperties().ToList().Find(t => t.Name == piName);
        if (pi == null || !pi.PropertyType.IsGenericType) return new();

        ABCode code = new();
        code.uiProType = pi.PropertyType;

        var arr = pi.PropertyType.BaseType.GetGenericArguments();
        code.uiType = arr[0];
        code.dataType = arr[1];
        code.getterType = arr[2];

        var codes = File.ReadAllLines(ShitSettings.Inst.HotPath + "_Gen/UUI.cs").ToList();
        int index = codes.FindIndex(t => t.Contains(gType.Name + " : UUI"));
        int indexStart = codes.FindIndex(index, t => t.Contains("protected sealed override void Binding()"));
        int indexEnd = codes.FindIndex(index, t => t.Contains("public override void Dispose()"));
        for (int i = indexStart + 1; i < indexEnd; i++)
        {
            if (codes[i].Contains($"this.{piName}"))
            {
                var s = codes[i].Split(new string[] { "t.", ");" }, StringSplitOptions.RemoveEmptyEntries)[1].Replace(" ", null);
                code.getterPath = s;
                break;
            }
        }

        return code;
    }
}

//[CustomEditor(typeof(Text), true)]
public class TextEditor2 : UnityEditor.UI.TextEditor
{
    Type gType;
    string[] types;
    int index = 0;

    int classIndex = 0;
    string[] className;

    int fieldIndex = 0;
    Dictionary<string, string[]> fieldName;

    protected override void OnEnable()
    {
        base.OnEnable();
        CompilationPipeline.assemblyCompilationFinished += OnAssemblyCompilationFinished;
        init();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        CompilationPipeline.assemblyCompilationFinished -= OnAssemblyCompilationFinished;
    }

    private void OnAssemblyCompilationFinished(string assemblyPath, CompilerMessage[] messages)
    {
        init();
    }

    void init()
    {
        var o = ((Component)target).transform;
        while (o.parent)
            o = o.parent;
        gType = typeof(Program).Assembly.GetType(o.name);
        ABCode.Inst = gType.GetABCode(target.name + target.GetType().Name);

        List<string> list = new List<string>() { "无", "UIPropertyBinding" };
        var ts = typeof(Program).Assembly.GetTypes();
        for (int i = 0; i < ts.Length; i++)
        {
            var t = ts[i];
            /*if (t.BaseType.IsGenericType && t.BaseType?.GetGenericTypeDefinition() == typeof(UIPropertyBinding<,,>))
            {
                if (t.BaseType.GenericTypeArguments[0].IsAssignableFrom(target.GetType()))
                {
                    list.Add(t.Name.Split('`')[0]);
                }
            }*/
        }
        if (ABCode.Inst.uiProType != null)
        {
            index = list.FindIndex(t => t == ABCode.Inst.uiProType.Name.Split('`')[0]);
            if (index == -1) index = 0;
        }
        else
            index = 0;
        types = list.ToArray();

        List<Type> ds = new();
        foreach (var item in typeof(CoreWorld).Assembly.GetTypes())
        {
            if (typeof(IData).IsAssignableFrom(item) && typeof(IData) != item && item != typeof(PB.PBMessage))
                ds.Add(item);
        }
        foreach (var item in typeof(Program).Assembly.GetTypes())
        {
            if (typeof(IData).IsAssignableFrom(item) && typeof(IData) != item && item != typeof(PB.PBMessage))
                ds.Add(item);
        }
        foreach (var item in typeof(TabM).Assembly.GetTypes())
        {
            if (typeof(IData).IsAssignableFrom(item) && typeof(IData) != item && item != typeof(PB.PBMessage))
                ds.Add(item);
        }

        list = new List<string>() { "无", };
        fieldName = new() { ["无"] = new string[1] { "无" } };
        foreach (var item in ds)
        {
            list.Add(item.FullName.Replace(".", "/"));

            List<string> v = new List<string>() { "无" };
            getDs(true,"", item, v);
            fieldName.Add(item.FullName.Replace(".", "/"), v.ToArray());

        }
        if (ABCode.Inst.dataType != null)
        {
            string s = ABCode.Inst.dataType.FullName.Replace(".", "/");
            classIndex = list.FindIndex(t => t == s);
            if (classIndex == -1) classIndex = 0;
        }
        else
            classIndex = 0;
        className = list.ToArray();
    }
    void getDs(bool isFirst, string path, Type type, List<string> list)
    {
        foreach (var item in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
        {
            if (typeof(IList).IsAssignableFrom(item.FieldType) || typeof(IDictionary).IsAssignableFrom(item.FieldType))
                continue;
            if (isFirst)
            {
                list.Add($"{item.Name}\t{item.FieldType.FullName}");
                if (!item.FieldType.IsPrimitive && item.FieldType != typeof(string))
                    getDs(false, $"{item.Name}", item.FieldType, list);
            }
            else
            {
                list.Add($"{path}/{item.Name}\t{item.FieldType.FullName}");
                if (!item.FieldType.IsPrimitive && item.FieldType != typeof(string))
                    getDs(false, $"{path}/{item.Name}", item.FieldType, list);
            }
        }
        foreach (var item in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (typeof(IList).IsAssignableFrom(item.PropertyType) || typeof(IDictionary).IsAssignableFrom(item.PropertyType))
                continue;
            if (isFirst)
            {
                list.Add($"{item.Name}\t{item.PropertyType.FullName}");
                if (!item.PropertyType.IsPrimitive && item.PropertyType != typeof(string))
                    getDs(false, $"{item.Name}", item.PropertyType, list);
            }
            else
            {
                list.Add($"{path}/{item.Name}\t{item.PropertyType.FullName}");
                if (!item.PropertyType.IsPrimitive && item.PropertyType != typeof(string))
                    getDs(false, $"{path}/{item.Name}", item.PropertyType, list);
            }
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (target.name.StartsWith("_"))
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("属性绑定");
            int index2 = EditorGUILayout.Popup(index, types);
            if (index2 != index)
            {
                index = index2;
                genClassCode();
            }
            int classIndex2 = EditorGUILayout.Popup(classIndex, className);
            if (classIndex2 != classIndex)
            {
                classIndex = classIndex2;
                genClassCode();
            }
            string[] c = fieldName[className[classIndex2]];
            int fieldIndex2 = EditorGUILayout.Popup(fieldIndex, c);
            if (fieldIndex2 != fieldIndex)
            {
                fieldIndex = fieldIndex2;
                genClassCode();
            }
        }
    }

    void genClassCode()
    {
        if (index == 0) return;
        if (classIndex == 0 || fieldIndex == 0)
            return;
        string s1 = types[index];
        string s2 = className[classIndex];
        string[] c = fieldName[className[classIndex]];
        string s3 = c[fieldIndex];
        if (gType == null)
        {
            var o = ((Component)target).transform;
            while (o.parent)
                o = o.parent;
            StringBuilder code = new();
            code.AppendLine(@$"partial class {o.name} : UUI");
            code.AppendLine(@$"{{");
            code.AppendLine($"    public sealed override string url => \"\";");
            code.AppendLine(@$"    public {s1}<{s2.Replace("/", ".")}, {s3.Split("\t")[1]}> {target.name + target.GetType().Name} {{ get; private set; }}");
            code.AppendLine(@$"    protected sealed override void Binding()");
            code.AppendLine(@$"    {{");
            code.AppendLine(@$"    }}");
            code.AppendLine(@$"}}");
            File.AppendAllText(ShitSettings.Inst.HotPath + $"/_Gen/UUI.cs", code.ToString());
            AssetDatabase.Refresh();
        }
    }
}