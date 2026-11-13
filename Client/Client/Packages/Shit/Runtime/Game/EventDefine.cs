
namespace Event
{
    public class EC_NetError { public int code; }
    public class EC_Disconnect { }
    public class EC_ClientLanucher { }
    public class EC_ServerLanucher { }
    public class EC_SendMesssage
    {
        public Message message;
    }
    public class EC_AcceptedMessage
    {
        public Message message;
    }
}