using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

class LockingCamera : BaseCamera
{
    public LockingCamera(CinemachineBrain brain) : base(brain)
    {
        input = new CMInput();

        if (Application.isEditor)
        {
            input.CMEditorMouseWheel.performed += editorWheel;
            input.CMEditor.Enable();
        }
        else if (Application.platform == RuntimePlatform.Android
              || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            input.CMMobile.Enable();
        }
    }

    public static new LockingCamera Current
    {
        get
        {
            return BaseCamera.Current as LockingCamera;
        }
    }

    CMInput input;

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
            Timer.Add(0, -1, mobileMove);
        input.Asset.Enable();
    }
    public override void DisableCamera()
    {
        if (!this.Enable)
            return;
        base.DisableCamera();
        if (Application.platform == RuntimePlatform.Android
            || Application.platform == RuntimePlatform.IPhonePlayer)
            Timer.Remove(mobileMove);
        input.Asset.Disable();
    }
    public override void SetFilterZone(Vector2 pos, Vector2 size)
    {
        base.SetFilterZone(pos, size);
    }
    void editorWheel(InputAction.CallbackContext e)
    {
        var m = Camera.main;
        if (!m) return;

        float _wheel = e.ReadValue<Vector2>().y;
        if (_wheel == 0)
            return;

        CinemachineVirtualCamera cvc = (CinemachineVirtualCamera)Brain.ActiveVirtualCamera;
        CinemachineTransposer ct = cvc.GetCinemachineComponent<CinemachineTransposer>();
        Vector3 offset = ct.m_FollowOffset;
        if (_wheel > 0)
        {
            if (offset.y > Setting.LockingCameraSetting.yMin)
            {
                offset += _wheel * Setting.LockingCameraSetting.wheelSpeed * m.transform.forward;
                ct.m_FollowOffset = offset;
            }
        }
        else
        {
            if (offset.y < Setting.LockingCameraSetting.yMax)
            {
                offset += _wheel * Setting.LockingCameraSetting.wheelSpeed * m.transform.forward;
                ct.m_FollowOffset = offset;
            }
        }
    }
    void mobileMove()
    {
        if (!Target)
            return;
        //2个手指不滑动
        var touches = Touchscreen.current.touches;

        int touchFingerCnt = 0;

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

                    CinemachineVirtualCamera cvc = (CinemachineVirtualCamera)Brain.ActiveVirtualCamera;
                    CinemachineTransposer ct = cvc.GetCinemachineComponent<CinemachineTransposer>();
                    Vector3 offset = ct.m_FollowOffset;
                    if (_wheel > 0)
                    {
                        if (offset.y > Setting.LockingCameraSetting.yMin)
                        {
                            offset += _wheel * Setting.LockingCameraSetting.wheelSpeed * m.transform.forward;
                            ct.m_FollowOffset = offset;
                        }
                    }
                    else
                    {
                        if (offset.y < Setting.LockingCameraSetting.yMax)
                        {
                            offset += _wheel * Setting.LockingCameraSetting.wheelSpeed * m.transform.forward;
                            ct.m_FollowOffset = offset;
                        }
                    }
                }
            }
        }
    }
}