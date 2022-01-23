using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Cinemachine;
using Game;
using Main;

static class CM
{
    static CM()
    {
        cm_cb = GameObject.FindObjectOfType<CinemachineBrain>();
        cm_vc = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        cm_ct = cm_vc.GetCinemachineComponent<CinemachineTransposer>();

        offset = cm_ct.m_FollowOffset.normalized;
        distance = cm_ct.m_FollowOffset.magnitude;
        vc_settings = AssetLoad.ScriptObjectLoader.Load("Config/SO/CMSetting.asset") as CMSetting;
    }
    static CinemachineBrain cm_cb;
    static CinemachineVirtualCamera cm_vc;
    static CinemachineTransposer cm_ct;

    static CMSetting vc_settings;
    static Vector3 offset;
    static float distance;

    public static void Init(WObject self)
    {
        cm_cb.ActiveVirtualCamera.Follow = self.GameObject.transform;
        cm_cb.ActiveVirtualCamera.LookAt = self.GameObject.transform;

        Timer.Add(0, -1, wheels);
    }
    public static void Exit()
    {
        cm_cb.ActiveVirtualCamera.Follow = null;
        cm_cb.ActiveVirtualCamera.LookAt = null;
        Timer.Remove(wheels);
    }
    static void wheels()
    {
        float _wheel = Input.GetAxis("Mouse ScrollWheel");
        if (_wheel != 0)
        {
            distance -= _wheel * vc_settings.wheelSpeed;
            distance = Mathf.Clamp(distance, vc_settings.near, vc_settings.far);
            cm_ct.m_FollowOffset = offset * distance;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, -1))
            {
                C2M_PathfindingResult msg = new C2M_PathfindingResult();
                msg.X = hit.point.x;
                msg.Y = hit.point.y;
                msg.Z = hit.point.z;
                SysNet.Send(msg);
            }
        }
    }
}