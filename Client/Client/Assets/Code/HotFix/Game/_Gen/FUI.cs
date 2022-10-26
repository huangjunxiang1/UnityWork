/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

partial class FUIFighting2 : FUI
{
    public override string url => "FUIFighting2";
    public GButton _btnBack;
    public GButton _play;

    protected override void Binding()
    {
        _btnBack = (GButton)this.UI.GetChild("_btnBack");
        _play = (GButton)this.UI.GetChild("_play");
    }
}

partial class FUIFighting : FUI
{
    public override string url => "FUIFighting";
    public GButton _btnBack;
    public GButton _play;

    protected override void Binding()
    {
        _btnBack = (GButton)this.UI.GetChild("_btnBack");
        _play = (GButton)this.UI.GetChild("_play");
    }
}

partial class FUILoading : FUI
{
    public override string url => "FUILoading";
    public GProgressBar _loadingBar;

    protected override void Binding()
    {
        _loadingBar = (GProgressBar)this.UI.GetChild("_loadingBar");
    }
}

partial class FUIFighting3 : FUI
{
    public override string url => "FUIFighting3";
    public GButton _btnBack;
    public GButton _rangeRoad;
    public GButton _play;

    protected override void Binding()
    {
        _btnBack = (GButton)this.UI.GetChild("_btnBack");
        _rangeRoad = (GButton)this.UI.GetChild("_rangeRoad");
        _play = (GButton)this.UI.GetChild("_play");
    }
}

partial class FUILogin : FUI
{
    public override string url => "FUILogin";
    public GComboBox _uiType;
    public GComboBox _gameTypeCB;
    public GButton _btnLogin;
    public GTextInput _acc;
    public GTextInput _pw;
    public GComboBox _demo;

    protected override void Binding()
    {
        _uiType = (GComboBox)this.UI.GetChild("_uiType");
        _gameTypeCB = (GComboBox)this.UI.GetChild("_gameTypeCB");
        _btnLogin = (GButton)this.UI.GetChild("_btnLogin");
        _acc = (GTextInput)this.UI.GetChild("_acc");
        _pw = (GTextInput)this.UI.GetChild("_pw");
        _demo = (GComboBox)this.UI.GetChild("_demo");
    }
}

partial class FUI3DHeader : FUI3D
{
    public GTextField _title;

    protected override void Binding()
    {
        _title = (GTextField)this.UI.GetChild("_title");
    }
}

partial class FUIGlobal : FUI
{
    public override string url => "FUIGlobal";
    public GButton _log;

    protected override void Binding()
    {
        _log = (GButton)this.UI.GetChild("_log");
    }
}