using System;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class SAttribute : Attribute
{
}
[AttributeUsage(AttributeTargets.Assembly)]
public abstract class AssemblyIncludedToShit : SAttribute
{
    public int SortOrder { get; }
    public AssemblyIncludedToShit(int sortOrder = 0)
    {
        this.SortOrder = sortOrder;
    }
}
public sealed class AssemblyIncludedToRuntimeShit : AssemblyIncludedToShit
{
    public AssemblyIncludedToRuntimeShit(int sortOrder = 0) : base(sortOrder) { }
}
