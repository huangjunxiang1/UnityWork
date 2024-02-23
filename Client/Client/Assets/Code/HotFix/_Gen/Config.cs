using UnityEngine;
using Game;
using System;
using UnityEngine.InputSystem;

public static partial class SettingL
{
	static LockingCameraSetting _LockingCameraSetting;
	public static LockingCameraSetting LockingCameraSetting => _LockingCameraSetting ??= (LockingCameraSetting)SAsset.Load<ScriptableObject>("Config/SO/Hotfix/LockingCameraSetting.asset");
}

public class ESCInput
{
    public ESCInput()
    {
        this.Asset = SAsset.Load<InputActionAsset>("Config/SO/Hotfix/ESCInput.inputactions");
        this.Asset.Enable();
        this.esc = this.Asset.FindActionMap(new Guid(0x8ff01451, 0xc5bb, 0x4a85, 0x99, 0xc4, 0xaf, 0x6e, 0x94, 0x66, 0xa4, 0x1c));
        this.esconEsc = this.esc.FindAction(new Guid(0xe64c28ad, 0x9fe6, 0x408b, 0xbc, 0x8b, 0xcc, 0x42, 0xaf, 0x9a, 0x03, 0xa2));
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
