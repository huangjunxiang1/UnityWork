using System;

namespace Game
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SceneAttribute : SAttribute
    {
        public string name { get; }
        public SceneAttribute(string name)
        {
            this.name = name;
        }
    }
}