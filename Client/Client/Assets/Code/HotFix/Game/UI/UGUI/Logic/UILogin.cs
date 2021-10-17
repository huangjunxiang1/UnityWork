using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

partial class UILogin : UUIBase
{
    protected override void OnEnter(params object[] data)
    {
        _loginButton.onClick.AddListener(login);
        _DropdownDropdown.ClearOptions();
        _DropdownDropdown.AddOptions(new List<string>
        {
            "联网模式",
            "单机模式"
        });
    }

    protected override void OnExit()
    {

    }

    [Msg(CMDL.R2C_Login)]
    void R2C_Login(IMessage message)
    {
        R2C_Login rep = message as R2C_Login;
        if (rep.Error == 0)
        {
            this.Dispose();
            UIManager.Inst.Open<UILoding>(1);
        }
    }
    [Event((int)EventIDM.NetError)]
    void connectRet(EventerContent e)
    {
        int error = e.ValueInt;

    }

    void login()
    {
        if (_DropdownDropdown.value == 0)
        {
            SysNet.Connect(NetType.KCP, Util.ToIPEndPoint(AppSetting.LoginAddress));
            var msg = new C2R_Login()
            {
                Account = _acInputField.text,
                Password = _pwInputField.text
            };
            SysNet.Send(msg);
        }
        else if (_DropdownDropdown.value == 1)
        {
            this.Dispose();
            UIManager.Inst.Open<UILoding>(1).OnDispose.Add(() =>
            {
                Main.SceneHelper.LoadScene("Main");
                SysEvent.ExcuteEvent((int)EventIDL.InScene, 10001);
            });
        }
    }
}