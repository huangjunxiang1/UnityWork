using UnityEngine;
using Game;
using Main;

namespace Game
{
	public partial class SettingM
	{
	    TestSo _testMainSo;
	    public TestSo testMainSo
	    {
	        get
	        {
	            if (!_testMainSo)
	                _testMainSo = (TestSo)AssetLoad.Load<ScriptableObject>("Config/SO/Main/testMainSo.asset");
	            return _testMainSo;
	        }
	    }
	}
}
