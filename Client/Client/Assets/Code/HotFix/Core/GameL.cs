using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;

static class GameL
{
    public static SettingL Setting { get; private set; }
    public static Scene Scene { get; private set; }
    public static Data Data { get; private set; }
    public static UIManager UI { get; private set; }
    public static WorldL World { get; private set; }

    public static void Init()
    {
        Setting = new SettingL();
        Scene = new Scene();
        Data = new Data();
        UI = new UIManager();
        World = new WorldL(GameM.World);
    }
    public static void Close()
    {
        
    }
}
