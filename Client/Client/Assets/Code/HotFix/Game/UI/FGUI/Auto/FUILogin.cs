/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

partial class FUILogin : FUIBase
{
    public GComboBox _uiType;
    public GComboBox _gameTypeCB;
    public GButton _btnLogin;
    public GTextInput _acc;
    public GTextInput _pw;

    protected override void Binding()
    {
        _uiType = (GComboBox)this.UI.GetChild("_uiType");
        _gameTypeCB = (GComboBox)this.UI.GetChild("_gameTypeCB");
        _btnLogin = (GButton)this.UI.GetChild("_btnLogin");
        _acc = (GTextInput)this.UI.GetChild("_acc");
        _pw = (GTextInput)this.UI.GetChild("_pw");
    }
}