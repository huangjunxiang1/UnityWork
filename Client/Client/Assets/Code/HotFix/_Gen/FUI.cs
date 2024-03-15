using FairyGUI;
using FairyGUI.Utils;
partial class G_Box_YesOrNo
{
    public GComponent ui { get; }
    public GButton _yes { get; private set; }
    public GButton _no { get; private set; }
    public GTextField _title { get; private set; }
    public GTextField _text { get; private set; }

    public G_Box_YesOrNo(GComponent ui)
    {
        this.ui = ui;
        _yes = (GButton)ui.GetChildAt(1);
        _no = (GButton)ui.GetChildAt(2);
        _title = (GTextField)ui.GetChildAt(3);
        _text = (GTextField)ui.GetChildAt(4);
        this.Enter();
    }
    partial void Enter();
    public G_Box_YesOrNo() : this((GComponent)UIPkg.ComPkg.CreateObject("Box_YesOrNo")) { }
    public void Dispose()
    {
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
    public override void Dispose()
    {
        base.Dispose();
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
    public override void Dispose()
    {
        base.Dispose();
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
    public override void Dispose()
    {
        base.Dispose();
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
    public override void Dispose()
    {
        base.Dispose();
    }
}
partial class FUIFighting5 : FUI
{
    public sealed override string url => "FUIFighting5";
    public GButton _btnBack { get; private set; }
    public GComboBox _demo { get; private set; }
    public GButton _reversion { get; private set; }

    protected sealed override void Binding()
    {
        GComponent ui = this.UI;
        _btnBack = (GButton)ui.GetChildAt(0);
        _demo = (GComboBox)ui.GetChildAt(1);
        _reversion = (GButton)ui.GetChildAt(2);
    }
    public override void Dispose()
    {
        base.Dispose();
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
    public override void Dispose()
    {
        base.Dispose();
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
    public override void Dispose()
    {
        base.Dispose();
    }
}
partial class G_LogReporter
{
    public GButton ui { get; }
    public GTextFieldPropertyBinding _yy { get; private set; }

    public G_LogReporter(GButton ui)
    {
        this.ui = ui;
        _yy = new((GTextField)ui.GetChildAt(4));
        this.Enter();
    }
    partial void Enter();
    public G_LogReporter() : this((GButton)UIPkg.ComPkg.CreateObject("LogReporter")) { }
    public void Dispose()
    {
        _yy.Dispose();
    }
}
partial class G_test
{
    public GComponent ui { get; }
    public GTextFieldPropertyBinding _n0 { get; private set; }
    public GLoader _a { get; private set; }
    public Controller _c2 { get; private set; }
    public Transition _t0 { get; private set; }

    public G_test(GComponent ui)
    {
        this.ui = ui;
        _n0 = new((GTextField)ui.GetChildAt(0));
        _a = (GLoader)ui.GetChildAt(2);
        _c2 = ui.GetControllerAt(1);
        _t0 = ui.GetTransitionAt(0);
        this.Enter();
    }
    partial void Enter();
    public G_test() : this((GComponent)UIPkg.ComPkg.CreateObject("test")) { }
    public void Dispose()
    {
        _n0.Dispose();
    }
}
partial class G_Tips
{
    public GComponent ui { get; }

    public G_Tips(GComponent ui)
    {
        this.ui = ui;
        this.Enter();
    }
    partial void Enter();
    public G_Tips() : this((GComponent)UIPkg.ComPkg.CreateObject("Tips")) { }
    public void Dispose()
    {
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
    public override void Dispose()
    {
        base.Dispose();
    }
}
