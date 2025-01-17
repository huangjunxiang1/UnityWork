using FairyGUI;
using FairyGUI.Utils;
class FUIBinder
{
    public static void Binding()
    {
        UIObjectFactory.SetPackageItemExtension("ui://zvziziwirjgbf", typeof(G_Box_YesOrNo));
        UIObjectFactory.SetPackageItemExtension("ui://zvziziwik8hcu", typeof(G_Connecting));
        UIObjectFactory.SetPackageItemExtension("ui://zvziziwioxwqn", typeof(G_test));
        UIObjectFactory.SetPackageItemExtension("ui://zvziziwiqnthd", typeof(G_Tips));
    }
}
partial class G_Box_YesOrNo : GComponent
{
    public GButton _yes { get; private set; }
    public GButton _no { get; private set; }
    public GTextField _title { get; private set; }
    public GTextField _text { get; private set; }

    public override void ConstructFromXML(XML xml)
    {
        _yes = (GButton)this.GetChildAt(1);
        _no = (GButton)this.GetChildAt(2);
        _title = (GTextField)this.GetChildAt(3);
        _text = (GTextField)this.GetChildAt(4);
        this.Enter();
    }
    partial void Enter();
    public static G_Box_YesOrNo Create() => (G_Box_YesOrNo)UIPackage.CreateObject("ComPkg", "Box_YesOrNo");
    public override void Dispose()
    {
        base.Dispose();
    }
}
partial class G_Connecting : GLabel
{

    public override void ConstructFromXML(XML xml)
    {
        this.Enter();
    }
    partial void Enter();
    public static G_Connecting Create() => (G_Connecting)UIPackage.CreateObject("ComPkg", "Connecting");
    public override void Dispose()
    {
        base.Dispose();
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
        GComponent ui = this.ui;
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
        GComponent ui = this.ui;
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
        GComponent ui = this.ui;
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
        GComponent ui = this.ui;
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
partial class FUIGame : FUI
{
    public sealed override string url => "FUIGame";
    public GButton _replay { get; private set; }
    public G_Tips _tips2 { get; private set; }

    protected sealed override void Binding()
    {
        GComponent ui = this.ui;
        _replay = (GButton)ui.GetChildAt(1);
        _tips2 = (G_Tips)ui.GetChildAt(2);
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
        GComponent ui = this.ui;
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
    public GButton _btnLogin { get; private set; }
    public GLabel _acc { get; private set; }
    public GLabel _pw { get; private set; }
    public GButton _btnEnter { get; private set; }
    public GButton _asServer { get; private set; }
    public GLabel _serverIP { get; private set; }
    public Controller _c1 { get; private set; }

    protected sealed override void Binding()
    {
        GComponent ui = this.ui;
        _btnLogin = (GButton)ui.GetChildAt(1);
        _acc = (GLabel)ui.GetChildAt(2);
        _pw = (GLabel)ui.GetChildAt(3);
        _btnEnter = (GButton)ui.GetChildAt(5);
        _asServer = (GButton)ui.GetChildAt(7);
        _serverIP = (GLabel)ui.GetChildAt(8);
        _c1 = ui.GetControllerAt(0);
    }
    public override void Dispose()
    {
        base.Dispose();
    }
}
partial class FUIRooms : FUI
{
    public sealed override string url => "FUIRooms";
    public GButton _ref { get; private set; }
    public GList _rooms { get; private set; }
    public GLabel _roomName { get; private set; }
    public GButton _create { get; private set; }

    protected sealed override void Binding()
    {
        GComponent ui = this.ui;
        _ref = (GButton)ui.GetChildAt(1);
        _rooms = (GList)ui.GetChildAt(2);
        _roomName = (GLabel)ui.GetChildAt(3);
        _create = (GButton)ui.GetChildAt(4);
    }
    public override void Dispose()
    {
        base.Dispose();
    }
}
partial class G_test : GLabel
{
    public GTextFieldPropertyBinding _n0 { get; private set; }
    public GLoader _a { get; private set; }
    public G_Tips _tips { get; private set; }
    public Controller _c2 { get; private set; }
    public Transition _t0 { get; private set; }

    public override void ConstructFromXML(XML xml)
    {
        _n0 = new((GTextField)this.GetChildAt(0));
        _a = (GLoader)this.GetChildAt(2);
        _tips = (G_Tips)this.GetChildAt(3);
        _c2 = this.GetControllerAt(1);
        _t0 = this.GetTransitionAt(0);
        this.Enter();
    }
    partial void Enter();
    public static G_test Create() => (G_test)UIPackage.CreateObject("ComPkg", "test");
    public override void Dispose()
    {
        base.Dispose();
        _n0.Dispose();
    }
}
partial class G_Tips : GComponent
{

    public override void ConstructFromXML(XML xml)
    {
        this.Enter();
    }
    partial void Enter();
    public static G_Tips Create() => (G_Tips)UIPackage.CreateObject("ComPkg", "Tips");
    public override void Dispose()
    {
        base.Dispose();
    }
}
partial class FUI3DHeader : FUI3D
{
    public sealed override string url => "UI_FUI3DHeader";
    public GTextField _title { get; private set; }

    protected sealed override void Binding()
    {
        GComponent ui = this.ui;
        _title = (GTextField)ui.GetChildAt(0);
    }
    public override void Dispose()
    {
        base.Dispose();
    }
}
