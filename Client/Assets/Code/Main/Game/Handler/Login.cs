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
        [Msg(OuterOpcode.R2C_Login)]
        static void R2C_Login(IMessage message)
        {
            R2C_Login rep = message as R2C_Login;
            SysNet.DisConnect();
            SysNet.Connect(NetType.KCP, MUtil.ToIPEndPoint(rep.Address));
            var C2G_LoginGate = new C2G_LoginGate() { Key = rep.Key, GateId = rep.GateId };
            SysNet.Send(C2G_LoginGate);
        }
        [Msg(OuterOpcode.G2C_LoginGate)]
        static async void login(IMessage message)
        {
            G2C_LoginGate G2C_LoginGate = message as G2C_LoginGate;
            SysNet.Send(new C2G_EnterMap());
            while (true)
            {
                await Task.Delay(1000);
                _ = SysNet.SendAsync(new C2G_Ping());
            }
            static void test()
            {
                Loger.Error(0);
            }
        }
    }
}
