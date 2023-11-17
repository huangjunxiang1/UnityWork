using FairyGUI;
using FairyGUI.Utils;
partial class G_Box_YesOrNo
{
    public GComponent ui { get; }
    public GButton _yes { get; }
    public GButton _no { get; }
    public GTextField _title { get; }
    public GTextField _text { get; }
    public G_Box_YesOrNo(GComponent ui)
    {
        this.ui = ui;
        _yes = (GButton)ui.GetChildAt(1);
        _no = (GButton)ui.GetChildAt(2);
        _title = (GTextField)ui.GetChildAt(3);
        _text = (GTextField)ui.GetChildAt(4);
    }
}
partial class G_test
{
    public GComponent ui { get; }
    public GTextField _n0 { get; }
    public Controller _c2 { get; }
    public Transition _t0 { get; }
    public G_test(GComponent ui)
    {
        this.ui = ui;
        _n0 = (GTextField)ui.GetChildAt(0);
        _c2 = ui.GetControllerAt(1);
        _t0 = ui.GetTransitionAt(0);
    }
}
partial class G_Tips
{
    public GComponent ui { get; }
    public G_Tips(GComponent ui)
    {
        this.ui = ui;
    }
}
partial class FUIFighting : FUI
{
    public sealed override string url => "FUIFighting";
    public GButton _btnBack;
    public GButton _play;
    public G_test _n5;
    public Controller _c2;
    public Transition _t1;
    protected sealed override void Binding()
    {
        GComponent ui = this.UI;
        _btnBack = (GButton)ui.GetChildAt(0);
        _play = (GButton)ui.GetChildAt(1);
        _n5 = new G_test(ui.GetChildAt(2).asCom);
        _c2 = ui.GetControllerAt(1);
        _t1 = ui.GetTransitionAt(1);
    }
}
partial class FUIFighting2 : FUI
{
    public sealed override string url => "FUIFighting2";
    public GButton _btnBack;
    public GButton _play;
    protected sealed override void Binding()
    {
        GComponent ui = this.UI;
        _btnBack = (GButton)ui.GetChildAt(0);
        _play = (GButton)ui.GetChildAt(1);
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
        GComponent ui = this.UI;
        _btnBack = (GButton)ui.GetChildAt(0);
        _rangeRoad = (GButton)ui.GetChildAt(1);
        _play = (GButton)ui.GetChildAt(2);
        _findStyle = (GComboBox)ui.GetChildAt(3);
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
        GComponent ui = this.UI;
        _btnBack = (GButton)ui.GetChildAt(0);
        _rangeRoad = (GButton)ui.GetChildAt(1);
        _play = (GButton)ui.GetChildAt(2);
        _showCube = (GButton)ui.GetChildAt(3);
    }
}
partial class FUIGlobal : FUI
{
    public sealed override string url => "FUIGlobal";
    public GButton _log;
    protected sealed override void Binding()
    {
        GComponent ui = this.UI;
        _log = (GButton)ui.GetChildAt(0);
    }
}
partial class FUILoading : FUI
{
    public sealed override string url => "FUILoading";
    public GProgressBar _loadingBar;
    protected sealed override void Binding()
    {
        GComponent ui = this.UI;
        _loadingBar = (GProgressBar)ui.GetChildAt(1);
    }
}
partial class FUILogin : FUI
{
    public sealed override string url => "FUILogin";
    public GLoader _bg;
    public GComboBox _uiType;
    public GComboBox _gameTypeCB;
    public GButton _btnLogin;
    public GTextInput _acc;
    public GTextInput _pw;
    public GComboBox _demo;
    protected sealed override void Binding()
    {
        GComponent ui = this.UI;
        _bg = (GLoader)ui.GetChildAt(0);
        _uiType = (GComboBox)ui.GetChildAt(3);
        _gameTypeCB = (GComboBox)ui.GetChildAt(4);
        _btnLogin = (GButton)ui.GetChildAt(5);
        _acc = (GTextInput)ui.GetChildAt(6);
        _pw = (GTextInput)ui.GetChildAt(7);
        _demo = (GComboBox)ui.GetChildAt(8);
    }
}
partial class FUI3DHeader : FUI3D
{
    public sealed override string url => "Assets/Res/UI/FUI/3DUI/FUI3DHeader.prefab";
    public GTextField _title;
    protected sealed override void Binding()
    {
        GComponent ui = this.UI;
        _title = (GTextField)ui.GetChildAt(0);
    }
}
