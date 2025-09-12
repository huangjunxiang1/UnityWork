using Core;
using FairyGUI;
using Game;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

class ObjectWindows : OdinMenuEditorWindow
{
    [MenuItem("Shit/对象检视视图")]
    static void open()
    {
        GetWindow<ObjectWindows>();
    }
    [RuntimeInitializeOnLoadMethod]
    static void Init()
    {
        if (EditorWindow.HasOpenInstances<ObjectWindows>())
        {
            GetWindow<ObjectWindows>().Awake();
        }
    }

    private void Awake()
    {
        reload();

        SObject.objChange -= reload;
        SObject.objChange += reload;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();

        SObject.objChange -= reload;
        World.Close -= reload;
    }

    void reload()
    {
        ThreadSynchronizationContext.MainThread?.Post(s =>
        {
            World.Close -= reload;
            World.Close += reload;

            this.ForceMenuTreeRebuild();
            this.UpdateEditors();
            this.Repaint();
        });
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree(supportsMultiSelect: true);

        for (int i = 0; i < World.Worlds.Count; i++)
            add(tree, World.Worlds[i].Root, World.Worlds[i].Name);

        WarpOther pool = new();
        if (Client.Data != null)
        {
            foreach (var item in Client.Data._dataMap)
                pool.Data.Add(GetTypeName(item.Key), item.Value);
        }
        foreach (var item in ObjectPool.p_pool)
            pool.In.Add(GetTypeName(item.Key), item.Value);
        foreach (var item in ObjectPool.p_malloc)
            pool.Out.Add(GetTypeName(item.Key), item.Value);

        tree.Add("Other", pool);

        return tree;
    }
    void add(OdinMenuTree tree, SObject obj, string path)
    {
        tree.Add(path, new WarpObject(obj));
        if (obj is STree)
        {
            var list = obj.As<STree>()._children;
            for (int i = 0; i < list.Count; i++)
            {
                var o = list[i];
                if (o.ActorId != 0)
                    add(tree, o, $"{path}/[{i}] {o.GetType().Name} Actor:{o.ActorId} Gid:{o.gid}");
                else
                    add(tree, o, $"{path}/[{i}] {o.GetType().Name} Gid:{o.gid}");
            }
        }
    }
    class WarpOther
    {
        [ShowInInspector]
        public Dictionary<string, object> Data = new();

        [ShowInInspector]
        public Dictionary<string, List<object>> In = new();
        [ShowInInspector]
        public Dictionary<string, List<object>> Out = new();
    }
    class WarpObject
    {
        [ShowInInspector, EnableGUI] object target => obj;

        [ShowInInspector, EnableGUI]
        Dictionary<string, object> components
        {
            get
            {
                Dictionary<string, object> ret = new();

                string str = null;
                foreach (var item in obj._components)
                {
                    if (item.Value is SObject)
                        str += item.Key.Name + "<-";
                }
                ret["_实体结构"] = str[..^2];

                foreach (var item in obj._components)
                {
                    if (item.Value is not SObject)
                        ret.Add(item.Key.Name, item.Value);
                }

                return ret;
            }
        }

        [ShowInInspector, EnableGUI]
        SystemHandler Systems
        {
            get
            {
                SystemHandler ret = new();

                addView(obj._In,ret.In);
                addView(obj._Out, ret.Out);
                addView(obj._Other.FindAll(t => t.type == SystemType.Change), ret.Change);
                addView(obj._Other.FindAll(t => t.type == SystemType.AnyChange), ret.AnyChange);
                addView(obj._Other.FindAll(t => t.type == SystemType.Update), ret.Update);
                addView(obj._Other.FindAll(t => t.type == SystemType.KvWatcher), ret.KVWatcher);
                addView(obj._EventWatcher, ret.EventWatcher);

                return ret;
            }
        }

        void addView(List<ComponentFilter> ds, List<View1> ret)
        {
            foreach (var d in ds)
                foreach (var d2 in (IList)d.system.GetActions())
                    addToList(ret, ((Delegate)d2).Method);
        }
        void addView(HashSet<__SystemHandle> ds, List<View1> ret)
        {
            foreach (var d in ds)
                foreach (var d2 in (IList)d.GetActions())
                    addToList(ret, ((Delegate)d2).Method);
        }

        void addToList(List<View1> ret, MethodInfo d)
        {
            View1 s = new();
            var method = d;

            s.Method = method.ReflectedType.Name + ":" + method.Name;
            s.Parameter = GetTypeName(method.GetParameters());
            s.Assembly = d.ReflectedType.Assembly.GetName().Name;

            ret.Add(s);
        }

        public WarpObject(SObject o) => obj = o;
        internal SObject obj;

        class SystemHandler
        {
            [TableList(ShowIndexLabels = true)]
            public List<View1> In = new();

            [Space(5)]
            [TableList(ShowIndexLabels = true)]
            public List<View1> Out = new();

            [Space(5)]
            [TableList(ShowIndexLabels = true)]
            public List<View1> Change = new();

            [Space(5)]
            [TableList(ShowIndexLabels = true)]
            public List<View1> AnyChange = new();

            [Space(5)]
            [TableList(ShowIndexLabels = true)]
            public List<View1> Update = new();

            [Space(5)]
            [TableList(ShowIndexLabels = true)]
            public List<View1> KVWatcher = new();

            [Space(5)]
            [TableList(ShowIndexLabels = true)]
            public List<View1> EventWatcher = new();
        }
        class View1
        {
            [LabelText("函数")]
            [LabelWidth(30)]
            public string Method;
            [LabelText("参数")]
            [LabelWidth(30)]
            public string Parameter;
            [LabelText("程序集")]
            [LabelWidth(40)]
            public string Assembly;
        }
    }

    static StringBuilder str = new(100);
    static string GetTypeName(ParameterInfo[] ps)
    {
        str.Clear();
        for (int i = 0; i < ps.Length; i++)
        {
            str.Append(GetTypeName(ps[i].ParameterType));
            str.Append("+");
        }
        return str.ToString();
    }
    static string GetTypeName(Type type)
    {
        string name;
        if (type.IsGenericType)
        {
            name = type.Name[..^2];
            name += "<";
            foreach (var item2 in type.GetGenericArguments())
                name += $"{GetTypeName(item2)},";
            name = name[..^1];
            name += ">";
        }
        else
            name = type.Name;
        return name;
    }
}
