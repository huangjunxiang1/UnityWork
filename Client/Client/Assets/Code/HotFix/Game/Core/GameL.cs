using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    static class GameL
    {
        static GameL()
        {
            Setting = new SettingL();
            Scene = new Scene();
            UI = new UIManager();
        }

        public static SettingL Setting { get; private set; }
        public static Scene Scene { get; private set; }
        public static Data Data { get; private set; }
        public static UIManager UI { get; private set; }

        public static void Init()
        {
            //数据丢弃原来的 重新new一个
            Data?.Dispose();
            Data = new Data();
        }
        public static void Close()
        {
            Data?.Dispose();
            Data = null;
        }
    }
}
