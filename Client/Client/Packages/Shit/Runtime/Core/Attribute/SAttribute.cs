using System;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class SAttribute : Attribute
{
}
[AttributeUsage(AttributeTargets.Assembly)]
public sealed class AssemblyInfoInShitRuntime : SAttribute
{
    public int SortOrder { get; }
    public AssemblyInfoInShitRuntime(int sortOrder = 0)
    {
        this.SortOrder = sortOrder;
    }
}
