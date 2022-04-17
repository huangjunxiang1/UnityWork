using UnityEngine;
using Game;
using Main;

static class Setting
{
    static CMSetting _CMSetting;
    public static CMSetting CMSetting
    {
        get
        {
            if (!_CMSetting)
                _CMSetting = (CMSetting)AssetLoad.Load<ScriptableObject>("Config/SO/CMSetting.asset");
            return _CMSetting;
        }
    }
}
