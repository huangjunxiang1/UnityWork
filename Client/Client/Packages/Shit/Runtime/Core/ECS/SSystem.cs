using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    enum SystemType
    {
        None,
        Change,
        AnyChange,
        KvWatcher,
        Update,
    }
    internal class SSystem
    {
        public SSystem(World world) => this.world = world;
        World world;

        internal Dictionary<Type, __SystemHandle> _BeforeUpdateSystem = new();
        internal Dictionary<Type, __SystemHandle> _UpdateSystem = new();
        internal Dictionary<Type, __SystemHandle> _LateUpdateSystem = new();
        internal Dictionary<Type, __SystemHandle> _ChangeSystem = new();
        internal Dictionary<Type, __SystemHandle> _AnyChangeSystem = new();
        internal Dictionary<Type, __SystemHandle> _InSystem = new();
        internal Dictionary<Type, __SystemHandle> _OutSystem = new();
        internal Dictionary<Type, __KVWatcher> _KvWatcherSystem = new();

        internal Dictionary<Type, List<__SystemHandle>> _beforeUpdateHandlerCreater = new();
        internal Dictionary<Type, List<__SystemHandle>> _updateHandlerCreater = new();
        internal Dictionary<Type, List<__SystemHandle>> _lateUpdateHandlerCreater = new();
        internal Dictionary<Type, List<__SystemHandle>> _changeHandlerCreater = new();
        internal Dictionary<Type, List<__SystemHandle>> _anyChangeHandlerCreater = new();
        internal Dictionary<Type, List<__SystemHandle>> _inCreater = new();
        internal Dictionary<Type, List<__SystemHandle>> _outCreater = new();
        internal Dictionary<Type, List<__SystemHandle>> _eventWatcherCreater = new();
        internal Dictionary<Type, List<__KVWatcher>> _kvWatcherCreater = new();

        ConcurrentQueue<ComponentFilter> changeWaitInvoke = ObjectPool.Get<ConcurrentQueue<ComponentFilter>>();
        internal HashSet<SComponent> waitRemove = new();
        Dictionary<Type, List<SObject>> eventWatcherFirstType = new(100);
        HashSet<Type> eventWatcherWaitRemove = new();

        internal void Load(List<MethodParseData> methods)
        {
            Type[] _systemHandlerGT = new Type[10]
            {
                typeof(SystemHandler<>),
                typeof(SystemHandler<,>),
                typeof(SystemHandler<,,>),
                typeof(SystemHandler<,,,>),
                typeof(SystemHandler<,,,,>),
                typeof(SystemHandler<,,,,,>),
                typeof(SystemHandler<,,,,,,>),
                typeof(SystemHandler<,,,,,,,>),
                typeof(SystemHandler<,,,,,,,,>),
                typeof(SystemHandler<,,,,,,,,,>),
            }; 
            Type[] _eventWatcherGT = new Type[9]
            {
                typeof(EventWatcher<,>),
                typeof(EventWatcher<,,>),
                typeof(EventWatcher<,,,>),
                typeof(EventWatcher<,,,,>),
                typeof(EventWatcher<,,,,,>),
                typeof(EventWatcher<,,,,,,>),
                typeof(EventWatcher<,,,,,,,>),
                typeof(EventWatcher<,,,,,,,,>),
                typeof(EventWatcher<,,,,,,,,,>),
            };
            Type[] _kvWatcherGT = new Type[10]
            {
                typeof(KVWatcher<>),
                typeof(KVWatcher<,>),
                typeof(KVWatcher<,,>),
                typeof(KVWatcher<,,,>),
                typeof(KVWatcher<,,,,>),
                typeof(KVWatcher<,,,,,>),
                typeof(KVWatcher<,,,,,,>),
                typeof(KVWatcher<,,,,,,,>),
                typeof(KVWatcher<,,,,,,,,>),
                typeof(KVWatcher<,,,,,,,,,>),
            };
            Type[] _actionGT = new Type[10]
            {
                 typeof(Action<>),
                 typeof(Action<,>),
                 typeof(Action<,,>),
                 typeof(Action<,,,>),
                 typeof(Action<,,,,>),
                 typeof(Action<,,,,,>),
                 typeof(Action<,,,,,,>),
                 typeof(Action<,,,,,,,>),
                 typeof(Action<,,,,,,,,>),
                 typeof(Action<,,,,,,,,,>),
            };
            Dictionary<Type, __SystemHandle> _EventWatcherSystem = new();

            for (int i = 0; i < methods.Count; i++)
            {
                var m = methods[i];
                if (m.attribute is BeforeUpdateSystem)
                {
                    var ts = m.parameters.Select(t => t.ParameterType).ToArray();
                    var st = _systemHandlerGT[ts.Length - 1].MakeGenericType(ts);

                    if (!_BeforeUpdateSystem.TryGetValue(st, out var v))
                    {
                        _BeforeUpdateSystem[st] = v = (__SystemHandle)Activator.CreateInstance(st);
                        for (int j = 0; j < ts.Length; j++)
                        {
                            var key = ts[j];
                            if (!_beforeUpdateHandlerCreater.TryGetValue(key, out var lst))
                                _beforeUpdateHandlerCreater[key] = lst = new();
                            lst.Add(v);
                        }
                    }
                    v.Add(m.method.CreateDelegate(_actionGT[ts.Length - 1].MakeGenericType(ts)));
                    continue;
                }
                if (m.attribute is UpdateSystem)
                {
                    var ts = m.parameters.Select(t => t.ParameterType).ToArray();
                    var st = _systemHandlerGT[ts.Length - 1].MakeGenericType(ts);

                    if (!_UpdateSystem.TryGetValue(st, out var v))
                    {
                        _UpdateSystem[st] = v = (__SystemHandle)Activator.CreateInstance(st);
                        for (int j = 0; j < ts.Length; j++)
                        {
                            var key = ts[j];
                            if (!_updateHandlerCreater.TryGetValue(key, out var lst))
                                _updateHandlerCreater[key] = lst = new();
                            lst.Add(v);
                        }
                    }
                    v.Add(m.method.CreateDelegate(_actionGT[ts.Length - 1].MakeGenericType(ts)));
                    continue;
                }
                if (m.attribute is LateUpdateSystem)
                {
                    var ts = m.parameters.Select(t => t.ParameterType).ToArray();
                    var st = _systemHandlerGT[ts.Length - 1].MakeGenericType(ts);

                    if (!_LateUpdateSystem.TryGetValue(st, out var v))
                    {
                        _LateUpdateSystem[st] = v = (__SystemHandle)Activator.CreateInstance(st);
                        for (int j = 0; j < ts.Length; j++)
                        {
                            var key = ts[j];
                            if (!_lateUpdateHandlerCreater.TryGetValue(key, out var lst))
                                _lateUpdateHandlerCreater[key] = lst = new();
                            lst.Add(v);
                        }
                    }
                    v.Add(m.method.CreateDelegate(_actionGT[ts.Length - 1].MakeGenericType(ts)));
                    continue;
                }
                if (m.attribute is ChangeSystem)
                {
                    var ts = m.parameters.Select(t => t.ParameterType).ToArray();
                    var st = _systemHandlerGT[ts.Length - 1].MakeGenericType(ts);

                    if (!_ChangeSystem.TryGetValue(st, out var v))
                    {
                        _ChangeSystem[st] = v = (__SystemHandle)Activator.CreateInstance(st);
                        for (int j = 0; j < ts.Length; j++)
                        {
                            var key = ts[j];
                            if (!_changeHandlerCreater.TryGetValue(key, out var lst))
                                _changeHandlerCreater[key] = lst = new();
                            lst.Add(v);
                        }
                    }
                    v.Add(m.method.CreateDelegate(_actionGT[ts.Length - 1].MakeGenericType(ts)));
                    continue;
                }
                if (m.attribute is AnyChangeSystem)
                {
                    var ts = m.parameters.Select(t => t.ParameterType).ToArray();
                    var st = _systemHandlerGT[ts.Length - 1].MakeGenericType(ts);

                    if (!_AnyChangeSystem.TryGetValue(st, out var v))
                    {
                        _AnyChangeSystem[st] = v = (__SystemHandle)Activator.CreateInstance(st);
                        for (int j = 0; j < ts.Length; j++)
                        {
                            var key = ts[j];
                            if (!_anyChangeHandlerCreater.TryGetValue(key, out var lst))
                                _anyChangeHandlerCreater[key] = lst = new();
                            lst.Add(v);
                        }
                    }
                    v.Add(m.method.CreateDelegate(_actionGT[ts.Length - 1].MakeGenericType(ts)));
                    continue;
                }
                if (m.attribute is InSystem)
                {
                    var ts = m.parameters.Select(t => t.ParameterType).ToArray();
                    var st = _systemHandlerGT[ts.Length - 1].MakeGenericType(ts);

                    if (!_InSystem.TryGetValue(st, out var v))
                    {
                        _InSystem[st] = v = (__SystemHandle)Activator.CreateInstance(st);
                        for (int j = 0; j < ts.Length; j++)
                        {
                            var key = ts[j];
                            if (!_inCreater.TryGetValue(key, out var lst))
                                _inCreater[key] = lst = new();
                            lst.Add(v);
                        }
                    }
                    v.Add(m.method.CreateDelegate(_actionGT[ts.Length - 1].MakeGenericType(ts)));
                    continue;
                }
                if (m.attribute is OutSystem)
                {
                    var ts = m.parameters.Select(t => t.ParameterType).ToArray();
                    var st = _systemHandlerGT[ts.Length - 1].MakeGenericType(ts);

                    if (!_OutSystem.TryGetValue(st, out var v))
                    {
                        _OutSystem[st] = v = (__SystemHandle)Activator.CreateInstance(st);
                        for (int j = 0; j < ts.Length; j++)
                        {
                            var key = ts[j];
                            if (!_outCreater.TryGetValue(key, out var lst))
                                _outCreater[key] = lst = new();
                            lst.Add(v);
                        }
                    }
                    v.Add(m.method.CreateDelegate(_actionGT[ts.Length - 1].MakeGenericType(ts)));
                    continue;
                }
                if (m.attribute is EventWatcherSystem)
                {
                    var ts = m.parameters.Select(t => t.ParameterType).ToArray();
                    var st = _eventWatcherGT[ts.Length - 2].MakeGenericType(ts);

                    if (!_EventWatcherSystem.TryGetValue(st, out var v))
                    {
                        _EventWatcherSystem[st] = v = (__SystemHandle)Activator.CreateInstance(st);
                        var key = ts[0];
                        if (!_eventWatcherCreater.TryGetValue(key, out var lst))
                            _eventWatcherCreater[key] = lst = new();
                        if (!eventWatcherFirstType.TryGetValue(ts[1], out var os))
                            eventWatcherFirstType[ts[1]] = os = new();
                        lst.Add(v);
                    }
                    v.Add(m.method.CreateDelegate(_actionGT[ts.Length - 1].MakeGenericType(ts)));
                    continue;
                }
                if (m.attribute is KVWatcherSystem ks)
                {
                    var ts = m.parameters.Select(t => t.ParameterType).ToArray();
                    var st = _kvWatcherGT[ts.Length - 1].MakeGenericType(ts);

                    if (!_KvWatcherSystem.TryGetValue(st, out var v))
                    {
                        _KvWatcherSystem[st] = v = (__KVWatcher)Activator.CreateInstance(st);
                        for (int j = 0; j < ts.Length; j++)
                        {
                            var key = ts[j];
                            if (!_kvWatcherCreater.TryGetValue(key, out var lst))
                                _kvWatcherCreater[key] = lst = new();
                            lst.Add(v);
                        }
                        if (!ts.Contains(typeof(KVComponent)))
                        {
                            if (!_kvWatcherCreater.TryGetValue(typeof(KVComponent), out var lst))
                                _kvWatcherCreater[typeof(KVComponent)] = lst = new();
                            lst.Add(v);
                        }
                    }
                    v.Add(ks.K, m.method.CreateDelegate(_actionGT[ts.Length - 1].MakeGenericType(ts)));
                    continue;
                }
            }
        }

        internal void RigisterHandler(Type type, SComponent c)
        {
            {
                if (_beforeUpdateHandlerCreater.TryGetValue(type, out var lst))
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        var o = lst[i].Filter(c.Entity, true);
                        if (o != null)
                        {
                            o.type = SystemType.Update;
                            o._addTo_HandlesList();
#if UNITY_EDITOR
                            c.Entity._Other.Add(o);
#endif
                        }
                    }
                }
            }
            {
                if (_updateHandlerCreater.TryGetValue(type, out var lst))
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        var o = lst[i].Filter(c.Entity, true);
                        if (o != null)
                        {
                            o.type = SystemType.Update;
                            o._addTo_HandlesList();
#if UNITY_EDITOR
                            c.Entity._Other.Add(o);
#endif
                        }
                    }
                }
            }
            {
                if (_lateUpdateHandlerCreater.TryGetValue(type, out var lst))
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        var o = lst[i].Filter(c.Entity, true);
                        if (o != null)
                        {
                            o.type = SystemType.Update;
                            o._addTo_HandlesList();
#if UNITY_EDITOR
                            c.Entity._Other.Add(o);
#endif
                        }
                    }
                }
            }
            {
                if (_changeHandlerCreater.TryGetValue(type, out var lst))
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        var o = lst[i].Filter(c.Entity);
                        if (o != null)
                        {
                            o.type = SystemType.Change;
                            o._addTo_HandlesList();
                            o.dirty = true;
                            changeWaitInvoke.Enqueue(o);
#if UNITY_EDITOR
                            c.Entity._Other.Add(o);
#endif
                        }
                    }
                }
            }
            {
                if (_anyChangeHandlerCreater.TryGetValue(type, out var lst))
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        var o = lst[i].Filter(c.Entity);
                        if (o != null)
                        {
                            o.type = SystemType.AnyChange;
                            o._addTo_HandlesList();
#if UNITY_EDITOR
                            c.Entity._Other.Add(o);
#endif
                        }
                    }
                }
            }
            {
                if (eventWatcherFirstType.TryGetValue(type, out var lst))
                    lst.Add(c.Entity);
            }
            {
                if (_kvWatcherCreater.TryGetValue(type, out var lst))
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        var o = lst[i].Filter(c.Entity);
                        if (o != null)
                        {
                            o.type = SystemType.KvWatcher;
                            o._addTo_HandlesList();
                            o._addTo_kvHandlesList();
#if UNITY_EDITOR
                            c.Entity._Other.Add(o);
#endif
                        }
                    }
                }
            }
            c.SetChangeFlag();
        }
        internal void UnRigisterHandler(Type type, SComponent c)
        {
            if (eventWatcherFirstType.ContainsKey(type))
                eventWatcherWaitRemove.Add(type);
        }

        internal void Change(SComponent c, bool call = false)
        {
            if (call)
            {
                for (int i = 0; i < c._Handles.Count; i++)
                    if (c._Handles[i].type == SystemType.AnyChange || (c._Handles[i].type == SystemType.Change && c._Handles[i].GetFirstComponent() == c))
                        c._Handles[i].dirty = true;
                for (int i = 0; i < c._Handles.Count; i++)
                {
                    if (c._Handles[i].dirty && (c._Handles[i].type == SystemType.AnyChange || (c._Handles[i].type == SystemType.Change && c._Handles[i].GetFirstComponent() == c)))
                    {
                        c._Handles[i].dirty = false;
                        if (c._Handles[i].Disposed || c._Handles[i].EnableCounter != 0) continue;
                        c._Handles[i].Invoke();
                        if (c.Disposed) break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < c._Handles.Count; i++)
                {
                    if (!c._Handles[i].dirty && (c._Handles[i].type == SystemType.AnyChange || (c._Handles[i].type == SystemType.Change && c._Handles[i].GetFirstComponent() == c)))
                    {
                        c._Handles[i].dirty = true;
                        changeWaitInvoke.Enqueue(c._Handles[i]);
                    }
                }
            }
        }
        internal void In(Type type, SComponent c)
        {
            if (!_inCreater.TryGetValue(type, out var v))
                return;
            for (int i = 0; i < v.Count; i++)
            {
                var o = v[i].Filter(c.Entity);
                if (o != null && o.EnableCounter == 0)
                {
                    o.Invoke();
#if UNITY_EDITOR
                    c.Entity._In.Add(o);
#endif
                }
            }
        }
        internal void Out(Type type, SComponent c)
        {
            if (!_outCreater.TryGetValue(type, out var v))
                return;
            for (int i = 0; i < v.Count; i++)
            {
                var o = v[i].Filter(c.Entity);
                if (o != null && o.EnableCounter == (c.Enable ? 0 : 1))
                {
                    o.Invoke();
#if UNITY_EDITOR
                    c.Entity._Out.Add(o);
#endif
                }
            }
        }
        internal void EventWatcher(object o, long ActorId = 0, long gid = 0)
        {
            if (!_eventWatcherCreater.TryGetValue(o.GetType(), out var handlers))
                return;
            if (ActorId != 0)
            {
                if (!world.ObjectManager.TryGetByActorId(ActorId, out var lst))
                    return;
                int len = lst.Count;
                for (int i = 0; i < len; i++)
                {
                    for (int j = 0; j < handlers.Count; j++)
                        if (!lst[i].Disposed)
                            handlers[j]._invoke_eventWatcher(o, lst[i]);
                }
            }
            else if (gid != 0)
            {
                if (!world.ObjectManager.TryGetByGid(gid, out var obj))
                    return;
                for (int j = 0; j < handlers.Count; j++)
                    if (!obj.Disposed)
                        handlers[j]._invoke_eventWatcher(o, obj);
            }
            else
            {
                for (int i = 0; i < handlers.Count; i++)
                {
                    if (eventWatcherFirstType.TryGetValue(handlers[i]._get_firstType(), out var lst))
                    {
                        int len = lst.Count;
                        for (int j = 0; j < len; j++)
                            if (!lst[j].Disposed)
                                handlers[i]._invoke_eventWatcher(o, lst[j]);
                    }
                }
            }
        }

        internal void beforeUpdate()
        {
            foreach (var item in _BeforeUpdateSystem.Values)
                item._invoke_update();
        }
        internal void update()
        {
            foreach (var item in _UpdateSystem.Values)
                item._invoke_update();

            if (changeWaitInvoke.Count > 0)
            {
                var change = changeWaitInvoke;
                changeWaitInvoke = ObjectPool.Get<ConcurrentQueue<ComponentFilter>>();
                while (change.TryDequeue(out var c))
                {
                    if (!c.dirty) continue;
                    c.dirty = false;
                    if (c.Disposed || c.EnableCounter != 0)
                        continue;
                    c.Invoke();
                }
                ObjectPool.Return(change);
            }
        }
        internal void lateUpdate()
        {
            foreach (var item in _LateUpdateSystem.Values)
                item._invoke_update();

            if (waitRemove.Count > 0)
            {
                foreach (var c in waitRemove)
                {
                    if (c.Disposed) continue;
                    c._Handles.RemoveAll(t => t.Disposed);
                }
                waitRemove.Clear();
            }

            if (eventWatcherWaitRemove.Count > 0)
            {
                foreach (var item in eventWatcherWaitRemove)
                    eventWatcherFirstType[item].RemoveAll(t => t.Disposed);
                eventWatcherWaitRemove.Clear();
            }
        }
    }
}
