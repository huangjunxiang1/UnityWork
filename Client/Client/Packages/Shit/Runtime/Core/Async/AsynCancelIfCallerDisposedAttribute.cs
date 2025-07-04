using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Method)]
public class AsynCancelIfCallerDisposedAttribute : SAttribute { }
public interface ICriticalNotifyCompletionV2 : ICriticalNotifyCompletion
{
    internal TaskItem Current { get; }
}
public interface INotifyCompletionV2 : ICriticalNotifyCompletion
{
    internal TaskItem Current { get; }
}
