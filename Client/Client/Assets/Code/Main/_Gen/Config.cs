using UnityEngine;
using Game;
using System;

public static partial class SettingM
{
	static FreedomCameraSetting _FreedomCameraSetting;
	public static FreedomCameraSetting FreedomCameraSetting => _FreedomCameraSetting ??= (FreedomCameraSetting)SAsset.Load<ScriptableObject>("Config/SO/Main/FreedomCameraSetting.asset");
}

public class CMInput
{
    public CMInput()
    {
        this.Asset = SAsset.Load<UnityEngine.InputSystem.InputActionAsset>("Config/SO/Main/CMInput.inputactions");
        this.Asset.Enable();
        this.CMEditor = this.Asset.FindActionMap(new Guid(0x8ab7c044, 0xb1d5, 0x4d00, 0xad, 0xd9, 0xb5, 0xad, 0x34, 0x65, 0x1f, 0xb2));
        this.CMEditorMouseClick = this.CMEditor.FindAction(new Guid(0xf6e24bbd, 0xe3fa, 0x44d8, 0x92, 0x3a, 0x92, 0x48, 0xbd, 0xef, 0xed, 0xc6));
        this.CMEditorMouseMove = this.CMEditor.FindAction(new Guid(0x64d68308, 0x88b7, 0x44de, 0x9d, 0x2d, 0xf3, 0x81, 0x5d, 0xb8, 0x77, 0x43));
        this.CMEditorMouseWheel = this.CMEditor.FindAction(new Guid(0xd993eab1, 0xfe85, 0x4a04, 0xb3, 0xe4, 0x64, 0x26, 0xc0, 0xd2, 0x31, 0x65));
        this.CMEditorMousePosition = this.CMEditor.FindAction(new Guid(0xa956c43d, 0x5bf2, 0x43a2, 0xbb, 0x48, 0xec, 0x7b, 0x4f, 0x52, 0xe2, 0x2f));
        this.CMMobile = this.Asset.FindActionMap(new Guid(0x6a40c8de, 0x6720, 0x47fa, 0x87, 0xcf, 0xac, 0x09, 0x51, 0x59, 0xcd, 0xd4));
        this.CMMobileMove = this.CMMobile.FindAction(new Guid(0xa5390b0f, 0x91f2, 0x482b, 0x87, 0x69, 0x12, 0x08, 0xe8, 0x96, 0x3f, 0x93));
    }

    public UnityEngine.InputSystem.InputActionAsset Asset { get; }

    public UnityEngine.InputSystem.InputActionMap CMEditor { get; }
    public UnityEngine.InputSystem.InputAction CMEditorMouseClick { get; }
    public UnityEngine.InputSystem.InputAction CMEditorMouseMove { get; }
    public UnityEngine.InputSystem.InputAction CMEditorMouseWheel { get; }
    public UnityEngine.InputSystem.InputAction CMEditorMousePosition { get; }
    public UnityEngine.InputSystem.InputActionMap CMMobile { get; }
    public UnityEngine.InputSystem.InputAction CMMobileMove { get; }

    public void Dispose()
    {
        SAsset.Release(Asset);
        this.CMEditor.Dispose();
        this.CMMobile.Dispose();
    }
}
public class PlayerControl
{
    public PlayerControl()
    {
        this.Asset = SAsset.Load<UnityEngine.InputSystem.InputActionAsset>("Config/SO/Main/PlayerControl.inputactions");
        this.Asset.Enable();
        this.Player = this.Asset.FindActionMap(new Guid(0x63cce95c, 0xfd08, 0x4620, 0xb2, 0xf4, 0x09, 0xfd, 0x85, 0x44, 0xfc, 0x42));
        this.PlayerMove = this.Player.FindAction(new Guid(0x3ad895d5, 0x5f1b, 0x48ec, 0x9e, 0x02, 0xd7, 0x3e, 0xee, 0x99, 0xca, 0x3f));
    }

    public UnityEngine.InputSystem.InputActionAsset Asset { get; }

    public UnityEngine.InputSystem.InputActionMap Player { get; }
    public UnityEngine.InputSystem.InputAction PlayerMove { get; }

    public void Dispose()
    {
        SAsset.Release(Asset);
        this.Player.Dispose();
    }
}
