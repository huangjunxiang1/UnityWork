using System;

namespace Event
{
    public class EC_NetError { public int code; }
    public class EC_Disconnect { public long rpc; }
    public class EC_ClientLanucher { }
    public class EC_ServerLanucher { }
    public class EC_SendMesssage
    {
        public Game.IMessage message;
    }
    public class EC_AcceptedMessage
    {
        public Game.IMessage message;
    }
}
