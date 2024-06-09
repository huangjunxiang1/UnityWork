using Core;
using Game;
using UnityEngine;

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
    public class EC_ClickSObject<T> : GenericEvent where T : SObject
    {
        public EC_ClickSObject(T t) => this.target = t;
        public T target { get; }
    }
    public class EC_GameObjectReplace
    {
        public GameObjectComponent Component;
        public GameObject old;
    }
}
