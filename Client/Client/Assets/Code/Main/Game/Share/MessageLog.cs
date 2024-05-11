using Event;
using game;
using Game;
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
        if (e.message is not C2S_Ping && e.message is not S2C_Ping)
        {
#if Server
            PrintField.Print($"发送消息 msg:[{e.message.GetType().Name}]  content:{{0}}", e.message);
#else
            PrintField.Print($"<Color=#FF0000>发送消息</Color> msg:[{e.message.GetType().Name}]  content:{{0}}", e.message);
#endif
        }
    }
    [Event]
    static void EC_AcceptedMessage(EC_AcceptedMessage e)
    {
        if (e.message is not C2S_Ping && e.message is not S2C_Ping)
        {
#if Server
            PrintField.Print($"收到消息 msg:[{e.message.GetType().Name}]  content:{{0}}", e.message);
#else
            PrintField.Print($"<Color=#00FF00>收到消息</Color> msg:[{e.message.GetType().Name}]  content:{{0}}", e.message);
#endif
        }
    }
}
