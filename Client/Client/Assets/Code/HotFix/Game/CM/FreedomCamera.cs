using Cinemachine;
using Core;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

class FreedomCamera : BaseCamera
{
    public FreedomCamera(CinemachineBrain brain) : base(brain)
    {
        input = new CMInput();

        if (Application.isEditor)
        {
            input.CMEditorMouseClick.started += editorClickDown;
            input.CMEditorMouseClick.canceled += editorClickUp;
            input.CMEditorMouseMove.performed += editorMove;
            input.CMEditorMouseWheel.performed += editorWheel;
        }
        else if (Application.platform == RuntimePlatform.Android
              || Application.platform == RuntimePlatform.IPhonePlayer)
        {
        }
    }

    public static new FreedomCamera Current
    {
        get
        {
            return BaseCamera.Current as FreedomCamera;
        }
    }

    CMInput input;
    bool isClickOutSide = false;
    bool isClickFui = false;

    public override void Init(GameObject target)
    {
        base.Init(target);
        var m = Camera.main;
        if (!m) return;
        var v3 = Target.transform.position;
        v3.y = Math.Clamp(Target.transform.position.y, SettingM.FreedomCameraSetting.yMin, SettingM.FreedomCameraSetting.yMax);
        Target.transform.position = v3;
    }
    public override void Dispose()
    {
        base.Dispose();
        input.Dispose();
    }
    public override void EnableCamera()
    {
        if (this.Enable)
            return;
        base.EnableCamera();
        if (Application.platform == RuntimePlatform.Android
             || Application.platform == RuntimePlatform.IPhonePlayer)
            GameWorld.World.Timer.Add(0, -1, mobileMove);
        input.Asset.Enable();
    }
    public override void DisableCamera()
    {
        if (!this.Enable)
            return;
        base.DisableCamera();
        if (Application.platform == RuntimePlatform.Android
            || Application.platform == RuntimePlatform.IPhonePlayer)
            GameWorld.World.Timer.Remove(mobileMove);
        input.Asset.Disable();
    }
    public override void SetFilterZone(Vector2 pos, Vector2 size)
    {
        base.SetFilterZone(pos, size);
    }
    void editorClickDown(InputAction.CallbackContext e)
    {
        if (!Target)
            return;
        Vector2 p = input.CMEditorMousePosition.ReadValue<Vector2>();
        if (p.x <= 0.1 || p.y <= 0.1 || p.x > Screen.width || p.y > Screen.height)
            isClickOutSide = true;
        else
            isClickFui = UIHelper.IsOnTouchFUI();
    }
    void editorClickUp(InputAction.CallbackContext e)
    {
        isClickOutSide = false;
        isClickFui = false;
    }
    void editorMove(InputAction.CallbackContext e)
    {
        if (!Target)
            return;
        if (isClickOutSide || isClickFui)
            return;
        if (!Mouse.current.leftButton.isPressed)
            return;
        var v2 = -e.ReadValue<Vector2>();
        Target.transform.position += new Vector3(v2.x, 0, v2.y) * SettingM.FreedomCameraSetting.moveSpeed;
    }
    void editorWheel(InputAction.CallbackContext e)
    {
        var m = Camera.main;
        if (!m) return;

        float _wheel = -e.ReadValue<Vector2>().y;
        if (_wheel == 0)
            return;

        CinemachineVirtualCamera cvc = (CinemachineVirtualCamera)Brain.ActiveVirtualCamera;
        cvc.m_Lens.FieldOfView += _wheel * SettingM.FreedomCameraSetting.wheelSpeed;
        cvc.m_Lens.FieldOfView = Mathf.Clamp(cvc.m_Lens.FieldOfView, SettingM.FreedomCameraSetting.yMin, SettingM.FreedomCameraSetting.yMax);
    }
    void mobileMove()
    {
        if (!Target)
            return;
        //2个手指不滑动
        var touches = Touchscreen.current.touches;

        int touchFingerCnt = 0;
        Vector2 cmMoveDelta = default;

        int minIdx = -1;
        Vector2 minPos = default;
        int maxIdx = -1;
        Vector2 maxPos = default;

        for (int i = 0; i < touches.Count; i++)
        {
            var touch = touches[i];
            if (!touch.press.isPressed)
                continue;

            var startPos = touch.startPosition.ReadValue();
            var p2 = touch.position.ReadValue();
            var d2 = touch.delta.ReadValue();

            if (startPos.x >= FilterZonePos.x
                && startPos.y >= FilterZonePos.y
                && startPos.x <= (FilterZonePos.x + FilterZoneSize.x)
                && startPos.y <= (FilterZonePos.y + FilterZoneSize.y))
                continue;

            if (UIHelper.IsOnTouchFUI(startPos))
                continue;

            touchFingerCnt++;

            float amin = Vector2.Angle(p2 - minPos, new Vector2(1, 1));
            if (minIdx == -1 || amin > 90)
            {
                minIdx = i;
                minPos = p2;
            }
            float amax = Vector2.Angle(p2 - maxPos, new Vector2(1, 1));
            if (maxIdx == -1 || amax < 90)
            {
                maxIdx = i;
                maxPos = p2;
            }

            if (cmMoveDelta.magnitude < d2.magnitude)
                cmMoveDelta = d2;
        }

        {
            if (touchFingerCnt >= 2)
            {
                var m = Camera.main;
                if (m)
                {
                    var minTouch = touches[minIdx];
                    var maxTouch = touches[maxIdx];

                    var minD2 = minTouch.delta.ReadValue();
                    var maxD2 = maxTouch.delta.ReadValue();
                    float dmin = Math.Abs(minD2.x) >= Math.Abs(minD2.y) ? minD2.x : minD2.y;
                    float dmax = Math.Abs(maxD2.x) >= Math.Abs(maxD2.y) ? maxD2.x : maxD2.y;

                    float _wheel = 0;
                    if (Math.Abs(dmin) > Math.Abs(dmax))
                    {
                        if (Math.Abs(dmin) >= 1)
                            _wheel = -dmin * 2;
                    }
                    else
                    {
                        if (Math.Abs(dmax) >= 1)
                            _wheel = dmax * 2;
                    }

                    if (_wheel > 0)
                    {
                        if (Target.transform.position.y > SettingM.FreedomCameraSetting.yMin)
                            Target.transform.position += _wheel * SettingM.FreedomCameraSetting.wheelSpeed * m.transform.forward;
                    }
                    else
                    {
                        if (Target.transform.position.y < SettingM.FreedomCameraSetting.yMax)
                            Target.transform.position += _wheel * SettingM.FreedomCameraSetting.wheelSpeed * m.transform.forward;
                    }
                }
            }
            else
            {
                var v2 = -cmMoveDelta;
                Target.transform.position += new Vector3(v2.x, 0, v2.y) * SettingM.FreedomCameraSetting.moveSpeed;
            }
        }
    }
}
