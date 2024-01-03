using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;

static class GameL
{
    public static Scene Scene { get; private set; }
    public static Data Data { get; private set; }
    public static UIManager UI { get; private set; }

    public static void Init()
    {
        Scene = new Scene();
        Data = new Data();
        UI = new UIManager();
    }
    public static void Close()
    {
        
    }
}
