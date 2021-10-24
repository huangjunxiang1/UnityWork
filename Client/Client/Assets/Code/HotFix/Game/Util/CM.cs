using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Cinemachine;

static class CM
{
    static CM()
    {
        cm_vc = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        vc_ts = cm_vc.GetCinemachineComponent<CinemachineTransposer>();
        offset = vc_ts.m_FollowOffset;
    }
    static CinemachineVirtualCamera cm_vc;
    static CinemachineTransposer vc_ts;
    static Vector3 offset;

    public static void Follow(Transform target)
    {
        cm_vc.Follow = target;
    }
    public static void LookAt(Transform target)
    {
        cm_vc.LookAt = target;
    }
    public static void Wheel(float v)
    {
        vc_ts.m_FollowOffset = offset * v;
    }
}