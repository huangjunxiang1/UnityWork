/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

partial class FUILoading : FUIBase
{
    public GProgressBar _loadingBar;

    protected override void Binding()
    {
        _loadingBar = (GProgressBar)this.UI.GetChild("_loadingBar");
    }
}