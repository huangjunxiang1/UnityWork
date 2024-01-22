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
    public static G_Box_YesOrNo Create()
    {
        return new G_Box_YesOrNo(UIPackage.CreateObject("ComPkg", "Box_YesOrNo").asCom);
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
    public static G_test Create()
    {
        return new G_test(UIPackage.CreateObject("ComPkg", "test").asCom);
    }
}
partial class G_Tips
{
    public GComponent ui { get; }
    public G_Tips(GComponent ui)
    {
        this.ui = ui;
    }
    public static G_Tips Create()
    {
        return new G_Tips(UIPackage.CreateObject("ComPkg", "Tips").asCom);
    }
}
partial class FUIFighting : FUI
{
    public sealed override string url => "FUIFighting";
    public GButton _btnBack { get; private set; }
    public GButton _play { get; private set; }
    public Controller _c2 { get; private set; }
    public Transition _t1 { get; private set; }
    protected sealed override void Binding()
    {
        GComponent ui = this.UI;
        _btnBack = (GButton)ui.GetChildAt(0);
        _play = (GButton)ui.GetChildAt(1);
        _c2 = ui.GetControllerAt(1);
        _t1 = ui.GetTransitionAt(1);
    }
}
partial class FUIFighting2 : FUI
{
    public sealed override string url => "FUIFighting2";
    public GButton _btnBack { get; private set; }
    public GButton _play { get; private set; }
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
    public GButton _btnBack { get; private set; }
    public GButton _rangeRoad { get; private set; }
    public GButton _play { get; private set; }
    public GComboBox _findStyle { get; private set; }
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
    public GButton _btnBack { get; private set; }
    public GButton _rangeRoad { get; private set; }
    public GButton _play { get; private set; }
    public GButton _showCube { get; private set; }
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
    public GButton _log { get; private set; }
    protected sealed override void Binding()
    {
        GComponent ui = this.UI;
        _log = (GButton)ui.GetChildAt(0);
    }
}
partial class FUILoading : FUI
{
    public sealed override string url => "FUILoading";
    public GProgressBar _loadingBar { get; private set; }
    protected sealed override void Binding()
    {
        GComponent ui = this.UI;
        _loadingBar = (GProgressBar)ui.GetChildAt(1);
    }
}
partial class FUILogin : FUI
{
    public sealed override string url => "FUILogin";
    public GLoader _bg { get; private set; }
    public GComboBox _uiType { get; private set; }
    public GComboBox _gameTypeCB { get; private set; }
    public GButton _btnLogin { get; private set; }
    public GTextInput _acc { get; private set; }
    public GTextInput _pw { get; private set; }
    public GComboBox _demo { get; private set; }
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
    public GTextField _title { get; private set; }
    protected sealed override void Binding()
    {
        GComponent ui = this.UI;
        _title = (GTextField)ui.GetChildAt(0);
    }
}
