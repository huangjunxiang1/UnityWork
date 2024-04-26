using Core;

public interface IData { }
namespace Event
{
    public class EC_NetError : IData { public int code; }
    public class EC_Disconnect : IData { public long rpc; }
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
