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
    public static WorldM World { get; private set; }

    public static void Init()
    {
        Setting = new SettingM();
        Event = new EventSystem();
        Event.RigisteAllStaticListener();
        Net = new NetSystem();
        World = new WorldM();
    }
    public static void Close()
    {
        Net.Dispose();
    }
}
