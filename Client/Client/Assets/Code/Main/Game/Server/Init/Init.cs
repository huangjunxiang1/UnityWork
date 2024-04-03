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
            Server.World.Root.AddChild(new Login() { ip = new IPEndPoint(IPAddress.Any, SettingM.serverPort) });
            Server.World.Root.AddChild(new Room() {});
        }
    }
}
