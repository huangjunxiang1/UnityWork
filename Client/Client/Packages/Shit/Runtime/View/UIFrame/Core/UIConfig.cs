using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UIConfig : SAttribute
    {
        public const int SortOrderRange = 100;
        public static UIConfig Default { get; } = new UIConfig(0);

        public int SortOrder { get; }
        public bool HideIfOpenOtherUI { get; set; } = true;//打开新UI 自动隐藏当前UI


        public UIConfig(int SortOrder)
        {
            if (SortOrder > SortOrderRange || SortOrder < -SortOrderRange)
                Loger.Error($"SortOrder must be range -{SortOrderRange} to {SortOrderRange}");

            this.SortOrder = SortOrder;
        }
    }
}
