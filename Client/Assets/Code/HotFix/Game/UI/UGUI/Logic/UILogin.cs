using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

partial class UILogin : UUIBase
{
    public override void Enter(object data)
    {
        _loginButton.onClick.AddListener(login);
    }

    public override void Exit()
    {

    }

    [Msg(OuterOpcode.R2C_Login)]
    void R2C_Login(IMessage message)
    {
        R2C_Login rep = message as R2C_Login;
        if (rep.Error == 0)
        {
            this.Dispose();
            UIManager.Inst.Open<UILoding>(1);
        }
    }

    void login()
    {
        var msg = new C2R_Login()
        {
            Account = _acInputField.text,
            Password = _pwInputField.text
        };
        SysNet.Send(msg);
    }
}