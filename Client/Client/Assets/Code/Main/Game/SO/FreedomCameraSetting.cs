using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "WSO/" + nameof(FreedomCameraSetting), fileName = nameof(FreedomCameraSetting), order = 1)]
public class FreedomCameraSetting : ScriptableObject
{
    public float yMin;
    public float yMax;
    public float wheelSpeed;
    public float moveSpeed;
}
