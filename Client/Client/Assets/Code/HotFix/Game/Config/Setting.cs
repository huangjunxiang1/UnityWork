using UnityEngine;
using Game;
using Main;

static class Setting
{
    static FreedomCameraSetting _FreedomCameraSetting;
    public static FreedomCameraSetting FreedomCameraSetting
    {
        get
        {
            if (!_FreedomCameraSetting)
                _FreedomCameraSetting = (FreedomCameraSetting)AssetLoad.Load<ScriptableObject>("Config/SO/FreedomCameraSetting.asset");
            return _FreedomCameraSetting;
        }
    }
    static LockingCameraSetting _LockingCameraSetting;
    public static LockingCameraSetting LockingCameraSetting
    {
        get
        {
            if (!_LockingCameraSetting)
                _LockingCameraSetting = (LockingCameraSetting)AssetLoad.Load<ScriptableObject>("Config/SO/LockingCameraSetting.asset");
            return _LockingCameraSetting;
        }
    }
}