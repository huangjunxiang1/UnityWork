using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
[Conditional("UNITY_EDITOR")]
public sealed class ShowInInspector : Attribute
{
}
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
[Conditional("UNITY_EDITOR")]
public sealed class HideInInspector : Attribute
{
}
