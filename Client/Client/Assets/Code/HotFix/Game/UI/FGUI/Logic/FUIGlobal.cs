using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.InputSystem;
using UnityEngine;
using Game;

partial class FUIGlobal
{
    bool showExit = false;
    ESCInput input;
    protected override void OnEnter(params object[] data)
    {
        this._log.visible = !Application.isEditor && AppSetting.Debug;
        if (this._log.visible)
            this._log.onClick.Add(log);

        if (!Application.isEditor)
        {
            input = new ESCInput();
            input.esconEsc.started += esc;
            input.esc.Enable();
        }
    }
    protected override void OnExit()
    {
        input?.Dispose();
    }
    void log()
    {
        AppSetting.ShowReporter = !AppSetting.ShowReporter;
    }
    void esc(InputAction.CallbackContext e)
    {
        if (showExit)
            return;
        if(e.ReadValueAsButton())
        {
            showExit = true;
            Box.Op_YesOrNo("退出游戏", "是否退出游戏?", "确定", "取消", () =>
            {
                UnityEngine.Application.Quit();
                showExit = false;
            },
            () =>
            {
                showExit = false;
            });
        }
    }
}