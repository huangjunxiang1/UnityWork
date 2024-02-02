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

        _GameTypeDropdown.ClearOptions();
        _GameTypeDropdown.AddOptions(new List<string>
        {
            "单机模式",
            "联网模式",
        });

        _UITypeDropdown.onValueChanged.AddListener(onValue);
        _UITypeDropdown.ClearOptions();
        _UITypeDropdown.AddOptions(new List<string>
        {
            "FGUI",
            "UGUI",
        });
        _UITypeDropdown.value = 1;
    }

    protected override void OnExit()
    {

    }

    [Event]
    void connectRet(EC_NetError e)
    {
        int error = e.code;

    }

    async void onValue(int v)
    {
        if (v == 0)
        {
            Setting.UIModel = UIModel.FGUI;
            await GameL.UI.OpenAsync<FUILogin>();
            this.Dispose();
        }
    }
    void login()
    {
        //ugui 只做个展示  实际使用fgui
    }
}