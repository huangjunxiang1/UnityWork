using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "WSO/" + nameof(LockingCameraSetting), fileName = nameof(LockingCameraSetting), order = 1)]
public class LockingCameraSetting : ScriptableObject
{
    public float yMin;
    public float yMax;
    public float wheelSpeed;
}
