﻿using Unity.Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

abstract class BaseCamera
{
    public BaseCamera(CinemachineBrain brain)
    {
        this.Brain = brain;
    }

    public static BaseCamera Current { get; private set; }

    public CinemachineBrain Brain { get; }
    public GameObject Target { get; private set; }
    public bool Enable { get; private set; } 
    public bool Disposed { get; private set; }
    //手机
    public Vector2 FilterZonePos { get; private set; }
    public Vector2 FilterZoneSize { get; private set; }

    public static void SetCamera(BaseCamera camera)
    {
        Current?.Dispose();
        Current = camera;
    }

    public virtual void Init(GameObject target)
    {
        this.Target = target;
        ((CinemachineCamera)Brain.ActiveVirtualCamera).Follow = target.transform;
        ((CinemachineCamera)Brain.ActiveVirtualCamera).LookAt = target.transform;
    }
    public virtual void Dispose()
    {
        this.Disposed = true;
        if (this == Current)
            Current = null;
    }
    public virtual void EnableCamera()
    {
        this.Enable = true;
    }
    public virtual void DisableCamera()
    {
        this.Enable = false;
    }
    public virtual void SetFilterZone(Vector2 pos, Vector2 size)
    {
        this.FilterZonePos = pos;
        this.FilterZoneSize = size;
    }
}