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
            GameL.UI.Open<UUILoading>(1);
        }
    }
    [Event((int)EventIDM.NetError)]
    void connectRet(EventerContent e)
    {
        int error = e.Value;

    }

    void onValue(int v)
    {
        if (v == 0)
        {
            GameL.Setting.UIModel = UIModel.FGUI;
            GameL.UI.Open<FUILogin>();
            this.Dispose();
        }
    }
    void login()
    {
        //ugui 只做个展示  实际使用fgui
    }
}