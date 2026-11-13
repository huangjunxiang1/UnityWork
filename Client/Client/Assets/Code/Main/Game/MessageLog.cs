using Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class MessageLog
{
    [Event]
    static void EC_SendMesssage(EC_SendMesssage e)
    {
        PrintField.Print($"<Color=#FF0000>发送消息</Color> msg:[{e.message.GetType().Name}]  content:\n{{0}}", e.message);
    }
    [Event]
    static void EC_AcceptedMessage(EC_AcceptedMessage e)
    {
        PrintField.Print($"<Color=#00FF00>收到消息</Color> msg:[{e.message.GetType().Name}]  content:\n{{0}}", e.message);
    }
}
