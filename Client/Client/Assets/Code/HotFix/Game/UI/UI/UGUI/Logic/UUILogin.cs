using Core;
using Event;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

partial class UUILogin
{
    //[Event]
    static async void EC_InScene(EC_InScene e)
    {
        if (e.sceneId == 1)
            await Client.UI.OpenAsync<UUILogin>();
    }

    protected override void OnEnter(params object[] data)
    {
        _loginButton.onClick.AddListener(login);

        _GameTypeDropdown.ClearOptions();
        _GameTypeDropdown.AddOptions(new List<string>
        {
            "单机模式",
            "联网模式",
        });

        //_sceneIDText.Binding<EC_InScene>(t => t.sceneId.ToString());
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
            await Client.UI.OpenAsync<FUILogin>();
            this.Dispose();
        }
    }
    int v = 5;
    void login()
    {
        World.Event.RunEvent(new EC_InScene() { sceneId = ++v });
        //ugui 只做个展示  实际使用fgui
    }
}