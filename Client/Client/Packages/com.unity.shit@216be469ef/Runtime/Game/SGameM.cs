using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class SGameM
{
    public static EventSystem Event { get; private set; }
    public static NetSystem Net { get; private set; }
    public static SWorld World { get; private set; }
    public static SServerData ServerData { get; private set; }

    public static void Init()
    {
        Event = new EventSystem();
        Event.RigisteAllStaticListener();
        Net = new NetSystem();
        World = new SWorld();
        ServerData = new SServerData();
    }
    public static void Close()
    {
        Net.Dispose();
    }
}
