using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;

class UIConfig
{
    private UIConfig(int sortOrder, bool closeOnChangeScene = true)
    {
        this.SortOrder = sortOrder;
        this.CloseOnChangeScene = closeOnChangeScene;
    }

    public int SortOrder { get; }
    public bool CloseOnChangeScene { get; }

    public static UIConfig Default { get; } = new(0);


    public static Dictionary<Type, UIConfig> UIConfigMap = new()
    {
        { typeof(UUILoading),  new(50) },

        { typeof(FUIFighting), new(20) },
        { typeof(FUILoading),  new(50) },
        { typeof(FUIGlobal),   new(100, false) },
    };
}
