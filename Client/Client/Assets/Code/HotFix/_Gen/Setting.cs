using UnityEngine;
using Game;
using Main;

namespace Game
{
	public static partial class Setting
	{
	    static FreedomCameraSetting _FreedomCameraSetting;
	    public static FreedomCameraSetting FreedomCameraSetting
	    {
	        get
	        {
	            if (!_FreedomCameraSetting)
	                _FreedomCameraSetting = (FreedomCameraSetting)SAsset.Load<ScriptableObject>("Config/SO/FreedomCameraSetting.asset");
	            return _FreedomCameraSetting;
	        }
	    }
	    static LockingCameraSetting _LockingCameraSetting;
	    public static LockingCameraSetting LockingCameraSetting
	    {
	        get
	        {
	            if (!_LockingCameraSetting)
	                _LockingCameraSetting = (LockingCameraSetting)SAsset.Load<ScriptableObject>("Config/SO/LockingCameraSetting.asset");
	            return _LockingCameraSetting;
	        }
	    }
	}
}
