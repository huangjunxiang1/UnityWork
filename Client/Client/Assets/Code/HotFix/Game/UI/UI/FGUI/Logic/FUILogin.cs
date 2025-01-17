using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using FairyGUI;
using System.Threading.Tasks;
using System;
using Event;
using main;

partial class FUILogin
{
    [Event]
    static async void EC_InScene(EC_InScene e)
    {
        if (e.sceneId == 1)
            await Client.UI.OpenAsync<FUILogin>();
    }

    protected override void OnEnter(params object[] data)
    {
        _acc.text = SettingL.Account;
        _acc.GetTextField().asTextInput.onChanged.Add(acc);
        _pw.text = SettingL.Password;
        _pw.GetTextField().asTextInput.onChanged.Add(pw);
        _btnLogin.onClick.Add(login);
        _btnEnter.onClick.Add(enter);
        _asServer.onClick.Add(asServer);

        _asServer.enabled = Server.World == null;
        if (Server.World != null)
            _serverIP.text = Server.World.Root.GetChild<Login>().ip.ToString();
    }

    void acc() => SettingL.Account = _acc.text;
    void pw() => SettingL.Password = _pw.text;

    async void login()
    {
        if (string.IsNullOrEmpty(this._serverIP.text)) return;
        if (NetComponent.Inst != null && !NetComponent.Inst.Disposed)
            NetComponent.Inst.Dispose();
        Client.World.Root.AddComponent(new NetComponent(true));
        NetComponent.Inst.SetSession(new STCP(Util.ToIPEndPoint(this._serverIP.text)));
        await NetComponent.Inst.Session.Connect();
        C2S_Login c = new();
        c.acc = SettingL.Account;
        c.pw = SettingL.Password;
        var s = (S2C_Login)await NetComponent.Inst.SendAsync(c);
        if (!string.IsNullOrEmpty(s.error))
        {
            Box.Tips(s.error);
            return;
        }
        Box.Tips("登录成功");
        this._c1.selectedIndex = 1;
    }
    async void enter()
    {
        await Client.UI.OpenAsync<FUIRooms>();
    }
    async void asServer()
    {
        if (Server.World != null) return;
        await Server.Load();
        _asServer.enabled = Server.World == null;
    }
}
