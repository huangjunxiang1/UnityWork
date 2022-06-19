using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SystemExecuteOrderAttribute : Attribute
    {
        public int SortOrder { get; set; }

        public SystemExecuteOrderAttribute(int sortOrder)
        {
            this.SortOrder = sortOrder;
        }
    }
}
