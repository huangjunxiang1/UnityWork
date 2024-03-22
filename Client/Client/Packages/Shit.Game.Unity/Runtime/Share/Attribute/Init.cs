using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[AttributeUsage(AttributeTargets.Method)]
public class Init : SAttribute
{
    public int SortOrder { get; }
    public Init(int sortOrder = 0)
    {
        SortOrder = sortOrder;
    }
}
