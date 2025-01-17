using Core;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Event
{
    public class EC_GameStart { }
    public class EC_QuitGame { }
    public class EC_InScene
    {
        public int sceneId;
        public int sceneType;
    }
    public class EC_OutScene
    {
        public int sceneId;
        public int sceneType;
    }
    public class EC_PointerClick<T> : GenericEvent where T : SObject
    {
        public EC_PointerClick(T t, PointerEventData d)
        {
            this.target = t;
            this.Pointer = d;
        }
        public T target { get; }
        public PointerEventData Pointer { get; }
    }
    public class EC_PointerDown<T> : GenericEvent where T : SObject
    {
        public EC_PointerDown(T t, PointerEventData d)
        {
            this.target = t;
            this.Pointer = d;
        }
        public T target { get; }
        public PointerEventData Pointer { get; }
    }
    public class EC_GameObjectReplace
    {
        public GameObjectComponent Component;
        public GameObject old;
    }
    public class EC_NetworkReconnection { }
    /// <summary>
    /// 模块测试
    /// </summary>
    public class EC_ModuleTest { }
}
