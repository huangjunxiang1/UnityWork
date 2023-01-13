using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public enum UIType
    {
        SceneUI,//场景UI 切场景关闭
        GlobalUI,//全局UI  只能主动关闭
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class UIConfig : Attribute
    {
        public static UIConfig Default { get; } = new UIConfig(0);

        public int SortOrder { get; }
        public UIType UIType { get; set; } = UIType.SceneUI;
        public bool HideOnOpenOtherUI { get; set; } = true;//打开新UI 自动隐藏当前UI


        public UIConfig(int SortOrder)
        {
            if (SortOrder > 100 || SortOrder < -100)
                Loger.Error("SortOrder不能超过-100到100");
            
            this.SortOrder = SortOrder;
        }
    }
}
