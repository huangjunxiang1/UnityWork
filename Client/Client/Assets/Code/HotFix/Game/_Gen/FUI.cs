/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

partial class FUIFighting2 : FUI
{
    public sealed override string url => "FUIFighting2";
    public GButton _btnBack;
    public GButton _play;

    protected sealed override void Binding()
    {
        _btnBack = (GButton)this.UI.GetChildAt(0);
        _play = (GButton)this.UI.GetChildAt(1);
    }
}

partial class FUIFighting : FUI
{
    public sealed override string url => "FUIFighting";
    public GButton _btnBack;
    public GButton _play;

    protected sealed override void Binding()
    {
        _btnBack = (GButton)this.UI.GetChildAt(0);
        _play = (GButton)this.UI.GetChildAt(1);
    }
}

partial class FUILoading : FUI
{
    public sealed override string url => "FUILoading";
    public GProgressBar _loadingBar;

    protected sealed override void Binding()
    {
        _loadingBar = (GProgressBar)this.UI.GetChildAt(1);
    }
}

partial class FUIFighting3 : FUI
{
    public sealed override string url => "FUIFighting3";
    public GButton _btnBack;
    public GButton _rangeRoad;
    public GButton _play;
    public GComboBox _findStyle;

    protected sealed override void Binding()
    {
        _btnBack = (GButton)this.UI.GetChildAt(0);
        _rangeRoad = (GButton)this.UI.GetChildAt(1);
        _play = (GButton)this.UI.GetChildAt(2);
        _findStyle = (GComboBox)this.UI.GetChildAt(3);
    }
}

partial class FUILogin : FUI
{
    public sealed override string url => "FUILogin";
    public GComboBox _uiType;
    public GComboBox _gameTypeCB;
    public GButton _btnLogin;
    public GTextInput _acc;
    public GTextInput _pw;
    public GComboBox _demo;

    protected sealed override void Binding()
    {
        _uiType = (GComboBox)this.UI.GetChildAt(3);
        _gameTypeCB = (GComboBox)this.UI.GetChildAt(4);
        _btnLogin = (GButton)this.UI.GetChildAt(5);
        _acc = (GTextInput)this.UI.GetChildAt(6);
        _pw = (GTextInput)this.UI.GetChildAt(7);
        _demo = (GComboBox)this.UI.GetChildAt(8);
    }
}

partial class FUI3DHeader : FUI3D
{
    public GTextField _title;

    protected sealed override void Binding()
    {
        _title = (GTextField)this.UI.GetChildAt(0);
    }
}

partial class FUIGlobal : FUI
{
    public sealed override string url => "FUIGlobal";
    public GButton _log;

    protected sealed override void Binding()
    {
        _log = (GButton)this.UI.GetChildAt(0);
    }
}

partial class FUIFighting4 : FUI
{
    public sealed override string url => "FUIFighting4";
    public GButton _btnBack;
    public GButton _rangeRoad;
    public GButton _play;
    public GButton _showCube;

    protected sealed override void Binding()
    {
        _btnBack = (GButton)this.UI.GetChildAt(0);
        _rangeRoad = (GButton)this.UI.GetChildAt(1);
        _play = (GButton)this.UI.GetChildAt(2);
        _showCube = (GButton)this.UI.GetChildAt(3);
    }
}