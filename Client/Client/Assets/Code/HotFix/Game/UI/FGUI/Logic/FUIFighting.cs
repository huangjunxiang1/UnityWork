using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

partial class FUIFighting
{
    protected override void OnEnter(params object[] data)
    {
        _btnBack.onClick.Add(_clickBack);
    }

    protected override void OnExit()
    {

    }

    void _clickBack()
    {
        _ = SceneMgr.Inst.InLoginScene();
    }
}
