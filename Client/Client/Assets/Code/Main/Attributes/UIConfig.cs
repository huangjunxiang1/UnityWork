using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class UIConfig : Attribute
    {
        public static UIConfig Default { get; } = new UIConfig(0);

        public int SortOrder { get; }
        public bool CloseOnChangeScene { get; set; } = true;


        public UIConfig(int SortOrder)
        {
            this.SortOrder = SortOrder;
        }
    }
}
