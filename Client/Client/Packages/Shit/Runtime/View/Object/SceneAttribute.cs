using System;

[AttributeUsage(AttributeTargets.Class)]
public class SceneAttribute : SAttribute
{
    public string AssetPath { get; }
    public SceneAttribute(string AssetPath)
    {
        this.AssetPath = AssetPath;
    }
}