using Core;

namespace Event
{
    public class EC_GameStart : IData { }
    public class EC_QuitGame : IData { }
    public class EC_InScene : IData
    {
        public int sceneId;
        public int sceneType;
    }
    public class EC_OutScene : IData
    {
        public int sceneId;
        public int sceneType;
    }
    public class EC_ClickSObject<T> where T : SObject
    {
        public EC_ClickSObject(T t) => this.target = t;
        public T target { get; }
    }
}
