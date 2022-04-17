using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "WSO/" + nameof(CMSetting), fileName = nameof(CMSetting), order = 1)]
public class CMSetting : ScriptableObject
{
    public float yMin;
    public float yMax;
    public float wheelSpeed;
    public float moveSpeed;
}
