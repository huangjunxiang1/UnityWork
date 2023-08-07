using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class AsyncBaseBuilder
{
    public TaskAwaiter Awaiter { get; protected set; }
    public IAsyncDisposed Target { get; protected set; }
}
