using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main;
using UnityEngine;

namespace Game
{
    static class Map
    {
        [Msg(OuterOpcode.G2C_EnterMap)]
        static void EnterMap(IMessage message)
        {
            G2C_EnterMap rep = message as G2C_EnterMap;
            Main.SceneHelper.LoadScene("Main");

            SysEvent.ExcuteEvent((int)EIDL.InScene, 10001);
        }

        [Event((int)EIDM.QuitGame)]
        static void QuitGame()
        {
            SysEvent.ExcuteEvent((int)EIDL.OutScene);
        }
    }
}
