using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    static class Ping
    {
        [Msg(OuterOpcode.G2C_Ping)]
        static void ping(IMessage message)
        {
            G2C_Ping G2C_Ping = message as G2C_Ping;
            Timer.ServerTime = G2C_Ping.Time;
        }
    }
}
