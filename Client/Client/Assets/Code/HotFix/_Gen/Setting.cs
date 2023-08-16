using UnityEngine;
using Game;
using Main;

namespace Game
{
	public partial class SettingL
	{
	    FreedomCameraSetting _FreedomCameraSetting;
	    public FreedomCameraSetting FreedomCameraSetting
	    {
	        get
	        {
	            if (!_FreedomCameraSetting)
	                _FreedomCameraSetting = (FreedomCameraSetting)AssetLoad.Load<ScriptableObject>("Config/SO/Hot/FreedomCameraSetting.asset");
	            return _FreedomCameraSetting;
	        }
	    }
	    LockingCameraSetting _LockingCameraSetting;
	    public LockingCameraSetting LockingCameraSetting
	    {
	        get
	        {
	            if (!_LockingCameraSetting)
	                _LockingCameraSetting = (LockingCameraSetting)AssetLoad.Load<ScriptableObject>("Config/SO/Hot/LockingCameraSetting.asset");
	            return _LockingCameraSetting;
	        }
	    }
	}
}
