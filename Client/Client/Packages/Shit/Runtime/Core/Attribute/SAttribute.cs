using System;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class SAttribute : Attribute
{
}
[AttributeUsage(AttributeTargets.Assembly)]
public sealed class AssemblyIncludedToShitRuntime : SAttribute
{
    public int SortOrder { get; }
    public AssemblyIncludedToShitRuntime(int sortOrder = 0)
    {
        this.SortOrder = sortOrder;
    }
}
