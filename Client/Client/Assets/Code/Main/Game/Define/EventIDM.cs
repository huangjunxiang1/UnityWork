using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public enum EventIDM
    {
        Min = 0,

        NetError,//网络链接出错
        QuitGame,//退出游戏

        InScene,//进入场景
        OutScene,//退出场景

        Max = 10000,//最大值
    }
}
