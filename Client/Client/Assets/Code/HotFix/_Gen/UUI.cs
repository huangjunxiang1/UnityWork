partial class U_TestButton
{
    public UnityEngine.RectTransform ui { get; private set; }
    public UnityEngine.UI.Button UI_Button { get; private set; }

    public U_TestButton2 _TestButton2 { get; private set; }
    public TextPropertyBinding _Text_TMPText { get; private set; }

    public U_TestButton(UnityEngine.Transform ui)
    {
        this.ui = (UnityEngine.RectTransform)ui;
        UnityEngine.Transform c;
        this.UI_Button = (UnityEngine.UI.Button)ui.GetComponent(typeof(UnityEngine.UI.Button));
        c = ui.GetChild(0);
        this._TestButton2 = new(c);
        c = ui.GetChild(1);
        this._Text_TMPText = new((UnityEngine.UI.Text)c.GetComponent(typeof(UnityEngine.UI.Text)));
        this.Enter();
    }
    public U_TestButton(Game.ReleaseMode mode = Game.ReleaseMode.Destroy) : this(Game.SAsset.LoadGameObject("UI/UUI/Prefab/items/_TestButton.prefab", mode).transform) { }
    partial void Enter();
    public void Dispose()
    {
        this._TestButton2.Dispose();
        this._Text_TMPText.Dispose();
    }
}
partial class U_TestButton2
{
    public UnityEngine.RectTransform ui { get; private set; }
    public UnityEngine.UI.Button UI_Button { get; private set; }

    public TextPropertyBinding _Text_TMPText { get; private set; }

    public U_TestButton2(UnityEngine.Transform ui)
    {
        this.ui = (UnityEngine.RectTransform)ui;
        UnityEngine.Transform c;
        this.UI_Button = (UnityEngine.UI.Button)ui.GetComponent(typeof(UnityEngine.UI.Button));
        c = ui.GetChild(0);
        this._Text_TMPText = new((UnityEngine.UI.Text)c.GetComponent(typeof(UnityEngine.UI.Text)));
        this.Enter();
    }
    public U_TestButton2(Game.ReleaseMode mode = Game.ReleaseMode.Destroy) : this(Game.SAsset.LoadGameObject("UI/UUI/Prefab/items/_TestButton2.prefab", mode).transform) { }
    partial void Enter();
    public void Dispose()
    {
        this._Text_TMPText.Dispose();
    }
}
partial class UUILoading : UUI
{
    public sealed override string url => "UI/UUI/Prefab/UUILoading.prefab";

    public ImagePropertyBinding _fillImage { get; private set; }
    public TextPropertyBinding _txtText { get; private set; }

    protected sealed override void Binding()
    {
        UnityEngine.RectTransform ui = this.UI;
        UnityEngine.Transform c;
        c = ui.GetChild(0);
        this._fillImage = new((UnityEngine.UI.Image)c.GetComponent(typeof(UnityEngine.UI.Image)));
        c = ui.GetChild(1);
        this._txtText = new((UnityEngine.UI.Text)c.GetComponent(typeof(UnityEngine.UI.Text)));
    }
    public override void Dispose()
    {
        this._fillImage.Dispose();
        this._txtText.Dispose();
        base.Dispose();
    }
}
partial class UUILogin : UUI
{
    public sealed override string url => "UI/UUI/Prefab/UUILogin.prefab";

    public UnityEngine.UI.InputField _acInputField { get; private set; }
    public UnityEngine.UI.InputField _pwInputField { get; private set; }
    public UnityEngine.UI.Button _loginButton { get; private set; }
    public UnityEngine.UI.Dropdown _UITypeDropdown { get; private set; }
    public UnityEngine.UI.Dropdown _GameTypeDropdown { get; private set; }
    public TextPropertyBinding _sceneIDText { get; private set; }

    protected sealed override void Binding()
    {
        UnityEngine.RectTransform ui = this.UI;
        UnityEngine.Transform c;
        c = ui.GetChild(0);
        this._acInputField = (UnityEngine.UI.InputField)c.GetComponent(typeof(UnityEngine.UI.InputField));
        c = ui.GetChild(1);
        this._pwInputField = (UnityEngine.UI.InputField)c.GetComponent(typeof(UnityEngine.UI.InputField));
        c = ui.GetChild(2);
        this._loginButton = (UnityEngine.UI.Button)c.GetComponent(typeof(UnityEngine.UI.Button));
        c = ui.GetChild(3);
        this._UITypeDropdown = (UnityEngine.UI.Dropdown)c.GetComponent(typeof(UnityEngine.UI.Dropdown));
        c = ui.GetChild(4);
        this._GameTypeDropdown = (UnityEngine.UI.Dropdown)c.GetComponent(typeof(UnityEngine.UI.Dropdown));
        c = ui.GetChild(5);
        this._sceneIDText = new((UnityEngine.UI.Text)c.GetComponent(typeof(UnityEngine.UI.Text)));
    }
    public override void Dispose()
    {
        this._sceneIDText.Dispose();
        base.Dispose();
    }
}
