using System;

namespace Game
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SceneAttribute : SAttribute
    {
        public int type { get; }
        public int ID { get; }
        public SceneAttribute(int type = 0, int id = 0)
        {
            this.type = type;
            this.ID = id;
        }
    }
}