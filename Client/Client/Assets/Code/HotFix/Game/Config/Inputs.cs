using UnityEngine.InputSystem;
using Main;
using UnityEngine;

public class CMInput
{
    public CMInput()
    {
        this.Asset = AssetLoad.Load<InputActionAsset>("Config/SO/CMInput.inputactions");
        this.CM = this.Asset.FindActionMap("CM", true);
        this.CMMouseClick = this.CM.FindAction("MouseClick");
        this.CMMouseMove = this.CM.FindAction("MouseMove");
        this.CMMouseWheel = this.CM.FindAction("MouseWheel");
    }

    public InputActionAsset Asset { get; }

    public InputActionMap CM { get; }
    public InputAction CMMouseClick { get; }
    public InputAction CMMouseMove { get; }
    public InputAction CMMouseWheel { get; }

    public void Dispose()
    {
        AssetLoad.Release(Asset);
        this.CM.Dispose();
    }
}
