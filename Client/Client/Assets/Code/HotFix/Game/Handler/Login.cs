using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    static class Login
    {
        [Msg(CMDL.R2C_Login)]
        static void R2C_Login(IMessage message)
        {
            R2C_Login rep = message as R2C_Login;
            GameM.Net.DisConnect();
            GameM.Net.Connect(NetType.TCP, Util.ToIPEndPoint(rep.Address));
            var C2G_LoginGate = new C2G_LoginGate() { Key = rep.Key, GateId = rep.GateId };
            GameM.Net.Send(C2G_LoginGate);
        }
        [Msg(CMDL.G2C_LoginGate)]
        static void login(IMessage message)
        {
            G2C_LoginGate G2C_LoginGate = message as G2C_LoginGate;
            GameM.Net.Send(new C2G_EnterMap());
            Timer.Add(10, -1, pingTimer);
        }
        [Event((int)EventIDM.QuitGame)]
        static void Quit()
        {
            Timer.Remove(pingTimer);
        }
        static void pingTimer()
        {
            GameM.Net.Send(new C2G_Ping());
        }
    }
}
