using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Cinemachine;
using Game;
using Main;
using UnityEngine.InputSystem;
using FairyGUI;

static class CM
{
    static CM()
    {
        cm_cb = GameObject.FindObjectOfType<CinemachineBrain>();

        input = new CMInput();
        input.Asset.Enable();

        input.CMMouseClick.started += clickDown;
        input.CMMouseClick.canceled += clickUp;
        input.CMMouseMove.performed += move;
        input.CMMouseWheel.performed += wheel;
    }
    static CinemachineBrain cm_cb;
    static CinemachineVirtualCamera cm_vc;
    static CinemachineTransposer cm_ct;

    static WObject cm;
    static CMInput input;
    static bool onDown = false;

    static void clickDown(InputAction.CallbackContext e)
    {
        if (cm == null) return;
        var ui = GRoot.inst.touchTarget;
        while (ui != null)
        {
            if (ui == GRoot.inst) return;
            ui = ui.parent;
        }
        onDown = e.ReadValueAsButton();
    }
    static void clickUp(InputAction.CallbackContext e)
    {
        onDown = false;
    }
    static void move(InputAction.CallbackContext e)
    {
        if (!onDown) return;
        var v2 = -e.ReadValue<Vector2>();
        cm.Position += new Vector3(v2.x, 0, v2.y) * Setting.CMSetting.moveSpeed;
    }
    static void wheel(InputAction.CallbackContext e)
    {
        var m = Camera.main;
        if (!m) return;
        float _wheel = e.ReadValue<Vector2>().y;
        if (_wheel > 0)
        {
            if (cm.Position.y > Setting.CMSetting.yMin)
                cm.Position += m.transform.forward * Setting.CMSetting.wheelSpeed * _wheel;
        }
        else if (_wheel < 0)
        {
            if (cm.Position.y < Setting.CMSetting.yMax)
                cm.Position += m.transform.forward * Setting.CMSetting.wheelSpeed * _wheel;
        }
    }

    public static void Init(WObject self)
    {
        cm = self;
        cm_cb.ActiveVirtualCamera.Follow = cm.GameObject.transform;
        //cm_cb.ActiveVirtualCamera.LookAt = cm.GameObject.transform;

        cm_vc = (CinemachineVirtualCamera)cm_cb.ActiveVirtualCamera;
        cm_ct = cm_vc.GetCinemachineComponent<CinemachineTransposer>();
    }
    public static void Exit()
    {
        if (cm_cb.ActiveVirtualCamera != null)
        {
            cm_cb.ActiveVirtualCamera.Follow = null;
            cm_cb.ActiveVirtualCamera.LookAt = null;
        }
    }
}