using System;

[AttributeUsage(AttributeTargets.Class)]
public class SceneAttribute : SAttribute
{
    public string name { get; }
    public SceneAttribute(string name)
    {
        this.name = name;
    }
}