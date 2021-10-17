using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;

class UIConfig
{
    public UIConfig(string resPath, int sortOrder)
    {
        this.ResPath = resPath;
        this.SortOrder = sortOrder;
    }

    public string ResPath { get; }
    public int SortOrder { get; }


    public static Dictionary<Type, UIConfig> UIConfigMap = new Dictionary<Type, UIConfig>()
    {
        { typeof(UILoding),         new UIConfig(null, 50)},
        { typeof(UILogin),          new UIConfig(null, 0)},
    };
}
