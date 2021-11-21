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
        vc_ts = cm_vc.GetCinemachineComponent<CinemachineTransposer>();
        offset = vc_ts.m_FollowOffset;
    }
    static CinemachineBrain cm_cb;
    static CinemachineVirtualCamera cm_vc;
    static CinemachineTransposer vc_ts;
    static Vector3 offset;

    public static void Init(Game.WObject self)
    {
        cm_cb.ActiveVirtualCamera.Follow = self.GameObject.transform;
        cm_cb.ActiveVirtualCamera.LookAt = self.GameObject.transform;

        Timer.Add(0, -1, wheels);
        wheel = 1;
    }
    public static void Exit()
    {
        cm_cb.ActiveVirtualCamera.Follow = null;
        cm_cb.ActiveVirtualCamera.LookAt = null;
        Timer.Remove(wheels);
    }
    static float wheel = 1;
    static void wheels()
    {
        float _wheel = Input.GetAxis("Mouse ScrollWheel");
        wheel -= _wheel;
        wheel = Mathf.Clamp(wheel, 0.1f, 3);
        vc_ts.m_FollowOffset = offset * wheel;

        if (Input.GetMouseButtonDown(1))
        {
            try
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
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
    }
}