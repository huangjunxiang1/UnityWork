using UnityEngine.InputSystem;
using Game;
using Main;
using UnityEngine;

public class CMInput
{
    public CMInput()
    {
        this.Asset = SAsset.Load<InputActionAsset>("Config/SO/CMInput.inputactions");
        this.CMEditor = this.Asset.FindActionMap("CMEditor", true);
        this.CMEditorMouseClick = this.CMEditor.FindAction("MouseClick");
        this.CMEditorMouseMove = this.CMEditor.FindAction("MouseMove");
        this.CMEditorMouseWheel = this.CMEditor.FindAction("MouseWheel");
        this.CMEditorMousePosition = this.CMEditor.FindAction("MousePosition");
        this.CMMobile = this.Asset.FindActionMap("CMMobile", true);
        this.CMMobileMove = this.CMMobile.FindAction("Move");
    }

    public InputActionAsset Asset { get; }

    public InputActionMap CMEditor { get; }
    public InputAction CMEditorMouseClick { get; }
    public InputAction CMEditorMouseMove { get; }
    public InputAction CMEditorMouseWheel { get; }
    public InputAction CMEditorMousePosition { get; }
    public InputActionMap CMMobile { get; }
    public InputAction CMMobileMove { get; }

    public void Dispose()
    {
        SAsset.Release(Asset);
        this.CMEditor.Dispose();
        this.CMMobile.Dispose();
    }
}
public class ESCInput
{
    public ESCInput()
    {
        this.Asset = SAsset.Load<InputActionAsset>("Config/SO/ESCInput.inputactions");
        this.esc = this.Asset.FindActionMap("esc", true);
        this.esconEsc = this.esc.FindAction("onEsc");
    }

    public InputActionAsset Asset { get; }

    public InputActionMap esc { get; }
    public InputAction esconEsc { get; }

    public void Dispose()
    {
        SAsset.Release(Asset);
        this.esc.Dispose();
    }
}
