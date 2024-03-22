using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class NetComponent : SComponent
    {
        public SBaseNet Session { get; set; }

        public void Send(IMessage message)
        {
            Session.Send(message);
        }
        public STask<IMessage> SendAsync(IMessage message)
        {
            return Session.SendAsync(message);
        }
    }
}
