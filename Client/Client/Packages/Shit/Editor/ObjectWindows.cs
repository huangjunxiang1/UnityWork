﻿using Core;
using FairyGUI;
using Game;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
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
        ThreadSynchronizationContext.MainThread?.Post(() =>
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
                add(tree, o, path + "/" + o.GetType().Name);
            }
        }
    }
    class WarpObject
    {
        [ShowInInspector] long rpc => obj.rpc;
        [ShowInInspector] long gid => obj.gid;
        [ShowInInspector] long tid => obj.tid;
        [ShowInInspector] object target => obj;

        [ShowInInspector]
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

        [ShowInInspector]
        SystemHandler Systems
        {
            get
            {
                SystemHandler ret = new();

                addView(obj._Awake, ret.Awake);
                addView(obj._components, ret.Dispose);
                addView(obj._In,ret.In);
                addView(obj._components, ret.Out, t =>
                {
                    obj.World.System.outHandle.TryGetValue(t, out var v);
                    return v;
                });

                addView2(c => c._changeHandles, ret.Change);
                addView2(c => c._kvWatcherHandles, ret.KVWatcher);
                obj._updates.RemoveAll(t => t.Disposed);
                addView2(obj._updates, ret.Update);
                obj._timers.RemoveAll(t => t.Disposed);
                addView2(obj._timers, ret.Timer);

                return ret;
            }
        }

        void addView(Dictionary<Type, SComponent> ks, Dictionary<string, SystemHandler.View2> ret)
        {
            foreach (var k in ks)
            {
                if (obj.World.Event.GetEventQueue(typeof(Dispose<>).MakeGenericType(k.Key), out var queue))
                {
                    var ds = queue;
                    foreach (var d in ds)
                        addToList(ret, d, k.Key.Name);
                }
            }
        }
        void addView(List<EventSystem.EvtData> ds, List<View1> ret)
        {
            foreach (var d in ds)
                addToList(ret, d);
        }
        void addView(Dictionary<Type, SComponent> ks, Dictionary<string, SystemHandler.View2> ret, Func<Type, List<(Action<Type, SComponent>, Action<SObject, Dictionary<Type, __OutHandle>>)>> handler)
        {
            foreach (var k in ks)
            {
                var lst2 = handler(k.Key);
                if (lst2 != null)
                {
                    for (int i = 0; i < lst2.Count; i++)
                    {
                        var rt = lst2[i].Item1.Method.ReflectedType;
                        bool find = true;
                        foreach (var item in rt.GetGenericArguments())
                        {
                            if (!obj.HasComponent(item))
                            {
                                find = false;
                                break;
                            }
                        }
                        if (!find) continue;
                        if (obj.World.Event.GetEventQueue(rt, out var lst))
                        {
                            for (int j = 0; j < lst.Count; j++)
                            {
                                var d = lst[j];
                                addToList(ret, d, k.Key.Name);
                            }
                        }
                    }
                }
            }
        }
        void addView2(Func<SComponent, IList> get, List<View1> ret)
        {
            HashSet<object> hs = new();
            foreach (var item in obj._components.Values)
            {
                var list = get(item);
                if (list != null)
                {
                    foreach (var item2 in list)
                    {
                        hs.Add(item2);
                    }
                }
            }
            foreach (var item2 in hs)
            {
                if (obj.World.Event.GetEventQueue(item2.GetType(), out var lst))
                {
                    foreach (var d in lst)
                        addToList(ret, d);
                }
            }
        }
        void addView2(IList list, List<View1> ret)
        {
            foreach (var item in list)
            {
                if (obj.World.Event.GetEventQueue(item.GetType(), out var lst))
                {
                    foreach (var d in lst)
                        addToList(ret, d);
                }
            }
        }

        void addToList(List<View1> ret,EventSystem.EvtData d)
        {
            View1 s = new();
            var method = d.action.Method;

            s.Method = method.ReflectedType.Name + ":" + method.Name;
            var type = method.GetParameters().FirstOrDefault().ParameterType;

            s.Parameter = type.Name[..^2];
            s.Parameter += "<";
            foreach (var item2 in type.GetGenericArguments())
                s.Parameter += $"{item2.Name},";
            s.Parameter = s.Parameter[..^1];
            s.Parameter += ">";

            s.Assembly = d.action.Method.ReflectedType.Assembly.GetName().Name;

            ret.Add(s);
        }
        void addToList(Dictionary<string, SystemHandler.View2> map, EventSystem.EvtData d,string cName)
        {
            if (!map.TryGetValue(cName, out var w))
                map[cName] = w = new();
            var ret = w.list;

            View1 s = new();
            var method = d.action.Method;

            s.Method = method.ReflectedType.Name + ":" + method.Name;
            var type = method.GetParameters().FirstOrDefault().ParameterType;

            s.Parameter = type.Name[..^2];
            s.Parameter += "<";
            foreach (var item4 in type.GetGenericArguments())
                s.Parameter += $"{item4.Name},";
            s.Parameter = s.Parameter[..^1];
            s.Parameter += ">";

            s.Assembly = d.action.Method.ReflectedType.Assembly.GetName().Name;

            ret.Add(s);
        }

        public WarpObject(SObject o) => obj = o;
        internal SObject obj;

        class SystemHandler
        {
            public class View2
            {
                [TableList(ShowIndexLabels = true)]
                public List<View1> list = new();
            }

            [TableList(ShowIndexLabels = true)]
            public List<View1> Awake = new();

            [Space(5)]
            [TableList(ShowIndexLabels = true)]
            public Dictionary<string, View2> Dispose = new();

            [TableList(ShowIndexLabels = true)]
            public List<View1> In = new();

            [Space(5)]
            [TableList(ShowIndexLabels = true)]
            public Dictionary<string, View2> Out = new();

            [Space(5)]
            [TableList(ShowIndexLabels = true)]
            public List<View1> Change = new();

            [Space(5)]
            [TableList(ShowIndexLabels = true)]
            public List<View1> KVWatcher = new();

            [Space(5)]
            [TableList(ShowIndexLabels = true)]
            public List<View1> Update = new();

            [Space(5)]
            [TableList(ShowIndexLabels = true)]
            public List<View1> Timer = new();
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
}