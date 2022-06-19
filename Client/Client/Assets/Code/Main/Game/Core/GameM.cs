using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class GameM
{
    public static SettingM Setting { get; private set; }
    public static EventSystem Event { get; private set; }
    public static NetSystem Net { get; private set; }
    public static World World { get; private set; }

    public static void Init()
    {
        Setting = new SettingM();
        Event = new EventSystem();
        Event.RigisteAllStaticListener();
        Net = new NetSystem();
        World = new World();
    }
    public static void Close()
    {
        Net.Dispose();
        Event.Clear();
        World.Dispose();
        World = null;
    }
}
