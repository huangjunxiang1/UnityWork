using Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Main
{
    public static class SSystem
    {
        static Dictionary<Type, List<Type>> condition = new();
        static Dictionary<Type, AddComponentIfTemp> addIf = new();
        static Dictionary<SObject, HashSet<Type>> checkComponents = ObjectPool<Dictionary<SObject, HashSet<Type>>>.Get();

        static List<SComponent> changeFlag = ObjectPool<List<SComponent>>.Get();
        static Dictionary<Type, List<ChangeHandler>> changeHandler = new();

#if UNITY_EDITOR
        internal static void EditorClear()
        {
            condition.Clear();
            addIf.Clear();
            checkComponents.Clear();

            changeFlag.Clear();
            changeHandler.Clear();
        }
#endif

        internal static void RigisteComponent(SComponent c)
        {
            if (condition.TryGetValue(c.GetType(), out var lst))
            {
                if (!checkComponents.TryGetValue(c.Entity, out var cs))
                    checkComponents[c.Entity] = cs = ObjectPool<HashSet<Type>>.Get();
                for (int i = 0; i < lst.Count; i++)
                    cs.Add(lst[i]);
            }
        }
        internal static void UnRigisteComponent(SComponent c)
        {
            if (condition.TryGetValue(c.GetType(), out var lst))
            {
                if (!checkComponents.TryGetValue(c.Entity, out var cs))
                    checkComponents[c.Entity] = cs = ObjectPool<HashSet<Type>>.Get();
                for (int i = 0; i < lst.Count; i++)
                    cs.Add(lst[i]);
            }
        }
        internal static void EnableComponent(SComponent c)
        {
            if (condition.TryGetValue(c.GetType(), out var lst))
            {
                if (!checkComponents.TryGetValue(c.Entity, out var cs))
                    checkComponents[c.Entity] = cs = ObjectPool<HashSet<Type>>.Get();
                for (int i = 0; i < lst.Count; i++)
                    cs.Add(lst[i]);
            }
        }
        internal static void SetChange(SComponent c) => changeFlag.Add(c);
        internal static void CheckComponent(SObject entity, HashSet<Type> types)
        {
            foreach (var type in types)
            {
                if (addIf.TryGetValue(type, out var tmp))
                {
                    if (tmp.isObj != null)
                    {
                        if (!tmp.isObj.Type.IsAssignableFrom(entity.GetType()))
                            goto remove;
                    }
                    if (tmp.all != null)
                    {
                        for (int j = 0; j < tmp.all.Types.Length; j++)
                        {
                            if (!entity.HasComponent(tmp.all.Types[j]))
                                goto remove;
                        }
                    }
                    if (tmp.any != null)
                    {
                        for (int j = 0; j < tmp.any.Types.Length; j++)
                        {
                            if (entity.HasComponent(tmp.any.Types[j]))
                                goto check;
                        }
                        goto remove;
                    }
                check:;
                    if (tmp.none != null)
                    {
                        for (int j = 0; j < tmp.none.Types.Length; j++)
                        {
                            if (entity.HasComponent(tmp.none.Types[j]))
                                goto remove;
                        }
                    }
                    entity.AddComponentInternal(type);
                    continue;
                remove: entity.RemoveComponentInternal(type);
                }
            }
        }
        internal static bool isAutoAddComponent(Type type) => addIf.ContainsKey(type);

        /// <summary>
        /// 反射注册所有静态函数的消息和事件监听
        /// </summary>
        internal static void Init(List<MethodParseData> methods)
        {
            Dictionary<Type, ChangeHandler> map = new();
            for (int i = 0; i < methods.Count; i++)
            {
                MethodParseData ma = methods[i];
                if (ma.attribute is EventAttribute && ma.method != null && typeof(__Change).IsAssignableFrom(ma.mainKey))
                {
                    if (!map.TryGetValue(ma.mainKey, out var handler))
                    {
                        map[ma.mainKey] = handler = new(ma.mainKey);
                        for (int j = 0; j < handler.types.Length; j++)
                        {
                            var key = handler.types[j];
                            if (!changeHandler.TryGetValue(key, out var lst))
                                changeHandler[key] = lst = new();
                            lst.Add(handler);
                        }
                    }
                }
            }
            for (int i = 0; i < Types.AllTypes.Count; i++)
            {
                Type t = Types.AllTypes[i];
                if (typeof(SComponent).IsAssignableFrom(t))
                {
                    var all = t.GetCustomAttribute<AddComponentIfAll>();
                    var any = t.GetCustomAttribute<AddComponentIfAny>();
                    var none = t.GetCustomAttribute<AddComponentIfNone>();
                    var isObj = t.GetCustomAttribute<AddComponentIfIsSObject>();
                    if (none != null && all == null && any == null)
                    {
                        Loger.Error($"{nameof(AddComponentIfNone)}不能单独存在");
                        continue;
                    }
                    if (all != null || any != null || none != null)
                    {
                        AddComponentIfTemp tmp = new();
                        tmp.all = all;
                        tmp.any = any;
                        tmp.none = none;
                        tmp.isObj = isObj;
                        addIf[t] = tmp;

                        if (all != null)
                        {
                            for (int j = 0; j < all.Types.Length; j++)
                            {
                                if (!condition.TryGetValue(all.Types[j], out var list))
                                    condition[all.Types[j]] = list = new();
                                if (!list.Contains(t))
                                    list.Add(t);
                            }
                        }
                        if (any != null)
                        {
                            for (int j = 0; j < any.Types.Length; j++)
                            {
                                if (!condition.TryGetValue(any.Types[j], out var list))
                                    condition[any.Types[j]] = list = new();
                                if (!list.Contains(t))
                                    list.Add(t);
                            }
                        }
                        if (none != null)
                        {
                            for (int j = 0; j < none.Types.Length; j++)
                            {
                                if (!condition.TryGetValue(none.Types[j], out var list))
                                    condition[none.Types[j]] = list = new();
                                if (!list.Contains(t))
                                    list.Add(t);
                            }
                        }
                    }
                }
            }
        }
        internal static void Update()
        {
            if (checkComponents.Count > 0)
            {
                var tmp = checkComponents;
                checkComponents = ObjectPool<Dictionary<SObject, HashSet<Type>>>.Get();
                foreach (var c in tmp)
                {
                    if (c.Key.Disposed) continue;
                    CheckComponent(c.Key, c.Value);
                    c.Value.Clear();
                    ObjectPool<HashSet<Type>>.Return(c.Value);
                }
                tmp.Clear();
                ObjectPool<Dictionary<SObject, HashSet<Type>>>.Return(tmp);
            }

            if (changeFlag.Count > 0)
            {
                var tmp = changeFlag;
                changeFlag = ObjectPool<List<SComponent>>.Get();
                for (int i = 0; i < tmp.Count; i++)
                {
                    var c = tmp[i];
                    if (!c.Enable) continue;
                    c._isChanged = false;
                    if (changeHandler.TryGetValue(c.GetType(), out var handlers))
                    {
                        for (int j = 0; j < handlers.Count; j++)
                        {
                            handlers[j].TryInvoke(c.Entity);
                            if (!c.Enable) break;
                        }
                    }
                }
                tmp.Clear();
                ObjectPool<List<SComponent>>.Return(tmp);
            }
        }
        internal static void AfterUpdate()
        {

        }

        class ChangeHandler
        {
            public ChangeHandler(Type gt)
            {
                this.gt = gt;
                this.ctor = gt.GetConstructors().First();
                this.types = gt.GetGenericArguments();
            }
            Type gt;
            ConstructorInfo ctor;
            public Type[] types;

            public void TryInvoke(SObject obj)
            {
                if (!GameM.Event.HasEvent(gt)) return;
                var ps = ArrayCache<object>.Get(types.Length);
                for (int i = 0; i < types.Length; i++)
                {
                    if (!obj.TryGetComponent(types[i], out var c) || !c.Enable)
                        return;
                    ps[i] = c;
                }
                GameM.Event.RunEvent(ctor.Invoke(ps));
            }
        }
        class AddComponentIfTemp
        {
            public AddComponentIfAll all;
            public AddComponentIfAny any;
            public AddComponentIfNone none;
            public AddComponentIfIsSObject isObj;
        }
    }
}
