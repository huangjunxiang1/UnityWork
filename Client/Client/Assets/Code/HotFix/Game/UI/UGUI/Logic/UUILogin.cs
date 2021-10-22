using Game;
using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

partial class UUILogin
{
    protected override void OnEnter(params object[] data)
    {
        _loginButton.onClick.AddListener(login);
        _DropdownGameTypeDropdown.ClearOptions();
        _DropdownGameTypeDropdown.AddOptions(new List<string>
        {
            "单机模式",
            "联网模式",
        });

        _DropdownUITypeDropdown.onValueChanged.AddListener(onValue);
        _DropdownUITypeDropdown.ClearOptions();
        _DropdownUITypeDropdown.AddOptions(new List<string>
        {
            "FGUI",
            "UGUI",
        });
        _DropdownUITypeDropdown.value = 1;
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
            UIS.Open<UUILoading>(1);
        }
    }
    [Event((int)EIDM.NetError)]
    void connectRet(EventerContent e)
    {
        int error = e.ValueInt;

    }

    void onValue(int v)
    {
        if (v == 0)
        {
            //这里直接立即调用  this.Dispose(); ugui会报错
            Timer.Add(0.01f, 1, () =>
              {
                  GameSetting.UIModel = UIModel.FGUI;
                  UIS.Open<FUILogin>();
                  this.Dispose();
              });
        }
        else
            GameSetting.UIModel = UIModel.UGUI;
    }
    void login()
    {
        if (_DropdownGameTypeDropdown.value == 0)
        {
            this.Dispose();
            UIS.Open<UUILoading>(1).OnDispose.Add(() =>
            {
                Main.SceneHelper.LoadScene("Main");
                SysEvent.ExcuteEvent((int)EIDL.InScene, 10001);
            });
        }
        else if (_DropdownGameTypeDropdown.value == 1)
        {
            SysNet.Connect(NetType.KCP, Util.ToIPEndPoint(AppSetting.LoginAddress));
            var msg = new C2R_Login()
            {
                Account = _acInputField.text,
                Password = _pwInputField.text
            };
            SysNet.Send(msg);
        }
    }
}