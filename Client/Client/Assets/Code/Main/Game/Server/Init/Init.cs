using Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class ServerInit
    {
        [Event]
        static void Init(EC_ServerLanucher e)
        {
            //var players = new Players { ip = new IPEndPoint(IPAddress.Any, SettingM.serverPort) };
            var rooms = new Room() { ip = new IPEndPoint(IPAddress.Any, SettingM.serverPort) };
            //Server.World.Root.AddChild(players);
            Server.World.Root.AddChild(rooms);
        }
    }
}
